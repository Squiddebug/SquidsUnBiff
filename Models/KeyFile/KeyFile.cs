using System;
using System.Collections.Generic;
using System.Text;

namespace SquidsUnBiff.Models.KeyFile
{
    class KeyFile
    {
        public Header Header;

        public List<FileTableEntry> FileTable;

        public List<KeyTableEntry> KeyTable;

        public KeyFile()
        {
            Header = new Header();

            FileTable = new List<FileTableEntry>();

            KeyTable = new List<KeyTableEntry>();
        }

        public void SaveToFile(string path)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Header);

            stringBuilder.Append(Environment.NewLine);

            foreach (var fileTableEntry in FileTable)
            {
                stringBuilder.Append(fileTableEntry + Environment.NewLine);
            }

            stringBuilder.Append(Environment.NewLine);

            foreach (var keyTableEntry in KeyTable)
            {
                stringBuilder.Append(keyTableEntry + Environment.NewLine);
            }

            System.IO.File.WriteAllText(path, stringBuilder.ToString());
        }
    }
}
