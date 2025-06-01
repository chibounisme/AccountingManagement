using ObjCRuntime;
using UIKit;

namespace AccountingManagement.UI.Maui.Platforms.MacCatalyst;

public class Program
{
    static void Main(string[] args)
    {
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}