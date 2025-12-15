using Stride.Core.Assets;
using Stride.Core.IO;
using Stride.Shaders.Compiler;
using Stride.Shaders.Parser.Mixins;

public class Compiler
{
    private FileSystemProvider? fsProvider;
    private EffectCompiler effectCompiler;
    private ShaderSourceManager shSourceManager;
    private string Pool;
    private static Version strideVersion = typeof(Stride.Core.IO.DatabaseFileProviderService).Assembly.GetName().Version;
    private static string sysUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private static string shaderPath;

    public Compiler()
    {
        // C:\Users\C:\Users\damzi\.nuget\packages\stride.graphics\4.3.0.1\stride\Assets
        shaderPath = $"C:\\Users\\{sysUser}\\.nuget\\packages\\stride.graphics\\{strideVersion.ToString()}\\stride\\Assets";
        if (fsProvider == null)
        {
            fsProvider = new("/test", "");
        }

        effectCompiler = new(fsProvider)
        {
            FileProvider = fsProvider,
            UseFileSystem   = true,            
        };
        effectCompiler.SourceDirectories.Add(shaderPath);
        effectCompiler.SourceDirectories.Add(EffectCompiler.DefaultSourceShaderFolder);

        shSourceManager = new ShaderSourceManager(fsProvider);
        shSourceManager.LoadShaderSource(shaderPath);
    }

    public static void Main(string[] args)
    {
        var compiler = new Compiler();
        var a = compiler.fsProvider;
        var b = compiler.effectCompiler;
        var c = compiler.shSourceManager;
    }
}