using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace regression_test_suite
{
    enum eStatus { READY, COMPLETED, FAILED, SKIPPED, STARTED }; //need to see if PropertyChanged Notification would be better?

    abstract class Step : INotifyPropertyChanged
    {
        public Step()
        {
            Status = eStatus.READY;
            IsSkippable = true;
            LogPath = Path.GetTempFileName().Replace(".tmp", ".txt");
            log = log4net.LogManager.GetLogger(this.GetType());
        }

        ///////////////////////////////////////////////////////////////////////////////

        #region DEFINING  DATA MEMBERS AND PROPERTIES

        protected readonly log4net.ILog log = null;// Log4Net logger.
        public string Name { get; set; }

        public bool IsSkippable { get; set; }
        private string logPath;
        public string LogPath
        {
            get
            {
                return logPath;
            }
            set
            {
                logPath = value;
            }
        }
        private eStatus status;
        public eStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////

        #region DEFINING EVENTS AND ITS HANDLERS
       
        public event PropertyChangedEventHandler PropertyChanged;
        public event StepEventHandler StepStarted;
        public event StepEventHandler StepCompleted;
        public event StepFailedEventHandler StepFailed;
        public event StepProgressEventHandler StepProgress; //this is not implemented properly

        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        protected void OnStepStarted(StepEventArgs e)
        {
            this.Status = eStatus.STARTED;
            log.Info(this.Name + ": Started");
            
            if (StepStarted != null)
            {
                StepStarted(this, e);
            }
        }

        protected void OnStepCompleted(StepEventArgs e)
        {
            this.Status = eStatus.COMPLETED;
            log.Info(this.Name + ": Completed");

            if (StepCompleted != null)
            {
                StepCompleted(this, e);
            }
        }

        protected void OnStepFailed(StepFailedEventArgs e)
        {
            this.Status = eStatus.FAILED;
            if (e.IsException)
                log.Error("Exception", e.Xception);
            else
                log.Error(e.ErrorString);

            log.Error(this.Name + ": Failed");

            if (StepFailed != null)
            {
                StepFailed(this, e);
            }
        }

        protected void OnStepProgess(StepProgressEventArgs e) //this is not implemented properly
        {
            if (StepProgress != null)
            {
                StepProgress(this, e);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////

        #region ABSTRACT METHODS

        public abstract void Execute(object iObject);

        #endregion

        ///////////////////////////////////////////////////////////////////////////////

        #region OTHER REUSABLE METHODS

        public void SetFileAppenderPath(string iFileAppenderName)
        {
            log4net.Repository.ILoggerRepository repository = log4net.LogManager.GetRepository();
            foreach (var thisAppender in repository.GetAppenders())
            {
                if (thisAppender is log4net.Appender.FileAppender && thisAppender.Name.Equals(iFileAppenderName))
                {
                    log4net.Appender.FileAppender fileAppender = thisAppender as log4net.Appender.FileAppender;
                    fileAppender.File = LogPath;
                    fileAppender.ActivateOptions();
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////

    }
}
