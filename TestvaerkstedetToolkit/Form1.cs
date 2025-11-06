using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using TestvaerkstedetToolkit.Utilities;
using TestvaerkstedetToolkit.Models;

namespace TestvaerkstedetToolkit
{
    public partial class Form1 : Form
    {
        #region Private Classes

        /// <summary>
        /// Repræsenterer et kolonne par til sammensatte PK support
        /// </summary>
        private class ColumnPair
        {
            public ComboBox ParentComboBox { get; set; }
            public ComboBox ChildComboBox { get; set; }
            public Label ParentLabel { get; set; }
            public Label ChildLabel { get; set; }
            public int PairNumber { get; set; }
        }

        /// <summary>
        /// XML schema kolonne information
        /// </summary>
        private class col
        {
            public int colNr = 0;
            public string dataType = "";
            public string nillable = "";
        }

        #endregion

        #region Dashboard Integration

        /// <summary>
        /// Tool mode enum for dashboard integration
        /// </summary>
        public enum ToolMode
        {
            CSVFKRepair,
            XMLFKRepair,
            XMLConversion
        }

        /// <summary>
        /// Indikerer hvis form køre i single mode (fra dashboard)
        /// </summary>
        private bool isSingleMode = false;
        private ToolMode currentMode;

        /// <summary>
        /// sætter form til single mode for specifik tool (kaldt fra dashboard)
        /// </summary>
        public void SetMode(ToolMode mode)
        {
            isSingleMode = true;
            currentMode = mode;

            // Skjul radio buttons i single mode
            HideRadioButtons();

            // Sæt den rigtige mode
            switch (mode)
            {
                case ToolMode.CSVFKRepair:
                    radioFKRepair.Checked = true;
                    break;
                case ToolMode.XMLFKRepair:
                    radioXMLFKRepair.Checked = true;
                    break;
                case ToolMode.XMLConversion:
                    radioXMLConversion.Checked = true;
                    break;
            }

            // Trigger mode change
            SetWorkMode();

            // Opdater window title
            UpdateWindowTitle();
        }

        /// <summary>
        /// Skjul radio buttons og justér layout for back button
        /// </summary>
        private void HideRadioButtons()
        {
            // Skjul alle radio buttons
            radioFKRepair.Visible = false;
            radioXMLFKRepair.Visible = false;
            radioXMLConversion.Visible = false;

            const int newTopPosition = 55;
            const int originalTop = 45;
            int adjustment = newTopPosition - originalTop; // +10px

            grpFKRepair.Location = new Point(grpFKRepair.Location.X, newTopPosition);
            grpXMLFKRepair.Location = new Point(grpXMLFKRepair.Location.X, newTopPosition);
            grpXMLConversion.Location = new Point(grpXMLConversion.Location.X, newTopPosition);

            grpFKRepair.Height -= adjustment;
            grpXMLFKRepair.Height -= adjustment;
            grpXMLConversion.Height -= adjustment;

            // Progress bar forbliver samme position (den er i bunden)
        }

        /// <summary>
        /// Opdater window title baseret på mode
        /// </summary>
        private void UpdateWindowTitle()
        {
            if (!isSingleMode)
            {
                this.Text = "Data Processing Tool";
                return;
            }

            switch (currentMode)
            {
                case ToolMode.CSVFKRepair:
                    this.Text = "Foreign Key Repair - CSV";
                    break;
                case ToolMode.XMLFKRepair:
                    this.Text = "Foreign Key Repair - XML";
                    break;
                case ToolMode.XMLConversion:
                    this.Text = "CSV til XML Konvertering";
                    break;
                default:
                    this.Text = "Data Processing Tool";
                    break;
            }
        }

        // Modificer form closing behavior for dashboard integration
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Cleanup temp filer før lukning
            CleanupTempFiles();

            base.OnFormClosing(e);
        }

        #endregion

        #region XML FK Repair Public fields

        /// <summary>
        /// Ny XML FK Repair klasse for at håndtere XML-baseret foreign key reparation
        /// </summary>
        private class XmlFKRepair
        {
            public string ParentXmlPath { get; set; }
            public string ChildXmlPath { get; set; }
            public string TableIndexPath { get; set; }  // OPTIONAL for metadata
            public List<string> ParentKeyColumns { get; set; } = new List<string>();
            public List<string> ChildKeyColumns { get; set; } = new List<string>();
            public string IntegrityErrorDescription { get; set; } = "Betydning ukendt. Rækken er tilføjet under aflevering til arkiv, for at sikre referentiel integritet i databasen af hensyn til langtidsbevaring";

            // Optional metadata from tableIndex
            public TableIndexEntry ParentTableEntry { get; set; }
            public TableIndexEntry ChildTableEntry { get; set; }
            public string AvidRootPath { get; set; }
        }

        /// <summary>
        /// XSD kolonne information udvidet
        /// </summary>
        private class XsdColumn
        {
            public string Name { get; set; }
            public string DataType { get; set; }
            public bool IsNillable { get; set; }
            public string DefaultValue { get; set; }
            public int Position { get; set; }
        }

        #endregion

        #region Fields

        // XML conversion fields (eksisterende)
        int currentRowCount = 0;
        Dictionary<int, string> xmlElementer = new Dictionary<int, string>();
        List<col> colList = new List<col>();
        List<NumericUpDown> numList = new List<NumericUpDown>();

        // FK repair fields
        private List<ColumnPair> columnPairs = new List<ColumnPair>();
        private int nextPairNumber = 2;
        private string currentMissingValuesFile = null; // Temp fil for missing values
        private XmlFKRepair currentXmlRepair = new XmlFKRepair();

        // XML FK repair sammensatte PK tracking
        private List<ColumnPair> xmlColumnPairs = new List<ColumnPair>();
        private int nextXmlPairNumber = 2;
        private List<XmlColumnInfo> currentParentXmlColumns = null;
        private List<XmlColumnInfo> currentChildXmlColumns = null;

        #endregion

        #region Constructor

        public Form1()
        {
            InitializeComponent();
            SetupCsvFKRepairEventHandlers();
            SetupXmlFKRepairEventHandlers();
            SetupXmlConversionEventHandlers();
            SetupListBoxContextMenu();
            SetupXmlListBoxContextMenu();
            InitializeDefaultColumnPair();
            SetWorkMode();

            this.FormClosed += (s, e) => CleanupTempFiles();
        }

        /// <summary>
        /// Ryd op i temp filer
        /// </summary>
        private void CleanupTempFiles()
        {
            if (!string.IsNullOrEmpty(currentMissingValuesFile) && File.Exists(currentMissingValuesFile))
            {
                try { File.Delete(currentMissingValuesFile); } catch { }
            }
        }

        #endregion

        #region Mode Switching

        /// <summary>
        /// Skifter mellem FK Repair og XML Conversion modes
        /// </summary>
        private void SetWorkMode()
        {
            if (radioFKRepair.Checked)
            {
                grpFKRepair.Visible = true;
                grpXMLFKRepair.Visible = false;
                grpXMLConversion.Visible = false;
                this.Text = "Data Processing Tool - CSV FK Repair Mode";
            }
            else if (radioXMLFKRepair.Checked)
            {
                grpFKRepair.Visible = false;
                grpXMLFKRepair.Visible = true;
                grpXMLConversion.Visible = false;
                this.Text = "Data Processing Tool - XML FK Repair Mode";
            }
            else
            {
                grpFKRepair.Visible = false;
                grpXMLFKRepair.Visible = false;
                grpXMLConversion.Visible = true;
                this.Text = "Data Processing Tool - XML Conversion Mode";
            }

            // Update title if in single mode
            if (isSingleMode)
            {
                UpdateWindowTitle();
            }
        }

        /// <summary>
        /// Setup event handlers for XML FK Repair controls
        /// </summary>
        private void SetupXmlFKRepairEventHandlers()
        {
            try
            {
                // Setup event handlers MED NULL CHECKS
                if (btnBrowseTableIndex != null)
                    btnBrowseTableIndex.Click += BtnBrowseTableIndex_Click;

                if (cmbParentTable != null)
                    cmbParentTable.SelectedIndexChanged += CmbParentTable_SelectedIndexChanged;

                if (cmbChildTable != null)
                    cmbChildTable.SelectedIndexChanged += CmbChildTable_SelectedIndexChanged;

                if (btnBrowseParentXml != null)
                    btnBrowseParentXml.Click += BtnBrowseParentXml_Click;

                if (btnBrowseChildXml != null)
                    btnBrowseChildXml.Click += BtnBrowseChildXml_Click;

                if (btnAnalyzeXmlFK != null)
                    btnAnalyzeXmlFK.Click += BtnAnalyzeXmlFK_Click;

                if (btnGenerateFixedXml != null)
                    btnGenerateFixedXml.Click += BtnGenerateFixedXml_Click;

                if (btnAddXmlPrimaryKey != null)
                    btnAddXmlPrimaryKey.Click += BtnAddXmlPrimaryKey_Click;

                if (btnRemoveXmlPrimaryKey != null)
                    btnRemoveXmlPrimaryKey.Click += BtnRemoveXmlPrimaryKey_Click;

                // Export/Copy funktionalitet
                if (btnCopyXmlSelected != null)
                    btnCopyXmlSelected.Click += BtnCopyXmlSelected_Click;

                if (btnExportXmlMissing != null)
                    btnExportXmlMissing.Click += BtnExportXmlMissing_Click;

                if (lstXmlMissingValues != null)
                    lstXmlMissingValues.KeyDown += LstXmlMissingValues_KeyDown;

                // Setup initial beskrivelse text
                if (txtIntegrityDesc != null && string.IsNullOrEmpty(txtIntegrityDesc.Text))
                    txtIntegrityDesc.Text = currentXmlRepair.IntegrityErrorDescription;

                System.Diagnostics.Debug.WriteLine("[Setup] XML FK Repair event handlers wired");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SetupXmlFKRepairEventHandlers error: {ex.Message}");
            }
        }

