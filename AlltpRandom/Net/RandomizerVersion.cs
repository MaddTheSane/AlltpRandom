using System;
using System.Text.RegularExpressions;
using AppKit;
//using System.Windows.Forms;

namespace AlttpRandomizer.Net
{
    public static class RandomizerVersion
    {
        public const string Current = "7";
        public const bool Debug = false;

        public static string CurrentDisplay
        {
            get
            {
                var retVal = Current;

                if (retVal.Contains("P"))
                {
                    retVal = string.Format("{0})", retVal.Replace("P", " (preview "));
                }

                return retVal;
            }
        }

        private const int checkVersion = 7;
        private static readonly string checkAddress = "http://dessyreqt.github.io/alttprandomizer/?" + DateTime.Now.Ticks;
        private const string updateAddress = "http://dessyreqt.github.io/alttprandomizer/";

        public static void CheckUpdate()
        {
            try
            {
                var response = GetResponse(checkAddress);

                if (string.IsNullOrWhiteSpace(response))
                    return;

                const string pattern = "Current Version: (?<version>\\S+)";
                var match = Regex.Match(response, pattern);

                if (match.Success)
                {
                    var currentVersion = match.Groups["version"].Value;
                    int currentVersionNum;

                    if (int.TryParse(currentVersion, out currentVersionNum))
                    {
                        if (checkVersion < currentVersionNum)
                        {
                            var alert = new NSAlert
                            {
                                MessageText = "Version Update",
                                InformativeText = string.Format(
                                        "You have v{0} and the current version is v{1}. Would you like to update?",
                                        Current,
                                        currentVersion)
                            };

                            alert.AddButton("Update");
                            alert.AddButton("Cancel");


                            var result = alert.RunModal();

                            if (result == 1000/*NSAlertFirstButtonReturn*/) {
                                //Help.ShowHelp(null, updateAddress);
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                // check for update failed, do nothing here
            }
        }

        private static string GetResponse(string address)
        {
            if (!address.Contains("dessyreqt.github.io/alttprandomizer"))
                return "";

            /*
            var webBrowser = new WebBrowser { ScrollBarsEnabled = false, ScriptErrorsSuppressed = true };
            webBrowser.Navigate(address);
            while (webBrowser.ReadyState != WebBrowserReadyState.Complete) { Application.DoEvents(); }

            return webBrowser.Document?.Body?.InnerHtml;
            */
            return "";
        }
    }
}
