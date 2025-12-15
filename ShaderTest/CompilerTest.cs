using Stride.Core.IO;
using Stride.Core.Shaders.Ast;
using Stride.Core.Storage;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Shaders;
using Stride.Shaders.Compiler;
using Stride.Shaders.Parser;
using CompilerParameters = Stride.Shaders.Compiler.CompilerParameters;
using Effect = Stride.Graphics.Effect;

namespace ShaderCompilerTest
{
    public class CompilerTest
    {
        public static void Main()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "StrideShaderEditor");
            Directory.CreateDirectory(tempDir);

            // Create and mount database file system
            var objDatabase = ObjectDatabase.CreateDefaultDatabase();

            // Only set a mount path if not mounted already
            var vfs = VirtualFileSystem.ApplicationDatabasePath;
            var mountPath = VirtualFileSystem.ResolveProviderUnsafe("/asset", true).Provider == null ? "/asset" : null;
            new DatabaseFileProvider(objDatabase, mountPath);

            //Shader shader;
    //        var strideShadersPath = Path.Combine(
    //Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
    //"Stride", "Sources", "Engine", "Stride.Shaders", "ShaderBase");

            var SysShadersDir = Path.Combine(Directory.GetCurrentDirectory(), "SystemShaders");
            FileSystemProvider sysShaders = new("/shaders", SysShadersDir);
            FileSystemProvider fs = new("/shader", tempDir);
            EffectCompiler effectCompiler = new(fs)
            {
                FileProvider = fs,
                UseFileSystem = true
            };

            //var defaultrSource = EffectCompiler.DefaultSourceShaderFolder;
            //effectCompiler.SourceDirectories.Add(defaultrSource);
            effectCompiler.SourceDirectories.Add(SysShadersDir);

            var adapter = GraphicsAdapterFactory.Default;
            var deviceCreationFlags = DeviceCreationFlags.None;
            var graphicsProfile = GraphicsProfile.Level_11_0;
            var graphicsDevice = GraphicsDevice.New(
                    adapter,
                    deviceCreationFlags,
                    graphicsProfile
                    );

            var shaderCode = @"
//shader CustomShader
//{
//stage stream float4 PSMain(float4 Position : SV_Position) : SV_Target0
//        {
//            // Используем мировую координату для изменения цвета (например, зелёный + высота)
//            stage float heightFactor = saturate(Position.y * 0.1); // нормализуем по высоте
//            return float4(0, 1 * heightFactor, 0, 1);
//        }
//};
shader CustomShader : ComputeColor
{
    // Простой шейдер, отображающий градиент на основе координат UV
    override float4 Compute()
    {
         return float4(0.5f, 0.5f, 0.5f, 1.0f);
    }
};
";

            File.WriteAllText(Path.Combine(tempDir, "CustomShader.sdsl"), shaderCode);


            var shaderMixinSource = new ShaderMixinSource();
            var source = new ShaderClassSource("CustomShader");
            shaderMixinSource.Mixins.Add(source);
            var shaderMixinParserMain = new ShaderMixinParser(sysShaders);
            var parse = shaderMixinParserMain.Parse(shaderMixinSource);
            var manager = shaderMixinParserMain.SourceManager;

            //var shaderMixinParser1 = new ShaderMixinParser(sysShaders);
            
            var parserResult = StrideShaderParser.TryParse(shaderCode, "CustomShader");
            //var parseTime = parserResult.TimeToParse;
            //var parsedShader = parserResult.Shader;
            //var firstDeclarations = parsedShader.Declarations[0];

            //var shaderMixinParserCustom = new ShaderMixinParser(sysShaders).Parse(shaderMixinSource);
            //ShaderMixinSource source1 = new ShaderMixinSource();
            //var par = source1.Parent;
            //var par1 = source1.Mixins;

            //var parseCust = shaderMixinParserCustom.Parse(shaderMixinSource);
            //var managerCust = shaderMixinParserCustom.SourceManager;


            //{
            //    Name = "CustomShader"  // <- Вот это ключевое исправление!
            //};
            shaderMixinSource.Mixins.Add(new ShaderClassSource("ComputeColor"));
            //shaderMixinSource.Mixins.Add(new ShaderClassSource("ComputeColor"));
            shaderMixinSource.Mixins.Add(new ShaderClassSource("Texturing"));
            //foreach(var file in )


            var compilerParameters = new CompilerParameters
            {
                EffectParameters = EffectCompilerParameters.Default
            };

            //var compilerResults = effectCompiler.Compile(shaderMixinSource, compilerParameters);

            //if (compilerResults.HasErrors)
            //{
            //    Console.WriteLine(string.Join(Environment.NewLine, compilerResults.Messages));
            //}
            //else
            //{
            //    Console.WriteLine("Shader compiled successfully!");
            //    EffectInstance shaderEffect = new(new Effect(graphicsDevice, compilerResults.Bytecode.Result.Bytecode));
            //}
        }
    }
}
