namespace DavidBekeris.Data.Entities
{
    public class IndexModel
    {
        public IndexModel(int id, Uri heroUrl, string heroText, string heroHeader)
        {
            Id = id;
            this.heroUrl = heroUrl;
            this.heroText = heroText;
            this.heroHeader = heroHeader;
        }

        public int Id { get; set; }
        public Uri heroUrl { get; protected set; }
        public string heroText { get; set; }
        public string heroHeader { get; set; }
    }
}
