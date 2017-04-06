using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fcNightlife.Models
{
    public class SearchModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Location (eg. Munich)")]
        public string Location { get; set; }
    }
}