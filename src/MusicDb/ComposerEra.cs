namespace NathanHarrenstein.MusicDB
{
    public class ComposerEra
    {
        public virtual Composer Composer { get; set; }
        public short ComposerID { get; set; }
        public virtual Era Era { get; set; }
        public short EraID { get; set; }
    }
}