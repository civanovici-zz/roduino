using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Helpers
{
    public class Flash
    {
        private string error = String.Empty;
        private string message = String.Empty;
        private List<string> validationErrorMessages = new List<string>();


        public Flash()
        {
        }


        public Flash(string error)
        {
            this.error = error;
        }


        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public List<string> ValidationErrorMessages
        {
            get { return validationErrorMessages; }
            set { validationErrorMessages = value; }
        }

        public string ValidationMessage
        {
            get
            {
                string s = String.Empty;
                if (validationErrorMessages != null)
                    foreach (string err in validationErrorMessages)
                        s += err + Environment.NewLine;
                return s;
            }
        }

        public bool ContainErrorMessage(string errorMessage)
        {
            foreach (string s in validationErrorMessages)
            {
                if (s.Equals(errorMessage)) return true;
            }
            return false;
        }
    }
}
