using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finalProject.Models
{
    public class AdvSumModel
    {
        public int Id { get; set; }

        public string Cat { get;
            set;
        }
       public string Status { get; set; }
        public string Title { get; set; }

        public float Price { get; set; }

        public string ImgUrl { get; set; }
        public int Img_ID { get;
            set;
        }

        public DateTime postedDate { get; set; }
    }
}