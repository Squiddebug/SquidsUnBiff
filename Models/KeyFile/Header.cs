using System;

namespace SquidsUnBiff.Models.KeyFile
{
    class Header
    {
        public string FileName;

        public string Signature;

        public string Version;

        public uint FileTableEntries;

        public uint KeyTableEntries;

        public uint FileTableOffset;

        public uint KeyTableOffset;

        public uint BuildYear;

        public uint BuildDay;

        public override string ToString()
        {
            var text = string.Empty;
            text += "FileName: " + FileName + Environment.NewLine;
            text += "Signature: " + Signature + Environment.NewLine;
            text += "Version: " + Version + Environment.NewLine;
            text += "FileTableEntries: " + FileTableEntries + Environment.NewLine;
            text += "KeyTableEntries: " + KeyTableEntries + Environment.NewLine;
            text += "FileTableOffset: " + FileTableOffset + Environment.NewLine;
            text += "KeyTableOffset: " + KeyTableOffset + Environment.NewLine;
            text += "BuildYear: " + BuildYear + Environment.NewLine;
            text += "BuildDay: " + BuildDay + Environment.NewLine;

            return text;
        }
    }
}
