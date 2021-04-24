using Netkraft.Messaging;
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
        public int Id;
        public int GridId;
        public byte color;
        public byte role;
        public LDAttribute[] attributes;
        public LDInputFrame[] timeLine;
    }
    [Writable]
    public struct LDAttribute
    {
        public byte type;
        public ushort value;
    }
    [Writable]
    public struct LDInputFrame
    {
        public byte action;
        public ushort cell;
    }
}
