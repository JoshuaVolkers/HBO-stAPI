using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Models.InputModels
{
    public class InternshipContractInput
    {
        [Required]
        public int Id { get; set; }        
        [Required]
        public IFormFile contract { get; set; }
    }
}
