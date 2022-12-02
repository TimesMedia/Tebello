using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Subs.Data;

namespace Tebello.MimsWeb.Models
{
    public class Basket
    {
       public  ObservableCollection<BasketItem> BasketItems { get; set;}
       public bool RequiresDeliveryAddress{ get; set; }
       public decimal TotalPrice { get; set;}
       public decimal TotalDiscount { get; set; }
       public decimal TotalDiscountedPrice { get; set; }
       public int InvoiceId { get; set;} 

       public Basket()
       {
            BasketItems = new ObservableCollection<BasketItem>();

       }
    }
}