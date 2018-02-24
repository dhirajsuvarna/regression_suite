using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regression_test_suite
{
    delegate void StepEventHandler (object o, StepEventArgs e);

    class StepEventArgs : EventArgs
    {
        public StepEventArgs(object obj)
        {
            argument = obj;
        }

        private object argument;
        public object Argument
        {
            get
            {
                return argument;
            }
        }
    }

    delegate void StepProgressEventHandler (object o, StepProgressEventArgs e);
    class StepProgressEventArgs : EventArgs
    {
        public StepProgressEventArgs(object o, string iProgressString)
        {
            argument = o;
            progressString = iProgressString;
        }

        private object argument;
        public object Argument
        {
            get
            {
                return argument;
            }
        }

        private string progressString;
        public string ProgressString
        {
            get
            {
                return progressString;
            }
        }

    }

    delegate void StepFailedEventHandler(object o, StepFailedEventArgs e);
    class StepFailedEventArgs : EventArgs
    {
        public StepFailedEventArgs(object o, string iErrorString)
        {
            argument = o;
            errorString = iErrorString;
            isException = false;
            exception = null;
        }

        public StepFailedEventArgs(object o, Exception e)
        {
            argument = o;
            exception = e;
            isException = true;
            errorString = "";
        }

        private bool isException;
        public bool IsException
        {
            get
            {
                return isException;
            }
        }

        private object argument;
        public object Argument
        {
            get
            {
                return argument;
            }
        }

        private string errorString;
        public string ErrorString
        {
            get
            {
                return errorString;
            }
        }

        private Exception exception;
        public Exception Xception
        {
            get
            {
                return exception;
            }
        }
    }
}
