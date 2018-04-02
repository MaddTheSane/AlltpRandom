using System;
using Foundation;
using AlttpRandomizer.Random;
using AlltpRandom;

namespace AlttpRandomizer.Properties
{
    internal static class Resources
    {
        /// <summary>
        ///   Looks up a resource of type System.Byte[].
        /// </summary>
        internal static byte[] RomImage
        {
            get
            {
                var romImage = NSBundle.MainBundle.GetUrlForResource("alttp", "sfc");
                var romData = NSData.FromUrl(romImage);
                var romBytes = romData.ToArray();
                return romBytes;
            }
        }
    }

    internal class Settings
    {
        private static Settings defaultInstance = new Settings();

        public static Settings Default => defaultInstance;


        public string OutputFile
        {
            get => NSUserDefaults.StandardUserDefaults.StringForKey(PreferenceNames.OutputFile);
            set => NSUserDefaults.StandardUserDefaults.SetString(value, PreferenceNames.OutputFile);
        }

        public RandomizerDifficulty RandomizerDifficulty
        {
            get
            {
                if (!Enum.IsDefined(typeof(RandomizerDifficulty), RandomizerDifficultyRaw))
                {
                    NSUserDefaults.StandardUserDefaults.RemoveObject(PreferenceNames.RandomizerDifficulty);
                }
                return (RandomizerDifficulty)Enum.ToObject(typeof(RandomizerDifficulty), RandomizerDifficultyRaw);
            }
            set => RandomizerDifficultyRaw = (int)value;
        }

        public nint RandomizerDifficultyRaw
        {
            get => NSUserDefaults.StandardUserDefaults.IntForKey(PreferenceNames.RandomizerDifficulty);
            set => NSUserDefaults.StandardUserDefaults.SetInt(value, PreferenceNames.RandomizerDifficulty);
        }


        public bool CreateSpoilerLog
        {
            get => NSUserDefaults.StandardUserDefaults.BoolForKey(PreferenceNames.CreateSpoilerLog);
            set => NSUserDefaults.StandardUserDefaults.SetBool(value, PreferenceNames.CreateSpoilerLog);
        }

        public bool SramTrace
        {
            get => NSUserDefaults.StandardUserDefaults.BoolForKey(PreferenceNames.SramTrace);
            set => NSUserDefaults.StandardUserDefaults.SetBool(value, PreferenceNames.SramTrace);
        }

        public AlttpRandomizer.Random.HeartBeepSpeed HeartBeepSpeed
        {
            get
            {
                if (!Enum.IsDefined(typeof(HeartBeepSpeed), HeartBeepSpeedRaw))
                {
                    NSUserDefaults.StandardUserDefaults.RemoveObject(PreferenceNames.HeartBeepSpeed);
                }
                return (HeartBeepSpeed)Enum.ToObject(typeof(HeartBeepSpeed), HeartBeepSpeedRaw);
            }
            set => HeartBeepSpeedRaw = (int)value;
        }

        public nint HeartBeepSpeedRaw
        {
            get => NSUserDefaults.StandardUserDefaults.IntForKey(PreferenceNames.HeartBeepSpeed);
            set => NSUserDefaults.StandardUserDefaults.SetInt(value, PreferenceNames.HeartBeepSpeed);
        }

        public int BulkCreateCount
        {
            get => (int)NSUserDefaults.StandardUserDefaults.IntForKey(PreferenceNames.BulkCreateCount);
            set => NSUserDefaults.StandardUserDefaults.SetInt(value, PreferenceNames.BulkCreateCount);
        }

        public bool ShowComplexity
        {
            get => NSUserDefaults.StandardUserDefaults.BoolForKey(PreferenceNames.ShowComplexity);
            set => NSUserDefaults.StandardUserDefaults.SetBool(value, PreferenceNames.ShowComplexity);
        }

        public NSUrl ParentDirectory {
            get => NSUserDefaults.StandardUserDefaults.URLForKey(PreferenceNames.ParentDirectory);
            set => NSUserDefaults.StandardUserDefaults.SetURL(value, PreferenceNames.ParentDirectory);
        }

        public void Save()
        {
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }
    }
}

namespace AlltpRandom
{
    static class AlltpHelpers
    {
        internal static HeartBeepSpeed? ToHeartBeep(nint rawVal)
        {
            if (!Enum.IsDefined(typeof(HeartBeepSpeed), (int)rawVal))
            {
                return null;
            }
            return (HeartBeepSpeed)Enum.ToObject(typeof(HeartBeepSpeed), rawVal);
        }

        internal static RandomizerDifficulty? ToDifficulty(nint rawVal)
        {
            if (!Enum.IsDefined(typeof(RandomizerDifficulty), (int)rawVal))
            {
                return null;
            }
            return (RandomizerDifficulty)Enum.ToObject(typeof(RandomizerDifficulty), rawVal);
        }
    }

    static class PreferenceNames 
    {
        internal const string OutputFile = "OutputFile";
        internal const string RandomizerDifficulty = "RandomizerDifficulty";
        internal const string CreateSpoilerLog = "CreateSpoilerLog";
        internal const string SramTrace = "sramTrace";
        internal const string HeartBeepSpeed = "HeartBeepSpeed";
        internal const string BulkCreateCount = "BulkCreateCount";
        internal const string ShowComplexity = "ShowComplexity";
        internal const string ParentDirectory = "ParentDirectory";
    }
}
