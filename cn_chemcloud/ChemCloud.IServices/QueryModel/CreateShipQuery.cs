using ChemColud.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace ChemCloud.IServices.QueryModel
{
   public class CreateShipQuery
    {
       public CreateShipQuery()
       {

       }
       public long OrderItemId { get; set; }
       public long OrderId { get; set; }

       public string Url { get; set; }
       public string Key { get; set; }
       public string Password { get; set; }

       public string AccountNumber { get; set; }

       public string MeterNumber { get; set; }

       public string CoinType { get; set; }

       public Address Origin { get; set; }

       public Address Dest { get; set; }


       public List<ShipPackage> ShipPkgs { get; set; }
     
    }



}
