using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDShortener.Models
{
    [Table("URL")]
    public class URL
    {
        [Key]
        public long UrlID { get; set; }

        [Required]
        [Display(Name="Long URL")]
        public string LongUrl { get; set; }

        [Required]
        [Display(Name = "Short URL")]
        public string ShortUrl { get; set; }

        public int UserID { get; set; }
        public long NoClicks { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }
}