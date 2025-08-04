// Shader compiler server https://github.com/stride3d/stride/blob/master/sources/tools/Stride.EffectCompilerServer/EffectCompilerServer.cs
// Window from code https://github.com/microdee/xenko-window-from-code/blob/master/FormsLlGfxTest/MyGame.cs
// CodeOnlyGame https://github.com/xen2/Xenko.CodeOnlySample/blob/master/Xenko.CodeOnlySample/CodeOnlyGame.cs
// database https://gist.github.com/aikixd/b0decb0cfc28bbdacb46d953691f76fd

// Источники шейдеров
// var effectCompiler = new EffectCompiler(провайдер);
// effectCompiler.SourceDirectories.Add(EffectCompilerBase.DefaultSourceShaderFolder);
// effectCompiler.FileProvider = провайдер;

using Stride.Core.IO;
using Stride.Engine;
using Stride.Games;
using Stride.Graphics;
using Stride.Graphics.GeometricPrimitives;
using Stride.Rendering;
using Stride.Rendering.Materials.ComputeColors;
using Stride.Shaders;
using Stride.Shaders.Compiler;
using Stride.Shaders.Parser.Mixins;
using Color = Stride.Core.Mathematics.Color;
//using CompilerParameters = Stride.Shaders.Compiler.CompilerParameters;

namespace ShaderTest;


public partial class ShaderEditorForm : Form
{
    private SplitContainer mainSplit;
    private TextBox shaderCodeEditor;
    private readonly Panel renderPanel;
    private TextBox errorOutput;
    private Game game;
    private EffectInstance shaderEffect;
    private DateTime lastCompileTime = DateTime.MinValue;
    private nint handler;

    // первая коррекция
    private GeometricPrimitive quadPrimitive;
    FileSystemProvider fs;
    EffectCompiler effectCompiler;
    ShaderSourceManager shm;
    string DefaultrSource = "";
    private Stream imageStream;

    public GameContextWinforms GameContext;
    public RenderBox RenderBox;

    public ShaderEditorForm()
    {
        renderPanel = new Panel();
        InitializeComponents();
        OpenFileDialog openImage = new OpenFileDialog
        {
            Filter = //"jpg |*.jpg|jpeg |*.jpeg|" +
                     //"png |*.png|bmp |*.bmp|" +
            "All media types |*.jpg||*.jpeg|*.png|*.bmp|*.tga|*.tiff|" +
            "All Files (*.*)|*.*",
            Title = "Open Image"
        };

        if (openImage.ShowDialog() == DialogResult.OK)
        {
            imageStream = openImage.OpenFile();
            //shaderCodeEditor.Text = "\r\n" + File.ReadAllText(openDialog.FileName);
        }
        else return;
        InitializeStride(renderPanel);
    }

    private void InitializeStride(Control control)
    {
        fs = new("/data1", "");
        effectCompiler = new(fs)
        {
            FileProvider = fs
        };
        effectCompiler.SourceDirectories.Add(EffectCompiler.DefaultSourceShaderFolder);     
                       
        //shm = new ShaderSourceManager(fs);

        GameContext = new GameContextWinforms(
                //renderPanel,
                control,
                requestedWidth: control.Width,
                requestedHeight: control.Height
                );
        handler = GameContext.Control.Handle;
        RenderBox = new RenderBox(imageStream)
        {
            GraphicsDeviceManager =
                {
                    PreferredGraphicsProfile = [GraphicsProfile.Level_11_0],
                    PreferredBackBufferWidth = control.ClientSize.Width,
                    PreferredBackBufferHeight = control.ClientSize.Height
                },
            AutoLoadDefaultSettings = true,
        };

        var thread = new Thread(() =>
        {
            if (renderPanel.InvokeRequired)
            {
                try
                {
                    renderPanel.Invoke(() => RenderBox.Run(GameContext));
                }
                catch (Exception ex)
                {
                    // Обработка ошибок в UI-потоке
                    renderPanel.Invoke(() =>
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    });
                }
            }
        })
        {
            IsBackground = true,
            Priority = ThreadPriority.AboveNormal
        };
        thread.Start();
        




        //var instance = new EffectInstance(
        //        new Effect(RenderBox.GraphicsDevice,
        //        SpriteEffect.Bytecode));
        //instance.Parameters.Set(
        //    SpriteBaseKeys.MatrixTransform,
        //    Matrix.Identity);
        //instance.UpdateEffect(RenderBox.GraphicsDevice);

