using o2.IO;


namespace o2.Entities.Models
{

    /// <summary>
    /// Base o2DataModel
    /// This class is created for reading and performing operations on CSV files.
    /// </summary>
    public sealed class o2DataModel
    {
        #region properties
        /// <summary>
        /// Names of columns in the data model
        /// </summary>
        public string[] Columns { get; set; }

        /// <summary>
        /// All rows & values in the data model
        /// </summary>
        public List<string[]> Rows { get; set; }
        #endregion

        //______Construct______//
        public o2DataModel(string[] Headers, List<string[]> Values)
        {
            Columns = Headers; Rows = Values;
        }

        #region methods

        /// <summary>
        /// This function retrieves data from the desired column in each data row as a Data Cell
        /// </summary>
        /// <param name="RowIndex">Index of the target column</param>
        /// <param name="Job">Delegate (o2DataCell cell )</param>
        /// <param name="BreakIndex">This parameter determines the number of data rows to be processed</param>
        public void GetValuesFromColumn(int RowIndex, GetEachValue Job, int BreakIndex = -0)
        {
            if (BreakIndex == -0)
                BreakIndex = Rows.Count;

            for (int i = 0; i < BreakIndex; i++)
                Job(new o2DataCell(i, RowIndex, Rows[i][RowIndex].ToString()));
        }
        /// <summary>
        /// This function retrieves the desired number of data from the specified column and matches it
        /// with the value in the target column in the processing row using the SetEachValue delegate.
        /// </summary>
        /// <param name="RowIndex">Index of the target column</param>
        /// <param name="Job">Delegate (o2DataCell cell )</param>
        /// <param name="BreakIndex">This parameter determines the number of data rows to be processed</param>
        public void SetValuesOnColumn(int RowIndex, SetEachValue Job, int BreakIndex = -0)
        {
            if (BreakIndex == -0)
                BreakIndex = Rows.Count;

            for (int i = 0; i < BreakIndex; i++)
                Rows[i][RowIndex] = Job(new o2DataCell(i, RowIndex, Rows[i][RowIndex].ToString())).ToString();
        }
        /// <summary>
        /// This function finds the searched data in the specified column for all rows and replaces the found data with a new value
        /// </summary>
        /// <param name="RowIndex">Index of target column</param>
        /// <param name="TargetVal">The searched value</param>
        /// <param name="NewVal">New value to replacement</param>
        public void NormalizeSingle(int RowIndex, object TargetVal, object NewVal)
        {
            SetValuesOnColumn(RowIndex, (data) =>
            {
                if (data.Value.ToString().Trim() == TargetVal.ToString())
                    return NewVal;
                else
                    return data.Value;
            });
        }
        public void NormalizeValues(int RowIndex, string[] TargetVals, object[] NewVals)
        {
            SetValuesOnColumn(RowIndex, (data) =>
            {
                int Index = Array.IndexOf(TargetVals, data.Value.ToString());
                if (Index != -1)
                    try
                    {
                        return NewVals[Index];
                    }
                    catch (Exception)
                    {
                        return data.ColumnIndex;
                    }
                else
                    return data.Value;
            });
        }
        #endregion

        #region Functions 

        /// <summary>
        /// This function finds unique values in the specified column for all rows.
        /// </summary>
        /// <param name="RowIndex">Index of Target Row</param>
        /// <returns>unique Value array</returns>
        public string[] GetUniqueValues(int RowIndex, bool print = false)
        {
            List<string> uniqueValues = new List<string>();
            SetValuesOnColumn(RowIndex, (data) =>
            {
                if (!uniqueValues.Contains(data.Value))
                    uniqueValues.Add(data.Value);
                return data.Value;
            });
            if (print)
                O2_IO.Logger($"Uniqe Values for Column named \" {Columns[RowIndex]} \" Are : [ {string.Join(", ", uniqueValues)} ] ");
            return uniqueValues.ToArray();
        }

        /// <summary>
        /// This function retrieves all rows from the specified start row to the end row within the data model, separates them as a new data model, and returns it.
        /// </summary>
        /// <param name="from">starting index</param>
        /// <param name="to">ending index (if its -0 it means it will continue to the end of the rows)</param>
        /// <returns>a new dataset</returns>
        public o2DataModel Split(int from, int to = -0)
        {
            if (to == -0)
                to = Rows.Count;
            try
            {
                var ValueList = new List<string[]>();

                for (int i = from; i < to - 1; i++)
                    ValueList.Add(Rows[i]);

                return new o2DataModel((string[])Columns.Clone(), ValueList);
            }
            catch (Exception)
            {
                O2_IO.Logger($"Exception Has Occurred. Inputs might be wrong | From {from} - | To {to} |");
                return null;
            }
        }

        /// <summary>
        /// This function separates all the data in the specified column into the specified number of segments and returns it as a o2DataColumn object.
        /// </summary>
        /// <param name="HeaderIndex"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public o2DataColumn SplitColumn(int HeaderIndex, int lenght = 0)
        {
            if (lenght < 1 || lenght > Rows.Count)
                lenght = Rows.Count;

            var ColumnVals = new List<string>();
            GetValuesFromColumn(HeaderIndex, (data) =>
            {
                ColumnVals.Add(data.Value);
            });

            return new o2DataColumn(Columns[HeaderIndex], ColumnVals);
        }

        /// <summary>
        /// This function changes the header of the target column.
        /// </summary>
        /// <param name="Old">Current name of the header</param>
        /// <param name="New">new name of the header</param>
        /// <returns></returns>
        public bool RenameHeader(string Old, string New)
        {
            Old = Old.ToLower().Trim();
            for (int i = 0; i < Columns.Length; i++)
            {
                if (Columns[i].ToLower().Trim() == Old)
                {
                    Columns[i] = New;
                    return true;
                }
            }
            return false;
        }



        #endregion

        #region Overloads
        public void NormalizeValues(int RowIndex, string[] TargetVals)
        {
            object[] NewVals = new object[TargetVals.Length];

            for (int i = 0; i < NewVals.Length; i++)
                NewVals[i] = i;

            NormalizeValues(RowIndex, TargetVals, NewVals);
        }
        public bool RenameHeader(int Index, string NewName)
        {
            return RenameHeader(Columns[Index], NewName);
        }
        public o2DataColumn SplitColumn(string Header, int lenght = 0)
        {
            if (Header == string.Empty || Columns.Contains(Header) == false)
            {
                O2_IO.Logger($"There is no column named \'{Header}\'");
                return null;
            }
            return SplitColumn(Array.IndexOf(Columns, Header), lenght);
        }
        public override string ToString()
        {
            return $" [Columns Count : [{Columns.Length}] Total Records : [{Rows.Count}]]";
        }
        #endregion

        #region Statics

        /// <summary>
        /// This function reads the .csv file at the specified address and returns it as an o2DataModel object.
        /// </summary>
        /// <param name="Path">The path to the .csv file.</param>
        /// <returns>new o2DataModel</returns>
        public static o2DataModel ReadCSV(string Path, int Limit = -1)
        {
            if (Path.Split(".")[1].ToLower() != "csv")
            {
                O2_IO.Logger("Files without the .csv extension cannot be read.");
                return null;
            }
            var DS = O2_IO.ReadFromFile(Path, Limit);
            if (DS == null) return null;
            O2_IO.Logger($"DataSet Imported.{DS}");
            return DS;
        }
        #endregion
    }

}
