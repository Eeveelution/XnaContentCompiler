using System.IO;
using System.Text;

namespace XnaContentCompiler {
    public abstract class ContentCompiler {
        private void WriteXnaHeader(BinaryWriter writer, byte xnaVersion, byte flags) {
            writer.Write((byte)'X');
            writer.Write((byte)'N');
            writer.Write((byte)'B');
            writer.Write((byte)'w');

            writer.Write(xnaVersion);
            writer.Write(flags);
        }

        private MemoryStream WriteResourceHeader() {
            MemoryStream stream = new();
            using BinaryWriter writer = new(stream, Encoding.Default, true);

            writer.Write7BitEncodedInt(1); //Type Reader Count
            writer.Write(this.ContentReader); //Content Reader
            writer.Write(this.ContentReaderVersion); //Content Reader Version
            writer.Write7BitEncodedInt(0); //Shared Resources
            writer.Write7BitEncodedInt(1); //Index of the Content reader to use (Not 0 Indexed)

            return stream;
        }

        public void Compile(string outFile) {
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream);

            this.WriteXnaHeader(writer, 1, 0);

            MemoryStream resourceHeader = this.WriteResourceHeader();
            MemoryStream resource = this.WriteResource();

            writer.Write(10 + (int)resourceHeader.Length + (int)resource.Length);  //Size

            writer.Write(resourceHeader.ToArray());
            writer.Write(resource.ToArray());

            File.WriteAllBytes(outFile, stream.ToArray());
        }

        #region Overrides

        public abstract string ContentReader { get; set; }
        public abstract int ContentReaderVersion { get; set; }
        public abstract MemoryStream WriteResource();


        #endregion

    }
}
