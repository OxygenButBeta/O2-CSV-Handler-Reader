using o2.Entities.Models;

/*
 * This CS file only contains the delegates
 */


// This Region Contains the Delegates to Data Model Class
#region DataModel Delegates 

public delegate object SetEachValue(o2DataCell cell);
public delegate void GetEachValue(o2DataCell cell);

#endregion


// This Region Contains the Delegates to o2DataColumn Class
#region DataColmn Delegates

public delegate void GetEachValueDelegate(o2DataCell cell);
#endregion