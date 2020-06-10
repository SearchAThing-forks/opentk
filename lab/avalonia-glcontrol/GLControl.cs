using Avalonia.Controls;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Graphics.OpenGL;
using Avalonia.Media.Imaging;
using Avalonia.Visuals.Media.Imaging;
using System.Threading;

// c# port of f# code for opentk 3 with adaptation for opentk 4
// source ref: https://gist.github.com/JaggerJo/59326790ee97a2c2ed61b62990ab98d6

namespace example_avalonia_opengl
{

    public class GLControl : Control
    {
        public GameWindow win = null;

        protected virtual void GetFrame(OpenToolkit.Mathematics.Vector2i winSize)
        {

        }

        public GLControl()
        {
            this.IsHitTestVisible = false;
        }

        protected override void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
        {
            System.Console.WriteLine($"=== ATTACHED TO VISUAL TREE");

            var gameWindowSettings = new GameWindowSettings
            {
            };
            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new OpenToolkit.Mathematics.Vector2i(640, 480)
            };

            win = new GameWindow(gameWindowSettings, nativeWindowSettings);
            win.IsVisible = true;
            //win.MakeCurrent();
        }

        protected override void OnDetachedFromVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
        {
            System.Console.WriteLine($"=== DETACHED FROM VISUAL TREE");
        }

        protected override Avalonia.Size MeasureOverride(Avalonia.Size availableSize)
        {
            System.Console.WriteLine($"=== MEASURE OVERRIDE");

            System.Console.WriteLine($"MeasureOverride size:{availableSize}");
            win.Size = new OpenToolkit.Mathematics.Vector2i((int)availableSize.Width, (int)availableSize.Height);            
            System.Console.WriteLine($"win.Size:{win.Size}");            
            win.MakeCurrent();

            GL.Viewport(0, 0, win.Size.X, win.Size.Y);
            //win.SwapBuffers();

            return availableSize;
        }

        public override void Render(Avalonia.Media.DrawingContext context)
        {
            System.Console.WriteLine($"=== RENDER");

            this.GetFrame(win.Size);
            GL.Flush();
            //win.SwapBuffers();

            using (var bitmap = new WriteableBitmap(
                new Avalonia.PixelSize(win.Size.X, win.Size.Y),
                new Avalonia.Vector(96.0, 96.0),
                new Avalonia.Platform.PixelFormat?(Avalonia.Platform.PixelFormat.Rgba8888)))
            {

                using (var l = bitmap.Lock())
                {
                    GL.PixelStore(PixelStoreParameter.PackRowLength, l.RowBytes / 4);
                    GL.ReadPixels(0, 0, win.Size.X, win.Size.Y, PixelFormat.Bgra, PixelType.UnsignedByte, l.Address);
                }

                context.DrawImage(bitmap, 1.0,
                    new Avalonia.Rect(bitmap.Size),
                    new Avalonia.Rect(bitmap.Size),
                    BitmapInterpolationMode.LowQuality);
            }

            //bitmap.Save("/home/devel0/Desktop/testout.bmp.png");
        }

    }

}