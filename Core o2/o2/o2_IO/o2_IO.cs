using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using o2.Entities.Models;
using o2.PlatformBase.Windows;

namespace o2.IO
{
    public static class O2_IO
    {

        public static event Action<string> LogActivity;

        public static o2DataModel ReadFromFile(string Path, int limit)
        {
            if (!File.Exists(Path))
            {
                Logger("File Not Exist \"" + Path + "\"");
                return null;
            }
            Platform.CheckFrameworkRecommendation();
            string[] headers;
            List<string[]> Rows = new List<string[]>();
            Logger("Reading : \"" + Path + "\"");
            using (var s = new StreamReader(Path))
            {
                headers = s.ReadLine().Split(",");
                for (int i = 0; i < headers.Length; i++)
                    headers[i] = headers[i].Replace("\"", "");

                if (headers.Length != 0)
                    while (!s.EndOfStream)
                    {
                        if (limit != -1 && Rows.Count == limit)
                            break;

                        string[] Row = s.ReadLine().Split(",");
                        Rows.Add(Row);
                    }
            }
            Logger("File Read \"" + Path + "\"");
            return new o2DataModel(headers, Rows);
        }

        public static void Logger(string str)
        {
            LogActivity?.Invoke(str);
            if (!Platform.IsConsole)
                return;
            o2WinConsole.PrintLog(str);
        }
    }
}