        //CompilerResults cr = new CompilerResults();


    }

    private void InitializeStride1()
    {
        game = new Game();
        // ---Создание графического адаптера
        // 1. Получаем графический адаптер (видеокарту)
        var adapter = GraphicsAdapterFactory.Default;

        // 2. Создаем параметры устройства
        var deviceCreationFlags = DeviceCreationFlags.None; // или, например, DeviceCreationFlags.Debug
        var graphicsProfile = GraphicsProfile.Level_11_0;   // Указываем уровень Direct3D/OpenGL

        // 3. Создаем GraphicsDevice
        var graphicsDevice = GraphicsDevice.New(
            adapter,
            deviceCreationFlags,
            graphicsProfile
        //deviceWindowHandle: windowHandle // HWND (Windows), NSView (macOS) и т. д.
        );



        // Создание менеджера графических адаптеров
        // 1. Создание GraphicsDeviceManager
        //game.GraphicsDeviceManager.PreferredBackBufferWidth = renderPanel.Width;
        //game.GraphicsDeviceManager.PreferredBackBufferHeight = renderPanel.Height;
        //game.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
        //game.GraphicsDeviceManager.PreferredDepthStencilFormat = PixelFormat.D24_UNorm_S8_UInt;
        //game.GraphicsDeviceManager.PreferredColorSpace = ColorSpace.Linear;
        //game.GraphicsDeviceManager.ShaderProfile = GraphicsProfile.Level_11_0;
        //game.GraphicsDeviceManager.RequiredAdapterUid = graphicsDevice.Adapter.AdapterUid;
        //game.GraphicsDeviceManager.PreferredGraphicsProfile = [GraphicsProfile.Level_11_0];
        //game.GraphicsDeviceManager.DeviceCreationFlags = DeviceCreationFlags.None;
        //game.GraphicsDeviceManager.ApplyChanges();

        //FileSystemProvider fs = new("/data", "");

        //var effectCompiler = new EffectCompiler(fs);


        //CompilerParameters param = new CompilerParameters();
        //ShaderSource shc;

        //var result = effectCompiler.Compile(,GetDefaultShaderCode(), new CompilerParameters(param)
        //{
        //    EffectParameters = EffectCompilerParameters.Default
        //    //Profile = graphicsDevice.ShaderProfile
        //});

        var sys = game.GameSystems;
        //game.Context.DeviceCreationFlags = DeviceCreationFlags.VideoSupport;

        //game.Window.IsBorderLess = true;
        //game.Window.AllowUserResizing = true;

        object d = null;
        //renderPanel.Handle

        var _windowHandle = new WindowHandle(AppContextType.Desktop, null, Handle);
        ////var form = (Form)FromHandle(game.Window.NativeWindow.Handle);
        Form? form = FromHandle(Handle) as Form;
        //Form? form = FromHandle(this.
        //form.TopLevel = false;
        //form.FormBorderStyle = FormBorderStyle.None;
        //form.Dock = DockStyle.Fill;
        //form.Visible = true;
        //renderPanel.Controls.Add(form);
        game.Script.AddTask(async () =>
        {
            await CompileShader();

            while (true)
            {
                game.GraphicsContext.CommandList.Clear(
                    game.GraphicsDevice.Presenter.BackBuffer,
                    Color.Black);

                if (shaderEffect != null)
                {
                    // Рендерим полноэкранный квад с нашим шейдером
                    game.GraphicsContext.CommandList.SetRenderTargetAndViewport(
                        game.GraphicsDevice.Presenter.DepthStencilBuffer,
                        game.GraphicsDevice.Presenter.BackBuffer);

                    //shaderEffect.Parameters.Set(TransformationKeys.WorldViewProjection, Matrix.Identity);
                    //shaderEffect.Apply(game.GraphicsContext);

                    //game.GraphicsDevice.DrawQuad();
                    quadPrimitive.Draw(game.GraphicsContext, shaderEffect);
                }

                await game.Script.NextFrame();
            }
        });
        var gameContext = new GameContextWinforms(
            control: renderPanel,
            isUserManagingRun: true); // Позволяет вручную управлять игровым циклом

        //game.Run(GameContextType.Embedded);
        game.Run(gameContext);
    }

        
    private async Task CompileShader()
    {

        try
        {
            var shaderCode = shaderCodeEditor.Text;

            // Создаем временный .sdsl файл
            var tempDir = Path.Combine(Path.GetTempPath(), "StrideShaderEditor");
            Directory.CreateDirectory(tempDir);
            var tempFile = Path.Combine(tempDir, "CustomShader.sdsl");
            File.WriteAllText(tempFile, shaderCode);

            try
            {
                // Настройки компилятора
                var shaderMixinSource = new ShaderMixinSource
                {
                    Name = "CustomShader"  // <- Вот это ключевое исправление!
                };
                shaderMixinSource.Mixins.Add(new ShaderClassSource("CustomShader"));

                var compilerParameters = new CompilerParameters
                {
                    EffectParameters = new EffectCompilerParameters
                    {
                        Platform = GraphicsPlatform.Direct3D11,
                        Profile = GraphicsProfile.Level_11_0
                    }
                };




                // Инициализация компилятора
                var defaultrSource = EffectCompilerBase.DefaultSourceShaderFolder;

                // var effectCompiler = new EffectCompiler(провайдер);
                // effectCompiler.SourceDirectories.Add(EffectCompilerBase.DefaultSourceShaderFolder);
                // effectCompiler.FileProvider = провайдер;

                effectCompiler.SourceDirectories.Add(tempDir);
                effectCompiler.SourceDirectories.Add(defaultrSource);



                // Компиляция шейдера
                var compilerResults = effectCompiler.Compile(shaderMixinSource, compilerParameters);

                if (compilerResults.HasErrors)
                {
                    errorOutput.Text = string.Join(Environment.NewLine, compilerResults.Messages);
                    shaderEffect = null;
                }
                else
                {
                    errorOutput.Text = "Shader compiled successfully!";
                    shaderEffect = new EffectInstance(new Effect(game.GraphicsDevice, compilerResults.Bytecode.Result.Bytecode));
                }
            }
            finally
            {
                // Удаляем временные файлы
                try { File.Delete(tempFile); } catch { }
                try { Directory.Delete(tempDir); } catch { }
            }
        }
        catch (Exception ex)
        {
            errorOutput.Text = $"Error: {ex.Message}";
        }
    }
    /*
    private void OnShaderCodeChanged(object sender, EventArgs e)
    {
        // Дебаунс компиляции, чтобы не компилировать на каждое нажатие клавиши
        if ((DateTime.Now - lastCompileTime).TotalSeconds > 1)
        {
            lastCompileTime = DateTime.Now;
            game.Script.AddTask(CompileShader);
        }
    }
    */
    private static string GetDefaultShaderCode()
    {
        return @"
shader CustomShader : ComputeColor
{
    // Simple shader with gray color
    override float4 Compute()
    {
        return float4(0.5f, 0.5f, 0.5f, 1.0f);
    }
};";
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new ShaderEditorForm());
    }
}