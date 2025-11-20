using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace TestvaerkstedetToolkit
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
            SetupDashboard();
            SetupCardEvents();
        }

        private void SetupDashboard()
        {
            this.Text = "Testværkstedets Toolkit - Dashboard";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);
        }

        /// <summary>
        /// Tilføj hover effekter til alle kort
        /// </summary>
        private void SetupCardEvents()
        {
            SetupCardHoverEffects(cardCSVFK, btnCSVFKRepair);
            SetupCardHoverEffects(cardXMLFK, btnXMLFKRepair);
            SetupCardHoverEffects(cardXMLConv, btnXMLConversion);
            SetupCardHoverEffects(cardTableSplit, btnXMLTableSplitter);
        }

        /// <summary>
        /// Setup hover effekter for et enkelt kort
        /// </summary>
        private void SetupCardHoverEffects(Panel card, Button button)
        {
            // Card hover effects
            card.MouseEnter += (s, e) => {
                card.BackColor = Color.FromArgb(248, 248, 248);
                this.Cursor = Cursors.Hand;
            };

            card.MouseLeave += (s, e) => {
                card.BackColor = Color.White;
                this.Cursor = Cursors.Default;
            };

            // Button hover effects
            button.MouseEnter += (s, e) => {
                button.BackColor = Color.FromArgb(0, 100, 180);
            };

            button.MouseLeave += (s, e) => {
                button.BackColor = Color.FromArgb(0, 120, 215);
            };

            // Card click navigation - klik på kortet åbner værktøjet
            card.Click += (s, e) => button.PerformClick();

            // Rekursivt tilføj click event til child controls
            AddClickEventToChildren(card, button);
        }

        /// <summary>
        /// Tilføj click event til alle child controls i kortet
        /// </summary>
        private void AddClickEventToChildren(Control parent, Button targetButton)
        {
            foreach (Control child in parent.Controls)
            {
                if (child != targetButton) // Undgå button i sig selv
                {
                    child.Click += (s, e) => targetButton.PerformClick();
                    if (child.HasChildren)
                    {
                        AddClickEventToChildren(child, targetButton);
                    }
                }
            }
        }

        /// <summary>
        /// Custom paint event for kort med subtle shadow effect
        /// </summary>
        private void Card_Paint(object sender, PaintEventArgs e)
        {
            Panel card = sender as Panel;
            if (card == null) return;

            // Tegn subtle shadow effect
            Rectangle shadowRect = new Rectangle(2, 2, card.Width - 2, card.Height - 2);
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 0)))
            {
                e.Graphics.FillRectangle(shadowBrush, shadowRect);
            }

            // Tegn hvid baggrund over shadow
            Rectangle cardRect = new Rectangle(0, 0, card.Width - 2, card.Height - 2);
            using (SolidBrush cardBrush = new SolidBrush(card.BackColor))
            {
                e.Graphics.FillRectangle(cardBrush, cardRect);
            }

            // Tegn border
            using (Pen borderPen = new Pen(Color.FromArgb(220, 220, 220), 1))
            {
                e.Graphics.DrawRectangle(borderPen, cardRect);
            }
        }

        // Navigation Event Handlers
        private void btnCSVFKRepair_Click(object sender, EventArgs e)
        {
            // CSV FK Repair har sin egen dedikerede form
            try
            {
                var csvFKRepairForm = new CSVFKRepairForm();

                // Brug docking version i stedet for absolut positionering
                AddBackButtonWithDocking(csvFKRepairForm);

                csvFKRepairForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved åbning af CSV FK Repair:\n{ex.Message}",
                               "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXMLFKRepair_Click(object sender, EventArgs e)
        {
            // XML FK Repair har sin egen dedikerede form
            try
            {
                var xmlFKRepairForm = new XMLFKRepairForm();

                // Brug docking version i stedet for absolut positionering
                AddBackButtonWithDocking(xmlFKRepairForm);

                xmlFKRepairForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved åbning af XML FK Repair:\n{ex.Message}",
                               "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXMLConversion_Click(object sender, EventArgs e)
        {
            // XML Conversion har sin egen dedikerede form
            try
            {
                var conversionForm = new XMLConversionForm();

                // Brug docking version i stedet for absolut positionering
                AddBackButtonWithDocking(conversionForm);

                conversionForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved åbning af XML Conversion:\n{ex.Message}",
                               "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXMLTableSplitter_Click(object sender, EventArgs e)
        {
            // XML Table Splitter har sin egen dedikerede form
            try
            {
                var splitterForm = new XMLTableSplitterForm();

                // FIKSET: Brug docking version i stedet for absolut positionering
                AddBackButtonWithDocking(splitterForm);  // ← Ændret fra AddBackButton

                splitterForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved åbning af XML Table Splitter:\n{ex.Message}",
                               "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Åbn Form1 i single mode for det valgte værktøj
        /// </summary>
        private void OpenToolInSingleMode(Form1.ToolMode mode)
        {
            try
            {
                var toolForm = new Form1();
                toolForm.SetMode(mode);

                // Add back navigation
                AddBackButtonWithDocking(toolForm);

                toolForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved åbning af værktøj:\n{ex.Message}",
                               "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tilføj tilbage-til-dashboard knap til værktøjsforms
        /// </summary>
        private void AddBackButton(Form toolForm)
        {
            var backButton = new Button();
            backButton.Name = "backButton";
            backButton.Text = "← Tilbage til Dashboard";
            backButton.Size = new Size(180, 35);
            backButton.Location = new Point(15, 15);
            backButton.BackColor = Color.FromArgb(108, 117, 125);
            backButton.ForeColor = Color.White;
            backButton.FlatStyle = FlatStyle.Flat;
            backButton.FlatAppearance.BorderSize = 0;
            backButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            backButton.Cursor = Cursors.Hand;

            // Hover effect
            backButton.MouseEnter += (s, e) => backButton.BackColor = Color.FromArgb(90, 98, 104);
            backButton.MouseLeave += (s, e) => backButton.BackColor = Color.FromArgb(108, 117, 125);

            // Back button functionality
            backButton.Click += (s, e) => {
                toolForm.Close();
                this.Show();
                this.BringToFront();
            };

            // Handle tool form closing
            toolForm.FormClosed += (s, e) => {
                if (!this.Visible)
                {
                    this.Show();
                    this.BringToFront();
                }
            };

            // Add button til form
            toolForm.Controls.Add(backButton);
            backButton.BringToFront();

            // FORBEDRET layout justering
            AdjustFormLayoutForBackButton(toolForm, backButton);
        }


        /// <summary>
        /// Justér layout for at give plads til tilbage knappen
        /// </summary>
        private void AdjustFormLayoutForBackButton(Form form, Button backButton)
        {
            const int backButtonReservedHeight = 65;
            const int minimumTopMargin = 70;

            // VIGTIG: Gem original form størrelse først
            var originalFormHeight = form.Height;
            var originalClientHeight = form.ClientSize.Height;

            // Find alle top-level controls der skal flyttes
            var controlsToMove = new List<Control>();

            foreach (Control control in form.Controls)
            {
                if (control.Name == "backButton") continue;

                if (control.Top < minimumTopMargin)
                {
                    controlsToMove.Add(control);
                }
            }

            // FORØG form størrelse FØRST - så controls ikke bliver klemt
            form.Height = Math.Max(originalFormHeight + backButtonReservedHeight, 700);

            // Flyt controls ned UDEN at ændre deres størrelse
            foreach (Control control in controlsToMove)
            {
                control.Top += backButtonReservedHeight;

                // VIGTIG: BEVAR original højde - ikke reducer
                // Kommenteret ud: groupBox.Height -= 20; 
            }

            // Sørg for at ProgressBar er placeret korrekt i bunden
            var progressBar = form.Controls.OfType<ProgressBar>().FirstOrDefault();
            if (progressBar != null)
            {
                // Placer progress bar i bunden af den nye form størrelse
                progressBar.Top = form.ClientSize.Height - 50;
                progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }

            // Debug output for at verificere ændringer
            System.Diagnostics.Debug.WriteLine($"Form adjusted: Original height {originalFormHeight} -> New height {form.Height}");
        }

        /// <summary>
        /// Alternative metode: Brug Dock og Margins i stedet for absolut positionering
        /// </summary>
        private void AddBackButtonWithDocking(Form toolForm)
        {
            // Opret container panel for tilbage-knap
            var backPanel = new Panel();
            backPanel.Height = 50;  // Reduceret fra 60 til 50
            backPanel.Dock = DockStyle.Top;
            backPanel.BackColor = Color.FromArgb(248, 248, 248);
            backPanel.Padding = new Padding(15, 8, 15, 8);  // Reduceret padding

            var backButton = new Button();
            backButton.Name = "backButton";
            backButton.Text = "← Tilbage til Dashboard";
            backButton.Size = new Size(180, 35);
            backButton.Dock = DockStyle.Left;
            backButton.BackColor = Color.FromArgb(108, 117, 125);
            backButton.ForeColor = Color.White;
            backButton.FlatStyle = FlatStyle.Flat;
            backButton.FlatAppearance.BorderSize = 0;
            backButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            backButton.Cursor = Cursors.Hand;

            // Hover effects
            backButton.MouseEnter += (s, e) => backButton.BackColor = Color.FromArgb(90, 98, 104);
            backButton.MouseLeave += (s, e) => backButton.BackColor = Color.FromArgb(108, 117, 125);

            // Navigation logic
            backButton.Click += (s, e) => {
                toolForm.Close();
                this.Show();
                this.BringToFront();
            };

            toolForm.FormClosed += (s, e) => {
                if (!this.Visible)
                {
                    this.Show();
                    this.BringToFront();
                }
            };

            // Add til form structure
            backPanel.Controls.Add(backButton);
            toolForm.Controls.Add(backPanel);
            backPanel.BringToFront();

            // Denne metode bruger Dock og påvirker IKKE eksisterende controls
        }

        /// <summary>
        /// Handle form closing - vis dashboard igen hvis skjult
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Application.Exit(); // Luk hele applikationen når dashboard lukkes
            base.OnFormClosed(e);
        }

        /// <summary>
        /// Handle window state changes
        /// </summary>
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);
            if (value && WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                BringToFront();
            }
        }
    }
}