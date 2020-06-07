using Avalonia.Controls;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Graphics.OpenGL;
using Avalonia.Media.Imaging;
using Avalonia.Visuals.Media.Imaging;

// c# port of f# code for opentk 3 with adaptation for opentk 4
// source ref: https://gist.github.com/JaggerJo/59326790ee97a2c2ed61b62990ab98d6

namespace example_avalonia_opengl
{

    public class GLControl : Control
    {
        GameWindow win = null;

        protected virtual void GetFrame(OpenToolkit.Mathematics.Vector2i winSize)
        {
            GL.ClearColor(0.7f, 0.0f, 0.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.AccumBufferBit);
        }

        protected virtual void Init()
        {

        }

        public GLControl()
        {
            this.IsHitTestVisible = false;
        }

        protected override void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
        {
            var gameWindowSettings = new GameWindowSettings
            {
            };
            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new OpenToolkit.Mathematics.Vector2i(640, 480)
            };

            win = new GameWindow(gameWindowSettings, nativeWindowSettings);
            //win.IsVisible = false;
            win.MakeCurrent();            

            this.Init();
        }

        protected override Avalonia.Size MeasureOverride(Avalonia.Size availableSize)
        {
            System.Console.WriteLine($"MeasureOverride size:{availableSize}");
            win.Size = new OpenToolkit.Mathematics.Vector2i((int)availableSize.Width, (int)availableSize.Height);
            GL.Viewport(0, 0, win.Size.X, win.Size.Y);
            System.Console.WriteLine($"win.Size:{win.Size}");

            return availableSize;
        }

        public override void Render(Avalonia.Media.DrawingContext context)
        {
            System.Console.WriteLine("---> RENDER");
            this.GetFrame(win.Size);
            GL.Flush();
            win.SwapBuffers();

            var bitmap = new WriteableBitmap(
                new Avalonia.PixelSize(win.Size.X, win.Size.Y),
                new Avalonia.Vector(96.0, 96.0),
                new Avalonia.Platform.PixelFormat?(Avalonia.Platform.PixelFormat.Rgba8888));

            using (var l = bitmap.Lock())
            {
                GL.PixelStore(PixelStoreParameter.PackRowLength, l.RowBytes / 4);
                GL.ReadPixels(0, 0, win.Size.X, win.Size.Y, PixelFormat.Bgra, PixelType.UnsignedByte, l.Address);                
            }

            context.DrawImage(bitmap, 1.0,
                new Avalonia.Rect(bitmap.Size),
                new Avalonia.Rect(bitmap.Size),
                BitmapInterpolationMode.LowQuality);

            //bitmap.Save("/home/devel0/Desktop/testout.bmp.png");
        }

    }

}