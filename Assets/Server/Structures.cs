using Netkraft.Messaging;
<<<<<<< HEAD
//data structures and stuff
namespace GameStructure
{
    [Writable]
    public struct LDTimeLine
    {
        public LDBlock[] timeLine;
    }
    [Writable]
    public struct LDTimeLineBranchRequest
    {
        public int branchBlockIndex;
        public LDBlock[] timeLine;
    }
    [Writable]
    public struct LDBlock
    {
        public ushort level;
        public LDAttribute[] mods;
        public LDCharacter[] characters;
        public int[] branches;
    }
    [Writable]
    public struct LDCharacter
    {
        public string name;
        public byte color;
        public byte role;
        public LDAttribute[] attributes;
        public LDInputFrame[] timeLine;
    }
    [Writable]
    public struct LDAttribute
=======
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
>>>>>>> 2f9d67a796f61b4f6d4faba62faa79f6b72bcb65
    {
        public byte type;
        public ushort value;
    }
<<<<<<< HEAD
    [Writable]
    public struct LDInputFrame
=======

    [System.Serializable]
    public struct InputFrame : IWritable
>>>>>>> 2f9d67a796f61b4f6d4faba62faa79f6b72bcb65
    {
        public byte action;
        public ushort cell;
    }
}
<<<<<<< HEAD
=======

>>>>>>> 2f9d67a796f61b4f6d4faba62faa79f6b72bcb65
