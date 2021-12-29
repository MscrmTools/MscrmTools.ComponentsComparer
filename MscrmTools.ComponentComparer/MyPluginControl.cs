using Diff.Net;
using McTools.Xrm.Connection;
using Menees.Diffs;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using MscrmTools.ComponentComparer.AppCode;
using Newtonsoft.Json.Linq;
using Rappen.XTB.Helpers.Controls;
using Rappen.XTB.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using XrmToolBox.Extensibility;

namespace MscrmTools.ComponentComparer
{
    public partial class MyPluginControl : MultipleConnectionsPluginControlBase
    {
        private bool doCompare = false;
        private Button sourceCompare;
        private CrmServiceClient TargetService;

        public MyPluginControl()
        {
            InitializeComponent();
        }

        internal enum CompareType
        {
            Auto,
            Text,
            Xml,
            Binary,
        }

        internal enum DiffType
        {
            File,
            Directory,
            Text,
        }

        public void Compare()
        {
            string entity, attribute, name;
            bool searchByPrimaryName = false;
            Guid recordId;

            if (sourceCompare == btnCompareSpecific)
            {
                if (comboBox1.SelectedItem?.ToString() == "Security Roles")
                {
                    try
                    {
                        var roleA = SecurityRoleHelper.GetRolePrivileges(textBox1.Tag != null ? (Guid)textBox1.Tag : Guid.Empty, textBox1.Text, Service);
                        var roleB = SecurityRoleHelper.GetRolePrivileges(textBox1.Tag != null ? (Guid)textBox1.Tag : Guid.Empty, textBox1.Text, TargetService);

                        Compare(roleA, roleB);
                    }
                    catch
                    {
                        MessageBox.Show(this, "Cannot find the specified role in the target environment", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (comboBox1.SelectedItem?.ToString() == "Webresources")
                {
                    try
                    {
                        var contentA = WebresourceHelper.GetWebresourceContent(textBox1.Tag != null ? (Guid)textBox1.Tag : Guid.Empty, textBox1.Text, Service);
                        var contentB = WebresourceHelper.GetWebresourceContent(textBox1.Tag != null ? (Guid)textBox1.Tag : Guid.Empty, textBox1.Text, TargetService);

                        Compare(contentA, contentB);
                    }
                    catch
                    {
                        MessageBox.Show(this, "Cannot find the specified web resource in the target environment", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                return;
            }

            if (sourceCompare == btnCompare)
            {
                if (txtFormId.Text.Length == 0 || txtEntity.Text.Length == 0 || txtAttribute.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please define an entity, an attribute and a record id before comparing", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                entity = txtEntity.Text;
                attribute = txtAttribute.Text;
                name = txtFormId.Text;

                if (!Guid.TryParse(txtFormId.Text, out recordId))
                {
                    searchByPrimaryName = true;
                }
            }
            else
            {
                if (txtRecord.Tag == null)
                {
                    MessageBox.Show(this, "Please select a record before comparing", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                entity = xecb.SelectedEntity.LogicalName;
                attribute = xacb.SelectedAttribute.LogicalName;
                recordId = (Guid)txtRecord.Tag;
                name = txtRecord.Text;
            }

            Entity eA, eB;
            try
            {
                if (searchByPrimaryName)
                {
                    var emd = ((List<EntityMetadata>)xecb.DataSource).First(e => e.LogicalName == entity);
                    eA = Service.RetrieveMultiple(new QueryExpression(entity)
                    {
                        ColumnSet = new ColumnSet(attribute),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                         {
                             new ConditionExpression(emd.PrimaryNameAttribute, ConditionOperator.Equal, name)
                         }
                        }
                    }).Entities.FirstOrDefault();

                    if (eA == null)
                    {
                        MessageBox.Show(this, $"No {entity} found with primary name \"{name}\"", "Source record cannot be found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    eA = Service.Retrieve(entity, recordId, new ColumnSet(attribute));
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(this, error.Message, "Source record cannot be found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (searchByPrimaryName)
                {
                    var emd = ((List<EntityMetadata>)xecb.DataSource).First(e => e.LogicalName == entity);
                    eB = TargetService.RetrieveMultiple(new QueryExpression(entity)
                    {
                        ColumnSet = new ColumnSet(attribute),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                         {
                             new ConditionExpression(emd.PrimaryNameAttribute, ConditionOperator.Equal, name)
                         }
                        }
                    }).Entities.FirstOrDefault();

                    if (eB == null)
                    {
                        MessageBox.Show(this, $"No {entity} found with primary name \"{name}\"", "Target record cannot be found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    eB = TargetService.Retrieve(entity, recordId, new ColumnSet(attribute));
                }
            }
            catch (Exception error)
            {
                var emd = ((List<EntityMetadata>)xecb.DataSource).First(e => e.LogicalName == entity);
                eB = TargetService.RetrieveMultiple(new QueryExpression(entity)
                {
                    ColumnSet = new ColumnSet(attribute),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                         {
                             new ConditionExpression(emd.PrimaryNameAttribute, ConditionOperator.Equal, name)
                         }
                    }
                }).Entities.FirstOrDefault();

                if (eB == null)
                {
                    MessageBox.Show(this, error.Message, "Target record cannot be found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string sA = TryFormatJson(GetStringContent(eA, attribute));
            string sB = TryFormatJson(GetStringContent(eB, attribute));

            Compare(sA, sB);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (string.IsNullOrEmpty(actionName))
            {
                tslSourceEnvSelected.Text = detail.ConnectionName;
                tslSourceEnvSelected.ForeColor = Color.Green;
                xecb.DataSource = detail.MetadataCache?.ToList() ?? GetMetadata();

                btnLookup.Enabled = true;
                btnLookupSpecific.Enabled = true;
            }
            else
            {
                tslTargetEnvSelected.Text = detail.ConnectionName;
                tslTargetEnvSelected.ForeColor = Color.Green;
                TargetService = detail.GetCrmServiceClient();

                if (doCompare)
                {
                    doCompare = false;
                    Compare();
                }
            }
        }

        protected override void ConnectionDetailsUpdated(NotifyCollectionChangedEventArgs e)
        { }

        private static void GetTextLines(string textA, string textB, out IList<string> a, out IList<string> b)
        {
            a = null;
            b = null;
            CompareType compareType = Options.CompareType;
            bool isAuto = compareType == CompareType.Auto;

            if (compareType == CompareType.Xml || isAuto)
            {
                a = TryGetXmlLines(DiffUtility.GetXmlTextLinesFromXml, "the left side text", textA, !isAuto);

                // If A failed to parse with Auto, then there's no reason to try B.
                if (a != null)
                {
                    b = TryGetXmlLines(DiffUtility.GetXmlTextLinesFromXml, "the right side text", textB, !isAuto);
                }

                // If we get here and the compare type was XML, then both
                // inputs parsed correctly, and both lists should be non-null.
                // If we get here and the compare type was Auto, then one
                // or both lists may be null, so we'll fallthrough to the text
                // handling logic.
            }

            if (a == null || b == null)
            {
                a = DiffUtility.GetStringTextLines(textA);
                b = DiffUtility.GetStringTextLines(textB);
            }
        }

        private static IList<string> TryGetXmlLines(Func<string, bool, IList<string>> converter, string name, string input, bool throwOnError)
        {
            IList<string> result = null;
            try
            {
                result = converter(input, Options.IgnoreXmlWhitespace);
            }
            catch (XmlException ex)
            {
                if (throwOnError)
                {
                    StringBuilder sb = new StringBuilder("An XML comparison was attempted, but an XML exception occurred while parsing ");
                    sb.Append(name).AppendLine(".").AppendLine();
                    sb.AppendLine("Exception Message:").Append(ex.Message);
                    throw new XmlException(sb.ToString(), ex);
                }
            }

            return result;
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            sourceCompare = (Button)sender;

            if (TargetService == null)
            {
                doCompare = true;
                AddAdditionalOrganization();
            }
            else
            {
                Compare();
            }
        }

        private void btnLookup_Click(object sender, EventArgs e)
        {
            var dialog = new XRMLookupDialog()
            {
                LogicalName = xecb.SelectedEntity.LogicalName,
                Service = Service
            };

            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                txtRecord.Text = dialog.Records.First().GetAttributeValue<string>(xecb.SelectedEntity.PrimaryNameAttribute);
                txtRecord.Tag = dialog.Records.First().Id;
            }
        }

        private void btnLookupSpecific_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem?.ToString() == "Security Roles")
            {
                var dialog = new XRMLookupDialog()
                {
                    LogicalName = "role",
                    Service = Service
                };

                Guid viewId = Guid.NewGuid();

                dialog.AdditionalViews = new Dictionary<string, List<Entity>>
                {
                    {"role", new List<Entity>
                        {
                            new Entity("savedquery")
                            {
                                Id = viewId,
                                Attributes =
                                {
                                    {"name", "Root roles" },
                                    {"fetchxml", $"<fetch mapping=\"logical\"><entity name=\"role\"><attribute name=\"name\"/><order attribute=\"name\"/><filter><condition attribute=\"parentroleid\" operator=\"null\"/></filter></entity></fetch>" },
                                    {"layoutxml", $"<grid name=\"resultset\" object=\"1036\" jump=\"name\" select=\"1\" icon=\"1036\" preview=\"1\"><row name=\"result\" id=\"roleid\"><cell name=\"name\" width=\"150\" /></row></grid>" },
                                }
                            }
                        }
                    }
                };
                dialog.SetDefaultView("role", viewId);

                var result = dialog.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    textBox1.Text = dialog.Records.First().GetAttributeValue<string>("name");
                    textBox1.Tag = dialog.Records.First().Id;
                }
            }
            else if (comboBox1.SelectedItem?.ToString() == "Webresources")
            {
                var dialog = new XRMLookupDialog()
                {
                    LogicalName = "webresource",
                    Service = Service
                };

                var result = dialog.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    textBox1.Text = dialog.Records.First().GetAttributeValue<string>("name");
                    textBox1.Tag = dialog.Records.First().Id;
                }
            }
        }

        private void Compare(string contentA, string contentB)
        {
            IList<string> a, b;
            int leadingCharactersToIgnore = 0;
            GetTextLines(contentA, contentB, out a, out b);

            bool isBinaryCompare = leadingCharactersToIgnore > 0;
            bool ignoreCase = !isBinaryCompare && Options.IgnoreCase;
            bool ignoreTextWhitespace = !isBinaryCompare && Options.IgnoreTextWhitespace;
            TextDiff diff = new TextDiff(Options.HashType, ignoreCase, ignoreTextWhitespace, leadingCharactersToIgnore, !Options.ShowChangeAsDeleteInsert);
            EditScript script = diff.Execute(a, b);

            string captionA = string.Empty;
            string captionB = string.Empty;

            this.diffControl1.SetData(a, b, script, captionA, captionB, ignoreCase, ignoreTextWhitespace, isBinaryCompare);

            if (Options.LineDiffHeight != 0)
            {
                this.diffControl1.LineDiffHeight = Options.LineDiffHeight;
            }
        }

        private List<EntityMetadata> GetMetadata()
        {
            return Service.LoadEntities().EntityMetadata.ToList();
        }

        private string GetStringContent(Entity record, string attribute)
        {
            if (record.Contains(attribute))
            {
                if (record[attribute] is EntityReference er)
                {
                    return $"{er.Id}\r\n{er.LogicalName}\r\n{er.Name}";
                }
                else if (record[attribute] is OptionSetValue ov)
                {
                    return $"{record.FormattedValues[attribute]}\r\n{ov.Value}";
                }
                else if (record[attribute] is Money m)
                {
                    return m?.Value.ToString() ?? "";
                }
                else
                {
                    return record[attribute].ToString();
                }
            }

            return string.Empty;
        }

        private string TryFormatJson(string s)
        {
            try
            {
                JObject json = JObject.Parse(s);
                return json.ToString();
            }
            catch
            {
                return s;
            }
        }

        private void tsbSetTargetEnv_Click(object sender, EventArgs e)
        {
            AddAdditionalOrganization();
        }

        private void xecb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var entity = xecb.SelectedEntity;
            if (entity != null && entity.Attributes == null)
            {
                entity = Service.GetEntity(entity.LogicalName);
            }
            xacb.DataSource = entity;

            txtRecord.Text = string.Empty;
            txtRecord.Tag = null;
        }
    }
}