using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChatApp.Views
{
    public class TextContentView : TextBlock
    {
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.RegisterAttached(
                "Content",
                typeof(string),
                typeof(TextContentView),
                new PropertyMetadata(null, new PropertyChangedCallback(OnContentPropertyChanged)));

        private static Regex _uncRegex = new Regex(@"((\\\\[^ \\/:*?""<>|]+\\[^ \\/:*?""<>|]+([ ]+[^ \\/:*?""<>|]+)*)|([a-zA-Z]:))(\\[^ \\/:*?""<>|]+([^ \\/:*?""<>|]+)*)*\\?", RegexOptions.Compiled);
        private static Regex _uriRegex = new Regex(@"https?://[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([^\\'|`^""<>)(}{\]\[]*)", RegexOptions.Compiled);

        public string Content { get; set; }

        public TextContentView()
        {
        }

        public static void OnContentPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var newString = e.NewValue as string;
            var textBlock = obj as TextBlock;

            if (newString == null)
            {
                textBlock.Text = newString;
                return;
            }

            var uriMatches = _uriRegex.Matches(newString).Cast<Match>();
            var runs = new List<Inline>();

            foreach (var uriMatch in uriMatches.OrderBy(m => m.Index))
            {
                if (uriMatch.Index != 0 && newString.Length > uriMatch.Index)
                {
                    var s = newString.Substring(0, uriMatch.Index - 1);

                    var uncMatches = _uncRegex.Matches(s).Cast<Match>();
                    foreach (var uncMatch in uncMatches.OrderBy(m => m.Index))
                    {
                        if (uncMatch.Index != 0 && s.Length > uncMatch.Index)
                        {
                            var s2 = s.Substring(0, uncMatch.Index - 1);
                            runs.Add(new Run(s2));
                        }
                        runs.Add(CreateUncHyperLink(uncMatch.Value));
                        s = s.Substring(uncMatch.Index + uncMatch.Length);
                    }
                    runs.Add(new Run(s));
                }
                runs.Add(CreateUriHyperLink(uriMatch.Value));
                newString = newString.Substring(uriMatch.Index + uriMatch.Length);
            }
            
            var uncMatches2 = _uncRegex.Matches(newString).Cast<Match>();
            foreach (var uncMatch in uncMatches2.OrderBy(m => m.Index))
            {
                if (uncMatch.Index != 0 && newString.Length > uncMatch.Index)
                {
                    var s3 = newString.Substring(0, uncMatch.Index - 1);
                    runs.Add(new Run(s3));
                }
                runs.Add(CreateUncHyperLink(uncMatch.Value));
                newString = newString.Substring(uncMatch.Index + uncMatch.Length);
            }

            if (newString.Length > 0)
            {
                runs.Add(new Run(newString));
            }
            textBlock.Inlines.Clear();
            textBlock.Inlines.AddRange(runs);
        }

        private static Inline CreateUriHyperLink(string uriString)
        {
            Inline inline;
            try
            {
                var uri = new Uri(uriString);
                var link = new Hyperlink(new Run(uriString));
                link.NavigateUri = new Uri(uriString);
                link.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(NavigateToUri);
                inline = link;
            }
            catch
            {
                inline = new Run(uriString);
            }
            return inline;
        }

        private static Inline CreateUncHyperLink(string uriString)
        {
            Inline inline;
            try
            {
                var uri = new Uri(uriString);
                var link = new Hyperlink(new Run(uriString));
                link.NavigateUri = new Uri(uriString);
                link.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(NavigateToUnc);
                inline = link;
            }
            catch
            {
                inline = new Run(uriString);
            }
            return inline;
        }

        static void NavigateToUri(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
            catch
            {
            }
        }

        static void NavigateToUnc(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.LocalPath));
                e.Handled = true;
            }
            catch
            {
            }
        }
    }
}