        /// <summary>
        /// Setup event handlers for CSV FK Repair controls
        /// </summary>
        private void SetupCsvFKRepairEventHandlers()
        {
            try
            {
                // Browse buttons
                if (btnBrowseParent != null)
                    btnBrowseParent.Click += btnBrowseParent_Click;

                if (btnBrowseChild != null)
                    btnBrowseChild.Click += btnBrowseChild_Click;

                // sammensatte PK buttons
                if (btnAddPrimaryKey != null)
                    btnAddPrimaryKey.Click += btnAddPrimaryKey_Click;

                if (btnRemovePrimaryKey != null)
                    btnRemovePrimaryKey.Click += btnRemovePrimaryKey_Click;

                // Analysis button
                if (btnAnalyzeFK != null)
                    btnAnalyzeFK.Click += btnAnalyzeFK_Click;

                // Generate/Export buttons
                if (btnGenerateDummies != null)
                    btnGenerateDummies.Click += btnGenerateDummies_Click;

                if (btnCopySelected != null)
                    btnCopySelected.Click += btnCopySelected_Click;

                if (btnExportMissing != null)
                    btnExportMissing.Click += btnExportMissing_Click;

                // Keyboard shortcut for listbox
                if (lstMissingValues != null)
                    lstMissingValues.KeyDown += (s, e) =>
                    {
                        if (e.Control && e.KeyCode == Keys.C)
                        {
                            btnCopySelected_Click(s, new EventArgs());
                            e.Handled = true;
                        }
                    };

                System.Diagnostics.Debug.WriteLine("[Setup] CSV FK Repair event handlers wired");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SetupCsvFKRepairEventHandlers error: {ex.Message}");
            }
        }

        /// <summary>
        /// Setup event handlers for XML Conversion controls
        /// </summary>
        private void SetupXmlConversionEventHandlers()
        {
            try
            {
                // Map button handlers
                if (buttonLæsCSV != null)
                    buttonLæsCSV.Click += button1_Click;  // Læs CSV handler

                if (buttonTilføjRækker != null)
                    buttonTilføjRækker.Click += button3_Click;  // Tilføj Rækker handler

                if (button4 != null)
                    button4.Click += button4_Click;  // Gem XML handler

                System.Diagnostics.Debug.WriteLine("[Setup] XML Conversion event handlers wired");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SetupXmlConversionEventHandlers error: {ex.Message}");
            }
        }

        private void radioFKRepair_CheckedChanged(object sender, EventArgs e)
        {
            SetWorkMode();
        }

        private void radioXMLConversion_CheckedChanged(object sender, EventArgs e)
        {
            SetWorkMode();
        }

        private void radioXMLFKRepair_CheckedChanged(object sender, EventArgs e)
        {
            SetWorkMode();
        }

        #endregion

        #region XML FK Repair - Refactored Without XSD

        /// <summary>
        /// XML kolonne information parsed direkte fra XML
        /// </summary>
        private class XmlColumnInfo
        {
            public string Name { get; set; }           // c1, c2, etc.
            public int Position { get; set; }
            public bool IsNillable { get; set; }

            // Optional fra tableIndex
            public string DisplayName { get; set; }    // barn_id, koen, etc.
            public string DataType { get; set; }       // VARCHAR(5), DECIMAL, etc.
            public string Description { get; set; }
        }

        #endregion

        #region XML FK Repair - Browse Events

        /// <summary>
        /// Browse for parent XML file
        /// </summary>
        private void BtnBrowseParentXml_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML filer|*.xml|Alle filer|*.*";
            openFileDialog1.Title = "Vælg Parent XML fil";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentXmlRepair.ParentXmlPath = openFileDialog1.FileName;
                txtParentXml.Text = openFileDialog1.FileName;

                // Hvis tableIndex er loaded, find metadata
                if (!string.IsNullOrEmpty(currentXmlRepair.TableIndexPath))
                {
                    try
                    {
                        var tables = TableIndexParser.ParseTableIndex(currentXmlRepair.TableIndexPath);
                        currentXmlRepair.ParentTableEntry = FindTableEntryByXmlPath(tables, currentXmlRepair.ParentXmlPath);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Could not enrich with tableIndex: {ex.Message}");
                    }
                }

