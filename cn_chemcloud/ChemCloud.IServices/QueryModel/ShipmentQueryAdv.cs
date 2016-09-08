using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace ChemCloud.IServices.QueryModel
{
   public class FedExQueryAdv
    {
       public FedExQueryAdv()
       {

       }

       public long ShopId { get; set; }

       public int OrigId { get; set; }
       public string OrigAddress { get; set; }

       public string OrigPostCode { get; set; }

       public long DestId { get; set; }
       public string DestAddress { get; set; }

       public string DestPostCode { get; set; }

       public string FedexKey { get; set; }

       public string FedexPassword { get; set; }

       public string FedexAccountNumber { get; set; }

       public string FedexMeterNumber { get; set; }

       public string CoinType { get; set; }

       public string ShipType { get; set; }

       public List<ShipPackage> PackagesList { get; set; }
    }

   public class ShipPackage {

       public ShipPackage()
       {

       }

      
       public int Num { get; set; }

      

       public string PackingUnit { get; set; }

       public long OrderItemId { get; set; }

       public string Description { get; set; }


   }




}
