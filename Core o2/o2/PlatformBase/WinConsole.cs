
using System;
using o2.Entities.Models;
using o2.IO;

/*
 This C# class should be used when the project is a console application. It is designed for use in console applications
 to write data to the console, align columns when writing, use different colors for text, and perform logging operations.
 It may lead to a compilation-time error when used outside of console applications.
 */

#region Uncomment the following codes to enable o2WinConsole only if you are using this library on Windows console application.
namespace o2.PlatformBase.Windows
{
    public static class o2WinConsole
    {
        #region properties
        public static int SpaceBetweenColumns { get; set; } = 0;
        public static bool DisplayColumnIndex { get; set; } = false;
        public static int ShortHeaders { get; set; } = 0;
        public static bool PrintLogToConsole { get; set; } = true;
        public static bool AlignColumnLenghtsToPrint { get; set; } = true;
        private static bool ColorSwitch = false;
        #endregion
     
        /// <summary>
        /// This method prints datas with certen Length
        /// </summary>
        /// <param name="PrintLenght"></param>
        public static void Print(this o2DataModel data, int PrintLenght = 0)
        {
            if(data is null)
                return;
            string[] HeadersToPrint = (string[])data.Columns.Clone();
            int[] MaxLenghts = new int[data.Columns.Length];


            if (PrintLenght == 0 || PrintLenght < 0 || PrintLenght > data.Rows.Count)
                PrintLenght = data.Rows.Count;



            if (ShortHeaders > 0)
                for (int i = 0; i < HeadersToPrint.Length; i++)
                {
                    if (ShortHeaders > HeadersToPrint[i].Length)
                        continue;

                    HeadersToPrint[i] = HeadersToPrint[i].Substring(0, ShortHeaders);
                }

            if (DisplayColumnIndex)
                for (int i = 0; i < HeadersToPrint.Length; i++)
                    HeadersToPrint[i] = $"[{i}]" + HeadersToPrint[i];

            if (AlignColumnLenghtsToPrint)
                for (int i = 0; i < data.Columns.Length; i++)
                {
                    int LongestOnTheColumn = 0;
                    data.GetValuesFromColumn(i, (data) =>
                    {
                        if (data.Value.Length > LongestOnTheColumn)
                            LongestOnTheColumn = data.Value.Length;

                    }, PrintLenght);
                    MaxLenghts[i] = LongestOnTheColumn;
                }

            for (int i = 0; i < HeadersToPrint.Length; i++)
            {
                int ValLenght = data.Rows[0][i].Length;
                if (ValLenght <= HeadersToPrint[i].Length)
                    continue;

                int gap = ValLenght - HeadersToPrint[i].Length;

                for (int x = 0; x < gap; x++)
                    if (x % 2 == 0)
                        HeadersToPrint[i] = " " + HeadersToPrint[i];
                    else
                        HeadersToPrint[i] = HeadersToPrint[i] + " ";
            }

            Console.WriteLine("\t\n" + string.Join(" | ", HeadersToPrint) + "\n");

            for (int i = 0; i < PrintLenght; i++)
            {
                LineColorSwitch(ConsoleColor.Red);
                if (AlignColumnLenghtsToPrint)
                {
                    string[] row = (string[])data.Rows[i].Clone();
                    for (int x = 0; x < row.Length; x++)
                    {
                        int gap = MaxLenghts[x] - row[x].Length + SpaceBetweenColumns;

                        for (int y = 0; y < gap; y++)
                            if (gap % 2 == 0)
                                row[x] = " " + row[x];
                            else
                                row[x] = row[x] + " ";
                    }
                    Console.WriteLine(" " + string.Join(" | ", row));
                }
                else
                    Console.WriteLine(" " + string.Join(" | ", data.Rows[i]));

                Console.ForegroundColor = ConsoleColor.White;
            }

            if (PrintLenght != data.Rows.Count)
                O2_IO.Logger($"....{PrintLenght} records of {data.Rows.Count}.\n");
        }
        public static void PrintLog(string str)
        {
            LineColorSwitch(ConsoleColor.Green);
            Console.WriteLine($" {DateTime.Now.Minute}:{DateTime.Now.Second}:{DateTime.Now.Millisecond} o2 Log   : {str}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LineColorSwitch(ConsoleColor c)
        {
            if (ColorSwitch)
            {
                Console.ForegroundColor = c; ColorSwitch = false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                ColorSwitch = true;
            }
        }
    }
}
#endregion
