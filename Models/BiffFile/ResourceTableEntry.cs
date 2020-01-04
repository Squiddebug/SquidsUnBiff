namespace SquidsUnBiff.Models.BiffFile
{
    class ResourceTableEntry
    {
        public uint ResourceId;

        public uint Flags;

        public uint ResourceDataOffset;

        public uint ResourceDataSize;

        public string ResourceType;

        public override string ToString()
        {
            var text = string.Empty;

            text += "ResourceId: " + ResourceId + ", Flags: " + Flags + ", ResourceDataOffset: " + ResourceDataOffset + ", ResourceDataSize: " + ResourceDataSize + ", ResourceType: " + ResourceType;

            return text;
        }
    }
}
