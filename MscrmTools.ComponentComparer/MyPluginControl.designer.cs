
namespace MscrmTools.ComponentComparer
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tslSourceEnv = new System.Windows.Forms.ToolStripLabel();
            this.tslSourceEnvSelected = new System.Windows.Forms.ToolStripLabel();
            this.tslTargetEnv = new System.Windows.Forms.ToolStripLabel();
            this.tslTargetEnvSelected = new System.Windows.Forms.ToolStripLabel();
            this.tsbSetTargetEnv = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.gbSpecific = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnLookupSpecific = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCompareSpecific = new System.Windows.Forms.Button();
            this.gbTechnical = new System.Windows.Forms.GroupBox();
            this.pnlTechnical = new System.Windows.Forms.Panel();
            this.lblEntity = new System.Windows.Forms.Label();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.txtFormId = new System.Windows.Forms.TextBox();
            this.txtAttribute = new System.Windows.Forms.TextBox();
            this.lblRecordId = new System.Windows.Forms.Label();
            this.txtEntity = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.gbSelection = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRecord = new System.Windows.Forms.TextBox();
            this.btnLookup = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.xacb = new Rappen.XTB.Helpers.Controls.XRMAttributeComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCompareSelection = new System.Windows.Forms.Button();
            this.xecb = new Rappen.XTB.Helpers.Controls.XRMEntityComboBox();
            this.diffControl1 = new Menees.Diffs.Windows.Forms.DiffControl();
            this.toolStripMenu.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.gbSpecific.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbTechnical.SuspendLayout();
            this.pnlTechnical.SuspendLayout();
            this.gbSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslSourceEnv,
            this.tslSourceEnvSelected,
            this.tslTargetEnv,
            this.tslTargetEnvSelected,
            this.tsbSetTargetEnv,
            this.toolStripSeparator1});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(1754, 38);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tslSourceEnv
            // 
            this.tslSourceEnv.Name = "tslSourceEnv";
            this.tslSourceEnv.Size = new System.Drawing.Size(70, 28);
            this.tslSourceEnv.Text = "Source:";
            // 
            // tslSourceEnvSelected
            // 
            this.tslSourceEnvSelected.ForeColor = System.Drawing.Color.Red;
            this.tslSourceEnvSelected.Name = "tslSourceEnvSelected";
            this.tslSourceEnvSelected.Size = new System.Drawing.Size(70, 28);
            this.tslSourceEnvSelected.Text = "Not set";
            // 
            // tslTargetEnv
            // 
            this.tslTargetEnv.Name = "tslTargetEnv";
            this.tslTargetEnv.Size = new System.Drawing.Size(64, 28);
            this.tslTargetEnv.Text = "Target:";
            // 
            // tslTargetEnvSelected
            // 
            this.tslTargetEnvSelected.ForeColor = System.Drawing.Color.Red;
            this.tslTargetEnvSelected.Name = "tslTargetEnvSelected";
            this.tslTargetEnvSelected.Size = new System.Drawing.Size(70, 28);
            this.tslTargetEnvSelected.Text = "Not set";
            // 
            // tsbSetTargetEnv
            // 
            this.tsbSetTargetEnv.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSetTargetEnv.Image = global::MscrmTools.ComponentComparer.Properties.Resources.lightning;
            this.tsbSetTargetEnv.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSetTargetEnv.Name = "tsbSetTargetEnv";
            this.tsbSetTargetEnv.Size = new System.Drawing.Size(34, 28);
            this.tsbSetTargetEnv.Text = "Set target environment";
            this.tsbSetTargetEnv.Click += new System.EventHandler(this.tsbSetTargetEnv_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.gbSpecific);
            this.pnlTop.Controls.Add(this.gbTechnical);
            this.pnlTop.Controls.Add(this.gbSelection);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 38);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1754, 124);
            this.pnlTop.TabIndex = 6;
            // 
            // gbSpecific
            // 
            this.gbSpecific.Controls.Add(this.panel1);
            this.gbSpecific.Controls.Add(this.btnCompareSpecific);
            this.gbSpecific.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbSpecific.Location = new System.Drawing.Point(1116, 0);
            this.gbSpecific.Name = "gbSpecific";
            this.gbSpecific.Size = new System.Drawing.Size(558, 124);
            this.gbSpecific.TabIndex = 15;
            this.gbSpecific.TabStop = false;
            this.gbSpecific.Text = "Specific mode";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btnLookupSpecific);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(455, 99);
            this.panel1.TabIndex = 10;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Security Roles",
            "Webresources"});
            this.comboBox1.Location = new System.Drawing.Point(121, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(328, 28);
            this.comboBox1.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 20);
            this.label5.TabIndex = 20;
            this.label5.Text = "Record";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(121, 36);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(275, 26);
            this.textBox1.TabIndex = 19;
            // 
            // btnLookupSpecific
            // 
            this.btnLookupSpecific.Location = new System.Drawing.Point(402, 35);
            this.btnLookupSpecific.Name = "btnLookupSpecific";
            this.btnLookupSpecific.Size = new System.Drawing.Size(47, 29);
            this.btnLookupSpecific.TabIndex = 18;
            this.btnLookupSpecific.Text = "...";
            this.btnLookupSpecific.UseVisualStyleBackColor = true;
            this.btnLookupSpecific.Enabled = false;
            this.btnLookupSpecific.Click += new System.EventHandler(this.btnLookupSpecific_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Entity";
            // 
            // btnCompareSpecific
            // 
            this.btnCompareSpecific.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCompareSpecific.Location = new System.Drawing.Point(458, 22);
            this.btnCompareSpecific.Name = "btnCompareSpecific";
            this.btnCompareSpecific.Size = new System.Drawing.Size(97, 99);
            this.btnCompareSpecific.TabIndex = 9;
            this.btnCompareSpecific.Text = "Compare";
            this.btnCompareSpecific.UseVisualStyleBackColor = true;
            this.btnCompareSpecific.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // gbTechnical
            // 
            this.gbTechnical.Controls.Add(this.pnlTechnical);
            this.gbTechnical.Controls.Add(this.btnCompare);
            this.gbTechnical.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbTechnical.Location = new System.Drawing.Point(558, 0);
            this.gbTechnical.Name = "gbTechnical";
            this.gbTechnical.Size = new System.Drawing.Size(558, 124);
            this.gbTechnical.TabIndex = 14;
            this.gbTechnical.TabStop = false;
            this.gbTechnical.Text = "Expert mode";
            // 
            // pnlTechnical
            // 
            this.pnlTechnical.Controls.Add(this.lblEntity);
            this.pnlTechnical.Controls.Add(this.lblAttribute);
            this.pnlTechnical.Controls.Add(this.txtFormId);
            this.pnlTechnical.Controls.Add(this.txtAttribute);
            this.pnlTechnical.Controls.Add(this.lblRecordId);
            this.pnlTechnical.Controls.Add(this.txtEntity);
            this.pnlTechnical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTechnical.Location = new System.Drawing.Point(3, 22);
            this.pnlTechnical.Name = "pnlTechnical";
            this.pnlTechnical.Size = new System.Drawing.Size(455, 99);
            this.pnlTechnical.TabIndex = 10;
            // 
            // lblEntity
            // 
            this.lblEntity.AutoSize = true;
            this.lblEntity.Location = new System.Drawing.Point(3, 6);
            this.lblEntity.Name = "lblEntity";
            this.lblEntity.Size = new System.Drawing.Size(49, 20);
            this.lblEntity.TabIndex = 7;
            this.lblEntity.Text = "Entity";
            // 
            // lblAttribute
            // 
            this.lblAttribute.AutoSize = true;
            this.lblAttribute.Location = new System.Drawing.Point(3, 38);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(70, 20);
            this.lblAttribute.TabIndex = 11;
            this.lblAttribute.Text = "Attribute";
            // 
            // txtFormId
            // 
            this.txtFormId.Location = new System.Drawing.Point(121, 67);
            this.txtFormId.Name = "txtFormId";
            this.txtFormId.Size = new System.Drawing.Size(331, 26);
            this.txtFormId.TabIndex = 5;
            // 
            // txtAttribute
            // 
            this.txtAttribute.Location = new System.Drawing.Point(121, 35);
            this.txtAttribute.Name = "txtAttribute";
            this.txtAttribute.Size = new System.Drawing.Size(331, 26);
            this.txtAttribute.TabIndex = 10;
            // 
            // lblRecordId
            // 
            this.lblRecordId.AutoSize = true;
            this.lblRecordId.Location = new System.Drawing.Point(3, 70);
            this.lblRecordId.Name = "lblRecordId";
            this.lblRecordId.Size = new System.Drawing.Size(79, 20);
            this.lblRecordId.TabIndex = 6;
            this.lblRecordId.Text = "Record Id";
            // 
            // txtEntity
            // 
            this.txtEntity.Location = new System.Drawing.Point(121, 3);
            this.txtEntity.Name = "txtEntity";
            this.txtEntity.Size = new System.Drawing.Size(331, 26);
            this.txtEntity.TabIndex = 8;
            // 
            // btnCompare
            // 
            this.btnCompare.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCompare.Location = new System.Drawing.Point(458, 22);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(97, 99);
            this.btnCompare.TabIndex = 9;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // gbSelection
            // 
            this.gbSelection.Controls.Add(this.label3);
            this.gbSelection.Controls.Add(this.txtRecord);
            this.gbSelection.Controls.Add(this.btnLookup);
            this.gbSelection.Controls.Add(this.label2);
            this.gbSelection.Controls.Add(this.xacb);
            this.gbSelection.Controls.Add(this.label1);
            this.gbSelection.Controls.Add(this.btnCompareSelection);
            this.gbSelection.Controls.Add(this.xecb);
            this.gbSelection.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbSelection.Location = new System.Drawing.Point(0, 0);
            this.gbSelection.Name = "gbSelection";
            this.gbSelection.Size = new System.Drawing.Size(558, 124);
            this.gbSelection.TabIndex = 13;
            this.gbSelection.TabStop = false;
            this.gbSelection.Text = "Selection mode";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "Record";
            // 
            // txtRecord
            // 
            this.txtRecord.Location = new System.Drawing.Point(121, 90);
            this.txtRecord.Name = "txtRecord";
            this.txtRecord.Size = new System.Drawing.Size(278, 26);
            this.txtRecord.TabIndex = 16;
            // 
            // btnLookup
            // 
            this.btnLookup.Location = new System.Drawing.Point(405, 88);
            this.btnLookup.Name = "btnLookup";
            this.btnLookup.Size = new System.Drawing.Size(47, 31);
            this.btnLookup.TabIndex = 15;
            this.btnLookup.Text = "...";
            this.btnLookup.UseVisualStyleBackColor = true;
            this.btnLookup.Enabled = false;
            this.btnLookup.Click += new System.EventHandler(this.btnLookup_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Attribute";
            // 
            // xacb
            // 
            this.xacb.FormattingEnabled = true;
            this.xacb.Location = new System.Drawing.Point(121, 56);
            this.xacb.Name = "xacb";
            this.xacb.Size = new System.Drawing.Size(331, 28);
            this.xacb.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Entity";
            // 
            // btnCompareSelection
            // 
            this.btnCompareSelection.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCompareSelection.Location = new System.Drawing.Point(458, 22);
            this.btnCompareSelection.Name = "btnCompareSelection";
            this.btnCompareSelection.Size = new System.Drawing.Size(97, 99);
            this.btnCompareSelection.TabIndex = 10;
            this.btnCompareSelection.Text = "Compare";
            this.btnCompareSelection.UseVisualStyleBackColor = true;
            this.btnCompareSelection.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // xecb
            // 
            this.xecb.Location = new System.Drawing.Point(121, 22);
            this.xecb.Name = "xecb";
            this.xecb.Size = new System.Drawing.Size(331, 28);
            this.xecb.TabIndex = 11;
            this.xecb.SelectedIndexChanged += new System.EventHandler(this.xecb_SelectedIndexChanged);
            // 
            // diffControl1
            // 
            this.diffControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diffControl1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.diffControl1.LineDiffHeight = 63;
            this.diffControl1.Location = new System.Drawing.Point(0, 162);
            this.diffControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.diffControl1.Name = "diffControl1";
            this.diffControl1.OverviewWidth = 44;
            this.diffControl1.ShowWhiteSpaceInLineDiff = true;
            this.diffControl1.Size = new System.Drawing.Size(1754, 733);
            this.diffControl1.TabIndex = 9;
            this.diffControl1.ViewFont = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.diffControl1);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.toolStripMenu);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(1754, 895);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.gbSpecific.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbTechnical.ResumeLayout(false);
            this.pnlTechnical.ResumeLayout(false);
            this.pnlTechnical.PerformLayout();
            this.gbSelection.ResumeLayout(false);
            this.gbSelection.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.ToolStripLabel tslSourceEnv;
        private System.Windows.Forms.ToolStripLabel tslSourceEnvSelected;
        private System.Windows.Forms.ToolStripLabel tslTargetEnv;
        private System.Windows.Forms.ToolStripLabel tslTargetEnvSelected;
        private System.Windows.Forms.ToolStripButton tsbSetTargetEnv;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.GroupBox gbSelection;
        private System.Windows.Forms.Button btnCompareSelection;
        private Rappen.XTB.Helpers.Controls.XRMEntityComboBox xecb;
        private System.Windows.Forms.Button btnLookup;
        private System.Windows.Forms.Label label2;
        private Rappen.XTB.Helpers.Controls.XRMAttributeComboBox xacb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRecord;
        private System.Windows.Forms.GroupBox gbTechnical;
        private System.Windows.Forms.Panel pnlTechnical;
        private System.Windows.Forms.Label lblEntity;
        private System.Windows.Forms.Label lblAttribute;
        private System.Windows.Forms.TextBox txtFormId;
        private System.Windows.Forms.TextBox txtAttribute;
        private System.Windows.Forms.Label lblRecordId;
        private System.Windows.Forms.TextBox txtEntity;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbSpecific;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCompareSpecific;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnLookupSpecific;
        private Menees.Diffs.Windows.Forms.DiffControl diffControl1;
    }
}
