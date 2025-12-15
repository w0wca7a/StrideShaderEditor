using System.Diagnostics;

namespace ShaderCompilerTest
{
    internal class Basics : Form
    {
        private WebBrowser helpBrowser;
        private Button closeButton;
        private Panel mainPanel;

        public Basics()
        {
            InitializeForm();
            CreateControls();
            LoadHelpContent();
        }

        private void InitializeForm()
        {
            // Basic form configuration
            this.Text = "Basics - Stride Shader Help";
            this.Width = 900;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private void CreateControls()
        {
            // Main container panel
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Help browser control
            helpBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill,
                IsWebBrowserContextMenuEnabled = false,
                WebBrowserShortcutsEnabled = false,
                AllowWebBrowserDrop = false
            };
            helpBrowser.Navigating += HelpBrowser_Navigating;

            // Close button
            closeButton = new Button
            {
                Text = "Close",
                Size = new Size(100, 40),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => this.Close();

            // Layout setup
            var buttonPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            buttonPanel.Controls.Add(closeButton);

            mainPanel.Controls.Add(helpBrowser);
            mainPanel.Controls.Add(buttonPanel);
            this.Controls.Add(mainPanel);
        }

        private void LoadHelpContent()
        {
            string file = String.Concat(Application.StartupPath, "doc/_basics.html");
            helpBrowser.DocumentText = File.ReadAllText(file);
        }

        private void HelpBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // Handle external links
            if (e.Url != null && !e.Url.ToString().StartsWith("about:blank"))
            {
                e.Cancel = true;
                OpenInSystemBrowser(e.Url.ToString());
            }
        }

        private void OpenInSystemBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot open link: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
