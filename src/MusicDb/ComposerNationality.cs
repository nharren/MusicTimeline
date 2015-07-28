namespace NathanHarrenstein.MusicDB
{
    public class ComposerNationality
    {
        public short ComposerID { get; set; }
        public short NationalityID { get; set; }
        public Composer Composer { get; set; }
        public Nationality Nationality { get; set; }
    }
}