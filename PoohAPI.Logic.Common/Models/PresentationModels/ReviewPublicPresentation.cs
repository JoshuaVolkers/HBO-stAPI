using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Models.PresentationModels
{
    public class ReviewPublicPresentation
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int Stars { get; set; }
        public string WrittenReview { get; set; }
        public string NameReviewer { get; set; }
    }
}
