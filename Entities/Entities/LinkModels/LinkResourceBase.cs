namespace Entities.LinkModels
{
    //bir kaynapa ait linklerin organizasyonu yapılacak
    public class LinkResourceBase
    {
        public LinkResourceBase()
        {
            
        }
        //birden fazla link
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
