namespace SquidsUnBiff.Models.KeyFile
{
    class FileTableEntry
    {
        public string FileName;
        public uint FileSize;
        public uint NameOffset;
        public uint NameSize;
        
        public override string ToString()
        {
            var text = string.Empty;

            text += "FileName: " + FileName + ", FileSize: " + FileSize + ", NameOffset: " + NameOffset + ", NameSize: " + NameSize;

            return text;
        }
    }
}
