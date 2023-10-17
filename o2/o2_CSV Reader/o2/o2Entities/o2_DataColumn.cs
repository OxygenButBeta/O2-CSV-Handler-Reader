
namespace o2.Entities.Models
{
    public sealed class o2DataColumn
    {
        #region Properties
        public string Header { get; set; }
        public List<string> Values { get; set; }
        #endregion

        #region constructors
        public o2DataColumn(string header, List<string> values)
        {
            Header = header;
            Values = values;
        }
        public o2DataColumn() { }
        #endregion

        public void GetEachValue(GetEachValueDelegate Job, int BreakIndex = -0)
        {
            if (BreakIndex == -0)
                BreakIndex = Values.Count;

            for (int i = 0; i < BreakIndex; i++)
                Job(new o2DataCell(i, Values[i]));
        }
    }
}
