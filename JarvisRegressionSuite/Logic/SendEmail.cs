using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace regression_test_suite
{
    class SendEmail : Step
    {
        //public List<Step> FailedSteps { get; set; }
        public Step FailedStep { get; set; }

        public override void Execute(object iObject)
        {

            try
            {
                SetFileAppenderPath("ThisStepFileAppender");

                OnStepStarted(new StepEventArgs(iObject));

                MailMessage mailMessage = new MailMessage();
                mailMessage.IsBodyHtml = true;
                mailMessage.From = new MailAddress(Properties.Regression.Default.EmailFromAddress, Properties.Regression.Default.EmailDisplayName);

                string toListPath = Path.GetFullPath(Path.Combine(Properties.Regression.Default.ScriptsFolder, "to_list.txt"));
                string[] toListArray = File.ReadAllLines(toListPath);
                var toList = toListArray.Select(str => str.Split(';'));
                foreach (var thisList in toList)
                {
                    foreach (var toAddress in thisList)
                        mailMessage.To.Add(toAddress);
                }

                string ccListPath = Path.GetFullPath(Path.Combine(Properties.Regression.Default.ScriptsFolder, "cc_list.txt"));
                if (File.Exists(ccListPath))  //cc list is optional
                {
                    string[] ccListArray = File.ReadAllLines(ccListPath);
                    var ccList = ccListArray.Select(str => str.Split(';'));
                    foreach (var thisList in ccList)
                    {
                        foreach (var ccAddress in thisList)
                            mailMessage.To.Add(ccAddress);
                    }
                }

                if (FailedStep != null)
                {
                    DraftFailureMessage(FailedStep, ref mailMessage);

                }
                else
                {
                    DraftSucessMessage(ref mailMessage);
                }



                //smtpServer.Port = 587;
                //smtpServer.Credentials = new System.Net.NetworkCredential("dhiraj.suvarna@gmail.com", "Raghu*123");
                //smtpServer.EnableSsl = true;
                string smtpServer = Properties.Regression.Default.SMTPServer;
                int portNumber = Int16.Parse(Properties.Regression.Default.SMTPPort);
                SmtpClient smtpClient = new SmtpClient(smtpServer, portNumber);
                smtpClient.Send(mailMessage);

                OnStepCompleted(new StepEventArgs(iObject));
            }
            catch (Exception ex)
            {
                OnStepFailed(new StepFailedEventArgs(iObject, ex));
            }
        }

        private void DraftFailureMessage(Step iFailedStep, ref MailMessage iMailMessage)
        {
            log.Info("Drafting Failure Message");
            iMailMessage.Subject = "Creo Cost Regression Failed to Run";

            string emailReport = Path.Combine(Properties.Regression.Default.TemplatePath, "fail_email_template.html");
            string msgBody = File.ReadAllText(emailReport);
            msgBody = msgBody.Replace("{RegTriggerDateTime}", Properties.Internal.Default.RegressionTriggerDateTime);
            msgBody = msgBody.Replace("{RegTimeTaken}", Properties.Internal.Default.RegressionTimeTaken);

            AlternateView errorMsgView = AlternateView.CreateAlternateViewFromString(msgBody, null, "text/html");

            LinkedResource linkedImage = new LinkedResource("regression_status.png");
            linkedImage.ContentId = "screenshot";
            linkedImage.ContentType = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Image.Jpeg);

            errorMsgView.LinkedResources.Add(linkedImage);
            iMailMessage.AlternateViews.Add(errorMsgView);

            if (File.Exists(iFailedStep.LogPath))
                iMailMessage.Attachments.Add(new Attachment(iFailedStep.LogPath));

            string regressionSuiteLog = Path.Combine(Properties.Regression.Default.LogFolderPath, "JarvisRegressionSuite.log");
            if(File.Exists(regressionSuiteLog))
                iMailMessage.Attachments.Add(new Attachment(regressionSuiteLog));
        }

        private void DraftSucessMessage(ref MailMessage mailMessage)
        {
            log.Info("Drafting Sucess Message");

            mailMessage.Subject = "Creo Cost Regression Report";
            string noDiffFile = Path.Combine(Properties.Regression.Default.ReportPath, "zerodiff");
            string emailReport = null;
            if (File.Exists(noDiffFile))
            {
                emailReport = Path.Combine(Properties.Regression.Default.ReportPath, "success_email_template_zerodiff.html");
            }
            else
            {
                emailReport = Path.Combine(Properties.Regression.Default.ReportPath, "success_email_template_nonzerodiff.html");
            }

            //attach sc_vs_creocost report
            string shouldCostReport = Path.Combine(Properties.Regression.Default.ReportPath, "SC_vs_CreoCost_Comparision_Report.xlsx");
            if (File.Exists(shouldCostReport))
                mailMessage.Attachments.Add(new Attachment(shouldCostReport));

            mailMessage.Body = File.ReadAllText(emailReport);
        }
    }
}
