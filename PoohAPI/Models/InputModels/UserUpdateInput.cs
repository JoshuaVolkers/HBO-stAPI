using System.ComponentModel.DataAnnotations;

namespace PoohAPI.Models.InputModels
{
    public class UserUpdateInput
    {
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
