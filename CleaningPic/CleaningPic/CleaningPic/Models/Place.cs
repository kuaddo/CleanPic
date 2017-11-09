namespace CleaningPic
{
    public enum Place
    {
        Kitchen,
        Entrance,
        Toilet,
        Bath,
        Window,
        Living
    }

    static class PlaceExtension
    {
        public static string DisplayName(this Place place)
        {
            string[] names = { "キッチン", "玄関", "トイレ", "浴槽・洗面所", "窓", "リビング"};
            return names[(int)place];
        }
    }
}
