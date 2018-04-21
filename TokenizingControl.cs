using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AnkiEditor
{
    /**
     * Based on https://blog.pixelingene.com/2010/10/tokenizing-control-convert-text-to-tokens/
     */
    public class TokenizingControl : RichTextBox
    {
        public static readonly DependencyProperty TokenTemplateProperty =
            DependencyProperty.Register(nameof(TokenTemplate), typeof (DataTemplate), typeof (TokenizingControl));

        public DataTemplate TokenTemplate
        {
            get => (DataTemplate) GetValue(TokenTemplateProperty);
            set => SetValue(TokenTemplateProperty, value);
        }

        public Func<string, object> TokenMatcher { get; set; }

        public TokenizingControl()
        {
            TextChanged += OnTokenTextChanged;
        }

        private void OnTokenTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = CaretPosition.GetTextInRun(LogicalDirection.Backward);
            var token = TokenMatcher?.Invoke(text);
            if (token != null)
            {
                ReplaceTextWithToken(text, token);
            }
        }

        private void ReplaceTextWithToken(string inputText, object token)
        {
            // Remove the handler temporarily as we will be modifying tokens below, causing more TextChanged events
            TextChanged -= OnTokenTextChanged;

            var para = CaretPosition.Paragraph;

            if (para != null && para.Inlines.FirstOrDefault(inline => (inline is Run run && run.Text.EndsWith(inputText))) is Run matchedRun) // Found a Run that matched the inputText
            {
                var tokenContainer = CreateTokenContainer(inputText, token);
                para.Inlines.InsertBefore(matchedRun, tokenContainer);

                // Remove only if the Text in the Run is the same as inputText, else split up
                if (matchedRun.Text == inputText)
                {
                    para.Inlines.Remove(matchedRun);
                }
                else // Split up
                {
                    var index = matchedRun.Text.IndexOf(inputText, StringComparison.Ordinal) + inputText.Length;
                    var tailEnd = new Run(matchedRun.Text.Substring(index));
                    para.Inlines.InsertAfter(matchedRun, tailEnd);
                    para.Inlines.Remove(matchedRun);
                }
            }

            TextChanged += OnTokenTextChanged;
        }

        private InlineUIContainer CreateTokenContainer(string inputText, object token)
        {
            // Note: we are not using the inputText here, but could be used in future

            var presenter = new ContentPresenter()
            {
                Content = token,
                ContentTemplate = TokenTemplate,
            };

            // BaselineAlignment is needed to align with Run
            return new InlineUIContainer(presenter) { BaselineAlignment = BaselineAlignment.TextBottom };
        }


    }
}