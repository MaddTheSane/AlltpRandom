using System;
using System.IO;

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
            var beep = AlltpHelpers.ToHeartBeep(heartBeepPopUp.SelectedTag);
            if (beep != null)
            {
                return (HeartBeepSpeed)beep;
            }
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
                difficultyPopUp.SelectItemWithTag((int)RandomizerDifficulty.Casual);
                seedField.StringValue = seedField.StringValue.ToUpper().Replace("C", "");
                difficulty = RandomizerDifficulty.Casual;
            }
            else if (seedField.StringValue.ToUpper().Contains("G"))
            {
                difficultyPopUp.SelectItemWithTag((int)RandomizerDifficulty.Glitched);
                seedField.StringValue = seedField.StringValue.ToUpper().Replace("G", "");
                difficulty = RandomizerDifficulty.Glitched;
            }
            else if (seedField.StringValue.ToUpper().Contains("NORAND"))
            {
                difficultyPopUp.SelectItemWithTag((int)RandomizerDifficulty.None);
                difficulty = RandomizerDifficulty.None;
            }
            else
            {
                var tag = difficultyPopUp.SelectedItem.Tag;
                var selected = AlltpHelpers.ToDifficulty(tag) ?? RandomizerDifficulty.None;
                difficulty = selected;
            }

            return difficulty;
        }

        private string filename
        {
            get
            {
                var dir = directoryField.StringValue;
                var fName = fileNameField.StringValue;
                return Path.Combine(dir, fName);
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

        private void SetSeedBasedOnDifficulty()
        {
            var tag = difficultyPopUp.SelectedItem.Tag;
            var selected = AlltpHelpers.ToDifficulty(tag);
            switch (selected)
            {
                case RandomizerDifficulty.Casual:
                    seedField.StringValue = string.Format("C{0:0000000}", (new SeedRandom()).Next(10000000));
                    break;

                case RandomizerDifficulty.Glitched:
                    seedField.StringValue = string.Format("G{0:0000000}", (new SeedRandom()).Next(10000000));
                    break;

                case RandomizerDifficulty.None:
                    seedField.StringValue = string.Format("NORAND");
                    break;

                default:
                    var alert = new NSAlert
                    {
                        MessageText = "Select Difficulty",
                        InformativeText = "Please select a difficulty."
                    };

                    alert.BeginSheet(this.View.Window);
                    break;
            }
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
        void GetSpoiler(NSObject sender)
        {
            var txt = seedField.StringValue;
            if (txt.Length == 0)
            {
                var alert = new NSAlert
                {
                    MessageText = "No Seed",
                    InformativeText = "There is no seed specified in the seed field.\n\nThis is needed to generate a spoiler from that seed"
                };

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
