using Netkraft.Messaging;
namespace LD48_Server.Serlilizer
{
    /// <summary>
    ///data structures and stuff
    /// </summary>
    struct TimeLine : IWritable
    {
        public Block[] timeLine;
    }
    struct Block : IWritable
    {
        public ushort level;
        public attribute[] mods;
        public character[] characters;
        public int[] branches;
    }
    struct character : IWritable
    {
        public string name;
        public byte color;
        public byte role;
        public attribute[] attributes;
        public input[] timeLine;
    }
    struct attribute : IWritable
    {
        public byte type;
        public ushort value;
    }
    struct input : IWritable
    {
        public string vel;
        public byte action;
        public ushort cell;
    }
}
