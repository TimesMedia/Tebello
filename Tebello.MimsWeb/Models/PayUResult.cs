using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tebello.MimsWeb.Models
{
    public class PayUResult
    {
        public PayUResult()
        { }

       
        public string Reference { get; set; }
        public string AmountPayed { get; set; }
        public string Currency { get; set; }
        public string CardExpiry { get; set; }
        public string CardNumber { get; set; }
        public string GatewayReference { get; set; }
        public string CardType { get; set; }
        public string NameOnCard { get; set; }
        public string PointOfFailure { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public bool Successful { get; set; }
        public string TransactionState { get; set; }
        public bool Reconciled { get; set; }

    }
}