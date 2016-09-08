using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    public interface IRateAdjuster
    {
        Rate AdjustRate(Rate rate);
    }
}
