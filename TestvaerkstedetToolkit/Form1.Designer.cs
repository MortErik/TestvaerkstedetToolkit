using System.Drawing;
using System.Windows.Forms;

namespace TestvaerkstedetToolkit
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // ═══════════════════════════════════════════════════════════
            // CONTROL DECLARATIONS
            // ═══════════════════════════════════════════════════════════

            // Mode Selection
            this.radioFKRepair = new System.Windows.Forms.RadioButton();
            this.radioXMLFKRepair = new System.Windows.Forms.RadioButton();
            this.radioXMLConversion = new System.Windows.Forms.RadioButton();

            // Shared Controls
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();

            // CSV FK REPAIR GROUP
            this.grpFKRepair = new System.Windows.Forms.GroupBox();
            this.labelParentCSV = new System.Windows.Forms.Label();
            this.txtParentCSV = new System.Windows.Forms.TextBox();
            this.btnBrowseParent = new System.Windows.Forms.Button();
            this.labelChildCSV = new System.Windows.Forms.Label();
            this.txtChildCSV = new System.Windows.Forms.TextBox();
            this.btnBrowseChild = new System.Windows.Forms.Button();
            this.labelParentCol = new System.Windows.Forms.Label();
            this.cmbParentColumn = new System.Windows.Forms.ComboBox();
            this.labelChildCol = new System.Windows.Forms.Label();
            this.cmbChildColumn = new System.Windows.Forms.ComboBox();
            this.btnAnalyzeFK = new System.Windows.Forms.Button();
            this.btnAddPrimaryKey = new System.Windows.Forms.Button();
            this.btnRemovePrimaryKey = new System.Windows.Forms.Button();
            this.pnlDynamicColumns = new System.Windows.Forms.Panel();
            this.labelDummyText = new System.Windows.Forms.Label();
            this.txtDummyText = new System.Windows.Forms.TextBox();
            this.labelMissingValues = new System.Windows.Forms.Label();
            this.lstMissingValues = new System.Windows.Forms.ListBox();
            this.lblFKStats = new System.Windows.Forms.Label();
            this.btnGenerateDummies = new System.Windows.Forms.Button();
            this.btnExportMissing = new System.Windows.Forms.Button();
            this.btnCopySelected = new System.Windows.Forms.Button();

            // XML FK REPAIR GROUP
            this.grpXMLFKRepair = new System.Windows.Forms.GroupBox();
            this.lblParentTable = new System.Windows.Forms.Label();
            this.cmbParentTable = new System.Windows.Forms.ComboBox();
            this.lblChildTable = new System.Windows.Forms.Label();
            this.cmbChildTable = new System.Windows.Forms.ComboBox();
            this.lblTableIndex = new System.Windows.Forms.Label();
            this.txtTableIndex = new System.Windows.Forms.TextBox();
            this.btnBrowseTableIndex = new System.Windows.Forms.Button();
            this.lblParentXml = new System.Windows.Forms.Label();
            this.txtParentXml = new System.Windows.Forms.TextBox();
            this.btnBrowseParentXml = new System.Windows.Forms.Button();
            this.lblChildXml = new System.Windows.Forms.Label();
            this.txtChildXml = new System.Windows.Forms.TextBox();
            this.btnBrowseChildXml = new System.Windows.Forms.Button();
            this.lblXmlMapping = new System.Windows.Forms.Label();
            this.cmbParentXmlColumns = new System.Windows.Forms.ComboBox();
            this.cmbChildXmlColumns = new System.Windows.Forms.ComboBox();
            this.btnAnalyzeXmlFK = new System.Windows.Forms.Button();
            this.btnAddXmlPrimaryKey = new System.Windows.Forms.Button();
            this.btnRemoveXmlPrimaryKey = new System.Windows.Forms.Button();
            this.lblXmlCompositeKey = new System.Windows.Forms.Label();
            this.pnlXmlDynamicColumns = new System.Windows.Forms.Panel();
            this.lblXmlFKStats = new System.Windows.Forms.Label();
            this.lstXmlMissingValues = new System.Windows.Forms.ListBox();
            this.btnGenerateFixedXml = new System.Windows.Forms.Button();
            this.lblIntegrityDesc = new System.Windows.Forms.Label();
            this.txtIntegrityDesc = new System.Windows.Forms.TextBox();
            this.btnExportXmlMissing = new System.Windows.Forms.Button();
            this.btnCopyXmlSelected = new System.Windows.Forms.Button();

            // XML CONVERSION GROUP
            this.grpXMLConversion = new System.Windows.Forms.GroupBox();
            this.richTextBoxXML = new System.Windows.Forms.RichTextBox();
            this.buttonLæsCSV = new System.Windows.Forms.Button();
            this.buttonTilføjRækker = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBoxTabel = new System.Windows.Forms.TextBox();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxStandardTekst = new System.Windows.Forms.TextBox();
            this.numericUpDownTekst = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBoxID = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label_CSVKolonner = new System.Windows.Forms.Label();

            this.grpFKRepair.SuspendLayout();
            this.grpXMLFKRepair.SuspendLayout();
            this.grpXMLConversion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTekst)).BeginInit();
            this.SuspendLayout();


            // ═══════════════════════════════════════════════════════════
            // MODE SELECTION RADIO BUTTONS
            // ═══════════════════════════════════════════════════════════

            // radioFKRepair
            this.radioFKRepair.AutoSize = true;
            this.radioFKRepair.Location = new System.Drawing.Point(12, 12);
            this.radioFKRepair.Name = "radioFKRepair";
            this.radioFKRepair.Size = new System.Drawing.Size(120, 17);
            this.radioFKRepair.TabIndex = 0;
            this.radioFKRepair.Text = "CSV FK Repair";
            this.radioFKRepair.UseVisualStyleBackColor = true;
            this.radioFKRepair.CheckedChanged += new System.EventHandler(this.radioFKRepair_CheckedChanged);

            // radioXMLFKRepair
            this.radioXMLFKRepair.AutoSize = true;
            this.radioXMLFKRepair.Checked = true;
            this.radioXMLFKRepair.Location = new System.Drawing.Point(150, 12);
            this.radioXMLFKRepair.Name = "radioXMLFKRepair";
            this.radioXMLFKRepair.Size = new System.Drawing.Size(120, 17);
            this.radioXMLFKRepair.TabIndex = 1;
            this.radioXMLFKRepair.TabStop = true;
            this.radioXMLFKRepair.Text = "XML FK Repair";
            this.radioXMLFKRepair.UseVisualStyleBackColor = true;
            this.radioXMLFKRepair.CheckedChanged += new System.EventHandler(this.radioXMLFKRepair_CheckedChanged);

            // radioXMLConversion
            this.radioXMLConversion.AutoSize = true;
            this.radioXMLConversion.Location = new System.Drawing.Point(290, 12);
            this.radioXMLConversion.Name = "radioXMLConversion";
            this.radioXMLConversion.Size = new System.Drawing.Size(150, 17);
            this.radioXMLConversion.TabIndex = 2;
            this.radioXMLConversion.Text = "CSV til XML Konvertering";
            this.radioXMLConversion.UseVisualStyleBackColor = true;
            this.radioXMLConversion.CheckedChanged += new System.EventHandler(this.radioXMLConversion_CheckedChanged);

            // ═══════════════════════════════════════════════════════════
            // SHARED CONTROLS
            // ═══════════════════════════════════════════════════════════

            // progressBar1
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 795);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1168, 23);
            this.progressBar1.TabIndex = 100;
            this.progressBar1.Visible = false;

            // ═══════════════════════════════════════════════════════════
            // CSV FK REPAIR GROUP
            // ═══════════════════════════════════════════════════════════

            this.grpFKRepair.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFKRepair.Location = new System.Drawing.Point(12, 45);
            this.grpFKRepair.Name = "grpFKRepair";
            this.grpFKRepair.Size = new System.Drawing.Size(1168, 570);
            this.grpFKRepair.TabIndex = 3;
            this.grpFKRepair.TabStop = false;
            this.grpFKRepair.Text = "CSV Foreign Key Repair";
            this.grpFKRepair.Visible = false;

            // labelParentCSV
            this.labelParentCSV.AutoSize = true;
            this.labelParentCSV.Location = new System.Drawing.Point(20, 25);
            this.labelParentCSV.Name = "labelParentCSV";
            this.labelParentCSV.Size = new System.Drawing.Size(65, 13);
            this.labelParentCSV.TabIndex = 0;
            this.labelParentCSV.Text = "Parent CSV:";

            // txtParentCSV
            this.txtParentCSV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParentCSV.Location = new System.Drawing.Point(130, 22);
            this.txtParentCSV.Name = "txtParentCSV";
            this.txtParentCSV.Size = new System.Drawing.Size(920, 20);
            this.txtParentCSV.TabIndex = 1;

            // btnBrowseParent
            this.btnBrowseParent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseParent.Location = new System.Drawing.Point(1070, 20);
            this.btnBrowseParent.Name = "btnBrowseParent";
            this.btnBrowseParent.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseParent.TabIndex = 2;
            this.btnBrowseParent.Text = "Browse...";
            this.btnBrowseParent.UseVisualStyleBackColor = true;

            // labelChildCSV
            this.labelChildCSV.AutoSize = true;
            this.labelChildCSV.Location = new System.Drawing.Point(20, 55);
            this.labelChildCSV.Name = "labelChildCSV";
            this.labelChildCSV.Size = new System.Drawing.Size(55, 13);
            this.labelChildCSV.TabIndex = 0;
            this.labelChildCSV.Text = "Child CSV:";

            // txtChildCSV
            this.txtChildCSV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChildCSV.Location = new System.Drawing.Point(130, 52);
            this.txtChildCSV.Name = "txtChildCSV";
            this.txtChildCSV.Size = new System.Drawing.Size(920, 20);
            this.txtChildCSV.TabIndex = 1;

            // btnBrowseChild
            this.btnBrowseChild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseChild.Location = new System.Drawing.Point(1070, 50);
            this.btnBrowseChild.Name = "btnBrowseChild";
            this.btnBrowseChild.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseChild.TabIndex = 2;
            this.btnBrowseChild.Text = "Browse...";
            this.btnBrowseChild.UseVisualStyleBackColor = true;

            // labelParentCol
            this.labelParentCol.AutoSize = true;
            this.labelParentCol.Location = new System.Drawing.Point(20, 95);
            this.labelParentCol.Name = "labelParentCol";
            this.labelParentCol.Size = new System.Drawing.Size(85, 13);
            this.labelParentCol.TabIndex = 0;
            this.labelParentCol.Text = "Parent Column 1:";

            // cmbParentColumn
            this.cmbParentColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParentColumn.Location = new System.Drawing.Point(130, 92);
            this.cmbParentColumn.Name = "cmbParentColumn";
            this.cmbParentColumn.Size = new System.Drawing.Size(300, 21);
            this.cmbParentColumn.TabIndex = 3;

            // labelChildCol
            this.labelChildCol.AutoSize = true;
            this.labelChildCol.Location = new System.Drawing.Point(450, 95);
            this.labelChildCol.Name = "labelChildCol";
            this.labelChildCol.Size = new System.Drawing.Size(75, 13);
            this.labelChildCol.TabIndex = 0;
            this.labelChildCol.Text = "Child Column 1:";

            // cmbChildColumn
            this.cmbChildColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChildColumn.Location = new System.Drawing.Point(550, 92);
            this.cmbChildColumn.Name = "cmbChildColumn";
            this.cmbChildColumn.Size = new System.Drawing.Size(300, 21);
            this.cmbChildColumn.TabIndex = 4;

            // btnAddPrimaryKey
            this.btnAddPrimaryKey.Location = new System.Drawing.Point(130, 125);
            this.btnAddPrimaryKey.Name = "btnAddPrimaryKey";
            this.btnAddPrimaryKey.Size = new System.Drawing.Size(100, 23);
            this.btnAddPrimaryKey.TabIndex = 5;
            this.btnAddPrimaryKey.Text = "Add Column";
            this.btnAddPrimaryKey.UseVisualStyleBackColor = true;

            // btnRemovePrimaryKey
            this.btnRemovePrimaryKey.Location = new System.Drawing.Point(240, 125);
            this.btnRemovePrimaryKey.Name = "btnRemovePrimaryKey";
            this.btnRemovePrimaryKey.Size = new System.Drawing.Size(100, 23);
            this.btnRemovePrimaryKey.TabIndex = 6;
            this.btnRemovePrimaryKey.Text = "Remove Last";
            this.btnRemovePrimaryKey.UseVisualStyleBackColor = true;
            this.btnRemovePrimaryKey.Enabled = false;

            // pnlDynamicColumns
            this.pnlDynamicColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDynamicColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDynamicColumns.Location = new System.Drawing.Point(130, 155);
            this.pnlDynamicColumns.Name = "pnlDynamicColumns";
            this.pnlDynamicColumns.Size = new System.Drawing.Size(900, 120);
            this.pnlDynamicColumns.TabIndex = 7;
            this.pnlDynamicColumns.AutoScroll = true;

            // btnAnalyzeFK
            this.btnAnalyzeFK.Location = new System.Drawing.Point(130, 290);
            this.btnAnalyzeFK.Name = "btnAnalyzeFK";
            this.btnAnalyzeFK.Size = new System.Drawing.Size(150, 30);
            this.btnAnalyzeFK.TabIndex = 8;
            this.btnAnalyzeFK.Text = "Analyze FK";
            this.btnAnalyzeFK.UseVisualStyleBackColor = true;
            this.btnAnalyzeFK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);

            // lblFKStats
            this.lblFKStats.AutoSize = true;
            this.lblFKStats.Location = new System.Drawing.Point(300, 300);
            this.lblFKStats.Name = "lblFKStats";
            this.lblFKStats.Size = new System.Drawing.Size(150, 13);
            this.lblFKStats.TabIndex = 0;
            this.lblFKStats.Text = "Klik 'Analyze FK' for at starte";
            this.lblFKStats.ForeColor = System.Drawing.Color.DarkBlue;

            // labelDummyText
            this.labelDummyText.AutoSize = true;
            this.labelDummyText.Location = new System.Drawing.Point(20, 335);
            this.labelDummyText.Name = "labelDummyText";
            this.labelDummyText.Size = new System.Drawing.Size(75, 13);
            this.labelDummyText.TabIndex = 0;
            this.labelDummyText.Text = "Dummy tekst:";

            // txtDummyText
            this.txtDummyText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDummyText.Location = new System.Drawing.Point(130, 332);
            this.txtDummyText.Name = "txtDummyText";
            this.txtDummyText.Size = new System.Drawing.Size(900, 20);
            this.txtDummyText.TabIndex = 9;
            this.txtDummyText.Text = "Betydning ukendt. Rækken er tilføjet under aflevering til arkiv, for at sikre referentiel integritet i databasen af hensyn til langtidsbevaring";

            // labelMissingValues
            this.labelMissingValues.AutoSize = true;
            this.labelMissingValues.Location = new System.Drawing.Point(20, 370);
            this.labelMissingValues.Name = "labelMissingValues";
            this.labelMissingValues.Size = new System.Drawing.Size(100, 13);
            this.labelMissingValues.TabIndex = 0;
            this.labelMissingValues.Text = "Manglende værdier:";

            // lstMissingValues
            this.lstMissingValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMissingValues.Location = new System.Drawing.Point(130, 370);
            this.lstMissingValues.Name = "lstMissingValues";
            this.lstMissingValues.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstMissingValues.Size = new System.Drawing.Size(700, 200);
            this.lstMissingValues.TabIndex = 10;

            // btnGenerateDummies
            this.btnGenerateDummies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateDummies.Location = new System.Drawing.Point(850, 370);
            this.btnGenerateDummies.Name = "btnGenerateDummies";
            this.btnGenerateDummies.Size = new System.Drawing.Size(220, 35);
            this.btnGenerateDummies.TabIndex = 11;
            this.btnGenerateDummies.Text = "Generer Nye Rækker til Parent";
            this.btnGenerateDummies.UseVisualStyleBackColor = true;
            this.btnGenerateDummies.Enabled = false;
            this.btnGenerateDummies.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerateDummies.BackColor = System.Drawing.Color.LightGreen;

            // btnExportMissing
            this.btnExportMissing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportMissing.Location = new System.Drawing.Point(869, 415);
            this.btnExportMissing.Name = "btnExportMissing";
            this.btnExportMissing.Size = new System.Drawing.Size(180, 25);
            this.btnExportMissing.TabIndex = 12;
            this.btnExportMissing.Text = "Export til fil";
            this.btnExportMissing.UseVisualStyleBackColor = true;
            this.btnExportMissing.Enabled = false;

            // btnCopySelected
            this.btnCopySelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopySelected.Location = new System.Drawing.Point(869, 450);
            this.btnCopySelected.Name = "btnCopySelected";
            this.btnCopySelected.Size = new System.Drawing.Size(180, 25);
            this.btnCopySelected.TabIndex = 13;
            this.btnCopySelected.Text = "Kopiér markerede";
            this.btnCopySelected.UseVisualStyleBackColor = true;
            this.btnCopySelected.Enabled = false;

            // Add all CSV FK controls to group
            this.grpFKRepair.Controls.Add(this.labelParentCSV);
            this.grpFKRepair.Controls.Add(this.txtParentCSV);
            this.grpFKRepair.Controls.Add(this.btnBrowseParent);
            this.grpFKRepair.Controls.Add(this.labelChildCSV);
            this.grpFKRepair.Controls.Add(this.txtChildCSV);
            this.grpFKRepair.Controls.Add(this.btnBrowseChild);
            this.grpFKRepair.Controls.Add(this.labelParentCol);
            this.grpFKRepair.Controls.Add(this.cmbParentColumn);
            this.grpFKRepair.Controls.Add(this.labelChildCol);
            this.grpFKRepair.Controls.Add(this.cmbChildColumn);
            this.grpFKRepair.Controls.Add(this.btnAddPrimaryKey);
            this.grpFKRepair.Controls.Add(this.btnRemovePrimaryKey);
            this.grpFKRepair.Controls.Add(this.pnlDynamicColumns);
            this.grpFKRepair.Controls.Add(this.btnAnalyzeFK);
            this.grpFKRepair.Controls.Add(this.lblFKStats);
            this.grpFKRepair.Controls.Add(this.labelDummyText);
            this.grpFKRepair.Controls.Add(this.txtDummyText);
            this.grpFKRepair.Controls.Add(this.labelMissingValues);
            this.grpFKRepair.Controls.Add(this.lstMissingValues);
            this.grpFKRepair.Controls.Add(this.btnGenerateDummies);
            this.grpFKRepair.Controls.Add(this.btnExportMissing);
            this.grpFKRepair.Controls.Add(this.btnCopySelected);

            // ═══════════════════════════════════════════════════════════
            // XML FK REPAIR GROUP
            // ═══════════════════════════════════════════════════════════

            this.grpXMLFKRepair.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpXMLFKRepair.Location = new System.Drawing.Point(12, 45);
            this.grpXMLFKRepair.Name = "grpXMLFKRepair";
            this.grpXMLFKRepair.Size = new System.Drawing.Size(1168, 740);
            this.grpXMLFKRepair.TabIndex = 4;
            this.grpXMLFKRepair.TabStop = false;
            this.grpXMLFKRepair.Text = "XML Foreign Key Repair (TableIndex)";
            this.grpXMLFKRepair.Visible = true;

            // ───────────────────────────────────────────────────────
            // SECTION 1: OPTIONAL TABLEINDEX
            // ───────────────────────────────────────────────────────

            // TableIndex label
            this.lblTableIndex.AutoSize = true;
            this.lblTableIndex.Location = new System.Drawing.Point(20, 25);
            this.lblTableIndex.Name = "lblTableIndex";
            this.lblTableIndex.Size = new System.Drawing.Size(120, 13);
            this.lblTableIndex.TabIndex = 0;
            this.lblTableIndex.Text = "TableIndex:";
            this.lblTableIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);

            // TableIndex textbox
            this.txtTableIndex.Location = new System.Drawing.Point(130, 25);
            this.txtTableIndex.Name = "txtTableIndex";
            this.txtTableIndex.Size = new System.Drawing.Size(800, 20);
            this.txtTableIndex.TabIndex = 1;
            this.txtTableIndex.ReadOnly = true;
            this.txtTableIndex.BackColor = System.Drawing.SystemColors.Control;
            this.txtTableIndex.ForeColor = System.Drawing.Color.Blue;

            // TableIndex browse button
            this.btnBrowseTableIndex.Location = new System.Drawing.Point(940, 23);
            this.btnBrowseTableIndex.Name = "btnBrowseTableIndex";
            this.btnBrowseTableIndex.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseTableIndex.TabIndex = 2;
            this.btnBrowseTableIndex.Text = "Browse...";
            this.btnBrowseTableIndex.UseVisualStyleBackColor = true;

            // Info label
            var lblTableIndexInfo = new System.Windows.Forms.Label();
            lblTableIndexInfo.AutoSize = false;
            lblTableIndexInfo.Location = new System.Drawing.Point(130, 60);
            lblTableIndexInfo.Size = new System.Drawing.Size(800, 30);
            lblTableIndexInfo.Text = "💡 TableIndex giver bedre kolonnenavne, datatyper og beskrivelser.\nKan springes over - programmet parser direkte fra XML.";
            lblTableIndexInfo.ForeColor = System.Drawing.Color.DarkGreen;
            lblTableIndexInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Italic);

            // Divider 1
            var separator1 = new System.Windows.Forms.Label();
            separator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            separator1.Location = new System.Drawing.Point(20, 105);
            separator1.Size = new System.Drawing.Size(1120, 2);

            // ───────────────────────────────────────────────────────
            // SECTION 2: PARENT TABLE & XML
            // ───────────────────────────────────────────────────────

            // Parent Table dropdown label
            this.lblParentTable.AutoSize = true;
            this.lblParentTable.Location = new System.Drawing.Point(20, 120);
            this.lblParentTable.Name = "lblParentTable";
            this.lblParentTable.Size = new System.Drawing.Size(120, 13);
            this.lblParentTable.TabIndex = 0;
            this.lblParentTable.Text = "Parent Table:";
            this.lblParentTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);

            // Parent Table dropdown
            this.cmbParentTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParentTable.Location = new System.Drawing.Point(130, 120);
            this.cmbParentTable.Name = "cmbParentTable";
            this.cmbParentTable.Size = new System.Drawing.Size(800, 21);
            this.cmbParentTable.TabIndex = 3;
            this.cmbParentTable.Enabled = false;

            // Parent XML Path label
            this.lblParentXml.AutoSize = true;
            this.lblParentXml.Location = new System.Drawing.Point(20, 150);
            this.lblParentXml.Name = "lblParentXml";
            this.lblParentXml.Size = new System.Drawing.Size(100, 13);
            this.lblParentXml.TabIndex = 0;
            this.lblParentXml.Text = "Parent XML Path:";

            // Parent XML textbox (auto-filled from dropdown)
            this.txtParentXml.Location = new System.Drawing.Point(130, 150);
            this.txtParentXml.Name = "txtParentXml";
            this.txtParentXml.Size = new System.Drawing.Size(800, 20);
            this.txtParentXml.TabIndex = 4;
            this.txtParentXml.ReadOnly = true;
            this.txtParentXml.BackColor = System.Drawing.SystemColors.Control;

            // Parent browse button (override)
            this.btnBrowseParentXml.Location = new System.Drawing.Point(940, 148);
            this.btnBrowseParentXml.Name = "btnBrowseParentXml";
            this.btnBrowseParentXml.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseParentXml.TabIndex = 5;
            this.btnBrowseParentXml.Text = "Browse...";
            this.btnBrowseParentXml.UseVisualStyleBackColor = true;

            // ───────────────────────────────────────────────────────
            // SECTION 3: CHILD TABLE & XML
            // ───────────────────────────────────────────────────────

            // Child Table dropdown label
            this.lblChildTable.AutoSize = true;
            this.lblChildTable.Location = new System.Drawing.Point(20, 185);
            this.lblChildTable.Name = "lblChildTable";
            this.lblChildTable.Size = new System.Drawing.Size(120, 13);
            this.lblChildTable.TabIndex = 0;
            this.lblChildTable.Text = "Child Table:";
            this.lblChildTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);

            // Child Table dropdown
            this.cmbChildTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChildTable.Location = new System.Drawing.Point(130, 185);
            this.cmbChildTable.Name = "cmbChildTable";
            this.cmbChildTable.Size = new System.Drawing.Size(800, 21);
            this.cmbChildTable.TabIndex = 6;
            this.cmbChildTable.Enabled = false;

            // Child XML Path label
            this.lblChildXml.AutoSize = true;
            this.lblChildXml.Location = new System.Drawing.Point(20, 215);
            this.lblChildXml.Name = "lblChildXml";
            this.lblChildXml.Size = new System.Drawing.Size(100, 13);
            this.lblChildXml.TabIndex = 0;
            this.lblChildXml.Text = "Child XML Path:";

            // Child XML textbox (auto-filled from dropdown)
            this.txtChildXml.Location = new System.Drawing.Point(130, 215);
            this.txtChildXml.Name = "txtChildXml";
            this.txtChildXml.Size = new System.Drawing.Size(800, 20);
            this.txtChildXml.TabIndex = 7;
            this.txtChildXml.ReadOnly = true;
            this.txtChildXml.BackColor = System.Drawing.SystemColors.Control;

            // Child browse button (override)
            this.btnBrowseChildXml.Location = new System.Drawing.Point(940, 213);
            this.btnBrowseChildXml.Name = "btnBrowseChildXml";
            this.btnBrowseChildXml.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseChildXml.TabIndex = 8;
            this.btnBrowseChildXml.Text = "Browse...";
            this.btnBrowseChildXml.UseVisualStyleBackColor = true;

            // Divider 2
            var separator2 = new System.Windows.Forms.Label();
            separator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            separator2.Location = new System.Drawing.Point(20, 250);
            separator2.Size = new System.Drawing.Size(1120, 2);

            // ───────────────────────────────────────────────────────
            // SECTION 4: FOREIGN KEY MAPPING
            // ───────────────────────────────────────────────────────

            // Mapping label
            this.lblXmlMapping.AutoSize = true;
            this.lblXmlMapping.Location = new System.Drawing.Point(20, 265);
            this.lblXmlMapping.Name = "lblXmlMapping";
            this.lblXmlMapping.Size = new System.Drawing.Size(150, 13);
            this.lblXmlMapping.TabIndex = 0;
            this.lblXmlMapping.Text = "Foreign Key Mapping (Base):";
            this.lblXmlMapping.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);

            // Parent column label
            var lblParentXmlCol = new System.Windows.Forms.Label();
            lblParentXmlCol.AutoSize = true;
            lblParentXmlCol.Location = new System.Drawing.Point(40, 285);
            lblParentXmlCol.Text = "Parent Column:";

            // Parent column combobox
            this.cmbParentXmlColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParentXmlColumns.Location = new System.Drawing.Point(160, 285);
            this.cmbParentXmlColumns.Name = "cmbParentXmlColumns";
            this.cmbParentXmlColumns.Size = new System.Drawing.Size(450, 21);
            this.cmbParentXmlColumns.TabIndex = 7;

            // Child column label
            var lblChildXmlCol = new System.Windows.Forms.Label();
            lblChildXmlCol.AutoSize = true;
            lblChildXmlCol.Location = new System.Drawing.Point(40, 315);
            lblChildXmlCol.Text = "Child Column:";

            // Child column combobox
            this.cmbChildXmlColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChildXmlColumns.Location = new System.Drawing.Point(160, 315);
            this.cmbChildXmlColumns.Name = "cmbChildXmlColumns";
            this.cmbChildXmlColumns.Size = new System.Drawing.Size(450, 21);
            this.cmbChildXmlColumns.TabIndex = 8;

            // ───────────────────────────────────────────────────────
            // SECTION 5: sammensatte PK
            // ───────────────────────────────────────────────────────

            // sammensatte PK label
            this.lblXmlCompositeKey.AutoSize = true;
            this.lblXmlCompositeKey.Location = new System.Drawing.Point(20, 350);
            this.lblXmlCompositeKey.Text = "Sammensatte PK (Optional):";
            this.lblXmlCompositeKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);

            // Add/Remove buttons
            this.btnAddXmlPrimaryKey.Location = new System.Drawing.Point(200, 348);
            this.btnAddXmlPrimaryKey.Size = new System.Drawing.Size(100, 23);
            this.btnAddXmlPrimaryKey.Text = "Add Column";
            this.btnAddXmlPrimaryKey.TabIndex = 9;
            this.btnAddXmlPrimaryKey.UseVisualStyleBackColor = true;

            this.btnRemoveXmlPrimaryKey.Location = new System.Drawing.Point(310, 348);
            this.btnRemoveXmlPrimaryKey.Size = new System.Drawing.Size(100, 23);
            this.btnRemoveXmlPrimaryKey.Text = "Remove Last";
            this.btnRemoveXmlPrimaryKey.TabIndex = 10;
            this.btnRemoveXmlPrimaryKey.UseVisualStyleBackColor = true;
            this.btnRemoveXmlPrimaryKey.Enabled = false;

            // Dynamic columns panel
            this.pnlXmlDynamicColumns.Location = new System.Drawing.Point(40, 380);
            this.pnlXmlDynamicColumns.Size = new System.Drawing.Size(970, 120);
            this.pnlXmlDynamicColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlXmlDynamicColumns.AutoScroll = true;
            this.pnlXmlDynamicColumns.TabIndex = 11;

            // Analyze button
            this.btnAnalyzeXmlFK.Location = new System.Drawing.Point(40, 515);
            this.btnAnalyzeXmlFK.Size = new System.Drawing.Size(150, 30);
            this.btnAnalyzeXmlFK.Text = "Analyze FK";
            this.btnAnalyzeXmlFK.TabIndex = 12;
            this.btnAnalyzeXmlFK.UseVisualStyleBackColor = true;
            this.btnAnalyzeXmlFK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);

            // Stats label
            this.lblXmlFKStats.AutoSize = true;
            this.lblXmlFKStats.Location = new System.Drawing.Point(200, 523);
            this.lblXmlFKStats.Text = "Klik 'Analyze FK' for at starte";
            this.lblXmlFKStats.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblXmlFKStats.TabIndex = 0;


            // ───────────────────────────────────────────────────────
            // SECTION 6: RESULTS
            // ───────────────────────────────────────────────────────

            // Results listbox
            this.lstXmlMissingValues.Location = new System.Drawing.Point(40, 560);
            this.lstXmlMissingValues.Size = new System.Drawing.Size(500, 120);
            this.lstXmlMissingValues.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstXmlMissingValues.TabIndex = 13;

            this.btnExportXmlMissing.Location = new System.Drawing.Point(560, 560);
            this.btnExportXmlMissing.Size = new System.Drawing.Size(120, 25);
            this.btnExportXmlMissing.Text = "Export til fil";
            this.btnExportXmlMissing.TabIndex = 14;
            this.btnExportXmlMissing.Enabled = false;
            this.btnExportXmlMissing.UseVisualStyleBackColor = true;

            this.btnCopyXmlSelected.Location = new System.Drawing.Point(560, 590);
            this.btnCopyXmlSelected.Size = new System.Drawing.Size(120, 25);
            this.btnCopyXmlSelected.Text = "Kopiér markerede";
            this.btnCopyXmlSelected.TabIndex = 15;
            this.btnCopyXmlSelected.Enabled = false;
            this.btnCopyXmlSelected.UseVisualStyleBackColor = true;

            // Integrity description
            this.lblIntegrityDesc.AutoSize = true;
            this.lblIntegrityDesc.Location = new System.Drawing.Point(560, 625);
            this.lblIntegrityDesc.Text = "Integrity beskrivelse:";
            this.lblIntegrityDesc.TabIndex = 0;

            this.txtIntegrityDesc.Location = new System.Drawing.Point(560, 642);
            this.txtIntegrityDesc.Size = new System.Drawing.Size(600, 20);
            this.txtIntegrityDesc.Multiline = false;
            this.txtIntegrityDesc.TabIndex = 16;
            this.txtIntegrityDesc.Text = "Betydning ukendt. Rækken er tilføjet under aflevering til arkiv, for at sikre referentiel integritet i databasen af hensyn til langtidsbevaring";

            // Generate button
            this.btnGenerateFixedXml.Location = new System.Drawing.Point(560, 667);
            this.btnGenerateFixedXml.Size = new System.Drawing.Size(220, 35);
            this.btnGenerateFixedXml.Text = "Generer Nye Rækker til Parent";
            this.btnGenerateFixedXml.TabIndex = 17;
            this.btnGenerateFixedXml.UseVisualStyleBackColor = true;
            this.btnGenerateFixedXml.Enabled = false;
            this.btnGenerateFixedXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerateFixedXml.BackColor = System.Drawing.Color.LightGreen;

            // Add all XML FK controls to group
            // Add controls to grpXMLFKRepair
            this.grpXMLFKRepair.Controls.Add(this.lblTableIndex);
            this.grpXMLFKRepair.Controls.Add(this.txtTableIndex);
            this.grpXMLFKRepair.Controls.Add(this.btnBrowseTableIndex);
            this.grpXMLFKRepair.Controls.Add(lblTableIndexInfo);
            this.grpXMLFKRepair.Controls.Add(separator1);

            // Parent section
            this.grpXMLFKRepair.Controls.Add(this.lblParentTable);
            this.grpXMLFKRepair.Controls.Add(this.cmbParentTable);
            this.grpXMLFKRepair.Controls.Add(this.lblParentXml);
            this.grpXMLFKRepair.Controls.Add(this.txtParentXml);
            this.grpXMLFKRepair.Controls.Add(this.btnBrowseParentXml);

            // Child section
            this.grpXMLFKRepair.Controls.Add(this.lblChildTable);
            this.grpXMLFKRepair.Controls.Add(this.cmbChildTable);
            this.grpXMLFKRepair.Controls.Add(this.lblChildXml);
            this.grpXMLFKRepair.Controls.Add(this.txtChildXml);
            this.grpXMLFKRepair.Controls.Add(this.btnBrowseChildXml);

            this.grpXMLFKRepair.Controls.Add(separator2);
            this.grpXMLFKRepair.Controls.Add(this.lblXmlMapping);
            this.grpXMLFKRepair.Controls.Add(lblParentXmlCol);
            this.grpXMLFKRepair.Controls.Add(this.cmbParentXmlColumns);
            this.grpXMLFKRepair.Controls.Add(lblChildXmlCol);
            this.grpXMLFKRepair.Controls.Add(this.cmbChildXmlColumns);
            this.grpXMLFKRepair.Controls.Add(this.lblXmlCompositeKey);
            this.grpXMLFKRepair.Controls.Add(this.btnAddXmlPrimaryKey);
            this.grpXMLFKRepair.Controls.Add(this.btnRemoveXmlPrimaryKey);
            this.grpXMLFKRepair.Controls.Add(this.pnlXmlDynamicColumns);
            this.grpXMLFKRepair.Controls.Add(this.btnAnalyzeXmlFK);
            this.grpXMLFKRepair.Controls.Add(this.lblXmlFKStats);
            this.grpXMLFKRepair.Controls.Add(this.lstXmlMissingValues);
            this.grpXMLFKRepair.Controls.Add(this.btnExportXmlMissing);
            this.grpXMLFKRepair.Controls.Add(this.btnCopyXmlSelected);
            this.grpXMLFKRepair.Controls.Add(this.lblIntegrityDesc);
            this.grpXMLFKRepair.Controls.Add(this.txtIntegrityDesc);
            this.grpXMLFKRepair.Controls.Add(this.btnGenerateFixedXml);

            // ═══════════════════════════════════════════════════════════
            // XML CONVERSION GROUP - ORIGINAL LAYOUT
            // ═══════════════════════════════════════════════════════════

            this.grpXMLConversion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpXMLConversion.Location = new System.Drawing.Point(12, 45);
            this.grpXMLConversion.Name = "grpXMLConversion";
            this.grpXMLConversion.Size = new System.Drawing.Size(1168, 570);
            this.grpXMLConversion.TabIndex = 5;
            this.grpXMLConversion.TabStop = false;
            this.grpXMLConversion.Text = "CSV til XML Konvertering";
            this.grpXMLConversion.Visible = false;

            // button4 - "Vælg tabel ..."
            this.button4.Location = new System.Drawing.Point(11, 13);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(106, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = "Vælg tabel ...";
            this.button4.UseVisualStyleBackColor = true;

            // textBoxTabel
            this.textBoxTabel.Location = new System.Drawing.Point(123, 13);
            this.textBoxTabel.Name = "textBoxTabel";
            this.textBoxTabel.Size = new System.Drawing.Size(405, 20);
            this.textBoxTabel.TabIndex = 1;

            // buttonLæsCSV - "Indlæs CSV"
            this.buttonLæsCSV.Enabled = false;
            this.buttonLæsCSV.Location = new System.Drawing.Point(11, 45);
            this.buttonLæsCSV.Name = "buttonLæsCSV";
            this.buttonLæsCSV.Size = new System.Drawing.Size(106, 23);
            this.buttonLæsCSV.TabIndex = 2;
            this.buttonLæsCSV.Text = "Indlæs CSV";
            this.buttonLæsCSV.UseVisualStyleBackColor = true;

            // textBoxID
            this.textBoxID.Location = new System.Drawing.Point(123, 47);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(405, 20);
            this.textBoxID.TabIndex = 3;

            // label1 - "Standard tekst"
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Standard tekst";

            // textBoxStandardTekst
            this.textBoxStandardTekst.Location = new System.Drawing.Point(123, 79);
            this.textBoxStandardTekst.Name = "textBoxStandardTekst";
            this.textBoxStandardTekst.Size = new System.Drawing.Size(405, 20);
            this.textBoxStandardTekst.TabIndex = 4;
            this.textBoxStandardTekst.Text = ".... INDTAST TEKST ....";

            // label3 - "Indsæt i kolonne"
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(534, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Indsæt  i kolonne";

            // numericUpDownTekst
            this.numericUpDownTekst.Location = new System.Drawing.Point(629, 79);
            this.numericUpDownTekst.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericUpDownTekst.Name = "numericUpDownTekst";
            this.numericUpDownTekst.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownTekst.TabIndex = 5;
            this.numericUpDownTekst.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // label_CSVKolonner - "CSV1 ...."
            this.label_CSVKolonner.AutoSize = true;
            this.label_CSVKolonner.Location = new System.Drawing.Point(120, 116);
            this.label_CSVKolonner.Name = "label_CSVKolonner";
            this.label_CSVKolonner.Size = new System.Drawing.Size(49, 13);
            this.label_CSVKolonner.TabIndex = 0;
            this.label_CSVKolonner.Text = "CSV1 ....";

            // label2 - "Vælg tabelkolonne"
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Vælg tabelkolonne";

            // flowLayoutPanel1
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(123, 132);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(405, 43);
            this.flowLayoutPanel1.TabIndex = 6;

            // label4 - "CSV"
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(120, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "CSV";

            // label5 - "Tabel (kun delvist indlæst)"
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(381, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Tabel (kun delvist indlæst)";

            // richTextBoxID - CSV preview (venstre)
            this.richTextBoxID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxID.Location = new System.Drawing.Point(123, 216);
            this.richTextBoxID.Name = "richTextBoxID";
            this.richTextBoxID.Size = new System.Drawing.Size(246, 304);
            this.richTextBoxID.TabIndex = 7;
            this.richTextBoxID.Text = "";
            this.richTextBoxID.WordWrap = false;

            // richTextBoxXML - XML preview (højre)
            this.richTextBoxXML.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxXML.Location = new System.Drawing.Point(384, 216);
            this.richTextBoxXML.Name = "richTextBoxXML";
            this.richTextBoxXML.Size = new System.Drawing.Size(766, 304);
            this.richTextBoxXML.TabIndex = 8;
            this.richTextBoxXML.Text = "";
            this.richTextBoxXML.WordWrap = false;

            // textBox1 - Instruktioner
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(709, 13);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(214, 86);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "HUSK!  Dette program arbejder kun på valide XML filer og skemaer som er linie baseret (pretty print)  ";

            // buttonTilføjRækker - "Tilføj nye rækker"
            this.buttonTilføjRækker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTilføjRækker.Enabled = false;
            this.buttonTilføjRækker.Location = new System.Drawing.Point(1050, 189);
            this.buttonTilføjRækker.Name = "buttonTilføjRækker";
            this.buttonTilføjRækker.Size = new System.Drawing.Size(100, 23);
            this.buttonTilføjRækker.TabIndex = 10;
            this.buttonTilføjRækker.Text = "Tilføj nye rækker";
            this.buttonTilføjRækker.UseVisualStyleBackColor = true;

            // Add all controls to grpXMLConversion
            this.grpXMLConversion.Controls.Add(this.button4);
            this.grpXMLConversion.Controls.Add(this.textBoxTabel);
            this.grpXMLConversion.Controls.Add(this.buttonLæsCSV);
            this.grpXMLConversion.Controls.Add(this.textBoxID);
            this.grpXMLConversion.Controls.Add(this.label1);
            this.grpXMLConversion.Controls.Add(this.textBoxStandardTekst);
            this.grpXMLConversion.Controls.Add(this.label3);
            this.grpXMLConversion.Controls.Add(this.numericUpDownTekst);
            this.grpXMLConversion.Controls.Add(this.label_CSVKolonner);
            this.grpXMLConversion.Controls.Add(this.label2);
            this.grpXMLConversion.Controls.Add(this.flowLayoutPanel1);
            this.grpXMLConversion.Controls.Add(this.label4);
            this.grpXMLConversion.Controls.Add(this.label5);
            this.grpXMLConversion.Controls.Add(this.richTextBoxID);
            this.grpXMLConversion.Controls.Add(this.richTextBoxXML);
            this.grpXMLConversion.Controls.Add(this.textBox1);
            this.grpXMLConversion.Controls.Add(this.buttonTilføjRækker);

            // ═══════════════════════════════════════════════════════════
            // FORM1 MAIN CONFIGURATION
            // ═══════════════════════════════════════════════════════════

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 830);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.radioFKRepair);
            this.Controls.Add(this.radioXMLFKRepair);
            this.Controls.Add(this.radioXMLConversion);
            this.Controls.Add(this.grpFKRepair);
            this.Controls.Add(this.grpXMLFKRepair);
            this.Controls.Add(this.grpXMLConversion);
            this.MinimumSize = new System.Drawing.Size(1216, 869);
            this.Name = "Form1";
            this.Text = "Data Processing Tool - XML FK Repair Mode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.grpFKRepair.ResumeLayout(false);
            this.grpFKRepair.PerformLayout();
            this.grpXMLFKRepair.ResumeLayout(false);
            this.grpXMLFKRepair.PerformLayout();
            this.grpXMLConversion.ResumeLayout(false);
            this.grpXMLConversion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTekst)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Control Declarations

        // Mode Selection
        private System.Windows.Forms.RadioButton radioFKRepair;
        private System.Windows.Forms.RadioButton radioXMLFKRepair;
        private System.Windows.Forms.RadioButton radioXMLConversion;

        // Shared
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ProgressBar progressBar1;

        // CSV FK Repair
        private System.Windows.Forms.GroupBox grpFKRepair;
        private System.Windows.Forms.Label labelParentCSV;
        private System.Windows.Forms.TextBox txtParentCSV;
        private System.Windows.Forms.Button btnBrowseParent;
        private System.Windows.Forms.Label labelChildCSV;
        private System.Windows.Forms.TextBox txtChildCSV;
        private System.Windows.Forms.Button btnBrowseChild;
        private System.Windows.Forms.Label labelParentCol;
        private System.Windows.Forms.ComboBox cmbParentColumn;
        private System.Windows.Forms.Label labelChildCol;
        private System.Windows.Forms.ComboBox cmbChildColumn;
        private System.Windows.Forms.Button btnAnalyzeFK;
        private System.Windows.Forms.Button btnAddPrimaryKey;
        private System.Windows.Forms.Button btnRemovePrimaryKey;
        private System.Windows.Forms.Panel pnlDynamicColumns;
        private System.Windows.Forms.Label labelDummyText;
        private System.Windows.Forms.TextBox txtDummyText;
        private System.Windows.Forms.Label labelMissingValues;
        private System.Windows.Forms.ListBox lstMissingValues;
        private System.Windows.Forms.Label lblFKStats;
        private System.Windows.Forms.Button btnGenerateDummies;
        private System.Windows.Forms.Button btnExportMissing;
        private System.Windows.Forms.Button btnCopySelected;

        // XML FK REPAIR
        private System.Windows.Forms.GroupBox grpXMLFKRepair;
        private System.Windows.Forms.Label lblParentTable;
        private System.Windows.Forms.ComboBox cmbParentTable;
        private System.Windows.Forms.Label lblChildTable;
        private System.Windows.Forms.ComboBox cmbChildTable;
        private System.Windows.Forms.Label lblTableIndex;
        private System.Windows.Forms.TextBox txtTableIndex;
        private System.Windows.Forms.Button btnBrowseTableIndex;
        private System.Windows.Forms.Label lblParentXml;
        private System.Windows.Forms.TextBox txtParentXml;
        private System.Windows.Forms.Button btnBrowseParentXml;
        private System.Windows.Forms.Label lblChildXml;
        private System.Windows.Forms.TextBox txtChildXml;
        private System.Windows.Forms.Button btnBrowseChildXml;
        private System.Windows.Forms.Label lblXmlMapping;
        private System.Windows.Forms.ComboBox cmbParentXmlColumns;
        private System.Windows.Forms.ComboBox cmbChildXmlColumns;
        private System.Windows.Forms.Button btnAnalyzeXmlFK;
        private System.Windows.Forms.Button btnAddXmlPrimaryKey;
        private System.Windows.Forms.Button btnRemoveXmlPrimaryKey;
        private System.Windows.Forms.Label lblXmlCompositeKey;
        private System.Windows.Forms.Panel pnlXmlDynamicColumns;
        private System.Windows.Forms.Label lblXmlFKStats;
        private System.Windows.Forms.ListBox lstXmlMissingValues;
        private System.Windows.Forms.Button btnGenerateFixedXml;
        private System.Windows.Forms.Label lblIntegrityDesc;
        private System.Windows.Forms.TextBox txtIntegrityDesc;
        private System.Windows.Forms.Button btnExportXmlMissing;
        private System.Windows.Forms.Button btnCopyXmlSelected;

        // XML Conversion
        private System.Windows.Forms.GroupBox grpXMLConversion;
        private System.Windows.Forms.RichTextBox richTextBoxXML;
        private System.Windows.Forms.Button buttonLæsCSV;
        private System.Windows.Forms.Button buttonTilføjRækker;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBoxTabel;
        private System.Windows.Forms.TextBox textBoxID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxStandardTekst;
        private System.Windows.Forms.NumericUpDown numericUpDownTekst;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBoxID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label_CSVKolonner;

        #endregion
    }
}