using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace DavidBekeris.Data.Entities
{
    public class ProjektModel
    {
        public ProjektModel(int id, Uri imageUrl, string header, string description, string projectLink)
        {
            Id = id;
            ImageUrl = imageUrl;
            Header = header;
            Description = description;
            ProjectLink = projectLink;
        }

        public int Id { get; set; }
        public Uri ImageUrl { get; protected set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public string ProjectLink { get; set; }
        public string UrlSlug { get; protected set; }

    }
}
