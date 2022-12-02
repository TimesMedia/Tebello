using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tebello.MimsWeb.Models
{
    public class LoginRequest
    {
        public string Email {get ; set;}
        public string Password { get; set; }
        public int? CustomerId { get; set; }
        public int CountryId { get; set; }
        public bool ResetFlag { get; set;}
    }

    public class ContactRequest
    {
         public string Name { get; set; }

        public string ContactNumber { get; set; }

        public string Message { get; set; }
    }


}