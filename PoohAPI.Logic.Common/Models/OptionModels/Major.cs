using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Models.OptionModels
{
    /// <summary>
    /// Major class that contains information about the supported majors
    /// </summary>
    public class Major
    {
        public int Id { get; set; }
        public string CrohoNumber { get; set; }
        public string Name { get; set; }
        public string EducationLevel { get; set; }
        public int Active { get; set; }
    }
}
