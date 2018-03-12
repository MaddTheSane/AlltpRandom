// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AlltpRandom
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        AppKit.NSButton complexityCheck { get; set; }

        [Outlet]
        AppKit.NSPopUpButton difficultyPopUp { get; set; }

        [Outlet]
        AppKit.NSPopUpButton heartBeepPopUp { get; set; }

        [Outlet]
        AppKit.NSTextView outputText { get; set; }

        [Outlet]
        AppKit.NSButton sramTraceCheck { get; set; }

        [Action ("complexityToggle:")]
        partial void complexityToggle (Foundation.NSObject sender);

        [Action ("difficultyChanged:")]
        partial void difficultyChanged (Foundation.NSObject sender);

        [Action ("generateRandomizedROM:")]
        partial void generateRandomizedROM (Foundation.NSObject sender);

        [Action ("heartBeepChanged:")]
        partial void heartBeepChanged (Foundation.NSObject sender);

        [Action ("sramCheckToggle:")]
        partial void sramCheckToggle (Foundation.NSObject sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (outputText != null) {
                outputText.Dispose ();
                outputText = null;
            }

            if (difficultyPopUp != null) {
                difficultyPopUp.Dispose ();
                difficultyPopUp = null;
            }

            if (heartBeepPopUp != null) {
                heartBeepPopUp.Dispose ();
                heartBeepPopUp = null;
            }

            if (sramTraceCheck != null) {
                sramTraceCheck.Dispose ();
                sramTraceCheck = null;
            }

            if (complexityCheck != null) {
                complexityCheck.Dispose ();
                complexityCheck = null;
            }
        }
    }
}
