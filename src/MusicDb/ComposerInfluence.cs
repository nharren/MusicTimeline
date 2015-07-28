namespace NathanHarrenstein.MusicDB
{
    public class ComposerInfluence
    {
        public short InfluenceID { get; set; }
        public Composer Influence { get; set; }
        public short ComposerID { get; set; }
        public Composer Composer { get; set; }
    }
}