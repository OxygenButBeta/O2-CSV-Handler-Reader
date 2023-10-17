/*
 * PLEASE READ
 
 This simple program example was created to provide illustrations for reading and processing any CSV file.

 NOTES:
 * 
 This example includes only a few simple and basic operations.
 You can access other functions from the library that are already available.

 I recommend using the delegates defined for manual listings with the 'GetValuesFromColumn' and 'SetValuesOnColumn' functions. 

 It's possible to do much more than this example currently offers. I'm considering adding visualized charts and more comprehensive analysis algorithms in the future.

 I hope this library proves somewhat useful for your work.

 */



/// Required name spaces
using o2.Entities.Models;
using o2.Entities;
using o2;
using System.Data;
using o2.PlatformBase.Windows;


PrintToCenter("Stage 1");
#region Sample Stage 1

// on console applications i enable to print longs on the console to 
o2WinConsole.PrintLogToConsole = true;

/* Enter the file path of the dataset as parameter to ReadCSV Function.
   (This dataset is taken as an example from the kaggle.com website.)"
   If you are going to read large-sized files, reading the entire file at once can cause issues,
   so we can specify how many rows we want to read as a second parameter.
   If the second parameter is not provided, the entire file will be read.
*/
var mydata = o2DataModel.ReadCSV(@"Samples\Invistico_Airline.csv", 20000);
// we will only read the first 20000 row in this example.

/*
    Lets print the first 10 row from the dataset.
    If no parameter is provided, all the data will be printed to the screen.
 */
mydata.Print(10);
#endregion

PrintToCenter("Stage 2");
#region Sample Stage 2
/*
 The dataset is ready, but there is a lot of redundant data.
 We can convert them into numerical values and perform operations on them.
 to do that we need to find unique values in target row.
 However, performing this operation on columns with a high number of unique values can lead to problems.
 */

//First Display the column indexes to choose columns easily and for 
o2WinConsole.DisplayColumnIndex = true;

//If the column names are too long for a console application, we can limit them.
o2WinConsole.ShortHeaders = 6; //With this code, we've limited the column names to 6 characters.


//To make normalization on columns lets get uniqe values from those columns.
var UniqeValuesForTravelType = mydata.GetUniqueValues(3);

// if you want uniqe values to display on console , you can add a boolean parameter to function
UniqeValuesForTravelType = mydata.GetUniqueValues(3, true);

// lets do the same thing to Class Column. and Customer type Column
var UniqeValuesForClass = mydata.GetUniqueValues(4, true);

var UniqeValuesForCustomerType = mydata.GetUniqueValues(1, true);

//lets normalize these columns
mydata.NormalizeValues(3, UniqeValuesForTravelType);
mydata.NormalizeValues(4, UniqeValuesForClass);
mydata.NormalizeValues(1, UniqeValuesForCustomerType);

mydata.Print(10);

#endregion

PrintToCenter("Stage 3");
#region Sample Stage 3
/*
 Now we have the normalized values, lets perform some simple operations on them.
    Printing logs to console is enabled so when we make some operations it will print the results to the console
 */
// We currently have 20,000 records let's see how many of them have 'eco class' for flights.

//First Lets get the Class Column.
var ClassColumn = mydata.SplitColumn(4);

var CountOfEcoClass = ClassColumn.CountOf(0);
// Now, let's look at the percentage of economy class passengers relative to the total number of passengers

var PercentageOfEcoClass = ClassColumn.Percentage(CountOfEcoClass);
Console.WriteLine("Percentage : %" + PercentageOfEcoClass);

// Now, let's do the same thing for other classes (Business, EcoPluss)
var CountOfBusiness = ClassColumn.CountOf(1);
var CountOfEcoPlus = ClassColumn.CountOf(2);

var PercentageOfBusiness = ClassColumn.Percentage(CountOfBusiness);
var PercentageOfEcoPlus = ClassColumn.Percentage(CountOfEcoPlus);

Console.WriteLine($"The percentage distribution of the 'Class' column. \t Eco: %{PercentageOfEcoClass} Business: %{PercentageOfBusiness} Eco Plus : %{PercentageOfEcoPlus}");

//Now, let's calculate the average ages of the passengers.
var AverageAge = mydata.SplitColumn(2).Average();
#endregion





#region ...
void PrintToCenter(string str)
{
    int screenWidth = Console.WindowWidth;
    int textWidth = str.Length;
    if (textWidth < screenWidth)
    {
        int leftPadding = (screenWidth - textWidth) / 2;
        Console.SetCursorPosition(leftPadding, Console.CursorTop);
        Console.WriteLine(str + "\n\n\n\n");
    }
}
Console.ReadKey();
#endregion