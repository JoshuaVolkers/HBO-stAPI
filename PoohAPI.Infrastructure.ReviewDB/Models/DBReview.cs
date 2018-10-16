using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.ReviewDB.Models
{
    public class DBReview
    {
        public int review_id;
        public int review_bedrijf_id;
        public int review_student_id;
        public int review_sterren;
        public string review_geschreven;
        public int review_anoniem;
        public DateTime review_datum;
        public int review_status;
        public int review_status_bevestigd_door;
        //public string blob;
    }
}
