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

        private static Regex _uncRegex = new Regex(@"((\\\\[^ \\/:*?""<>|]+\\[^ \\/:*?""<>|]+([^ \\/:*?""<>|]+)*)|([a-zA-Z]:))(\\[^ \\/:*?""<>|]+([^ \\/:*?""<>|]+)*)*\\?", RegexOptions.Compiled);
        private static Regex _uriRegex = new Regex(@"https?://[-a-zA-Z0-9@:%._\+~#=]{2,256}\b([^\\'|`^""<>)(}{\]\[]*)", RegexOptions.Compiled);

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

            var subAmount1 = 0;
            foreach (var uriMatch in uriMatches.OrderBy(m => m.Index))
            {
                if (uriMatch.Index != 0 && newString.Length > (uriMatch.Index - subAmount1))
                {
                    var s = newString.Substring(0, (uriMatch.Index - subAmount1));

                    var uncMatches = _uncRegex.Matches(s).Cast<Match>();
                    var subAmount2 = 0;
                    foreach (var uncMatch in uncMatches.OrderBy(m => m.Index))
                    {
                        if (uncMatch.Index != 0 && s.Length > (uncMatch.Index - subAmount2))
                        {
                            var s2 = s.Substring(0, (uncMatch.Index - subAmount2));
                            runs.Add(new Run(s2));
                        }
                        runs.Add(CreateUncHyperLink(uncMatch.Value));
                        s = s.Substring((uncMatch.Index - subAmount2) + uncMatch.Length);
                        subAmount2 += uncMatch.Index + uncMatch.Length;
                    }
                    runs.Add(new Run(s));
                }
                runs.Add(CreateUriHyperLink(uriMatch.Value));
                newString = newString.Substring((uriMatch.Index - subAmount1) + uriMatch.Length);
                subAmount1 += uriMatch.Index + uriMatch.Length;
            }
            
            var uncMatches2 = _uncRegex.Matches(newString).Cast<Match>();
            var subAmount3 = 0;
            foreach (var uncMatch in uncMatches2.OrderBy(m => m.Index))
            {
                if (uncMatch.Index != 0 && newString.Length > (uncMatch.Index - subAmount3))
                {
                    var s3 = newString.Substring(0, (uncMatch.Index - subAmount3));
                    runs.Add(new Run(s3));
                }
                runs.Add(CreateUncHyperLink(uncMatch.Value));
                newString = newString.Substring((uncMatch.Index - subAmount3) + uncMatch.Length);
                subAmount3 += uncMatch.Index + uncMatch.Length;
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
            Uri uri = null;
            if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
            {
                var link = new Hyperlink(new Run(uriString));
                link.NavigateUri = new Uri(uriString);
                link.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(NavigateToUnc);
                return link;
            }
            else
            {
                return new Run(uriString);
            }
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
