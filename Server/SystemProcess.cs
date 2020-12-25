using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Server {
    public class SystemProcess {
        public class ExecuteResult {
            public List<string> outBuffer = new List<string>();
            public List<string> errBuffer = new List<string>();
            public int exitCode;

            public bool Success {
                get { return exitCode == 0; }
            }
        }

        public static ExecuteResult Execute(string executable, bool redirectToConsole, string arguments = "", string workingDirectory = "", Dictionary<string, string> envrionmentVars = null) {
            ExecuteResult result = new ExecuteResult();
            Process p = new Process();
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.StartInfo.FileName = executable;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;

            if (envrionmentVars != null) {
                foreach (var dataPair in envrionmentVars) {
                    if (dataPair.Key == null || dataPair.Value == null) {
                        throw new ArgumentException(string.Format("Cannot apply environment variable with key {0} and value {1}", dataPair.Key, dataPair.Value));
                    }

                    p.StartInfo.Environment[dataPair.Key] = dataPair.Value;
                }
            }

            p.OutputDataReceived += (object sender, DataReceivedEventArgs e) => { if (e.Data != null) result.outBuffer.Add(e.Data); if (redirectToConsole) Console.WriteLine(e.Data); };
            p.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => { if (e.Data != null) result.errBuffer.Add(e.Data); if (redirectToConsole) Console.Error.WriteLine(e.Data); };

            try {
                if (!p.Start()) {
                    throw new Exception("Failed to start process: " + executable);
                } else {
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                }
            }
            catch (Exception ex) {
                throw new Exception("Failed to start process: " + executable + ". " + ex.Message);
            }

            p.WaitForExit();
            result.exitCode = p.ExitCode;
            return result;
        }


    }
}
