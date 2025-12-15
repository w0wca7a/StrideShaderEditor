namespace ShaderCompilerTest;

public partial class ShaderEditorForm
{
    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Упростите инициализацию объекта", Justification = "<Ожидание>")]
    private void InitializeComponents()
    {
        Text = "Stride Shader Editor";
        Width = 1200;
        Height = 800;

        // Menu
        var menuStrip = new MenuStrip();

        // File
        var fileMenu = new ToolStripMenuItem("&File");
        var newItem = new ToolStripMenuItem("&New", null, OnNewClicked);
        var openItem = new ToolStripMenuItem("&Open...", null, OnOpenClicked);
        var saveItem = new ToolStripMenuItem("&Save", null, OnSaveClicked);
        var exitItem = new ToolStripMenuItem("E&xit", null, OnExitClicked);

        fileMenu.DropDownItems.AddRange([newItem, openItem, saveItem, new ToolStripSeparator(), exitItem]);

        // Edit
        //var editMenu = new ToolStripMenuItem("&Edit");
        //var cutItem = new ToolStripMenuItem("Cu&t", null, OnCutClicked);
        //var copyItem = new ToolStripMenuItem("&Copy", null, OnCopyClicked);
        //var pasteItem = new ToolStripMenuItem("&Paste", null, OnPasteClicked);

        //editMenu.DropDownItems.AddRange(new ToolStripItem[] { cutItem, copyItem, pasteItem });

        // View
        //var viewMenu = new ToolStripMenuItem("&View");
        //var darkThemeItem = new ToolStripMenuItem("&Dark Theme", null, OnThemeClicked);
        //var lightThemeItem = new ToolStripMenuItem("&Light Theme", null, OnThemeClicked);

        //viewMenu.DropDownItems.AddRange(new ToolStripItem[] { darkThemeItem, lightThemeItem });

        // Help
        var helpMenu = new ToolStripMenuItem("&Help");
        var basicsItem = new ToolStripMenuItem("&Basics", null, OnBasicsClicked);
        var aboutItem = new ToolStripMenuItem("&About", null, OnAboutClicked);

        helpMenu.DropDownItems.AddRange([basicsItem,aboutItem]);

        // Add menu to menuStrip
        menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, /*editMenu, viewMenu, */helpMenu });

        // main split container
        mainSplit = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal,
            SplitterDistance = Height - 150
        };

        // editor and button container
        var editorPanel = new Panel
        {
            Dock = DockStyle.Fill
        };

        // editor split container
        var editorRenderSplit = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterDistance = Width / 25
        };

        // editor
        shaderCodeEditor = new TextBox
        {
            Multiline = true,
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 12),
            ForeColor = Color.White,
            Text = GetDefaultShaderCode(),
            BackColor = Color.DarkSlateGray,
            ScrollBars = ScrollBars.Vertical,
            
        };
        shaderCodeEditor.TextChanged += OnShaderCodeEditorTextChanged;

        // compile button
        var compileButton = new Button
        {
            Text = "Compile Shader",
            Dock = DockStyle.Bottom,
            Height = 40,
            BackColor = Color.SteelBlue,
            ForeColor = Color.White,
            Font = new Font("Arial", 10, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat
        };
        compileButton.FlatAppearance.BorderSize = 0;
        compileButton.Click += CompileButton_Click; // Добавляем обработчик события

        //render
        renderPanel.Dock = DockStyle.Fill;
        renderPanel.BackColor = Color.Black;
        renderPanel.Name = "render panel";

        //compile result
        errorOutput = new TextBox
        {
            Multiline = true,
            Dock = DockStyle.Fill,
            ReadOnly = true,
            BackColor = Color.Black,
            ForeColor = Color.White,
            Font = new Font("Consolas", 10)
        };

        editorPanel.Controls.Add(shaderCodeEditor);
        editorPanel.Controls.Add(compileButton);

        editorRenderSplit.Panel1.Controls.Add(editorPanel);
        editorRenderSplit.Panel2.Controls.Add(renderPanel);

        mainSplit.Panel1.Controls.Add(editorRenderSplit);
        mainSplit.Panel2.Controls.Add(errorOutput);

        // Создаем главный контейнер для правильного размещения меню
        var mainPanel = new Panel
        {
            Dock = DockStyle.Fill
        };
        mainPanel.Controls.Add(mainSplit);

        // Добавляем элементы на форму в правильном порядке
        Controls.AddRange([menuStrip, mainPanel]);
        //Controls.Add(mainSplit);

        // Важно: устанавливаем MainMenuStrip для правильной работы горячих клавиш
        MainMenuStrip = menuStrip;

    }

    private void OnShaderCodeEditorTextChanged(object sender, EventArgs e)
    {
        var text = (sender as TextBox).Text;

        
    }

    private void OnBasicsClicked(object? sender, EventArgs e)
    {
        var basics = new Basics();
        basics.Show();
    }

    private void OnNewClicked(object? sender, EventArgs e)
    {
        //shaderCodeEditor.Text = GetDefaultShaderCode();
        shaderCodeEditor.Text = GetDefaultShaderCode();
    }

    private void OnAboutClicked(object? sender, EventArgs e)
    {
        var about = new Form
        {
            HelpButton = false,
            MaximizeBox = false,
            MinimizeBox = false,
            Width = 500,
            Height = 300,
            FormBorderStyle = FormBorderStyle.FixedDialog
        };
        about.Show();
    }

    private void OnExitClicked(object? sender, EventArgs e)
    {
        base.Close();
        // TODO save to template
    }

    private void OnSaveClicked(object? sender, EventArgs e)
    {
        var saveDialog = new SaveFileDialog
        {
            Filter = "Shader Files (*.sdsl)|*.sdsl|All Files (*.*)|*.*",
            Title = "Save SDSL Shader File"
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(saveDialog.FileName, shaderCodeEditor.Text);
        }
    }

    private void OnOpenClicked(object? sender, EventArgs e)
    {
        var openDialog = new OpenFileDialog
        {
            Filter = "Shader Files (*.sdsl)|*.sdsl|All Files (*.*)|*.*",
            Title = "Open SDSL Shader File"
        };

        if (openDialog.ShowDialog() == DialogResult.OK)
        {
            shaderCodeEditor.Text = "\r\n" + File.ReadAllText(openDialog.FileName);
        }
    }

    private void CompileButton_Click(object? sender, EventArgs e)
    {
        CompileShader().Wait();
    }
}
