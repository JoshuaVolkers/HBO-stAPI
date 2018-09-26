using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    /// <summary>
    /// Class with token for client.
    /// </summary>
    public class Token
    {
        [Required]
        public string TokenString { get; set; }
    }
}
