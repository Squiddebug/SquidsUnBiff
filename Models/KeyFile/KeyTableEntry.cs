namespace SquidsUnBiff.Models.KeyFile
{
    class KeyTableEntry
    {
        public string ResourceName;

        public string ResourceType;

        public uint ResourceId;

        public uint Flags;

        public string Biff;

        public override string ToString()
        {
            var text = string.Empty;

            text += "ResourceName: " + ResourceName + ", ResourceType: " + ResourceType + ", ResourceId: " + ResourceId + ", Flags: " + Flags + ", Biff: " + Biff;

            return text;
        }
    }
}
