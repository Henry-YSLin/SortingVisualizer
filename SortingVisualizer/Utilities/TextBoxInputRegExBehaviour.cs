using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SortingVisualizer.Utilities;

/// <summary>
///     Regular expression for TextBox with properties:
///         <see cref="RegularExpression"/>,
///         <see cref="MaxLength"/>,
///         <see cref="EmptyValue"/>.
/// </summary>
public class TextBoxInputRegExBehaviour : Behavior<TextBox>
{
    #region DependencyProperties
    public static readonly DependencyProperty REGULAR_EXPRESSION_PROPERTY =
        DependencyProperty.Register("RegularExpression", typeof(string), typeof(TextBoxInputRegExBehaviour), new FrameworkPropertyMetadata(".*"));

    public string RegularExpression
    {
        get => (string)GetValue(REGULAR_EXPRESSION_PROPERTY);
        set => SetValue(REGULAR_EXPRESSION_PROPERTY, value);
    }

    public static readonly DependencyProperty MAX_LENGTH_PROPERTY =
        DependencyProperty.Register("MaxLength", typeof(int), typeof(TextBoxInputRegExBehaviour),
            new FrameworkPropertyMetadata(int.MinValue));

    public int MaxLength
    {
        get => (int)GetValue(MAX_LENGTH_PROPERTY);
        set => SetValue(MAX_LENGTH_PROPERTY, value);
    }

    public static readonly DependencyProperty EMPTY_VALUE_PROPERTY =
        DependencyProperty.Register("EmptyValue", typeof(string), typeof(TextBoxInputRegExBehaviour), null);

    public string EmptyValue
    {
        get => (string)GetValue(EMPTY_VALUE_PROPERTY);
        set => SetValue(EMPTY_VALUE_PROPERTY, value);
    }
    #endregion

    /// <summary>
    ///     Attach our behaviour. Add event handlers
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.PreviewTextInput += previewTextInputHandler;
        AssociatedObject.PreviewKeyDown += previewKeyDownHandler;
        DataObject.AddPastingHandler(AssociatedObject, pastingHandler);
    }

    /// <summary>
    ///     Deattach our behaviour. remove event handlers
    /// </summary>
    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject.PreviewTextInput -= previewTextInputHandler;
        AssociatedObject.PreviewKeyDown -= previewKeyDownHandler;
        DataObject.RemovePastingHandler(AssociatedObject, pastingHandler);
    }

    #region Event handlers [PRIVATE] --------------------------------------

    void previewTextInputHandler(object sender, TextCompositionEventArgs e)
    {
        string text;
        if (this.AssociatedObject.Text.Length < this.AssociatedObject.CaretIndex)
            text = this.AssociatedObject.Text;
        else
        {
            //  Remaining text after removing selected text.

            text = treatSelectedText(out var remainingTextAfterRemoveSelection)
                ? remainingTextAfterRemoveSelection.Insert(AssociatedObject.SelectionStart, e.Text)
                : AssociatedObject.Text.Insert(this.AssociatedObject.CaretIndex, e.Text);
        }

        e.Handled = !validateText(text);
    }

    /// <summary>
    ///     PreviewKeyDown event handler
    /// </summary>
    void previewKeyDownHandler(object sender, KeyEventArgs e)
    {
        if (string.IsNullOrEmpty(this.EmptyValue))
            return;

        string text = null;

        // Handle the Backspace key
        if (e.Key == Key.Back)
        {
            if (!this.treatSelectedText(out text))
            {
                if (AssociatedObject.SelectionStart > 0)
                    text = this.AssociatedObject.Text.Remove(AssociatedObject.SelectionStart - 1, 1);
            }
        }
        // Handle the Delete key
        else if (e.Key == Key.Delete)
        {
            // If text was selected, delete it
            if (!this.treatSelectedText(out text) && this.AssociatedObject.Text.Length > AssociatedObject.SelectionStart)
            {
                // Otherwise delete next symbol
                text = this.AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, 1);
            }
        }

        if (text == string.Empty)
        {
            this.AssociatedObject.Text = this.EmptyValue;
            if (e.Key == Key.Back)
                AssociatedObject.SelectionStart++;
            e.Handled = true;
        }
    }

    private void pastingHandler(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(DataFormats.Text))
        {
            string text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));

            if (!validateText(text))
                e.CancelCommand();
        }
        else
            e.CancelCommand();
    }
    #endregion Event handlers [PRIVATE] -----------------------------------

    #region Auxiliary methods [PRIVATE] -----------------------------------

    /// <summary>
    ///     Validate certain text by our regular expression and text length conditions
    /// </summary>
    /// <param name="text"> Text for validation </param>
    /// <returns> True - valid, False - invalid </returns>
    private bool validateText(string text)
    {
        return (new Regex(this.RegularExpression, RegexOptions.IgnoreCase)).IsMatch(text) && (MaxLength == int.MinValue || text.Length <= MaxLength);
    }

    /// <summary>
    ///     Handle text selection
    /// </summary>
    /// <returns>true if the character was successfully removed; otherwise, false. </returns>
    private bool treatSelectedText(out string text)
    {
        text = null;
        if (AssociatedObject.SelectionLength <= 0)
            return false;

        var length = this.AssociatedObject.Text.Length;
        if (AssociatedObject.SelectionStart >= length)
            return true;

        if (AssociatedObject.SelectionStart + AssociatedObject.SelectionLength >= length)
            AssociatedObject.SelectionLength = length - AssociatedObject.SelectionStart;

        text = this.AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, AssociatedObject.SelectionLength);
        return true;
    }
    #endregion Auxiliary methods [PRIVATE] --------------------------------
}
