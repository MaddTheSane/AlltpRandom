using System;

using AppKit;
using Foundation;
using AlttpRandomizer.Properties;

namespace AlltpRandom
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            sramTraceCheck.State = Settings.Default.SramTrace ? NSCellStateValue.On : NSCellStateValue.Off;
            complexityCheck.State = Settings.Default.ShowComplexity ? NSCellStateValue.On : NSCellStateValue.Off;
        }

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

        }

        [Export("getSpoiler:")]
        void GetSpoiler(Foundation.NSObject sender)
        {

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
