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
