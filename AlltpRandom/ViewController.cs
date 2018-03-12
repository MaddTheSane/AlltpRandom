using System;

using AppKit;
using Foundation;
using AlttpRandomizer.IO;
using AlttpRandomizer.Net;
using AlttpRandomizer.Properties;
using AlttpRandomizer.Random;
using AlttpRandomizer.Rom;

namespace AlltpRandom
{
    public partial class ViewController : NSViewController
    {
        #region Windows wrapping
        private HeartBeepSpeed GetHeartBeepSpeed()
        {
            HeartBeepSpeed retVal;

            if (Enum.TryParse(heartBeepPopUp.SelectedItem.Title, true, out retVal))
            {
                return retVal;
            }

            return HeartBeepSpeed.Normal;
        }

        private RandomizerDifficulty GetRandomizerDifficulty()
        {
            RandomizerDifficulty difficulty;

            if (seedField.StringValue.ToUpper().Contains("C"))
            {
                difficultyPopUp.SelectItem("Casual");
                seedField.StringValue = seedField.StringValue.ToUpper().Replace("C", "");
                difficulty = RandomizerDifficulty.Casual;
            }
            else if (seedField.StringValue.ToUpper().Contains("G"))
            {
                difficultyPopUp.SelectItem("Glitched");
                seedField.StringValue = seedField.StringValue.ToUpper().Replace("G", "");
                difficulty = RandomizerDifficulty.Glitched;
            }
            else if (seedField.StringValue.ToUpper().Contains("NORAND"))
            {
                difficultyPopUp.SelectItem("No Randomization");
                difficulty = RandomizerDifficulty.None;
            }
            else
            {
                switch (difficultyPopUp.SelectedItem.Title)
                {
                    case "Casual":
                        difficulty = RandomizerDifficulty.Casual;
                        break;
                    case "Glitched":
                        difficulty = RandomizerDifficulty.Glitched;
                        break;
                    default:
                        return RandomizerDifficulty.None;
                }
            }

            return difficulty;
        }

        private string filename {
            get {
                var dir = directoryField.StringValue;
                var fName = fileNameField.StringValue;
                var str = new NSString(dir);
                str = str.AppendPathComponent(new NSString(fName));
                return str.ToString();
            }
        }

        private RandomizerOptions GetOptions()
        {
            return new RandomizerOptions
            {
                Filename = filename,
                SramTrace = sramTraceCheck.State == NSCellStateValue.On,
                ShowComplexity = complexityCheck.State == NSCellStateValue.On,
                Difficulty = GetRandomizerDifficulty(),
                HeartBeepSpeed = GetHeartBeepSpeed(),
            };
        }
        #endregion

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            sramTraceCheck.State = Settings.Default.SramTrace ? NSCellStateValue.On : NSCellStateValue.Off;
            complexityCheck.State = Settings.Default.ShowComplexity ? NSCellStateValue.On : NSCellStateValue.Off;
            fileNameField.StringValue = Settings.Default.OutputFile;
        }

        #region IBActions
        [Export("changeFolder:")]
        void ChangeFolder(Foundation.NSObject sender)
        {

        }

        [Export("complexityToggle:")]
        void ComplexityToggle(Foundation.NSObject sender)
        {

        }

        [Export("difficultyChanged:")]
        void DifficultyChanged(Foundation.NSObject sender)
        {

        }

        [Export("generateRandomizedROM:")]
        void GenerateRandomizedROM(Foundation.NSObject sender)
        {
            if (sender is NSButton senderButton)
            {
                if (senderButton.Tag == 1)
                {
                    // TODO: Generate multiple ROMs.
                    return;
                }
            }
            // TODO: Generate single ROM.
        }

        [Export("getSpoiler:")]
        void GetSpoiler(Foundation.NSObject sender)
        {
            var txt = seedField.StringValue;
            if (txt.Length == 0)
            {
                var alert = new NSAlert();
                alert.MessageText = "No Seed";
                alert.InformativeText = "There is no seed specified in the seed field.\n\nThis is needed to ";

                alert.BeginSheet(this.View.Window);
                return;
            }

        }

        [Export("heartBeepChanged:")]
        void HeartBeepChanged(Foundation.NSObject sender)
        {

        }

        [Export("spoilerLogToggle:")]
        void SpoilerLogToggle(Foundation.NSObject sender)
        {

        }

        [Export("sramCheckToggle:")]
        void SramCheckToggle(Foundation.NSObject sender)
        {

        }
        #endregion

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
