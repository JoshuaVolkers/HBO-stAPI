using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.ReviewDB.Models
{
    public class DBReview
    {
        public int review_id { get; set; }
        public int review_bedrijf_id { get; set; }
        public int review_student_id { get; set; }
        public int review_sterren { get; set; }
        public string review_geschreven { get; set; }
        public int review_anoniem { get; set; }
        public DateTime review_datum { get; set; }
        public int review_status { get; set; }
        public int review_status_bevestigd_door { get; set; }
        //public string blob;
    }
}
