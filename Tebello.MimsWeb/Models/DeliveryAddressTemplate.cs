using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Tebello.MimsWeb.Models
{
    public class DeliveryAddressTemplate
    {
        public int DeliveryAddressId { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public SelectList CountrySelectList { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Suburb { get; set; }

        public string Street { get; set; }
        public string StreetExtension { get; set; }
        public string StreetSuffix { get; set; }

        public string StreetNo { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }

        [Required]
        public string PostCode { get; set; }
    }
}
