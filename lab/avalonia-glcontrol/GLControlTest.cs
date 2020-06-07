using OpenToolkit.Graphics.OpenGL;
using System.Drawing;

namespace example_avalonia_opengl
{

    public class GLControlTest : GLControl
    {

        const string VertexShaderSource = @"
            #version 330

            layout(location = 0) in vec4 position;

            void main(void)
            {
                gl_Position = position;
            }
        ";

        const string FragmentShaderSource = @"
            #version 330

            out vec4 outputColor;

            uniform vec4 inColor;

            void main(void)
            {
                outputColor = inColor;
            }
        ";

        // Points of a triangle in normalized device coordinates.
        readonly float[] Points = new float[] {
            // X, Y, Z, W
            -0.5f, 0.0f, 0.0f, 1.0f,
            0.5f, 0.0f, 0.0f, 1.0f,
            0.0f, 0.5f, 0.0f, 1.0f };

        int VertexShader;
        int FragmentShader;
        int ShaderProgram;
        int VertexBufferObject;
        int VertexArrayObject;

        protected override void Init()
        {            
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);
            GL.CompileShader(VertexShader);
            
            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);
            GL.CompileShader(FragmentShader);
            
            ShaderProgram = GL.CreateProgram();
            GL.AttachShader(ShaderProgram, VertexShader);
            GL.AttachShader(ShaderProgram, FragmentShader);
            GL.LinkProgram(ShaderProgram);
            
            VertexBufferObject = GL.GenBuffer();            
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Points.Length * sizeof(float), Points, BufferUsageHint.StaticDraw);
            
            var positionLocation = GL.GetAttribLocation(ShaderProgram, "position");            
            VertexArrayObject = GL.GenVertexArray();            
            GL.BindVertexArray(VertexArrayObject);
            GL.VertexAttribPointer(positionLocation, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(positionLocation);
        }

        protected override void GetFrame(OpenToolkit.Mathematics.Vector2i winSize)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, winSize.X, winSize.Y, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.AccumBufferBit);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);            
            GL.BindVertexArray(VertexArrayObject);            
            var colorLocation = GL.GetUniformLocation(ShaderProgram, "inColor");            
            GL.UseProgram(ShaderProgram);            
            GL.Uniform4(colorLocation, Color.Green);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);            
        }

    }

}