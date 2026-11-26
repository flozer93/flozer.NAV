
/*
static void ExecuteCommand(string command)
{
    int exitCode;
    ProcessStartInfo processInfo;
    Process process;

    processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
    processInfo.CreateNoWindow = true;
    processInfo.UseShellExecute = false;
    // *** Redirect the output ***
    processInfo.RedirectStandardError = true;
    processInfo.RedirectStandardOutput = true;

    process = Process.Start(processInfo);
    process.WaitForExit();

    // *** Read the streams ***
    // Warning: This approach can lead to deadlocks, see Edit #2
    string output = process.StandardOutput.ReadToEnd();
    string error = process.StandardError.ReadToEnd();

    exitCode = process.ExitCode;

    Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
    Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
    Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
    process.Close();
}
*/
/*
internal string RunScriptPowerShell(string StringP)
{
    try
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -ExecutionPolicy ByPass -File \"{StringP}\"",
            UseShellExecute = false
        };
        Process.Start(startInfo);
        return "";
    }
    catch (Exception ex)
    {
        return ex.Message;
    }
}
*/