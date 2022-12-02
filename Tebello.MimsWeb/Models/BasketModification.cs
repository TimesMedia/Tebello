using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tebello.MimsWeb.Models
{
    public class BasketModification
    {
        public bool Drop { get; set; }
        public int DeliveryMethod { get; set; }
        public int UnitsPerIssue { get; set; }
    }
}