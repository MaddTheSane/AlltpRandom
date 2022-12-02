using System;
using Foundation;
using AlttpRandomizer.Random;
using AlltpRandom;

namespace AlttpRandomizer.Properties
{
    static class Resources
    {
        /// <summary>
        ///   Looks up a resource of type System.Byte[].
        /// </summary>
        internal static byte[] RomImage
        {
            get
            {
                using NSUrl romImage = NSBundle.MainBundle.GetUrlForResource("alttp", "sfc");
                using NSData romData = NSData.FromUrl(romImage);
                byte[] romBytes = romData.ToArray();
                return romBytes;
            }
        }
    }

    internal class Settings
    {
        public static Settings Default { get; } = new Settings();


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

        public HeartBeepSpeed HeartBeepSpeed
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

        public NSUrl ParentDirectory
        {
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
        private const string outputFile = "OutputFile";
        private const string randomizerDifficulty = "RandomizerDifficulty";
        private const string createSpoilerLog = "CreateSpoilerLog";
        private const string sramTrace = "sramTrace";
        private const string heartBeepSpeed = "HeartBeepSpeed";
        private const string bulkCreateCount = "BulkCreateCount";
        private const string showComplexity = "ShowComplexity";
        private const string parentDirectory = "ParentDirectory";

        internal static string OutputFile => outputFile;
        internal static string RandomizerDifficulty => randomizerDifficulty;
        internal static string CreateSpoilerLog => createSpoilerLog;
        internal static string SramTrace => sramTrace;
        internal static string HeartBeepSpeed => heartBeepSpeed;
        internal static string BulkCreateCount => bulkCreateCount;
        internal static string ShowComplexity => showComplexity;
        internal static string ParentDirectory => parentDirectory;
    }
}
