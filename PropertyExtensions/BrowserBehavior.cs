using System.Windows;
using System.Windows.Controls;

namespace AnkiEditor.PropertyExtensions
{
    /// <summary>
    /// https://stackoverflow.com/a/4204350
    /// </summary>
    public class BrowserBehavior
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html",
            typeof(string),
            typeof(BrowserBehavior),
            new FrameworkPropertyMetadata(OnHtmlChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser d)
        {
            return (string)d.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebBrowser d, string value)
        {
            d.SetValue(HtmlProperty, value);
        }

        static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is WebBrowser webBrowser)
                webBrowser.NavigateToString(e.NewValue as string ?? "&nbsp;");
        }
    }

}
