using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace watchstore.Models
{
    public class Watches
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PhotoURL { get; set; }
        public bool isLimited { get; set; }
        public string CategoryId { get; set; }

        public virtual Categories Category { get; set; }
    }
}
