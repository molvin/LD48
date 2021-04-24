using Netkraft.Messaging;
/// <summary>
///data structures and stuff
/// </summary>

namespace GameStructure
{
    [System.Serializable]
    public struct TimeLine : IWritable
    {
        public Block[] timeLine;
    }

    [System.Serializable]
    public struct Block : IWritable
    {
        public ushort level;
        public Attribute[] mods;
        public Character[] characters;
        public int[] branches;
    }

    [System.Serializable]
    public struct Character : IWritable
    {
        [SkipIndex] public int Id;
        [SkipIndex] public int GridId;
        public string name;
        public byte color;
        public byte role;
        public Attribute[] attributes;
        public InputFrame[] timeLine;
    }

    [System.Serializable]
    public struct Attribute : IWritable
    {
        public byte type;
        public ushort value;
    }

    [System.Serializable]
    public struct InputFrame : IWritable
    {
        public byte action;
        public ushort cell;
    }
}

