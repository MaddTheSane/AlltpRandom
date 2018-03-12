using System;
using Foundation;

namespace AlttpRandomizer.Properties
{
    internal class Resources
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources()
        {
        }

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AlttpRandomizer.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }


        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] RomImage
        {
            get
            {
                object obj = ResourceManager.GetObject("RomImage", resourceCulture);
                return ((byte[])(obj));
            }
        }
    }

    internal partial class Settings
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

        public void Save() {
            // do nothing
        }
    }
}