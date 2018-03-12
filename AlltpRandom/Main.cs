using Foundation;
using AppKit;

namespace AlltpRandom
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            NSApplication.Init();
            var dict = new NSMutableDictionary();
            dict.SetValueForKey(new NSString("ALttP Random - <seed>.sfc"), new NSString("OutputFile"));
            dict.SetValueForKey(new NSString("Casual"), new NSString("RandomizerDifficulty"));
            dict.SetValueForKey(new NSNumber(false), new NSString("CreateSpoilerLog"));
            dict.SetValueForKey(new NSNumber(false), new NSString("SramTrace"));
            dict.SetValueForKey(new NSString("Normal"), new NSString("HeartBeepSpeed"));
            dict.SetValueForKey(new NSNumber(5), new NSString("BulkCreateCount"));
            dict.SetValueForKey(new NSNumber(true), new NSString("ShowComplexity"));
            NSUserDefaults.StandardUserDefaults.RegisterDefaults(dict);
            dict = null;
            NSApplication.Main(args);
        }
    }
}
