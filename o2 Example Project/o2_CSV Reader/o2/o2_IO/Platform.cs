using System.Reflection;
using System.Runtime.InteropServices;


/* Information About the Target Platform Selection.

   This project is designed for .NET Core and contains LINQ elements that work much faster with .NET 7.0 Framework.
   The built-in logger captures all the information throughout the project. If you are using or including this project in your Console Application,
   you can print log messages to the console. On the other hand, if you are using a Windows Forms App, the log print functionality will be disabled,
   but you can still track the log messages from the LogActivity event in the class named "o2_IO\LogActivity" within the "o2.IO" namespace.

*/
public static class Platform
{
    private static byte C = 0;
    public static bool IsConsole
    {
        get
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            return entryAssembly != null && entryAssembly.EntryPoint.DeclaringType == typeof(Program);
        }
    }
    public static string CurrentFrameworkVersion
    {
        get
        {
            return RuntimeInformation.FrameworkDescription;
        }
    }
    public static void CheckFrameworkRecommendation()
    {
        if (C != 0)
            return;
        int.TryParse(CurrentFrameworkVersion.Split('.')[1].Split(" ")[1], out int DotnetVersion);
        if (DotnetVersion < 7)
            o2.IO.O2_IO.Logger($"You are using an older framework than the recommended. .NET Core 7.0 Framework. or Higher Versions are recommended.\nCurrent framework version is {CurrentFrameworkVersion}.\n ");
        C++;
    }
}

