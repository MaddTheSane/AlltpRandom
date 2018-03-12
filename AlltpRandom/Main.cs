using Foundation;
using AppKit;
using AlttpRandomizer.Random;

namespace AlltpRandom
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            NSApplication.Init();
            var pool = new NSAutoreleasePool();
            var dict = new NSMutableDictionary();
            dict.SetValueForKey(new NSString("ALttP Random - <seed>.sfc"), new NSString(PreferenceNames.OutputFile));
            dict.SetValueForKey(new NSNumber((int)RandomizerDifficulty.Casual), new NSString(PreferenceNames.RandomizerDifficulty));
            dict.SetValueForKey(new NSNumber(false), new NSString(PreferenceNames.CreateSpoilerLog));
            dict.SetValueForKey(new NSNumber(false), new NSString(PreferenceNames.SramTrace));
            dict.SetValueForKey(new NSNumber((int)HeartBeepSpeed.Normal), new NSString(PreferenceNames.HeartBeepSpeed));
            dict.SetValueForKey(new NSNumber(5), new NSString(PreferenceNames.BulkCreateCount));
            dict.SetValueForKey(new NSNumber(true), new NSString(PreferenceNames.ShowComplexity));
            var docURL = NSFileManager.DefaultManager.GetUrl(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, null, false, out NSError unused);
            dict.SetValueForKey(docURL, new NSString(PreferenceNames.ParentDirectory));
            NSUserDefaults.StandardUserDefaults.RegisterDefaults(dict);
            dict = null;
            pool.Dispose();
            pool = null;
            NSApplication.Main(args);
        }
    }
}
