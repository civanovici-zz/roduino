using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace RoDuino.SMS.Components
{
    public class RoPasswordBox : TextBox
    {
        private int lastCursorPosition;
        private char passwordMaskChar = '●';
        private string text;

        public RoPasswordBox()
        {
            text = string.Empty;
            lastCursorPosition = 0;
            this.TextChanged += PasswordTextBox_TextChanged;
        }

        public new string Text
        {
            get { return text; }
            set { text = value; }
        }

        public void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //            Debug.WriteLine("In PasswordTextBox_TextChanged");
            if (base.Text.Length > text.Length)
            {
                // append some chars in a position position
                for (int i = 0; i < base.Text.Length; i++)
                {
                    if (base.Text[i] != passwordMaskChar)
                    {
                        text = text.Insert(i, base.Text[i].ToString());
                    }
                }
            }
            else if (base.Text.Length == text.Length)
            {
                // replace an existing char
                for (int i = 0; i < base.Text.Length; i++)
                {
                    if (base.Text[i] != passwordMaskChar)
                    {
                        text = text.Substring(0, i) + base.Text[i] + text.Substring(i + 1);
                    }
                }
            }
            else if (base.Text.Length == 0)
            {
                text = "";
            }
            else if (base.Text.Length < text.Length)
            {
                int amountOfCharsToReplace = text.Length - base.Text.Length + 1;
                text = text.Substring(0, lastCursorPosition) + base.Text[lastCursorPosition] +
                       text.Substring(lastCursorPosition + amountOfCharsToReplace);
            }

            //            Debug.WriteLine(text);

            MaskText();
        }

        public void MaskText()
        {
            int cursorPosition = this.SelectionStart;
            base.Text = new string(passwordMaskChar, text.Length);
            this.Select((cursorPosition > text.Length ? text.Length : cursorPosition), 0);
        }
    }
}
