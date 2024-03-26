namespace DavidBekeris.Data.Entities
{
    public class IndexModel
    {
        public IndexModel(int id, Uri heroUrl, string heroText, string heroHeader)
        {
            Id = id;
            HeroUrl = heroUrl;
            HeroText = heroText;
            HeroHeader = heroHeader;
        }

        public int Id { get; set; }
        public Uri HeroUrl { get; protected set; }
        public string HeroText { get; set; }
        public string HeroHeader { get; set; }
    }
}
