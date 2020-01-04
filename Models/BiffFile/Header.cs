using System;

namespace SquidsUnBiff.Models.BiffFile
{
    class Header
    {
        public string FileName;

        public string Signature;

        public string Version;

        public uint ResourceCount;

        public uint ResourceTableOffset;

        public override string ToString()
        {
            var text = string.Empty;
            text += "FileName: " + FileName + Environment.NewLine;
            text += "Signature: " + Signature + Environment.NewLine;
            text += "Version: " + Version + Environment.NewLine;
            text += "ResourceCount: " + ResourceCount + Environment.NewLine;
            text += "ResourceTableOffset: " + ResourceTableOffset + Environment.NewLine;

            return text;
        }
    }
}
