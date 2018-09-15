using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    //Class that contains basic information about a vacature.
    public class BaseVacature
    {
        [Required]
        public int Id { get; set; } 
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public IEnumerable<string> Education { get; set; }
        public string Location { get; set; }
        [Required]
        public string Description { get; set; }
        public string Company { get; set; }
    }
}