                LoadXmlColumns(currentXmlRepair.ParentXmlPath, cmbParentXmlColumns, true);
            }
        }

        /// <summary>
        /// Browse for child XML file
        /// </summary>
        private void BtnBrowseChildXml_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML filer|*.xml|Alle filer|*.*";
            openFileDialog1.Title = "Vælg Child XML fil";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentXmlRepair.ChildXmlPath = openFileDialog1.FileName;
                txtChildXml.Text = openFileDialog1.FileName;

                // Hvis tableIndex er loaded, find metadata
                if (!string.IsNullOrEmpty(currentXmlRepair.TableIndexPath))
                {
                    try
                    {
                        var tables = TableIndexParser.ParseTableIndex(currentXmlRepair.TableIndexPath);
                        currentXmlRepair.ChildTableEntry = FindTableEntryByXmlPath(tables, currentXmlRepair.ChildXmlPath);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Could not enrich with tableIndex: {ex.Message}");
                    }
                }

                LoadXmlColumns(currentXmlRepair.ChildXmlPath, cmbChildXmlColumns, false);
            }
        }

        /// <summary>
        /// Browse for tableIndex.xml
        /// </summary>
        private void BtnBrowseTableIndex_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML filer|*.xml|Alle filer|*.*";
            openFileDialog1.Title = "Vælg tableIndex.xml fil";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string tableIndexPath = openFileDialog1.FileName;

                    // 1. DETECT AVID ROOT
                    string tableIndexDir = Path.GetDirectoryName(tableIndexPath);  // C:\...\Indices
                    string avidRoot = Path.GetDirectoryName(tableIndexDir);        // C:\AVID.SA.12345.1

                    System.Diagnostics.Debug.WriteLine($"[TableIndex] Path: {tableIndexPath}");
                    System.Diagnostics.Debug.WriteLine($"[TableIndex] AVID Root: {avidRoot}");

                    // 2. PARSE TABLEINDEX
                    var tables = TableIndexParser.ParseTableIndex(tableIndexPath);
                    System.Diagnostics.Debug.WriteLine($"[TableIndex] Parsed {tables.Count} tables");

                    // 3. SAVE TO STATE
                    currentXmlRepair.TableIndexPath = tableIndexPath;
                    currentXmlRepair.AvidRootPath = avidRoot;
                    txtTableIndex.Text = tableIndexPath;

                    // 4. POPULATE DROPDOWNS
                    cmbParentTable.Items.Clear();
                    cmbChildTable.Items.Clear();
                    PopulateTableDropdowns(tables);

                    // 5. ENABLE DROPDOWNS
                    cmbParentTable.Enabled = true;
                    cmbChildTable.Enabled = true;

                    System.Diagnostics.Debug.WriteLine($"[TableIndex] Dropdowns populated and enabled");

                    // 6. OPTIONAL: Hvis Parent/Child XML allerede valgt via browse, enrich metadata
                    if (!string.IsNullOrEmpty(currentXmlRepair.ParentXmlPath))
                    {
                        currentXmlRepair.ParentTableEntry = FindTableEntryByXmlPath(tables, currentXmlRepair.ParentXmlPath);
                        LoadXmlColumns(currentXmlRepair.ParentXmlPath, cmbParentXmlColumns, true);
                        System.Diagnostics.Debug.WriteLine("[TableIndex] Re-enriched parent columns");
                    }

                    if (!string.IsNullOrEmpty(currentXmlRepair.ChildXmlPath))
                    {
                        currentXmlRepair.ChildTableEntry = FindTableEntryByXmlPath(tables, currentXmlRepair.ChildXmlPath);
                        LoadXmlColumns(currentXmlRepair.ChildXmlPath, cmbChildXmlColumns, false);
                        System.Diagnostics.Debug.WriteLine("[TableIndex] Re-enriched child columns");
                    }

                    MessageBox.Show($"TableIndex indlæst: {tables.Count} tabeller fundet\n\nVælg Parent og Child tabel fra dropdowns",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fejl ved indlæsning af tableIndex: {ex.Message}", "Fejl",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    currentXmlRepair.TableIndexPath = null;
                    currentXmlRepair.AvidRootPath = null;
                    txtTableIndex.Text = "";

                    cmbParentTable.Enabled = false;
                    cmbChildTable.Enabled = false;
                }
            }
        }

        private void PopulateTableDropdowns(List<TableIndexEntry> tables)
        {
            foreach (var table in tables)
            {
                // Format: "Forældre (table6)"
                string displayText = $"{table.Name} ({table.Folder})";

                var item = new TableComboBoxItem
                {
                    DisplayText = displayText,
                    TableEntry = table
                };

                cmbParentTable.Items.Add(item);
                cmbChildTable.Items.Add(item);
            }
        }

        private void CmbParentTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbParentTable.SelectedItem == null)
                return;

            var selectedItem = (TableComboBoxItem)cmbParentTable.SelectedItem;
            var tableEntry = selectedItem.TableEntry;

            System.Diagnostics.Debug.WriteLine($"[Parent Dropdown] Selected: {tableEntry.Name} ({tableEntry.Folder})");

            // Auto-construct path
            string xmlPath = Path.Combine(
                currentXmlRepair.AvidRootPath,
                "Tables",
                tableEntry.Folder,
                $"{tableEntry.Folder}.xml"
            );

            txtParentXml.Text = xmlPath;
            System.Diagnostics.Debug.WriteLine($"[Parent Dropdown] Auto-path: {xmlPath}");

            if (File.Exists(xmlPath))
            {
                // SUCCESS - load columns
                txtParentXml.ForeColor = Color.Blue;
                currentXmlRepair.ParentXmlPath = xmlPath;
                currentXmlRepair.ParentTableEntry = tableEntry;

                LoadXmlColumns(xmlPath, cmbParentXmlColumns, true);
                System.Diagnostics.Debug.WriteLine($"[Parent Dropdown] ✓ Loaded {cmbParentXmlColumns.Items.Count} columns");

                btnBrowseParentXml.Enabled = true;  // Enable override
            }
            else
            {
                // ERROR - show warning
                txtParentXml.ForeColor = Color.Red;
                System.Diagnostics.Debug.WriteLine("[Parent Dropdown] File not found!");

                MessageBox.Show($"Fil ikke fundet:\n{xmlPath}\n\nBrug 'Browse...' knappen for at vælge manuelt.",
                    "Fil ikke fundet", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                btnBrowseParentXml.Enabled = true;  // Enable override
            }
        }

        private void CmbChildTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbChildTable.SelectedItem == null)
                return;

            var selectedItem = (TableComboBoxItem)cmbChildTable.SelectedItem;
            var tableEntry = selectedItem.TableEntry;

            System.Diagnostics.Debug.WriteLine($"[Child Dropdown] Selected: {tableEntry.Name} ({tableEntry.Folder})");

            // Auto-construct path
            string xmlPath = Path.Combine(
                currentXmlRepair.AvidRootPath,
                "Tables",
                tableEntry.Folder,
                $"{tableEntry.Folder}.xml"
            );

            txtChildXml.Text = xmlPath;
            System.Diagnostics.Debug.WriteLine($"[Child Dropdown] Auto-path: {xmlPath}");

            if (File.Exists(xmlPath))
            {
                // SUCCESS - load columns
                txtChildXml.ForeColor = Color.Blue;
                currentXmlRepair.ChildXmlPath = xmlPath;
                currentXmlRepair.ChildTableEntry = tableEntry;

                LoadXmlColumns(xmlPath, cmbChildXmlColumns, false);
                System.Diagnostics.Debug.WriteLine($"[Child Dropdown] ✓ Loaded {cmbChildXmlColumns.Items.Count} columns");

                btnBrowseChildXml.Enabled = true;  // Enable override
            }
            else
            {
                // ERROR - show warning
                txtChildXml.ForeColor = Color.Red;
                System.Diagnostics.Debug.WriteLine("[Child Dropdown] ✗ File not found!");

                MessageBox.Show($"Fil ikke fundet:\n{xmlPath}\n\nBrug 'Browse...' knappen for at vælge manuelt.",
                    "Fil ikke fundet", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                btnBrowseChildXml.Enabled = true;  // Enable override
            }
        }

        private class TableComboBoxItem
        {
            public string DisplayText { get; set; }     // "Forældre (table6)"
            public TableIndexEntry TableEntry { get; set; }

            public override string ToString() => DisplayText;
        }

        #endregion

        #region XML FK Repair - Column Parsing WITHOUT XSD

        /// <summary>
        /// Load XML kolonner direkte fra XML fil
        /// </summary>
        private void LoadXmlColumns(string xmlPath, ComboBox comboBox, bool isParent)
        {
            try
            {
                var columns = ParseColumnsFromXml(xmlPath);

                // Enrich med tableIndex metadata hvis tilgængelig
                if (!string.IsNullOrEmpty(currentXmlRepair.TableIndexPath))
                {
                    var tableEntry = isParent ? currentXmlRepair.ParentTableEntry : currentXmlRepair.ChildTableEntry;
                    if (tableEntry != null)
                    {
                        columns = EnrichColumnsWithTableIndexMetadata(columns, tableEntry);
                    }
                }

                comboBox.Items.Clear();

                foreach (var column in columns.OrderBy(c => c.Position))
                {
                    // Display format: c1: barn_id (VARCHAR) - Description hvis tilgængelig
                    string displayText = column.Name;

                    if (!string.IsNullOrEmpty(column.DisplayName))
                    {
                        displayText = $"{column.Name}: {column.DisplayName}";
                    }

                    if (!string.IsNullOrEmpty(column.DataType))
                    {
                        displayText += $" ({column.DataType})";
                    }

                    if (!string.IsNullOrEmpty(column.Description))
                    {
                        displayText += $" - {column.Description}";
                    }

                    comboBox.Items.Add(displayText);
                }

                if (comboBox.Items.Count > 0)
                    comboBox.SelectedIndex = 0;

                // GEM kolonner til auto-population
                if (isParent)
                    currentParentXmlColumns = columns;
                else
                    currentChildXmlColumns = columns;

                // AUTO-POPULER eksisterende pairs
                var currentTableEntry = isParent ? currentXmlRepair.ParentTableEntry : currentXmlRepair.ChildTableEntry;
                foreach (var pair in xmlColumnPairs)
                {
                    if (isParent)
                        PopulateXmlComboBoxWithColumns(pair.ParentComboBox, columns, currentTableEntry);
                    else
                        PopulateXmlComboBoxWithColumns(pair.ChildComboBox, columns, currentTableEntry);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved læsning af XML kolonner: {ex.Message}", "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Parse kolonner direkte fra XML første row
        /// </summary>
        private List<XmlColumnInfo> ParseColumnsFromXml(string xmlPath)
        {
            var columns = new List<XmlColumnInfo>();

            var settings = new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                IgnoreComments = true,
                DtdProcessing = DtdProcessing.Ignore
            };

            using (var reader = XmlReader.Create(xmlPath, settings))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "row")
                    {
                        // Læs første row for kolonne struktur
                        var rowSubtree = reader.ReadSubtree();
                        int position = 1;

                        while (rowSubtree.Read())
                        {
                            if (rowSubtree.NodeType == XmlNodeType.Element && rowSubtree.Name.StartsWith("c"))
                            {
                                bool isNillable = rowSubtree.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance") == "true";

                                columns.Add(new XmlColumnInfo
                                {
                                    Name = rowSubtree.Name,
                                    Position = position++,
                                    IsNillable = isNillable
                                });
                            }
                        }
                        break; // Kun læs første row
                    }
                }
            }

            return columns;
        }

        /// <summary>
        /// Enrich parsed columns med tableIndex metadata
        /// </summary>
        private List<XmlColumnInfo> EnrichColumnsWithTableIndexMetadata(List<XmlColumnInfo> columns, TableIndexEntry tableEntry)
        {
            if (tableEntry == null || tableEntry.Columns == null)
                return columns;

            // Map columnID (c1, c2, etc.) til tableIndex metadata
            foreach (var col in columns)
            {
                // Find matching column i tableIndex
                var tableColumn = tableEntry.Columns.FirstOrDefault(tc => tc.ColumnID == col.Name);

                if (tableColumn != null)
                {
                    col.DisplayName = tableColumn.Name;        // barn_id, koen, etc.
                    col.DataType = tableColumn.DataType;           // VARCHAR(5), DECIMAL, etc.
                    col.Description = tableColumn.Description;
                }
            }

            return columns;
        }

        /// <summary>
        /// Find TableIndexEntry ved at matche XML filpath
        /// </summary>
        private TableIndexEntry FindTableEntryByXmlPath(List<TableIndexEntry> tables, string xmlPath)
        {
            string xmlFileName = Path.GetFileNameWithoutExtension(xmlPath); // "table1"

            // 1: Match på folder navn
            var match = tables.FirstOrDefault(t => t.Folder == xmlFileName);
            if (match != null) return match;

            // 2: Match på tabel navn
            match = tables.FirstOrDefault(t => string.Equals(t.Name, xmlFileName, StringComparison.OrdinalIgnoreCase));
            if (match != null) return match;

            // 3: Find XML i alle tabeller
            string tableIndexDirectory = Path.GetDirectoryName(currentXmlRepair.TableIndexPath);
            foreach (var table in tables)
            {
                string foundXmlPath = table.FindXmlPath(tableIndexDirectory);
                if (foundXmlPath != null && Path.GetFullPath(foundXmlPath) == Path.GetFullPath(xmlPath))
                {
                    return table;
                }
            }

            return null;
        }

        #endregion

        #region XML FK Repair - Analysis and Generation

        /// <summary>
        /// Analyzer XML FK relationships
        /// </summary>
        private async void BtnAnalyzeXmlFK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentXmlRepair.ParentXmlPath) || string.IsNullOrEmpty(currentXmlRepair.ChildXmlPath))
            {
                MessageBox.Show("Vælg både Parent og Child XML filer først");
                return;
            }

            // Tjek at alle ekstra kolonner er valgt
            foreach (var pair in xmlColumnPairs)
            {
                if (pair.ParentComboBox.SelectedItem == null || pair.ChildComboBox.SelectedItem == null)
                {
                    MessageBox.Show($"Vælg kolonner for primærnøgle {pair.PairNumber}");
                    return;
                }
            }

            // saml alle primærnøgle kolonner (base + sammensatte)
            var parentKeys = new List<string> { ExtractColumnName(cmbParentXmlColumns.Text) };
            var childKeys = new List<string> { ExtractColumnName(cmbChildXmlColumns.Text) };

            foreach (var pair in xmlColumnPairs)
            {
                parentKeys.Add(ExtractColumnName(pair.ParentComboBox.Text));
                childKeys.Add(ExtractColumnName(pair.ChildComboBox.Text));
            }

            currentXmlRepair.ParentKeyColumns = parentKeys;
            currentXmlRepair.ChildKeyColumns = childKeys;

            try
            {
                btnAnalyzeXmlFK.Enabled = false;
                lblXmlFKStats.Text = "Analyserer...";
                progressBar1.Visible = true;
                progressBar1.Style = ProgressBarStyle.Marquee;

                var progress = new Progress<(long processed, string stage)>(update =>
                {
                    lblXmlFKStats.Text = $"{update.stage}: {update.processed:N0} rækker";
                });

                var repairTool = new ScalableXmlFKRepair();
                var missingKeys = await repairTool.AnalyzeMissingKeysAsync(
                    currentXmlRepair.ParentXmlPath,
                    currentXmlRepair.ChildXmlPath,
                    parentKeys,
                    childKeys,
                    progress,
                    CancellationToken.None
                );

                lstXmlMissingValues.Items.Clear();
                foreach (var key in missingKeys.OrderBy(k => k))
                {
                    lstXmlMissingValues.Items.Add(key);
                }

                progressBar1.Visible = false;
                lblXmlFKStats.Text = $"Analyse komplet: {missingKeys.Count:N0} manglende værdier fundet";

                btnGenerateFixedXml.Enabled = missingKeys.Count > 0;
                btnExportXmlMissing.Enabled = missingKeys.Count > 0;
            }
            catch (Exception ex)
            {
                progressBar1.Visible = false;
                lblXmlFKStats.Text = "Analyse fejlet";
                MessageBox.Show($"Fejl under analyse: {ex.Message}", "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnAnalyzeXmlFK.Enabled = true;
            }
        }

        /// <summary>
        /// Generate repareret XML med missing FK rows
        /// </summary>
        private async void BtnGenerateFixedXml_Click(object sender, EventArgs e)
        {
            if (lstXmlMissingValues.Items.Count == 0)
            {
                MessageBox.Show("Ingen manglende værdier at reparere");
                return;
            }

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "XML filer|*.xml";
                saveDialog.Title = "Gem repareret Parent XML";
                saveDialog.FileName = Path.GetFileNameWithoutExtension(currentXmlRepair.ParentXmlPath) + "_repaired.xml";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        btnGenerateFixedXml.Enabled = false;
                        progressBar1.Visible = true;
                        progressBar1.Style = ProgressBarStyle.Continuous;

                        string outputXmlPath = saveDialog.FileName;

                        var missingKeys = lstXmlMissingValues.Items.Cast<string>().ToList();
                        string integrityDescription = txtIntegrityDesc.Text;

                        // Get first key column for single-column FK
                        var keyColumns = new List<string>();
                        keyColumns.Add(ExtractColumnName(cmbParentXmlColumns.Text));  // Primary key

                        var progress = new Progress<(long processed, string stage)>(update =>
                        {
                            lblXmlFKStats.Text = $"{update.stage}: {update.processed:N0}";
                            if (update.processed > 0)
                            {
                                progressBar1.Value = Math.Min((int)(update.processed % 100), 100);
                            }
                        });

                        var repairTool = new ScalableXmlFKRepair();

                        // Pass optional tableIndex metadata
                        var tableEntry = currentXmlRepair.ParentTableEntry;

                        foreach (var pair in xmlColumnPairs)
                        {
                            string parentColName = ExtractColumnName(pair.ParentComboBox.Text);
                            if (!string.IsNullOrEmpty(parentColName) && !keyColumns.Contains(parentColName))
                            {
                                keyColumns.Add(parentColName);
                            }
                        }

                        await repairTool.GenerateRepairedXmlAsync(
                            currentXmlRepair.ParentXmlPath,
                            outputXmlPath,
                            missingKeys,               // ["123|ABC|999", "456|DEF|888"]
                            keyColumns,                // ["c1", "c2", "c3"]
                            integrityDescription,
                            tableEntry,
                            progress,
                            CancellationToken.None
                        );

                        progressBar1.Visible = false;
                        lblXmlFKStats.Text = $"Genereret {missingKeys.Count:N0} nye rækker";

                        MessageBox.Show($"Genereret repareret XML med {missingKeys.Count:N0} nye rækker\n\n" +
                                      $"Output: {outputXmlPath}\n\n",
                                      "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        progressBar1.Visible = false;
                        lblXmlFKStats.Text = "Fejl under generering";
                        MessageBox.Show($"Fejl ved generering af repareret XML: {ex.Message}", "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        btnGenerateFixedXml.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Extract clean column name from ComboBox display text
        /// </summary>
        private string ExtractColumnName(string comboBoxText)
        {
            if (string.IsNullOrEmpty(comboBoxText))
                return "";

            // Format: "c1: barn_id (VARCHAR) - Description" -> extract "c1"
            var match = System.Text.RegularExpressions.Regex.Match(comboBoxText, @"^(c\d+)");
            if (match.Success)
                return match.Groups[1].Value;

            // Fallback: split på kolon eller mellemrum
            return comboBoxText.Split(new[] { ':', ' ', '(' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
        }

        #endregion

        #region XML FK Repair - Composite PK Support

        /// <summary>
        /// Populate en ComboBox med XML kolonner
        /// </summary>
        private void PopulateXmlComboBoxWithColumns(ComboBox comboBox, List<XmlColumnInfo> columns, TableIndexEntry tableEntry)
        {
            if (comboBox == null || columns == null) return;

            try
            {
                comboBox.Items.Clear();

                foreach (var column in columns.OrderBy(c => c.Position))
                {
                    string displayText = column.Name;
                    if (!string.IsNullOrEmpty(column.DisplayName))
                        displayText = $"{column.Name}: {column.DisplayName}";
                    if (!string.IsNullOrEmpty(column.DataType))
                        displayText += $" ({column.DataType})";
                    if (!string.IsNullOrEmpty(column.Description))
                        displayText += $" - {column.Description}";

                    comboBox.Items.Add(displayText);
                }

                if (comboBox.Items.Count > 0)
                    comboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PopulateXmlComboBoxWithColumns error: {ex.Message}");
            }
        }

        /// <summary>
        /// Add button click handler
        /// </summary>
        private void BtnAddXmlPrimaryKey_Click(object sender, EventArgs e)
        {
            AddNewXmlColumnPair();
        }

        /// <summary>
        /// Remove button click handler
        /// </summary>
        private void BtnRemoveXmlPrimaryKey_Click(object sender, EventArgs e)
        {
            RemoveLastXmlColumnPair();
        }

        /// <summary>
        /// Tilføj nyt XML column pair til sammensatte PK
        /// </summary>
        private void AddNewXmlColumnPair()
        {
            var pair = new ColumnPair { PairNumber = nextXmlPairNumber };

            // Skab labels
            pair.ParentLabel = new Label
            {
                Text = $"Parent {nextXmlPairNumber}:",
                Location = new Point(20, 15 + (nextXmlPairNumber - 2) * 30),
                Size = new Size(70, 13),
                AutoSize = true
            };

            pair.ChildLabel = new Label
            {
                Text = $"Child {nextXmlPairNumber}:",
                Location = new Point(500, 15 + (nextXmlPairNumber - 2) * 30),
                Size = new Size(70, 13),
                AutoSize = true
            };

            // Skab ComboBoxes
            pair.ParentComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(110, 12 + (nextXmlPairNumber - 2) * 30),
                Size = new Size(350, 21),
                Name = $"cmbXmlParent{nextXmlPairNumber}"
            };

            pair.ChildComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(580, 12 + (nextXmlPairNumber - 2) * 30),
                Size = new Size(350, 21),
                Name = $"cmbXmlChild{nextXmlPairNumber}"
            };

            // Tilføj til panel
            pnlXmlDynamicColumns.Controls.Add(pair.ParentLabel);
            pnlXmlDynamicColumns.Controls.Add(pair.ChildLabel);
            pnlXmlDynamicColumns.Controls.Add(pair.ParentComboBox);
            pnlXmlDynamicColumns.Controls.Add(pair.ChildComboBox);

            // Auto-populer hvis kolonner allerede er indlæst
            if (currentParentXmlColumns != null)
                PopulateXmlComboBoxWithColumns(pair.ParentComboBox, currentParentXmlColumns, currentXmlRepair.ParentTableEntry);
            if (currentChildXmlColumns != null)
                PopulateXmlComboBoxWithColumns(pair.ChildComboBox, currentChildXmlColumns, currentXmlRepair.ChildTableEntry);

            xmlColumnPairs.Add(pair);
            nextXmlPairNumber++;

            UpdateXmlRemoveButtonState();
        }

        /// <summary>
        /// Fjern sidste XML column pair
        /// </summary>
        private void RemoveLastXmlColumnPair()
        {
            if (xmlColumnPairs.Count == 0) return;

            var lastPair = xmlColumnPairs.Last();

            // Fjern controls fra panel
            pnlXmlDynamicColumns.Controls.Remove(lastPair.ParentLabel);
            pnlXmlDynamicColumns.Controls.Remove(lastPair.ChildLabel);
            pnlXmlDynamicColumns.Controls.Remove(lastPair.ParentComboBox);
            pnlXmlDynamicColumns.Controls.Remove(lastPair.ChildComboBox);

            xmlColumnPairs.Remove(lastPair);
            nextXmlPairNumber--;

            UpdateXmlRemoveButtonState();
        }

        /// <summary>
        /// Opdater remove button enabled state
        /// </summary>
        private void UpdateXmlRemoveButtonState()
        {
            btnRemoveXmlPrimaryKey.Enabled = xmlColumnPairs.Count > 0;
        }

        #endregion

        #region XML FK Repair - Export/Copy Functionality

        /// <summary>
        /// Setup context menu til XML ListBox
        /// </summary>
        private void SetupXmlListBoxContextMenu()
        {
            var contextMenu = new ContextMenuStrip();

            var copyItem = new ToolStripMenuItem("Kopiér markerede");
            copyItem.Click += (s, e) => BtnCopyXmlSelected_Click(s, new EventArgs());
            contextMenu.Items.Add(copyItem);

            var selectAllItem = new ToolStripMenuItem("Markér alle");
            selectAllItem.Click += (s, e) => {
                for (int i = 0; i < lstXmlMissingValues.Items.Count; i++)
                    lstXmlMissingValues.SetSelected(i, true);
            };
            contextMenu.Items.Add(selectAllItem);

            contextMenu.Items.Add(new ToolStripSeparator());

            var exportItem = new ToolStripMenuItem("Eksportér alle til fil");
            exportItem.Click += (s, e) => BtnExportXmlMissing_Click(s, new EventArgs());
            contextMenu.Items.Add(exportItem);

            lstXmlMissingValues.ContextMenuStrip = contextMenu;
        }

        /// <summary>
        /// Kopiér markerede items til clipboard
        /// </summary>
        private void BtnCopyXmlSelected_Click(object sender, EventArgs e)
        {
            if (lstXmlMissingValues.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vælg items at kopiere");
                return;
            }

            var selectedText = string.Join("\r\n", lstXmlMissingValues.SelectedItems.Cast<string>());
            Clipboard.SetText(selectedText);

            MessageBox.Show($"Kopieret {lstXmlMissingValues.SelectedItems.Count} items til clipboard");
        }

        /// <summary>
        /// Eksporter alle missing values til tekstfil
        /// </summary>
        private void BtnExportXmlMissing_Click(object sender, EventArgs e)
        {
            if (lstXmlMissingValues.Items.Count == 0)
            {
                MessageBox.Show("Ingen data at eksportere");
                return;
            }

            saveFileDialog1.Filter = "Tekstfiler|*.txt|Alle filer|*.*";
            saveFileDialog1.FileName = $"missing_xml_fk_values_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sb = new StringBuilder();

                    // Header information
                    sb.AppendLine("Missing XML Foreign Key Values Report");
                    sb.AppendLine("======================================");
                    sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    sb.AppendLine();

                    // TABELNAVNE
                    sb.AppendLine("Tables:");
                    string parentTableName = currentXmlRepair.ParentTableEntry?.Name ?? Path.GetFileNameWithoutExtension(currentXmlRepair.ParentXmlPath);
                    string childTableName = currentXmlRepair.ChildTableEntry?.Name ?? Path.GetFileNameWithoutExtension(currentXmlRepair.ChildXmlPath);
                    sb.AppendLine($"  Parent Table: {parentTableName}");
                    sb.AppendLine($"  Child Table: {childTableName}");
                    sb.AppendLine();

                    // FILE PATHS
                    sb.AppendLine("File Paths:");
                    sb.AppendLine($"  Parent XML: {currentXmlRepair.ParentXmlPath}");
                    sb.AppendLine($"  Child XML: {currentXmlRepair.ChildXmlPath}");
                    sb.AppendLine();

                    // KOLONNENAVNE MED DETALJER
                    sb.AppendLine("Foreign Key Columns:");
                    sb.AppendLine("  Parent Columns:");
                    for (int i = 0; i < currentXmlRepair.ParentKeyColumns.Count; i++)
                    {
                        string columnID = currentXmlRepair.ParentKeyColumns[i];
                        string columnName = GetColumnDisplayName(columnID, currentXmlRepair.ParentTableEntry);
                        sb.AppendLine($"    {i + 1}. {columnID} - {columnName}");
                    }
                    sb.AppendLine();
                    sb.AppendLine("  Child Columns:");
                    for (int i = 0; i < currentXmlRepair.ChildKeyColumns.Count; i++)
                    {
                        string columnID = currentXmlRepair.ChildKeyColumns[i];
                        string columnName = GetColumnDisplayName(columnID, currentXmlRepair.ChildTableEntry);
                        sb.AppendLine($"    {i + 1}. {columnID} - {columnName}");
                    }
                    sb.AppendLine();

                    // Key type
                    string keyType = currentXmlRepair.ParentKeyColumns.Count > 1 ? "Composite Primary Key" : "Single Primary Key";
                    sb.AppendLine($"Key Type: {keyType}");

                    if (currentXmlRepair.ParentKeyColumns.Count > 1)
                    {
                        sb.AppendLine($"Key Format: {string.Join(" | ", currentXmlRepair.ParentKeyColumns)}");
                    }

                    sb.AppendLine($"Total Missing Values: {lstXmlMissingValues.Items.Count}");
                    sb.AppendLine();
                    sb.AppendLine("Missing Values:");
                    sb.AppendLine("---------------");

                    foreach (var item in lstXmlMissingValues.Items)
                    {
                        sb.AppendLine(item.ToString());
                    }

                    File.WriteAllText(saveFileDialog1.FileName, sb.ToString(), Encoding.UTF8);

                    MessageBox.Show($"Eksporteret {lstXmlMissingValues.Items.Count} værdier til {saveFileDialog1.FileName}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fejl ved eksport: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Hjælpemetode til at få kolonnenavn med metadata
        /// </summary>
        private string GetColumnDisplayName(string columnID, TableIndexEntry tableEntry)
        {
            if (tableEntry != null && tableEntry.Columns != null)
            {
                var column = tableEntry.Columns.FirstOrDefault(c => c.ColumnID == columnID);
                if (column != null)
                {
                    return $"{column.Name} ({column.DataType})";
                }
            }

            return "(metadata ikke tilgængelig)";
        }

        /// <summary>
        /// Keyboard shortcuts til XML ListBox
        /// </summary>
        private void LstXmlMissingValues_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                BtnCopyXmlSelected_Click(sender, new EventArgs());
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                for (int i = 0; i < lstXmlMissingValues.Items.Count; i++)
                    lstXmlMissingValues.SetSelected(i, true);
                e.Handled = true;
            }
        }

        #endregion

        #region CSV FK Repair - File Selection

        /// <summary>
        /// Browse for parent CSV file
        /// </summary>
        private void btnBrowseParent_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSV filer|*.csv|Alle filer|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtParentCSV.Text = openFileDialog1.FileName;
                AutoDetectColumns(openFileDialog1.FileName, true);
            }
        }

        /// <summary>
        /// Browse for child CSV file
        /// </summary>
        private void btnBrowseChild_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSV filer|*.csv|Alle filer|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtChildCSV.Text = openFileDialog1.FileName;
                AutoDetectColumns(openFileDialog1.FileName, false);
            }
        }

        /// <summary>
        /// Auto-detect og populer column dropdowns med kolonnenavne
        /// </summary>
        private void AutoDetectColumns(string filePath, bool isParent)
        {
            try
            {
                using (var reader = new StreamReader(filePath, Encoding.UTF8, true))
                {
                    string headerLine = reader.ReadLine();
                    if (string.IsNullOrEmpty(headerLine)) return;

                    var headers = ParseCSVLine(headerLine);
                    var columnList = new List<string>();

                    for (int i = 0; i < headers.Length; i++)
                    {
                        string displayText = $"{headers[i].Trim()} (kolonne {i + 1})";
                        columnList.Add(displayText);
                    }

                    // Populer alle relevante dropdowns
                    if (isParent)
                    {
                        PopulateComboBox(cmbParentColumn, columnList);
                        foreach (var pair in columnPairs)
                        {
                            PopulateComboBox(pair.ParentComboBox, columnList);
                        }
                    }
                    else
                    {
                        PopulateComboBox(cmbChildColumn, columnList);
                        foreach (var pair in columnPairs)
                        {
                            PopulateComboBox(pair.ChildComboBox, columnList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved læsning af kolonner: {ex.Message}");
            }
        }

        /// <summary>
        /// PopulateComboBox med null checks
        /// </summary>
        private void PopulateComboBox(ComboBox comboBox, List<string> columnList)
        {
            if (comboBox == null || columnList == null) return;

            try
            {
                comboBox.Items.Clear();
                foreach (string column in columnList)
                {
                    comboBox.Items.Add(column);
                }
                if (comboBox.Items.Count > 0)
                    comboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PopulateComboBox error: {ex.Message}");
            }
        }

        /// <summary>
        /// GetSelectedColumnIndex med null checks
        /// </summary>
        private int GetSelectedColumnIndex(ComboBox comboBox)
        {
            if (comboBox?.SelectedItem == null) return -1;

            try
            {
                string selectedText = comboBox.SelectedItem.ToString();
                // Extract "(kolonne X)" delen
                int start = selectedText.LastIndexOf("(kolonne ") + 9;
                int end = selectedText.LastIndexOf(")");

                if (start > 8 && end > start)
                {
                    string columnNumberText = selectedText.Substring(start, end - start);
                    if (int.TryParse(columnNumberText, out int columnNumber))
                    {
                        return columnNumber - 1; // Convert til 0-based index
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetSelectedColumnIndex error: {ex.Message}");
            }

            return -1;
        }

        #endregion

        #region CSV FK Repair - sammensatte PK Support

        /// <summary>
        /// Initialiser default column pair (den første)
        /// </summary>
        private void InitializeDefaultColumnPair()
        {
            // Default pair er allerede i Designer (cmbParentColumn + cmbChildColumn)
            // Vi tracker dem som pair nummer 1
        }

        /// <summary>
        /// Tilføj nyt column pair til sammensatte PK
        /// </summary>
        private void btnAddPrimaryKey_Click(object sender, EventArgs e)
        {
            AddNewColumnPair();
        }

        /// <summary>
        /// Fjern sidste column pair
        /// </summary>
        private void btnRemovePrimaryKey_Click(object sender, EventArgs e)
        {
            RemoveLastColumnPair();
        }

        /// <summary>
        /// Skab et nyt column pair programmatisk
        /// </summary>
        private void AddNewColumnPair()
        {
            var pair = new ColumnPair { PairNumber = nextPairNumber };

            // Skab labels
            pair.ParentLabel = new Label
            {
                Text = $"Parent {nextPairNumber}:",
                Location = new Point(20, 15 + (nextPairNumber - 2) * 30),
                Size = new Size(70, 13),
                AutoSize = true
            };

            pair.ChildLabel = new Label
            {
                Text = $"Child {nextPairNumber}:",
                Location = new Point(380, 15 + (nextPairNumber - 2) * 30),
                Size = new Size(60, 13),
                AutoSize = true
            };

            // Skab ComboBoxes
            pair.ParentComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(105, 12 + (nextPairNumber - 2) * 30),
                Size = new Size(250, 21),
                Name = $"cmbParent{nextPairNumber}"
            };

            pair.ChildComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(460, 12 + (nextPairNumber - 2) * 30),
                Size = new Size(250, 21),
                Name = $"cmbChild{nextPairNumber}"
            };

            // Tilføj til panel
            pnlDynamicColumns.Controls.Add(pair.ParentLabel);
            pnlDynamicColumns.Controls.Add(pair.ChildLabel);
            pnlDynamicColumns.Controls.Add(pair.ParentComboBox);
            pnlDynamicColumns.Controls.Add(pair.ChildComboBox);

            // Auto-populer hvis filer allerede er valgt
            if (!string.IsNullOrEmpty(txtParentCSV.Text))
                AutoDetectColumns(txtParentCSV.Text, true);
            if (!string.IsNullOrEmpty(txtChildCSV.Text))
                AutoDetectColumns(txtChildCSV.Text, false);

            columnPairs.Add(pair);
            nextPairNumber++;

            UpdateRemoveButtonState();
        }

        /// <summary>
        /// Fjern sidste column pair
        /// </summary>
        private void RemoveLastColumnPair()
        {
            if (columnPairs.Count == 0) return;

            var lastPair = columnPairs.Last();

            // Fjern controls fra panel
            pnlDynamicColumns.Controls.Remove(lastPair.ParentLabel);
            pnlDynamicColumns.Controls.Remove(lastPair.ChildLabel);
            pnlDynamicColumns.Controls.Remove(lastPair.ParentComboBox);
            pnlDynamicColumns.Controls.Remove(lastPair.ChildComboBox);

            columnPairs.Remove(lastPair);
            nextPairNumber--;

            UpdateRemoveButtonState();
        }

        /// <summary>
        /// Opdater remove button enabled state
        /// </summary>
        private void UpdateRemoveButtonState()
        {
            btnRemovePrimaryKey.Enabled = columnPairs.Count > 0;
        }

        #endregion

        #region CSV FK Repair - Analysis

        /// <summary>
        /// Hovedmetode til at analysere FK relationships
        /// </summary>
        private async void btnAnalyzeFK_Click(object sender, EventArgs e)
        {
            // Cleanup tidligere temp filer
            CleanupTempFiles();

            // Validering
            if (string.IsNullOrEmpty(txtParentCSV.Text) || string.IsNullOrEmpty(txtChildCSV.Text))
            {
                MessageBox.Show("Vælg både parent og child CSV filer");
                return;
            }

            if (cmbParentColumn.SelectedItem == null || cmbChildColumn.SelectedItem == null)
            {
                MessageBox.Show("Vælg kolonner i begge filer");
                return;
            }

            // Tjek at alle ekstra kolonner er valgt
            foreach (var pair in columnPairs)
            {
                if (pair.ParentComboBox.SelectedItem == null || pair.ChildComboBox.SelectedItem == null)
                {
                    MessageBox.Show($"Vælg kolonner for primærnøgle {pair.PairNumber}");
                    return;
                }
            }

            try
            {
                progressBar1.Visible = true;
                progressBar1.Value = 0;

                // Byg column index arrays
                var parentColumnIndexes = new List<int> { GetSelectedColumnIndex(cmbParentColumn) };
                var childColumnIndexes = new List<int> { GetSelectedColumnIndex(cmbChildColumn) };

                foreach (var pair in columnPairs)
                {
                    parentColumnIndexes.Add(GetSelectedColumnIndex(pair.ParentComboBox));
                    childColumnIndexes.Add(GetSelectedColumnIndex(pair.ChildComboBox));
                }

                progressBar1.Value = 25;

                // Find missing values med sammensatte PK support
                var missingCount = await FindMissingFKValuesAsync(
                    txtParentCSV.Text, parentColumnIndexes.ToArray(),
                    txtChildCSV.Text, childColumnIndexes.ToArray()
                );

                progressBar1.Value = 100;

                // Vis resultater
                DisplayFKResults(missingCount, parentColumnIndexes.Count > 1);

                progressBar1.Visible = false;
            }
            catch (Exception ex)
            {
                progressBar1.Visible = false;
                MessageBox.Show($"Fejl ved FK analyse: {ex.Message}");
            }
        }

        /// <summary>
        /// Find missing FK values med support for sammensatte PKs - MEMORY OPTIMIZED
        /// </summary>
        private async Task<int> FindMissingFKValuesAsync(string parentPath, int[] parentColumns,
                                                         string childPath, int[] childColumns)
        {
            // Cleanup forrige temp fil
            CleanupTempFiles();

            // Opret ny temp fil
            currentMissingValuesFile = Path.GetTempFileName();

            var parentKeys = new HashSet<string>();
            var processedChildKeys = new HashSet<string>();
            int missingCount = 0;

            // Læs parent keys
            using (var reader = new StreamReader(parentPath, Encoding.UTF8, true))
            {
                string line = await reader.ReadLineAsync(); // Skip header

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var fields = ParseCSVLine(line);
                    var key = BuildCompositeKey(fields, parentColumns);

                    if (!string.IsNullOrEmpty(key))
                        parentKeys.Add(key);
                }
            }

            // Læs child keys og skriv missing direkte til temp fil
            using (var writer = new StreamWriter(currentMissingValuesFile, false, Encoding.UTF8))
            {
                using (var reader = new StreamReader(childPath, Encoding.UTF8, true))
                {
                    string line = await reader.ReadLineAsync(); // Skip header

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var fields = ParseCSVLine(line);
                        var key = BuildCompositeKey(fields, childColumns);

                        if (!string.IsNullOrEmpty(key) &&
                            !parentKeys.Contains(key) &&
                            !processedChildKeys.Contains(key))
                        {
                            await writer.WriteLineAsync(key);
                            processedChildKeys.Add(key);
                            missingCount++;
                        }
                    }
                }
            }

            return missingCount;
        }

        /// <summary>
        /// Byg sammensatte PK fra felter og column indexes
        /// </summary>
        private string BuildCompositeKey(string[] fields, int[] columnIndexes)
        {
            var keyParts = new List<string>();

            foreach (int index in columnIndexes)
            {
                if (index >= 0 && index < fields.Length)
                {
                    keyParts.Add(fields[index].Trim());
                }
            }

            return keyParts.Count == columnIndexes.Length ? string.Join("|", keyParts) : null;
        }

        /// <summary>
        /// Robust CSV line parser med quote og escape support
        /// </summary>
        private string[] ParseCSVLine(string line)
        {
            var fields = new List<string>();
            var currentField = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ';' && !inQuotes)
                {
                    fields.Add(currentField.ToString());
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(c);
                }
            }

            fields.Add(currentField.ToString());
            return fields.ToArray();
        }

        /// <summary>
        /// Vis FK analysis resultater i UI - MEMORY OPTIMIZED
        /// </summary>
        private async void DisplayFKResults(int totalMissing, bool isCompositeKey)
        {
            lstMissingValues.Items.Clear();
            string keyType = isCompositeKey ? "sammensatte nøgler" : "nøgler";

            if (totalMissing == 0)
            {
                lblFKStats.Text = $"RESULTAT: Ingen manglende {keyType} fundet";
                btnGenerateDummies.Enabled = false;
                btnExportMissing.Enabled = false;
                btnCopySelected.Enabled = false;
                return;
            }

            // Læs første 500 linjer fra temp fil til UI display
            if (!string.IsNullOrEmpty(currentMissingValuesFile) && File.Exists(currentMissingValuesFile))
            {
                using (var reader = new StreamReader(currentMissingValuesFile, Encoding.UTF8))
                {
                    string line;
                    int displayCount = 0;

                    while ((line = await reader.ReadLineAsync()) != null && displayCount < 500)
                    {
                        lstMissingValues.Items.Add(line);
                        displayCount++;
                    }
                }
            }

            // Opdater statistik
            lblFKStats.Text = $"Fundet {totalMissing} manglende {keyType}";
            if (totalMissing > 500)
                lblFKStats.Text += $"\n(viser første 500 af {totalMissing})";

            btnGenerateDummies.Enabled = true;
            btnExportMissing.Enabled = true;
            btnCopySelected.Enabled = true;
        }

        #endregion

        #region CSV FK Repair - Dummy Generation

        /// <summary>
        /// Generer dummy records for missing FK values
        /// </summary>
        private async void btnGenerateDummies_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentMissingValuesFile) || !File.Exists(currentMissingValuesFile))
            {
                MessageBox.Show("Ingen manglende værdier at generere rææker for");
                return;
            }

            // Count linjer i temp fil
            int missingCount = File.ReadAllLines(currentMissingValuesFile).Length;
            string parentTableName = Path.GetFileNameWithoutExtension(txtParentCSV.Text);
            string parentFileName = Path.GetFileNameWithoutExtension(txtParentCSV.Text);

            saveFileDialog1.Filter = "CSV filer|*.csv";
            saveFileDialog1.FileName = $"{parentFileName}_updated.csv";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    progressBar1.Visible = true;
                    progressBar1.Value = 0;

                    await GenerateDummyRecordsAsync(saveFileDialog1.FileName);

                    progressBar1.Visible = false;
                    MessageBox.Show($"Opdateret {parentTableName} med {missingCount} nye rækker:\n\n{saveFileDialog1.FileName}",
                        "Fuldført", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    progressBar1.Visible = false;
                    MessageBox.Show($"Fejl ved generering af rækker: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Generer dummy CSV records med auto-detection af dato/tid kolonner - MEMORY OPTIMIZED
        /// </summary>
        private async Task GenerateDummyRecordsAsync(string outputPath)
        {
            string dummyDescription = txtDummyText.Text.Trim();
            if (string.IsNullOrEmpty(dummyDescription))
                dummyDescription = "Generated row record";

            using (var writer = new StreamWriter(outputPath, false, Encoding.UTF8))
            {
                // Læs original header fra parent file
                string originalHeader;
                using (var reader = new StreamReader(txtParentCSV.Text, Encoding.UTF8, true))
                {
                    originalHeader = await reader.ReadLineAsync();
                }

                // Auto-detect kolonnetyper baseret på eksisterende data
                var columnTypes = await DetectColumnTypesAsync(txtParentCSV.Text);

                // Tilføj ekstra kolonne til header
                string Header = originalHeader + ";row_description";
                await writer.WriteLineAsync(Header);

                // Kopier eksisterende data UDEN dummy beskrivelse
                using (var reader = new StreamReader(txtParentCSV.Text, Encoding.UTF8, true))
                {
                    string line = await reader.ReadLineAsync(); // Skip header - allerede skrevet

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            string Line = line + ";"; // Tom dummy kolonne for eksisterende data
                            await writer.WriteLineAsync(Line);
                        }
                    }
                }

                // Parse original header for at kende struktur
                var originalFields = ParseCSVLine(originalHeader);
                int originalColumnCount = originalFields.Length;

                // Definer key columns
                var keyColumns = new List<int> { GetSelectedColumnIndex(cmbParentColumn) };
                foreach (var pair in columnPairs)
                {
                    keyColumns.Add(GetSelectedColumnIndex(pair.ParentComboBox));
                }

                // Generer dummy records fra temp fil i chunks
                if (!string.IsNullOrEmpty(currentMissingValuesFile) && File.Exists(currentMissingValuesFile))
                {
                    using (var missingReader = new StreamReader(currentMissingValuesFile, Encoding.UTF8))
                    {
                        string missingKey;
                        int processed = 0;
                        int totalMissing = File.ReadAllLines(currentMissingValuesFile).Length;

                        while ((missingKey = await missingReader.ReadLineAsync()) != null)
                        {
                            // Parse sammensatte PK
                            var keyParts = missingKey.Split('|');

                            // Byg dummy record
                            var dummyRecord = new string[originalColumnCount + 1]; // +1 for ekstra kolonne

                            // Udfyld alle felter
                            for (int i = 0; i < originalColumnCount; i++)
                            {
                                // Tjek om dette er en af de PK kolonner
                                int keyPartIndex = keyColumns.IndexOf(i);
                                if (keyPartIndex >= 0 && keyPartIndex < keyParts.Length)
                                {
                                    // Primary key kolonne - brug faktisk værdi
                                    dummyRecord[i] = keyParts[keyPartIndex];
                                }
                                else
                                {
                                    // Ikke-PK kolonne - brug datatype-specific default værdi
                                    string columnType = (i < columnTypes.Length) ? columnTypes[i] : "string";

                                    switch (columnType)
                                    {
                                        case "date":
                                            dummyRecord[i] = "9999-12-31";
                                            break;
                                        case "timestamp":
                                            dummyRecord[i] = "9999-12-31T23:59:59";
                                            break;
                                        case "time":
                                            dummyRecord[i] = "23:59:59";
                                            break;
                                        default:
                                            dummyRecord[i] = ""; // Tom streng for string og andre typer
                                            break;
                                    }
                                }
                            }

                            // Ekstra kolonne med beskrivelse - KUN for dummy records
                            dummyRecord[originalColumnCount] = dummyDescription;

                            // Skriv record
                            await writer.WriteLineAsync(string.Join(";", dummyRecord));

                            processed++;
                            if (processed % 100 == 0)
                            {
                                progressBar1.Value = (int)((processed * 100.0) / totalMissing);
                                await Task.Delay(1); // UI responsiveness
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region CSV FK Repair - Column Type Detection

        /// <summary>
        /// Analyser parent CSV og detekter datatyper for hver kolonne
        /// </summary>
        private async Task<string[]> DetectColumnTypesAsync(string parentPath)
        {
            var columnSamples = new List<List<string>>();
            int columnCount = 0;

            using (var reader = new StreamReader(parentPath, Encoding.UTF8, true))
            {
                // Skip header
                string headerLine = await reader.ReadLineAsync();
                if (string.IsNullOrEmpty(headerLine)) return new string[0];

                var headers = ParseCSVLine(headerLine);
                columnCount = headers.Length;

                // Initialiser sample lister
                for (int i = 0; i < columnCount; i++)
                {
                    columnSamples.Add(new List<string>());
                }

                // Saml samples fra første 100 rækker
                string line;
                int sampleCount = 0;
                while ((line = await reader.ReadLineAsync()) != null && sampleCount < 100)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var fields = ParseCSVLine(line);
                    for (int i = 0; i < Math.Min(fields.Length, columnCount); i++)
                    {
                        if (!string.IsNullOrWhiteSpace(fields[i]))
                        {
                            columnSamples[i].Add(fields[i].Trim());
                        }
                    }
                    sampleCount++;
                }
            }

            // Analyser hver kolonne
            var columnTypes = new string[columnCount];
            for (int i = 0; i < columnCount; i++)
            {
                columnTypes[i] = DetermineColumnType(columnSamples[i]);
            }

            return columnTypes;
        }

        /// <summary>
        /// Bestem kolonnetype baseret på samples
        /// </summary>
        private string DetermineColumnType(List<string> samples)
        {
            if (samples.Count == 0) return "string";

            int dateCount = 0;
            int timestampCount = 0;
            int timeCount = 0;

            foreach (string sample in samples)
            {
                if (IsDateValue(sample)) dateCount++;
                else if (IsTimestampValue(sample)) timestampCount++;
                else if (IsTimeValue(sample)) timeCount++;
            }

            double totalSamples = samples.Count;

            // Hvis 80%+ matcher et dato/tid format, klassificer som sådan
            if (timestampCount / totalSamples >= 0.8) return "timestamp";
            if (dateCount / totalSamples >= 0.8) return "date";
            if (timeCount / totalSamples >= 0.8) return "time";

            return "string";
        }

        /// <summary>
        /// Tjek om værdi matcher dato format (YYYY-MM-DD)
        /// </summary>
        private bool IsDateValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            // Check for YYYY-MM-DD format
            if (System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d{4}-\d{2}-\d{2}$"))
            {
                return DateTime.TryParseExact(value, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _);
            }

            return false;
        }

        /// <summary>
        /// Tjek om værdi matcher timestamp format (YYYY-MM-DD HH:MM:SS eller YYYY-MM-DDTHH:MM:SS)
        /// </summary>
        private bool IsTimestampValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            // Check for various timestamp formats
            var timestampPatterns = new[]
            {
                @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$",           // YYYY-MM-DD HH:MM:SS
                @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$",           // YYYY-MM-DDTHH:MM:SS
                @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d+$",      // With milliseconds
                @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+$"       // ISO with milliseconds
            };

            foreach (var pattern in timestampPatterns)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(value, pattern))
                {
                    return DateTime.TryParse(value, out _);
                }
            }

            return false;
        }

        /// <summary>
        /// Tjek om værdi matcher tid format (HH:MM:SS)
        /// </summary>
        private bool IsTimeValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            // Check for HH:MM:SS format
            if (System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d{2}:\d{2}:\d{2}$"))
            {
                return TimeSpan.TryParseExact(value, @"hh\:mm\:ss", null, out _);
            }

            return false;
        }

        #endregion

        #region CSV FK Repair - Export/Copy Functionality

        /// <summary>
        /// Setup context menu til ListBox
        /// </summary>
        private void SetupListBoxContextMenu()
        {
            var contextMenu = new ContextMenuStrip();

            var copyItem = new ToolStripMenuItem("Kopiér markerede");
            copyItem.Click += (s, e) => btnCopySelected_Click(s, new EventArgs());
            contextMenu.Items.Add(copyItem);

            var selectAllItem = new ToolStripMenuItem("Markér alle");
            selectAllItem.Click += (s, e) => {
                for (int i = 0; i < lstMissingValues.Items.Count; i++)
                    lstMissingValues.SetSelected(i, true);
            };
            contextMenu.Items.Add(selectAllItem);

            contextMenu.Items.Add(new ToolStripSeparator());

            var exportItem = new ToolStripMenuItem("Eksportér alle til fil");
            exportItem.Click += (s, e) => btnExportMissing_Click(s, new EventArgs());
            contextMenu.Items.Add(exportItem);

            lstMissingValues.ContextMenuStrip = contextMenu;
        }

        /// <summary>
        /// Kopiér markerede items til clipboard
        /// </summary>
        private void btnCopySelected_Click(object sender, EventArgs e)
        {
            if (lstMissingValues.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vælg items at kopiere");
                return;
            }

            var selectedText = string.Join("\r\n", lstMissingValues.SelectedItems.Cast<string>());
            Clipboard.SetText(selectedText);

            MessageBox.Show($"Kopieret {lstMissingValues.SelectedItems.Count} items til clipboard");
        }

        /// <summary>
        /// Eksporter alle missing values til tekstfil - MEMORY OPTIMIZED
        /// </summary>
        private async void btnExportMissing_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentMissingValuesFile) || !File.Exists(currentMissingValuesFile))
            {
                MessageBox.Show("Ingen data at eksportere");
                return;
            }

            saveFileDialog1.Filter = "Tekstfiler|*.txt|Alle filer|*.*";
            saveFileDialog1.FileName = $"missing_fk_values_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sb = new StringBuilder();

                    // Header information
                    sb.AppendLine("Missing Foreign Key Values Report");
                    sb.AppendLine("=");
                    sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    sb.AppendLine($"Parent CSV: {txtParentCSV.Text}");
                    sb.AppendLine($"Child CSV: {txtChildCSV.Text}");

                    // Column mapping info
                    sb.AppendLine($"Parent Column: {cmbParentColumn.Text}");
                    sb.AppendLine($"Child Column: {cmbChildColumn.Text}");

                    if (columnPairs.Count > 0)
                    {
                        sb.AppendLine("Additional sammensatte PK Columns:");
                        foreach (var pair in columnPairs)
                        {
                            sb.AppendLine($"  Parent {pair.PairNumber}: {pair.ParentComboBox.Text}");
                            sb.AppendLine($"  Child {pair.PairNumber}: {pair.ChildComboBox.Text}");
                        }
                    }

                    // Count total missing values
                    int totalMissing = File.ReadAllLines(currentMissingValuesFile).Length;
                    sb.AppendLine($"Total Missing Values: {totalMissing}");
                    sb.AppendLine();
                    sb.AppendLine("Missing Values:");
                    sb.AppendLine("---------------");

                    // Write header til fil
                    File.WriteAllText(saveFileDialog1.FileName, sb.ToString(), Encoding.UTF8);

                    // Append missing values direkte fra temp fil
                    using (var reader = new StreamReader(currentMissingValuesFile, Encoding.UTF8))
                    using (var writer = new StreamWriter(saveFileDialog1.FileName, append: true, Encoding.UTF8))
                    {
                        string line;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            await writer.WriteLineAsync(line);
                        }
                    }

                    MessageBox.Show($"Eksporteret {totalMissing} værdier til {saveFileDialog1.FileName}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fejl ved eksport: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Keyboard shortcuts til ListBox
        /// </summary>
        private void lstMissingValues_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                btnCopySelected_Click(sender, new EventArgs());
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                for (int i = 0; i < lstMissingValues.Items.Count; i++)
                    lstMissingValues.SetSelected(i, true);
                e.Handled = true;
            }
        }

        #endregion

        #region XML Conversion - Eksisterende Funktionalitet

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Tekst|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                textBoxID.Text = openFileDialog1.FileName;
                richTextBoxID.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }

            string[] items = richTextBoxID.Lines[0].Split(';');
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private int GetColCount(string tabelFilename)
        {
            bool firstFound = false;
            int currentRowCount = 0;
            foreach (string line in File.ReadAllLines(tabelFilename, Encoding.Default))
            {
                if (firstFound == false)
                {
                    if (line.Trim().ToLower() == "<row>")
                    {
                        firstFound = true;
                    }
                }
                else
                {
                    if (line.Trim().ToLower() == "</row>")
                    {
                        break;
                    }
                    currentRowCount++;
                }
            }
            return (currentRowCount);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isKolonnevalgValid().ToString().ToLower() != "true")
            {
                MessageBox.Show("De valgte kolonner overlapper hinanden!");
                return;
            }

            string destinationFilename = "";
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(textBoxTabel.Text) + "_new.xml";
            DialogResult res = saveFileDialog1.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                destinationFilename = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }

            if (File.Exists(textBoxTabel.Text) == false)
            {
                MessageBox.Show("Kunne ikke finde tabelfil:" + textBoxTabel.Text);
                return;
            }

            string schemaFilePath = Path.Combine(Path.GetDirectoryName(textBoxTabel.Text), Path.GetFileNameWithoutExtension(textBoxTabel.Text) + ".xsd");
            if (File.Exists(schemaFilePath) == false)
            {
                MessageBox.Show("Kunne ikke finde den tilhørende schemafil:" + schemaFilePath);
                return;
            }

            LavKolonneliste(@schemaFilePath);

            TextWriter tw = File.CreateText(destinationFilename);
            foreach (string line in File.ReadAllLines(textBoxTabel.Text, Encoding.UTF8))
            {
                if (line.Trim().ToLower() == "</table>")
                {
                    break;
                }
                tw.WriteLine(line);
            }

            progressBar1.Maximum = richTextBoxID.Lines.Length;
            progressBar1.Value = 0;
            progressBar1.Visible = true;

            foreach (string line in richTextBoxID.Lines)
            {
                progressBar1.Value++;
                if (line.Trim() == "") continue;

                string[] CSV_items = line.Trim().Split(';');

                tw.WriteLine("\t<row>");

                for (int i = 1; i < currentRowCount + 1; i++)
                {
                    bool fundet = false;
                    for (int CSV_No = 0; CSV_No < numList.Count; CSV_No++)
                    {
                        if (numList[CSV_No].Value == i)
                        {
                            fundet = true;
                            tw.WriteLine("\t\t" + "<c" + i.ToString() + ">" + CSV_items[CSV_No].Trim() + "</c" + i.ToString() + ">");
                        }
                    }

                    if (!fundet)
                    {
                        if (i == numericUpDownTekst.Value)
                        {
                            tw.WriteLine("\t\t" + "<c" + i.ToString() + ">" + textBoxStandardTekst.Text + "</c" + i.ToString() + ">");
                        }
                        else
                        {
                            if (colList[i - 1].dataType == "string")
                            {
                                if (colList[i - 1].nillable == "true")
                                {
                                    tw.WriteLine("\t\t" + "<c" + i.ToString() + " xsi:nil=\"true\"/>");
                                }
                                else if (colList[i - 1].nillable == "false")
                                {
                                    tw.WriteLine("\t\t" + "<c" + i.ToString() + ">" + "default" + "</c" + i.ToString() + ">");
                                }
                            }
                            else if (colList[i - 1].dataType == "date")
                            {
                                if (colList[i - 1].nillable == "true")
                                {
                                    tw.WriteLine("\t\t" + "<c" + i.ToString() + " xsi:nil=\"true\"/>");
                                }
                                else
                                    tw.WriteLine("\t\t" + "<c" + i.ToString() + ">" + "9999-12-31" + "</c" + i.ToString() + ">");
                            }
                            else if (colList[i - 1].dataType == "time")
                            {
                                if (colList[i - 1].nillable == "true")
                                {
                                    tw.WriteLine("\t\t" + "<c" + i.ToString() + " xsi:nil=\"true\"/>");
                                }
                                else
                                    tw.WriteLine("\t\t" + "<c" + i.ToString() + ">" + "23:59:59" + "</c" + i.ToString() + ">");
                            }
                            else if (colList[i - 1].dataType == "datetime")
                            {
                                if (colList[i - 1].nillable == "true")
                                {
                                    tw.WriteLine("\t\t" + "<c" + i.ToString() + " xsi:nil=\"true\"/>");
                                }
                                else
                                    tw.WriteLine("\t\t" + "<c" + i.ToString() + ">" + "9999-12-31T23:59:59" + "</c" + i.ToString() + ">");
                            }
                            else
                            {
                                if (colList[i - 1].nillable == "true")
                                {
                                    tw.WriteLine("\t\t" + "<c" + i.ToString() + " xsi:nil=\"true\"/>");
                                }
                                else if (colList[i - 1].nillable == "false")
                                {
                                    tw.WriteLine("\t\t" + "<c" + i.ToString() + ">" + "" + "</c" + i.ToString() + ">");
                                }
                            }
                        }
                    }
                }

                tw.WriteLine("\t</row>");
            }

            tw.WriteLine("</table>");
            tw.Close();

            progressBar1.Visible = false;
            MessageBox.Show("Done");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            buttonLæsCSV.Enabled = false;
            buttonTilføjRækker.Enabled = false;

            richTextBoxXML.Clear();
            openFileDialog1.Filter = "XML|*.xml|Tekst|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                textBoxTabel.Text = openFileDialog1.FileName;

                currentRowCount = GetColCount(textBoxTabel.Text);

                richTextBoxXML.Clear();
                int counter = 0;
                foreach (string line in File.ReadAllLines(textBoxTabel.Text, Encoding.Default))
                {
                    richTextBoxXML.AppendText(line + "\r\n");
                    counter++;
                    if (counter > 1000) break;
                }

                buttonLæsCSV.Enabled = true;
                buttonTilføjRækker.Enabled = true;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
        }

        private void LavKolonneliste(string xsdPath)
        {
            colList = new List<col>();

            foreach (string line in File.ReadAllLines(@xsdPath))
            {
                if (line.Contains("name=\"c"))
                {
                    int start = line.IndexOf("name=\"c");
                    int slut = line.IndexOf("\"", start + 7);

                    string cNum = line.Substring(start + 7, slut - (start + 7));

                    col c = new col();
                    c.colNr = int.Parse(cNum);

                    if (line.ToLower().Contains("nillable=\"true\"") == true)
                        c.nillable = "true";
                    else
                        c.nillable = "false";

                    if (line.ToLower().Contains("type=\"xs:integer\"") == true)
                        c.dataType = "integer";
                    else if (line.ToLower().Contains("type=\"xs:string\"") == true)
                        c.dataType = "string";
                    else if (line.ToLower().Contains("type=\"xs:decimal\"") == true)
                        c.dataType = "decimal";
                    else if (line.ToLower().Contains("type=\"xs:float\"") == true)
                        c.dataType = "float";
                    else if (line.ToLower().Contains("type=\"xs:double\"") == true)
                        c.dataType = "double";
                    else if (line.ToLower().Contains("type=\"xs:boolean\"") == true)
                        c.dataType = "boolean";
                    else if (line.ToLower().Contains("type=\"xs:date\"") == true)
                        c.dataType = "date";
                    else if (line.ToLower().Contains("type=\"xs:time\"") == true)
                        c.dataType = "time";
                    else if (line.ToLower().Contains("type=\"xs:datetime\"") == true)
                        c.dataType = "datetime";
                    else if (line.ToLower().Contains("type=\"xs:duration\"") == true)
                        c.dataType = "duration";
                    else MessageBox.Show("Kunne ikke bestemme datatype!");

                    colList.Add(c);
                }
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
        }

        private bool isKolonnevalgValid()
        {
            SortedList<decimal, string> sint = new SortedList<decimal, string>();

            sint.Add(numericUpDownTekst.Value, "sdfsdf");

            foreach (NumericUpDown num in numList)
            {
                try
                {
                    sint.Add(num.Value, "sdfsdf");
                }
                catch
                {
                    return (false);
                }
            }
            return (true);
        }

        #endregion
    }
}