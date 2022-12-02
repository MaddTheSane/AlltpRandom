using System;
using System.IO;
using System.Threading;
using System.Text;

using AppKit;
using Foundation;
using AlttpRandomizer.IO;
using AlttpRandomizer.Properties;
using AlttpRandomizer.Random;
using AlttpRandomizer.Rom;

namespace AlltpRandom
{
    public partial class ViewController : NSViewController
    {
        private Thread createRomThread;

        #region Windows wrapping
        private HeartBeepSpeed GetHeartBeepSpeed()
        {
            var beep = AlltpHelpers.ToHeartBeep(heartBeepPopUp.SelectedTag);
            if (beep != null)
            {
                return (HeartBeepSpeed)beep;
            }

            if (Enum.TryParse(heartBeepPopUp.SelectedItem.Title, true, out HeartBeepSpeed retVal))
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
                var tag = difficultyPopUp.SelectedTag;
                var selected = AlltpHelpers.ToDifficulty(tag) ?? RandomizerDifficulty.None;
                difficulty = selected;
            }

            return difficulty;
        }

        private string Filename
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
                Filename = Filename,
                SramTrace = sramTraceCheck.State == NSCellStateValue.On,
                ShowComplexity = complexityCheck.State == NSCellStateValue.On,
                Difficulty = GetRandomizerDifficulty(),
                HeartBeepSpeed = GetHeartBeepSpeed()
            };
        }

        private void SetSeedBasedOnDifficulty()
        {
            var tag = difficultyPopUp.SelectedTag;
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
                    seedField.StringValue = "NORAND";
                    break;

                default:
                    var alert = new NSAlert
                    {
                        MessageText = "Select Difficulty",
                        InformativeText = "Please select a difficulty.",
                        AlertStyle = NSAlertStyle.Warning
                    };

                    alert.BeginSheet(this.View.Window);
                    break;
            }
        }

        private void CreateSpoilerLog(RandomizerDifficulty difficulty)
        {
            if (!int.TryParse(seedField.StringValue, out int parsedSeed))
            {
                var alert = new NSAlert
                {
                    MessageText = "Seed Error",
                    InformativeText = "Seed must be numeric or blank.",
                    AlertStyle = NSAlertStyle.Warning
                };
                alert.BeginSheet(this.View.Window);
                WriteOutput("Seed must be numeric or blank.", true);
            }
            else
            {
                var romPlms = RomLocationsFactory.GetRomLocations(difficulty);
                var log = new RandomizerLog(string.Format(romPlms.SeedFileString, parsedSeed));

                seedField.StringValue = string.Format(romPlms.SeedFileString, parsedSeed);

                try
                {
                    var randomizer = new Randomizer(parsedSeed, romPlms, log);

                    WriteOutput(CreateRomThread(randomizer, new RandomizerOptions { SpoilerOnly = true, Difficulty = difficulty }));
                }
                catch (RandomizationException ex)
                {
                    WriteOutput(ex.ToString(), true);
                }
            }
        }

        private int CreateRom(IRomLocations romLocations, RandomizerLog log, RandomizerOptions options, int parsedSeed)
        {
            var randomizer = new Randomizer(parsedSeed, romLocations, log);

            CreateRomThread(randomizer, options);

            return randomizer.GetComplexity();
        }

        private string CreateRomThread(Randomizer randomizer, RandomizerOptions options)
        {
            var retVal = "";

            SetButtonsEnabled(false);

            createRomThread = new Thread(() => retVal = randomizer.CreateRom(options));
            createRomThread.Start();

            while (createRomThread.IsAlive)
            {
                NSRunLoop.Current.RunUntil(NSDate.Now.AddSeconds(0.2));
            }

            SetButtonsEnabled(true);

            return retVal;
        }

        private void WriteOutput(NSAttributedString text)
        {
            NSOperationQueue.MainQueue.AddOperation(() =>
            {
                outputText.TextStorage.BeginEditing();
                outputText.TextStorage.Append(text);
                outputText.TextStorage.Append(new NSAttributedString("\n"));
                outputText.TextStorage.EndEditing();
            });
        }

        private void WriteOutput(string text, bool error = false)
        {
            var attrStr = new NSMutableAttributedString(text);
            if (error)
            {
                attrStr.AddAttribute(NSStringAttributeKey.ForegroundColor, NSColor.Red, new NSRange(0, attrStr.Length));
            }
            WriteOutput(attrStr);
        }

        private void SetButtonsEnabled(bool enabled)
        {
            bulkGenerateButton.Enabled = enabled;
            generateButton.Enabled = enabled;
            spoilerButton.Enabled = enabled;
        }

        private void WriteCasualMessage()
        {
            var outputText2 = new StringBuilder();

            outputText2.AppendLine("Some quick hints:");
            outputText2.AppendLine("- You can toggle between Shovel & Flute in the inventory screen with the Y button.");
            outputText2.AppendLine("- You can use the Cape to get into Hyrule Castle Tower without the Master Sword.");
            outputText2.AppendLine();

            WriteOutput(outputText2.ToString());
        }

        private void SaveRandomizerSettings()
        {
            Settings.Default.BulkCreateCount = bulkStepper.IntValue;
            Settings.Default.SramTrace = sramTraceCheck.State == NSCellStateValue.On;
            Settings.Default.ShowComplexity = complexityCheck.State == NSCellStateValue.On;
            Settings.Default.OutputFile = fileNameField.StringValue;
            Settings.Default.HeartBeepSpeedRaw = heartBeepPopUp.SelectedTag;
            Settings.Default.RandomizerDifficultyRaw = difficultyPopUp.SelectedTag;
            var dirStr = directoryField.StringValue;
            var dirURL = new NSUrl(dirStr, true);
            if (dirURL.CheckPromisedItemIsReachable(out _))
            {
                Settings.Default.ParentDirectory = dirURL;
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
            directoryField.StringValue = Settings.Default.ParentDirectory.Path;
            heartBeepPopUp.SelectItemWithTag(Settings.Default.HeartBeepSpeedRaw);
            difficultyPopUp.SelectItemWithTag(Settings.Default.RandomizerDifficultyRaw);
            bulkStepper.IntValue = Settings.Default.BulkCreateCount;
            bulkTextField.IntValue = Settings.Default.BulkCreateCount;
        }

        private void ClearOutput()
        {
            outputText.TextStorage.BeginEditing();
            outputText.TextStorage.SetString(new NSAttributedString());
            outputText.TextStorage.EndEditing();
        }

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();
            createRomThread?.Abort();
        }

        #region IBActions
        [Export("changeFolder:")]
        public void ChangeFolder(NSObject sender)
        {
            var panel = NSOpenPanel.OpenPanel;
            panel.CanChooseDirectories = true;
            panel.CanCreateDirectories = true;
            panel.CanChooseFiles = false;
            panel.DirectoryUrl = new NSUrl(directoryField.StringValue, true);

            panel.BeginSheet(this.View.Window, (nint hi) =>
            {
                if (hi == (int)NSModalResponse.OK)
                {
                    var newURL = panel.Url;
                    directoryField.StringValue = newURL.Path;
                }
            });
        }

        [Export("seedTextChanged:")]
        public void SeedTextChanged(NSObject sender)
        {
            if (seedField.StringValue.ToUpper().StartsWith("C", StringComparison.Ordinal))
            {
                if (difficultyPopUp.SelectedTag != (int)RandomizerDifficulty.Casual)
                {
                    difficultyPopUp.SelectItemWithTag((int)RandomizerDifficulty.Casual);
                }
            }
            else if (seedField.StringValue.ToUpper().StartsWith("G", StringComparison.Ordinal))
            {
                if (difficultyPopUp.SelectedTag != (int)RandomizerDifficulty.Glitched)
                {
                    difficultyPopUp.SelectItemWithTag((int)RandomizerDifficulty.Glitched);
                }
            }
            else if (seedField.StringValue.ToUpper() == "NORAND")
            {
                if (difficultyPopUp.SelectedTag != (int)RandomizerDifficulty.None)
                {
                    difficultyPopUp.SelectItemWithTag((int)RandomizerDifficulty.None);
                }
            }
        }


        [Export("difficultyChanged:")]
        public void DifficultyChanged(Foundation.NSObject sender)
        {
            var tag = difficultyPopUp.SelectedTag;
            var difficulty = AlltpHelpers.ToDifficulty(tag) ?? RandomizerDifficulty.Casual;
            switch (difficulty)
            {
                case RandomizerDifficulty.Casual:
                    bulkGenerateButton.Enabled = true;
                    if (seedField.StringValue.ToUpper().StartsWith("G", StringComparison.Ordinal))
                    {
                        seedField.StringValue = seedField.StringValue.ToUpper().Replace('G', 'C');
                    }
                    else if (seedField.StringValue.ToUpper() == "NORAND")
                    {
                        seedField.StringValue = "";
                    }
                    break;
                case RandomizerDifficulty.Glitched:
                    bulkGenerateButton.Enabled = true;
                    if (seedField.StringValue.ToUpper().StartsWith("C", StringComparison.Ordinal))
                    {
                        seedField.StringValue = seedField.StringValue.ToUpper().Replace('C', 'G');
                    }
                    else if (seedField.StringValue.ToUpper() == "NORAND")
                    {
                        seedField.StringValue = "";
                    }
                    break;
                case RandomizerDifficulty.None:
                    seedField.StringValue = "NORAND";
                    bulkGenerateButton.Enabled = false;
                    break;
            }
        }

        [Export("generateMultipleROMs:")]
        public void GenerateMultipleROMs(NSObject sender)
        {
            NSAlert alert;
            if (!Filename.Contains("<seed>"))
            {
                alert = new NSAlert
                {
                    MessageText = "Filename Error",
                    InformativeText = "Bulk create requires \"<seed>\" be in the file name.",
                    AlertStyle = NSAlertStyle.Critical
                };
                alert.BeginSheet(this.View.Window);
                WriteOutput("Bulk create requires \"<seed>\" be in the file name.", true);
            }
            else
            {
                ClearOutput();

                SetSeedBasedOnDifficulty();

                var difficulty = GetRandomizerDifficulty();

                if (difficulty == RandomizerDifficulty.None)
                {
                    return;
                }

                if (difficulty == RandomizerDifficulty.Casual)
                {
                    WriteCasualMessage();
                }

                int successCount = 0;
                int failCount = 0;

                for (int seedNum = 0; seedNum < bulkStepper.IntValue; seedNum++)
                {
                    var parsedSeed = new SeedRandom().Next(10000000);
                    var romLocations = RomLocationsFactory.GetRomLocations(difficulty);
                    RandomizerLog log = null;

                    var outputString = new StringBuilder();
                    outputString.Append("Creating Seed: ");
                    outputString.AppendFormat(romLocations.SeedFileString, parsedSeed);
                    outputString.AppendFormat(" ({0} Difficulty){1}", romLocations.DifficultyName, Environment.NewLine);
                    WriteOutput(outputString.ToString());

                    if (spoilerLogCheck.State == NSCellStateValue.On)
                    {
                        log = new RandomizerLog(string.Format(romLocations.SeedFileString, parsedSeed));
                    }

                    seedField.StringValue = string.Format(romLocations.SeedFileString, parsedSeed);

                    try
                    {

                        var complexity = CreateRom(romLocations, log, GetOptions(), parsedSeed);

                        outputString = new StringBuilder();
                        outputString.AppendFormat("Completed Seed: ");
                        outputString.AppendFormat(romLocations.SeedFileString, parsedSeed);
                        if (complexityCheck.State == NSCellStateValue.On)
                        {
                            outputString.AppendFormat(" ({0} Difficulty - Complexity {2}){1}{1}", romLocations.DifficultyName, Environment.NewLine, complexity);
                        }
                        else
                        {
                            outputString.AppendFormat(" ({0} Difficulty){1}{1}", romLocations.DifficultyName, Environment.NewLine);
                        }
                        WriteOutput(outputString.ToString());

                        successCount++;
                    }
                    catch (RandomizationException ex)
                    {
                        outputString = new StringBuilder();
                        outputString.AppendFormat("FAILED Creating Seed: ");
                        outputString.AppendFormat(romLocations.SeedFileString, parsedSeed);
                        outputString.AppendFormat(" ({0} Difficulty) - {1}{2}{2}", romLocations.DifficultyName, ex.Message, Environment.NewLine);
                        WriteOutput(outputString.ToString());

                        failCount++;
                        seedNum--;

                        if (failCount >= 3)
                        {
                            WriteOutput(string.Format("Stopping bulk creation after {0} failures.{1}", failCount, Environment.NewLine), true);
                        }

                    }
                }

                var finishedString = new NSMutableAttributedString(string.Format("Completed! {0} successful", successCount));

                if (failCount > 0)
                {
                    finishedString.Append(new NSAttributedString(", "));
                    finishedString.Append(new NSAttributedString(string.Format("{0} failed. ", failCount), null, NSColor.Red));
                }
                else
                {
                    finishedString.Append(new NSAttributedString("."));
                }

                WriteOutput(finishedString);
                alert = new NSAlert
                {
                    MessageText = "Bulk Creation Complete",
                    InformativeText = finishedString.Value,
                    AlertStyle = failCount > 0 ? NSAlertStyle.Informational : NSAlertStyle.Warning
                };
                alert.BeginSheet(this.View.Window);
            }

            SaveRandomizerSettings();
        }

        [Export("generateRandomizedROM:")]
        public void GenerateRandomizedROM(NSObject sender)
        {
            if (string.IsNullOrWhiteSpace(seedField.StringValue))
            {
                SetSeedBasedOnDifficulty();
            }

            ClearOutput();

            var options = GetOptions();

            if (options.Difficulty == RandomizerDifficulty.None)
            {
                options.NoRandomization = true;
                var randomizer = new Randomizer(0, new RomLocationsNoRandomization(), null);
                randomizer.CreateRom(options);
                WriteOutput("Non-randomized rom created.");

                return;
            }


            if (!int.TryParse(seedField.StringValue, out int parsedSeed))
            {
                var alert = new NSAlert
                {
                    MessageText = "Seed Error",
                    InformativeText = "Seed must be numeric or blank.",
                    AlertStyle = NSAlertStyle.Warning
                };

                alert.BeginSheet(this.View.Window);

                WriteOutput("Seed must be numeric or blank.", true);
            }
            else
            {
                try
                {
                    var romLocations = RomLocationsFactory.GetRomLocations(options.Difficulty);
                    RandomizerLog log = null;

                    if (spoilerLogCheck.State == NSCellStateValue.On)
                    {
                        log = new RandomizerLog(string.Format(romLocations.SeedFileString, parsedSeed));
                    }

                    seedField.StringValue = string.Format(romLocations.SeedFileString, parsedSeed);

                    if (options.Difficulty == RandomizerDifficulty.Casual)
                    {
                        WriteCasualMessage();
                    }

                    var complexity = CreateRom(romLocations, log, options, parsedSeed);

                    var outputString = new StringBuilder();

                    outputString.AppendFormat("Done!{0}{0}{0}Seed: ", Environment.NewLine);
                    outputString.AppendFormat(romLocations.SeedFileString, parsedSeed);
                    if (complexityCheck.State == NSCellStateValue.On)
                    {
                        outputString.AppendFormat(" ({0} Difficulty - Complexity {2}){1}{1}", romLocations.DifficultyName, Environment.NewLine, complexity);
                    }
                    else
                    {
                        outputString.AppendFormat(" ({0} Difficulty){1}{1}", romLocations.DifficultyName, Environment.NewLine);
                    }

                    WriteOutput(outputString.ToString());
                }
                catch (RandomizationException ex)
                {
                    WriteOutput(ex.ToString(), true);
                }
            }

            SaveRandomizerSettings();
        }

        [Export("getSpoiler:")]
        public void GetSpoiler(NSObject sender)
        {
            var txt = seedField.StringValue;
            if (txt.Length == 0)
            {
                var alert = new NSAlert
                {
                    MessageText = "No Seed",
                    InformativeText = "There is no seed specified in the seed field.\n\nThis is needed to generate a spoiler for that seed.",
                    AlertStyle = NSAlertStyle.Warning
                };

                alert.BeginSheet(this.View.Window);
                WriteOutput("No seed specified.", true);
                return;
            }
            ClearOutput();
            var difficulty = GetRandomizerDifficulty();
            CreateSpoilerLog(difficulty);

        }

        [Export("bulkChanged:")]
        public void BulkChanged(Foundation.NSObject sender)
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
