using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datagen.Core.Extensions
{
    internal static class RandomExtensions
    {
        public static decimal NextDecimal(this Random rng, decimal maxValue = decimal.MaxValue, decimal minValue = 0)
        {
            return ((decimal)rng.NextDouble()) * (maxValue - minValue) + minValue;
        }
    }
}
