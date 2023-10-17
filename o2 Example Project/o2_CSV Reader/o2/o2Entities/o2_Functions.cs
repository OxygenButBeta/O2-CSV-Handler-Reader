using System.Data;
using o2.Entities.Models;
using o2.IO;

namespace o2.Entities
{
    public static class DataFunctions
    {
        /// <summary>
        /// This variable holds the value to be added as a placeholder if any value within the data column
        /// cannot be converted to a numerical value.
        /// </summary>
        public static decimal SkipValue { get; set; } = 0;


        /// <summary>
        ///This function returns the average of normalized numerical values from index 0 up to the specified index.
        /// </summary>
        /// <param name="DC"></param>
        /// <param name="To"></param>
        /// <returns></returns>
        public static decimal Average(this o2DataColumn DC, int To = -0)
        {
            if (To == -0)
                To = DC.Values.Count;

            decimal Average = ColumnAsDecimalList(DC).Take(To).Average();
            O2_IO.Logger($"The numerical average of data up to index {To}, starting from index 0 is {Average}");
            return Average;
        }

        /// <summary>
        /// This function converts a normalized data column into a numerical list and returns it.
        /// </summary>
        /// <param name="DC"></param>
        /// <returns></returns>
        public static List<decimal> ColumnAsDecimalList(o2DataColumn DC)
        {
            List<decimal> ValsAsDecimal = DC.Values.Select(str =>
            {
                if (decimal.TryParse(str, out decimal result))
                    return result;
                else
                    O2_IO.Logger($"Unconvertable value \"{str}\" in {DC.Header}. Adding {SkipValue} as skip value");

                return SkipValue;

            }).ToList();
            return ValsAsDecimal;
        }
        
        /// <summary>
        ///  This function returns the smallest numerical value within a data column.
        /// </summary>
        /// <param name="DC"></param>
        /// <returns></returns>
        public static decimal MinValue(this o2DataColumn DC)
        {
            decimal Lowest = ColumnAsDecimalList(DC).Min();
            O2_IO.Logger($"Lowest value in column named {DC.Header} is {Lowest}");
            return Lowest;

        }
        
        /// <summary>
        /// This function returns the number of occurrences of a specific value within a data column.
        /// </summary>
        /// <param name="DC"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int CountOf(this o2DataColumn DC, object value)
        {
            int count = DC.Values.Count(val => val == value.ToString());
            O2_IO.Logger($"Count of {value} in Column named {DC.Header} is {count}");
            return count;
        }

        /// <summary>
        /// This function returns the largest numerical value within a data column.
        /// </summary>
        /// <param name="DC"></param>
        /// <returns></returns>
        public static decimal MaxValue(this o2DataColumn DC)
        {
            decimal Highest = ColumnAsDecimalList(DC).Max();
            O2_IO.Logger($"Highest value in column named {DC.Header} is {Highest}");
            return Highest;

        }

        /// <summary>
        /// This function returns the percentage of the total of a given data column that a specified number represents
        /// </summary>
        /// <param name="count"></param>
        /// <param name="DC"></param>
        /// <returns></returns>
        public static decimal Percentage(this o2DataColumn DC,object count)
        {
            return Convert.ToDecimal(count) * 100 / DC.Values.Count;
        }

        /// <summary>
        /// This Function defines the type of the value
        /// </summary>
        /// <param name="val"></param>
        /// <returns>DataType Enum</returns>
        public static DataType DefineDataType(string val)
        {

            if (val.Contains("'") && val.Length == 1)
                if (char.TryParse(val, out char c))
                    return DataType.Char;

                else if (decimal.TryParse(val, out decimal d))
                {
                    if (val.Contains('.'))
                        return DataType.Decimal;
                    else
                        return DataType.Integer;
                }
                else if (DateTime.TryParse(val.ToString(), out DateTime dt))
                    return DataType.DateTime;
                else if (val.Length == 1)
                    return DataType.Char;

            return DataType.String;
        }
    }
}
