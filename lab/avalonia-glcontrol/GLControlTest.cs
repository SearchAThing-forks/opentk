using OpenToolkit.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

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

        static int cnt = 0;

        bool shader_initialized = false;

        protected override void GetFrame(OpenToolkit.Mathematics.Vector2i winSize)
        {
            if (!shader_initialized)
            {
                System.Console.WriteLine("===== INIT"); ;
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

                DebugProc cback = (src, type, id, severity, length, message, userParam) =>
                {
                    var msg = Marshal.PtrToStringAuto(message);
                    System.Console.WriteLine($"GL ERROR:" + msg);
                };
                GL.Enable(EnableCap.DebugOutput);
                GL.DebugMessageCallback(cback, (IntPtr)null);

                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
                GL.BindVertexArray(VertexArrayObject);
                GL.UseProgram(ShaderProgram);

                shader_initialized = true;
            }
            GL.ClearColor(Color.LightYellow);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            System.Console.WriteLine($"***GetFrame");

            var colorLocation = GL.GetUniformLocation(ShaderProgram, "inColor");

            GL.Uniform4(colorLocation, cnt % 2 == 0 ? Color.Blue : Color.Red);
            ++cnt;
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            GL.Flush();
        }

    }

}