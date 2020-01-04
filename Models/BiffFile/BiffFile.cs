using System;
using System.Collections.Generic;
using System.Text;

namespace SquidsUnBiff.Models.BiffFile
{
    class BiffFile
    {
        public Header Header;

        public List<ResourceTableEntry> ResourceTable;

        public byte[] Data;

        public BiffFile()
        {
            Header = new Header();
            
            ResourceTable = new List<ResourceTableEntry>();
        }

        public void SaveToFile(string path)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Header);

            stringBuilder.Append(Environment.NewLine);

            foreach (var resourceTableEntry in ResourceTable)
            {
                stringBuilder.Append(resourceTableEntry + Environment.NewLine);
            }

            System.IO.File.WriteAllText(path, stringBuilder.ToString());
        }
    }
}
