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
using System.Xml.Linq;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MscrmTools.ComponentComparer
{
    public partial class MyPluginControl : MultipleConnectionsPluginControlBase, IMessageBusHost
    {
        private bool doCompare = false;
        private Button sourceCompare;
        private List<EntityMetadata> targetEmds;
        private CrmServiceClient targetService;

        public MyPluginControl()
        {
            InitializeComponent();
        }

        public event EventHandler<MessageBusEventArgs> OnOutgoingMessage;

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
                        var roleB = SecurityRoleHelper.GetRolePrivileges(textBox1.Tag != null ? (Guid)textBox1.Tag : Guid.Empty, textBox1.Text, targetService);

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
                        var contentB = WebresourceHelper.GetWebresourceContent(textBox1.Tag != null ? (Guid)textBox1.Tag : Guid.Empty, textBox1.Text, targetService);

                        Compare(contentA, contentB);
                    }
                    catch
                    {
                        MessageBox.Show(this, "Cannot find the specified web resource in the target environment", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (comboBox1.SelectedItem?.ToString() == "Model driven app")
                {
                    try
                    {
                        var recordA = Service.Retrieve("appmodule", (Guid)textBox1.Tag, new ColumnSet("descriptor", "uniquename"));
                        var contentA = recordA.GetAttributeValue<string>("descriptor");
                        var contentB = targetService.RetrieveMultiple(new QueryExpression("appmodule")
                        {
                            ColumnSet = new ColumnSet("descriptor"),
                            Criteria = new FilterExpression
                            {
                                Conditions =
                                 {
                                     new ConditionExpression("uniquename", ConditionOperator.Equal, recordA.GetAttributeValue<string>("uniquename"))
                                 }
                            }
                        }).Entities.FirstOrDefault()?.GetAttributeValue<string>("descriptor");

                        contentA = ModernAppHelper.OptimizeAppDescriptor(contentA, Service, (List<EntityMetadata>)xecb.DataSource);
                        contentB = ModernAppHelper.OptimizeAppDescriptor(contentB, targetService, targetEmds);

                        Compare(TryFormatJson(contentA, out bool _), TryFormatJson(contentB, out bool _));
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(this, $"An error occured when working with Model driven app descriptor: {error.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                return;
            }

            if (sourceCompare == btnCompare)
            {
                if (txtRecordId.Text.Length == 0 || txtEntity.Text.Length == 0 || txtAttribute.Text.Length == 0)
                {
                    MessageBox.Show(this, "Please define an entity, an attribute and a record id before comparing", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                entity = txtEntity.Text;
                attribute = txtAttribute.Text;
                name = txtRecordId.Text;

                if (!Guid.TryParse(txtRecordId.Text, out recordId))
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
                    eB = targetService.RetrieveMultiple(new QueryExpression(entity)
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
                    eB = targetService.Retrieve(entity, recordId, new ColumnSet(attribute));
                }
            }
            catch (Exception error)
            {
                var emd = ((List<EntityMetadata>)xecb.DataSource).First(e => e.LogicalName == entity);
                eB = targetService.RetrieveMultiple(new QueryExpression(entity)
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

            string sA = TryFormatJson(GetStringContent(eA, attribute), out bool isJsonA);
            string sB = TryFormatJson(GetStringContent(eB, attribute), out bool isJsonB);

            if (!isJsonA || !isJsonB)
            {
                sA = TryFormatXml(sA);
                sB = TryFormatXml(sB);
            }

            Compare(sA, sB);
        }

        public void OnIncomingMessage(MessageBusEventArgs message)
        {
            if (message.SourcePlugin == "Solution Layers Explorer" &&
               message.TargetArgument is string parameter &&
               !string.IsNullOrWhiteSpace(parameter))
            {
                var parameters = parameter.Split(';');
                if (parameters.Length == 4)
                {
                    if (bool.Parse(parameters[0]))
                    {
                        comboBox1.SelectedItem = parameters[1];
                        textBox1.Text = parameters[2];
                        textBox1.Tag = new Guid(parameters[3]);

                        btnCompare_Click(btnCompareSpecific, new EventArgs());
                    }
                    else
                    {
                        txtEntity.Text = parameters[1];
                        txtAttribute.Text = parameters[2];
                        txtRecordId.Text = parameters[3];

                        btnCompare_Click(btnCompare, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show(this, "Unexpected number of arguments", $"4 parameters were expected. Receieved {parameters.Length}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
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
                xecb.DataSource = detail.MetadataCache?.ToList() ?? GetMetadata(newService);

                btnLookup.Enabled = true;
                btnLookupSpecific.Enabled = true;
            }
            else
            {
                tslTargetEnvSelected.Text = detail.ConnectionName;
                tslTargetEnvSelected.ForeColor = Color.Green;
                targetService = detail.GetCrmServiceClient();
                targetEmds = detail.MetadataCache?.ToList() ?? GetMetadata(targetService);

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

            if (targetService == null)
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
            else if (comboBox1.SelectedItem?.ToString() == "Model driven app")
            {
                var dialog = new XRMLookupDialog()
                {
                    LogicalName = "appmodule",
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

        private List<EntityMetadata> GetMetadata(IOrganizationService service)
        {
            return service.LoadEntities().EntityMetadata.ToList();
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

        private string TryFormatJson(string s, out bool isJson)
        {
            try
            {
                JObject json = JObject.Parse(s);
                isJson = true;
                return json.ToString();
            }
            catch
            {
                isJson = false;
                return s;
            }
        }

        private string TryFormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);

                doc = Sort(doc, 0, "", 1);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = false;

                var sb = new StringBuilder();

                using (XmlWriter writer = XmlWriter.Create(sb, settings))
                {
                    doc.WriteTo(writer);
                }

                return doc.ToString();
            }
            catch (Exception)
            {
                return xml;
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

        #region Private Method - Sort

        ///<summary>
        /// Sort an XML Element based on a minimum level to perform the
        /// sort from and either based on the value
        /// of an attribute of an XML Element or by the name of the XML Element.
        /// Code from : https://www.codeproject.com/Articles/166357/XML-Alphabetizer
        ///</summary>
        ///<param name="file">File to load and sort</param>
        ///<param name="level">Minimum level to apply the sort from.
        /// 0 for root level.</param>
        ///<param name="attribute">Name of the attribute to sort by.
        /// "" for no sort</param>
        ///<param name="sortAttributes">Sort attributes none,
        /// ascending or descending for all sorted XML nodes</param>
        ///<returns>Sorted XElement based on the criteria passed in.</returns>

        private static XDocument Sort(XDocument file,
            int level, string attribute, int sortAttributes)
        {
            return new XDocument(Sort(file.Root, level, attribute, sortAttributes));
        }

        ///<summary>
        /// Sort an XML Element based on a minimum level to perform the
        /// sort from and either based on the value
        /// of an attribute of an XML Element or by the name of the XML Element.
        /// Code from : https://www.codeproject.com/Articles/166357/XML-Alphabetizer
        ///</summary>
        ///<param name="element">Element to sort</param>
        ///<param name="level">Minimum level to apply the sort from.
        /// 0 for root level.</param>
        ///<param name="attribute">Name of the attribute to sort by.
        /// "" for no sort</param>
        ///<param name="sortAttributes">Sort attributes none,
        /// ascending or descending for all sorted XML nodes</param>
        ///<returns>Sorted XElement based on the criteria passed in.</returns>

        private static XElement Sort(XElement element,
            int level, string attribute, int sortAttributes)
        {
            XElement newElement = new XElement(element.Name,
                from child in element.Elements()
                    //orderby
                    //    (child.Ancestors().Count() > level)
                    //        ? (
                    //            (child.HasAttributes &&
                    //                !string.IsNullOrEmpty(attribute)
                    //            && child.Attribute(attribute) != null)
                    //                ? child.Attribute(attribute).
                    //                    Value.ToString()
                    //                : child.Name.ToString()
                    //            )
                    //        : ""  //End of the orderby clause
                select Sort(child, level, attribute, sortAttributes));
            if (element.HasAttributes)
            {
                switch (sortAttributes)
                {
                    case 0: //None
                        foreach (XAttribute attrib in element.Attributes())
                        {
                            newElement.SetAttributeValue
                                (attrib.Name, attrib.Value);
                        }
                        break;

                    case 1: //Ascending
                        foreach (XAttribute attrib in element.Attributes().
                        OrderBy(a => a.Name.ToString()))
                        {
                            newElement.SetAttributeValue
                                (attrib.Name, attrib.Value);
                        }
                        break;

                    case 2: //Descending
                        foreach (XAttribute attrib in element.Attributes().
                        OrderByDescending(a => a.Name.ToString()))
                        {
                            newElement.SetAttributeValue
                                (attrib.Name, attrib.Value);
                        }
                        break;

                    default:
                        break;
                }
            }
            return newElement;
        }

        #endregion Private Method - Sort
    }
}