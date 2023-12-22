using System.Collections.Generic;

namespace watchstore.Models
{
    public class Categories
    {
        public Categories()
        {
            Watches = new List<Watches>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Watches> Watches { get; set; }
    }
}
