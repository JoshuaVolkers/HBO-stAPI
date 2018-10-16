using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Models.InputModels
{
    public class ReviewUpdateInput
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int EducationalAttainmentId { get; set; }
        [Required]
        public int EducationId { get; set; }
        [Required]
        public int PreferredLanguageId { get; set; }
        [Required]
        public int CountryId { get; set; }
        public string City { get; set; }
        /// <summary>
        /// Municipality, province or state.
        /// This is required to find the correct coordinates of the city if there are multiple cities with the same name.
        /// This value will not be stored in the database.
        /// </summary>
        public string AdditionalLocationIdentifier { get; set; }
    }
}
