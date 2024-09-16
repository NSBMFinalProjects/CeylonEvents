namespace EventHandler.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }  

        public ICollection<Event> Events { get; set; }
    }

}
