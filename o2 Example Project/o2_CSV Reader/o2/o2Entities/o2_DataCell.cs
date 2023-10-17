
namespace o2.Entities.Models
{
    /// <summary>
    /// Basic Data Struct to hold value and the other necessary information about the value
    /// </summary>
    public struct o2DataCell
    {

        /// <summary>
        /// index of the column containing the data
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// index of the row containing the data
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Actual value as string
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Data type of the value
        /// </summary>
        public DataType Type { get { return DataFunctions.DefineDataType(Value); } private set { } }

        #region Constructors
        public o2DataCell(int rowIndex, int columnIndex, string value, DataType type = DataType.None)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            Value = value;
            this.Type = type;
        }
        public o2DataCell(int rowIndex, string value, int columnIndex = -0, DataType type = DataType.None)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            Value = value;
            this.Type = type;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return Value;
        }
        public static bool operator ==(o2DataCell left, o2DataCell right)
        {
            if (left.Type == right.Type)
                return left.Value == right.Value;
            else
                return false;
        }
        public static bool operator !=(o2DataCell left, o2DataCell right)
        {
            return !(left == right);
        }
        public override bool Equals(object obj)
        {
            if (obj is not o2DataCell)
                return false;
            return this == (o2DataCell)obj;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

    /// <summary>
    /// Simple enum for types
    /// </summary>
    public enum DataType
    {
        String,
        Char,
        Integer,
        Decimal,
        DateTime,
        None
    }


}
