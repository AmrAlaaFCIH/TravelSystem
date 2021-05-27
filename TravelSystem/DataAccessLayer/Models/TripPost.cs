using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelSystem.DataAccessLayer.Models
{
    public class TripPost
    {
        public Guid Id { get; set; }
        [Required][MaxLength(60)]
        public string AgencyName { get; set; }
        [Required][MaxLength(100)]
        public string Title { get; set; }
        public string Details { get; set; }
        [Required][MaxLength(100)]
        public string Date { get; set; }
        [Required][MaxLength(100)]
        public string PostData { get; set; }
        [Required][MaxLength(200)]
        public string Destination { get; set; }
        [MaxLength(600)]
        public string PhotoPath { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public bool Accepted { get; set; }

        public string LikedPostID { get; set; }
        public ApplicationUser LikedPost { get; set; }

        public string DislikedPostID { get; set; }
        public ApplicationUser DislikedPost { get; set; }

        public string SavedPostID { get; set; }
        public ApplicationUser SavedPost { get; set; }

        public string OwnerID { get; set; }
        public ApplicationUser Owner { get; set; }
    }
}
