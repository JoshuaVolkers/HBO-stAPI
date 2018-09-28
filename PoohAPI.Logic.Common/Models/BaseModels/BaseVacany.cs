using System;
using System.Collections.Generic;

namespace PoohAPI.Logic.Common.Models.BaseModels
{
    /// <summary>
    /// Class that contains basic information about a vacancy.
    /// </summary>
    public class BaseVacancy
    {
         
        public int Id { get; set; } 
         
        public string Title { get; set; }
         
        public DateTime CreationDate { get; set; }
         
        public IEnumerable<string> Education { get; set; }
        public string Location { get; set; }
         
        public string Description { get; set; }
         
        public Company Company { get; set; }
         
        public string Language { get; set; }
    }
}
