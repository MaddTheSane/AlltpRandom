using System;
using Foundation;
using AlttpRandomizer.Random;

namespace AlttpRandomizer.Properties
{
    internal static class Resources
    {
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] RomImage
        {
            get
            {
                var romImage = NSBundle.MainBundle.GetUrlForResource("alltp", "sfc");
                var romData = NSData.FromUrl(romImage);
                var romBytes = romData.ToArray();
                return romBytes;
            }
        }
    }

    internal class Settings
    {

        private static Settings defaultInstance = new Settings();

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }


        public string OutputFile
        {
            get => NSUserDefaults.StandardUserDefaults.StringForKey("OutputFile");
            set => NSUserDefaults.StandardUserDefaults.SetString(value, "OutputFile");
        }

        public string RandomizerDifficulty
        {
            get => NSUserDefaults.StandardUserDefaults.StringForKey("RandomizerDifficulty");
            set => NSUserDefaults.StandardUserDefaults.SetString(value, "RandomizerDifficulty");
        }

        public bool CreateSpoilerLog
        {
            get => NSUserDefaults.StandardUserDefaults.BoolForKey("CreateSpoilerLog");
            set => NSUserDefaults.StandardUserDefaults.SetBool(value, "CreateSpoilerLog");
        }

        public bool SramTrace
        {
            get => NSUserDefaults.StandardUserDefaults.BoolForKey("SramTrace");
            set => NSUserDefaults.StandardUserDefaults.SetBool(value, "SramTrace");
        }

        public string HeartBeepSpeed
        {
            get => NSUserDefaults.StandardUserDefaults.StringForKey("HeartBeepSpeed");
            set => NSUserDefaults.StandardUserDefaults.SetString(value, "HeartBeepSpeed");
        }

        public int BulkCreateCount
        {
            get => (int)NSUserDefaults.StandardUserDefaults.IntForKey("HeartBeepSpeed");
            set => NSUserDefaults.StandardUserDefaults.SetInt(value, "HeartBeepSpeed");
        }

        public bool ShowComplexity
        {
            get => NSUserDefaults.StandardUserDefaults.BoolForKey("ShowComplexity");
            set => NSUserDefaults.StandardUserDefaults.SetBool(value, "ShowComplexity");
        }

        public void Save()
        {
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }
    }
}

namespace AlltpRandom {
    internal static class AlltpHelpers {
        public static HeartBeepSpeed? ToHeartBeep(nint rawVal)
        {
            if (!Enum.IsDefined(typeof(HeartBeepSpeed), rawVal))
            {
                return null;
            }
            return (HeartBeepSpeed)Enum.ToObject(typeof(HeartBeepSpeed), rawVal);
        }

        public static RandomizerDifficulty? ToDifficulty(nint rawVal)
        {
            if (!Enum.IsDefined(typeof(RandomizerDifficulty), rawVal))
            {
                return null;
            }
            return (RandomizerDifficulty)Enum.ToObject(typeof(RandomizerDifficulty), rawVal);
        }
    }
}
