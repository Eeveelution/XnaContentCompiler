using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;


namespace XnaContentCompiler.Compilers {
    public class Texture2dCompiler : ContentCompiler {
        private byte[] _bitmap;
        private int    _width, _height;

        public Texture2dCompiler(string inFile) {
            using (Bitmap b = new(inFile))
            {
                BitmapData data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly,
                                             PixelFormat.Format32bppArgb);

                this._bitmap = new byte[b.Width * b.Height * 4];
                this._width  = b.Width;
                this._height    = b.Height;

                Marshal.Copy(data.Scan0, this._bitmap, 0, this._bitmap.Length);
                b.UnlockBits(data);
            }
        }
        protected override string ContentReader { get; set; } = "Microsoft.Xna.Framework.Content.Texture2DReader";
        protected override int ContentReaderVersion { get; set; } = 0;
        protected override MemoryStream WriteResource() {
            MemoryStream stream = new();
            using BinaryWriter writer = new(stream, Encoding.Default, true);

            writer.Write(1); //SurfaceFormat.Color
            writer.Write(this._width);
            writer.Write(this._height);
            writer.Write(1); //Number of Levels

            writer.Write(this._bitmap.Length);
            writer.Write(this._bitmap);

            return stream;
        }
    }
}
