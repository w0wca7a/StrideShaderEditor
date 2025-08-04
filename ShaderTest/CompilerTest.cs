using Stride.Core.IO;
using Stride.Core.Shaders.Ast;
using Stride.Core.Storage;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Shaders;
using Stride.Shaders.Compiler;
using CompilerParameters = Stride.Shaders.Compiler.CompilerParameters;
using Effect = Stride.Graphics.Effect;

namespace ShaderTest
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

            FileSystemProvider fs = new("/shader", tempDir);
            EffectCompiler effectCompiler = new(fs)
            {
                FileProvider = fs,
                UseFileSystem = true
            };

            var defaultrSource = EffectCompiler.DefaultSourceShaderFolder;
            effectCompiler.SourceDirectories.Add(defaultrSource);
            effectCompiler.SourceDirectories.Add(tempDir);

            var adapter = GraphicsAdapterFactory.Default;
            var deviceCreationFlags = DeviceCreationFlags.None;
            var graphicsProfile = GraphicsProfile.Level_11_0;
            var graphicsDevice = GraphicsDevice.New(
                    adapter,
                    deviceCreationFlags,
                    graphicsProfile
                    );

            var shaderCode = @"shader CustomShader : ComputeColor
{
    // Простой шейдер, отображающий градиент на основе координат UV
    override float4 Compute()
    {
         return float4(0.5f, 0.5f, 0.5f, 1.0f);
    }
};";

            File.WriteAllText(Path.Combine(tempDir, "CustomShader.sdsl"), shaderCode);
            var shaderMixinSource = new ShaderMixinSource
            {
                Name = "CustomShader"  // <- Вот это ключевое исправление!
            };
            //shaderMixinSource.Mixins.Add(new ShaderClassSource("ComputeColor"));
            shaderMixinSource.Mixins.Add(new ShaderClassSource("ComputeColor"));
            shaderMixinSource.Mixins.Add(new ShaderClassSource("Texturing"));
            shaderMixinSource.Mixins.Add(new ShaderClassSource("CustomShader"));


            var compilerParameters = new CompilerParameters
            {
                EffectParameters = EffectCompilerParameters.Default
            };

            var compilerResults = effectCompiler.Compile(shaderMixinSource, compilerParameters);

            if (compilerResults.HasErrors)
            {
                Console.WriteLine(string.Join(Environment.NewLine, compilerResults.Messages));
            }
            else
            {
                Console.WriteLine("Shader compiled successfully!");
                EffectInstance shaderEffect = new(new Effect(graphicsDevice, compilerResults.Bytecode.Result.Bytecode));
            }
        }
    }
}
