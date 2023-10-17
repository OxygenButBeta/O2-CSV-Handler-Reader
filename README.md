# O2 CSV Handler/Reader
 o2 is an open-source project for reading and performing operations on CSV Files. I recommend using the delegates defined for manual listings with the 'GetValuesFromColumn' and 'SetValuesOnColumn' functions from o2DataModel It's possible to do more than the example currently offers. I'm considering adding visualized charts and more comprehensive analysis algorithms in the future. I hope this library proves somewhat useful for your work.

Information About the Target Platform Selection. This project is designed for .NET Core and contains LINQ elements and in my tests o2 work faster with .NET 7.0 Framework.

The built-in logger captures all the information throughout the project. If you are using or including this project in your Console Application, you can print log messages to the console. On the other hand, if you are using a Windows Forms App, the log print functionality will be disabled, but you can still track the log messages from the LogActivity event in the class named "o2_IO\LogActivity" within the "o2.IO" namespace.
