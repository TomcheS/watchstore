using watchstore.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace watchstore.ViewModels
{
    public class SearchWatchesViewModel
    {
        [Required]
        [DisplayName("Serach")]
        public string SearchText { get; set; }


        public IEnumerable<Watches> WatchList { get; set; }

    }
}
