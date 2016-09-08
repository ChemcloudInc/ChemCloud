using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    public class ChemPackage
    {
        /// <summary>
        /// 获取Chem包裹标准
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,Package> GetChemPackage(){
            Dictionary<string,Package> result = new Dictionary<string,Package>();
            Package package = null;
            int weight=0;
            //T1
            weight=15*15*15/5000;
            package = new Package(15, 15, 5, weight, 0);
            package.TotalSize=100;
            result.Add("T1",package);

            //T2
            weight = 20 * 14 * 4 / 5000;
            package = new Package(20, 14, 4, weight, 0);
            package.TotalSize = 50;
            result.Add("T2", package);

            //T2L
            weight = 22 * 14 * 4 / 5000;
            package = new Package(22, 14, 4, weight, 0);
            package.TotalSize = 50;
            result.Add("T2L", package);

            //T3
            weight = 27 * 17 * 4 / 5000;
            package = new Package(27, 17, 5, weight, 0);
            package.TotalSize = 100;
            result.Add("T3", package);

            //T4
            weight = 25 * 20 * 7 / 5000;
            package = new Package(25, 20, 7, weight, 0);
            package.TotalSize = 250;
            result.Add("T4", package);

            //T5
            weight = 30 * 22 * 5 / 5000;
            package = new Package(30, 22, 5, weight, 0);
            package.TotalSize = 100;
            result.Add("T5", package);

            //T6
            weight = 36 * 30 * 6 / 5000;
            package = new Package(36, 30, 6, weight, 0);
            package.TotalSize = 250;
            result.Add("T6", package);


            //T7
            weight = 36 * 25 * 12 / 5000;
            package = new Package(36, 25, 12, weight, 0);
            package.TotalSize = 1000;
            result.Add("T7", package);

            //T10
            weight = 20 * 10 * 4 / 5000;
            package = new Package(20, 10, 4, weight, 0);
            package.TotalSize = 100;
            result.Add("T10", package);

            //TA
            weight = 37 * 28 * 6 / 5000;
            package = new Package(37, 28, 6, weight, 0);
            package.TotalSize = 250;
            result.Add("TA", package);



            return result;
        }

        public static List<Package> GetPackageList()
        {
            List<Package> result = new List<Package>();
            Package package = null;
            decimal weight = 0;
            //T1
            weight = (decimal)15 * 15 * 15 / 5000;
            package = new Package(15, 15, 5, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 100;
            result.Add(package);

            //T2
            weight = (decimal)20 * 14 * 4 / 5000;
            package = new Package(20, 14, 4, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 50;
            result.Add(package);

            //T2L
            weight = (decimal)22 * 14 * 4 / 5000;
            package = new Package(22, 14, 4, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 50;
            result.Add(package);

            //T3
            weight = (decimal)27 * 17 * 4 / 5000;
            package = new Package(27, 17, 5, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 100;
            result.Add(package);

            //T4
            weight = (decimal)25 * 20 * 7 / 5000;
            package = new Package(25, 20, 7, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 250;
            result.Add(package);

            //T5
            weight = (decimal)30 * 22 * 5 / 5000;
            package = new Package(30, 22, 5, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 100;
            result.Add(package);

            //T6
            weight = (decimal)36 * 30 * 6 / 5000;
            package = new Package(36, 30, 6, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 250;
            result.Add( package);


            //T7
            weight = (decimal)36 * 25 * 12 / 5000;
            package = new Package(36, 25, 12, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 1000;
            result.Add(package);

            //T10
            weight = (decimal)20 * 10 * 4 / 5000;
            package = new Package(20, 10, 4, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 100;
            result.Add(package);

            //TA
            weight = (decimal)37 * 28 * 6 / 5000;
            package = new Package(37, 28, 6, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 250;
            result.Add(package);



            return result;
        }

        public static List<Package> GetPackageList(decimal thickness)
        {
            List<Package> result = new List<Package>();
            Package package = null;
            decimal weight = 0;
            //T1
            weight = (decimal)15 * 15 * 15 / thickness;
            weight = Math.Round(weight,2);
            package = new Package(15, 15, 5, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 100;
            result.Add(package);

            //T2
            weight = (decimal)20 * 14 * 4 / thickness;
            weight = Math.Round(weight, 2);
            package = new Package(20, 14, 4, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 50;
            result.Add(package);

            //T2L
            weight = (decimal)22 * 14 * 4 / thickness;
            weight = Math.Round(weight, 2);
            package = new Package(22, 14, 4, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 50;
            result.Add(package);

            //T3
            weight = (decimal)27 * 17 * 4 / thickness;
            weight = Math.Round(weight, 2);
            package = new Package(27, 17, 5, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 100;
            result.Add(package);

            //T4
            weight = (decimal)25 * 20 * 7 / thickness;
            weight = Math.Round(weight, 2);
            package = new Package(25, 20, 7, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 250;
            result.Add(package);

            //T5
            weight = (decimal)30 * 22 * 5 / thickness;
            weight = Math.Round(weight, 2);
            package = new Package(30, 22, 5, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 100;
            result.Add(package);

            //T6
            weight = (decimal)36 * 30 * 6 / thickness;
            weight = Math.Round(weight, 2);
            package = new Package(36, 30, 6, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 250;
            result.Add(package);


            //T7
            weight = (decimal)36 * 25 * 12 / thickness;
            weight = Math.Round(weight, 2);
            package = new Package(36, 25, 12, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 1000;
            result.Add(package);

            //T10
            weight = (decimal)20 * 10 * 4 / thickness;
            weight = Math.Round(weight, 2);
            package = new Package(20, 10, 4, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 100;
            result.Add(package);

            //TA
            weight = (decimal)37 * 28 * 6 / thickness;
            weight = Math.Round(weight, 2);
            package = new Package(37, 28, 6, weight, 0);
            package.ShipWeightUnit = WeightUnit.KG;
            package.ShipLinearUnit = LinearUnit.CM;
            package.TotalSize = 250;
            result.Add(package);



            return result;
        }
    }
}
