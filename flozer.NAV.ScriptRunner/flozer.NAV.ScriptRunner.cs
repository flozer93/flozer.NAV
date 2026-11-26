using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flozer.NAV.ScriptRunner
{
    public class ScriptRunner
    {
        //ToDO
        // - Function to Check Script / App exists
        // - 
        // - 
        public string Run(string StringP)
        {
            try
            {
                if (!File.Exists(StringP))
                {
                    return "File not found!";
                }
                if (StringP.EndsWith(".ps1") || StringP.EndsWith(".psm1") || StringP.EndsWith(".psd1"))
                {
                    return RunScriptPowerShell(StringP);
                }
                if (StringP.EndsWith(".bat") || StringP.EndsWith(".cmd") || StringP.EndsWith(".nt"))
                {
                    return RunScriptCmd(StringP);
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        internal string RunScriptPowerShell(string StringP)
        {
            try
            {
                ProcessStartInfo processInfo;
                processInfo = new ProcessStartInfo("powershell.exe", "-File " + StringP);
                //processInfo = new ProcessStartInfo("powershell.exe", "-NoProfile -ExecutionPolicy ByPass " + "-File " + StringP);

                return RunScript(processInfo);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private string RunScriptCmd(string StringP)
        {
            try
            {
                ProcessStartInfo processInfo;
                processInfo = new ProcessStartInfo("powershell.exe", "/c " + StringP);

                return RunScript(processInfo);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private string RunScript(ProcessStartInfo processInfo)
        {
            try
            {
                Process process;
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;

                process = Process.Start(processInfo);
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                string message = output;
                if (!String.IsNullOrEmpty(error))
                {
                    message += error;
                }

                process.Close();

                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
