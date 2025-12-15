using Stride.Core.DataSerializers;
using Stride.Core.IO;
using Stride.Core.Mathematics;
using Stride.Core.Shaders.Ast;
using Stride.Core.Shaders.Utility;


//using Stride.Core.Shaders.Utility;
using Stride.Engine;
using Stride.Games;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.ComputeEffect;
using Stride.Rendering.Materials.ComputeColors;
using Stride.Shaders;
using Stride.Shaders.Compiler;
using Stride.Shaders.Parser.Mixins;


//using Stride.Shaders.Compiler;
//using Stride.Shaders.Parser.Mixins;
using Buffer = Stride.Graphics.Buffer;
using Color = Stride.Core.Mathematics.Color;

namespace ShaderCompilerTest
{
    public class RenderBox(Stream stream) : Game
    {
        public GraphicsDevice GraphicsDevice => base.GraphicsDevice;
        private Buffer buffer;
        private MutablePipelineState pipelineState;
        private EffectInstance simpleEffect;

        private EffectInstance customEffectInstance;
        private ShaderClassString ShaderClassString;
        private ShaderClassSource ShaderClassSource;
        //private GraphicsDevice graphicsDevice;
        private readonly Stream imageStream = stream;


        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            var commandList = GraphicsContext.CommandList;
            commandList.ResetTargets();
            var gp = GraphicsDevice.Presenter;
            commandList.Clear(gp.BackBuffer, Color.Black);
            commandList.Clear(gp.DepthStencilBuffer, DepthStencilClearOptions.DepthBuffer | DepthStencilClearOptions.Stencil);
            // Render to the backbuffer
            commandList.SetRenderTargetAndViewport(gp.DepthStencilBuffer, gp.BackBuffer);

            if (buffer == null)
            {
                Init();
            }
            simpleEffect.UpdateEffect(GraphicsDevice);

            pipelineState.State.RootSignature = simpleEffect.RootSignature;
            pipelineState.State.EffectBytecode = simpleEffect.Effect.Bytecode;
            pipelineState.State.BlendState = BlendStates.Default;
            pipelineState.State.Output.CaptureState(commandList);
            pipelineState.Update();

            commandList.SetPipelineState(pipelineState.CurrentState);

            // Apply the effect
            simpleEffect.Apply(GraphicsContext);
            commandList.SetVertexBuffer(0, buffer, 0, 12);
            commandList.Draw(6);
        }

        private void Init()
        {
            buffer = Buffer.New(
                GraphicsDevice,
                [
                    // top left corner
                    new Vector3( 1f, -1f, 0), // 2
                    new Vector3(-1f, -1f, 0), //3
                    new Vector3(-1f,  1f, 0), // 0

                    // down right corner
                    new Vector3(-1f,  1f, 0), // 0
                    new Vector3( 1f,  1f, 0), // 1
                    new Vector3( 1f, -1f, 0), // 2
                ],
            BufferFlags.VertexBuffer, 
            GraphicsResourceUsage.Immutable);

            Texture whiteTexture = Texture.New2D(
                GraphicsDevice, 
                1, 
                1, 
                PixelFormat.R8G8B8A8_UNorm,
                [Color.Aquamarine]);

            string source = @"
shader CustomShader : ComputeColor
{
    // Simple shader with gray color
    override float4 Compute()
    {
        return float4(0.5f, 0.5f, 0.5f, 1.0f);
    }
};";

            var loadTexture = Texture.Load(GraphicsDevice, imageStream);
            var sr = loadTexture.IsShaderResource;
            simpleEffect = new EffectInstance(
                new Effect(GraphicsDevice,
                SpriteEffect.Bytecode));
            //simpleEffect.Parameters.Set(TexturingKeys.Texture0, whiteTexture);
            simpleEffect.Parameters.Set(TexturingKeys.Texture0, loadTexture);
            //simpleEffect.Parameters.Set(SpriteBaseKeys.MatrixTransform,
            //                Matrix.OrthoOffCenterRH(0, GraphicsDevice.Presenter.BackBuffer.Width,
            //               GraphicsDevice.Presenter.BackBuffer.Height, 0, 0, 1));
            //simpleEffect.Parameters.Set(
            //    SpriteBaseKeys.MatrixTransform,
            //    Matrix.Identity);
            simpleEffect.UpdateEffect(GraphicsDevice);

            //var loggerResult = new LoggerResult();
            //var source = ShaderLoader.ParseSource(shaderCode, loggerResult);
            //if (loggerResult.HasErrors)
            //{
            //    loggerResult.DumpMessages(ReportMessageLevel.Info, new Informator());
            //}

            //var shadername = source.Name;

            
            //var sh = new ShaderClassString(source.Name, shaderCode);

            //var CacheLoadSource = EffectBytecodeCacheLoadSource.DynamicCache;
            //var effectbyte = new EffectBytecode();
            //var eb = new EffectBytecodeCompilerResult(effectbyte, CacheLoadSource);
            //var log = eb.CompilationLog;
            //var effect = new Effect
            //    (GraphicsDevice,
            //    effectbyte
            //    );

            //var effect = new Effect(
            //    GraphicsDevice,
            //    //EffectBytecode. FromBytesSafe(Stride.Core.Shaders.Properties.Resources. CustomShader)
            //    );
            //customEffectInstance = new EffectInstance(effect);
            //customEffectInstance.Parameters.Set(MyCustomShaderKeys.MyColorParameter, new Color4(0.5f, 0.5f, 0.5f, 1.0f));
            //customEffectInstance.UpdateEffect(GraphicsDevice);

            pipelineState = new MutablePipelineState(GraphicsDevice);
            pipelineState.State.SetDefaults();
            pipelineState.State.InputElements = VertexPositionNormalTexture.Layout.CreateInputElements();
            pipelineState.State.PrimitiveType = PrimitiveType.TriangleList;
        }

        protected override void PrepareContext()
        {
            Context.InitializeDatabase = true;
            base.PrepareContext();
        }
    }
}
