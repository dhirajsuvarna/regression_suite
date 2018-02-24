using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Helper
{
    public class ProcessHelper
    {
        public DataReceivedEventHandler dataReceivedHandler;
        public DataReceivedEventHandler errorReceivedHandler;

        public string EnviornmentPath { get; set; }

        public int Execute(string iProcessName, string iArguments)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = iProcessName;
            proc.StartInfo.Arguments = iArguments;
            //check if the below line (setting path vairable) can be done in better way.
            //there is some problem in using the Enviornment variables, we need to use fully qualified path to start the exe
            if (EnviornmentPath != null)
                proc.StartInfo.EnvironmentVariables["path"] = EnviornmentPath + ";" + proc.StartInfo.EnvironmentVariables["path"];

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true; //this line was added because "fc.exe" was behaving wierd

            proc.ErrorDataReceived += errorReceivedHandler;
            proc.OutputDataReceived += dataReceivedHandler;

            proc.Start();
            proc.BeginOutputReadLine(); //to get real time output from the command
            proc.BeginErrorReadLine();  //to get real time error from the command
            proc.WaitForExit();
            int exitCode = proc.ExitCode;
            proc.Close();

            return exitCode;
        }

    }
}
