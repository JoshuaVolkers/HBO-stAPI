using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Models
{
    public class ReviewPublic
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int Stars { get; set; }
        public string WrittenReview { get; set; }
        public string NameReviewer { get; set; }
        public string ReviewDate { get; set; }
    }
}
