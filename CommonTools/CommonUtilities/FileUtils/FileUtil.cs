using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.FileUtils
{
    public class FileUtil
    {
        public static bool IsFileLocked(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open,
                         FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Az copy. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160523. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="sourceFilePath">   Full pathname of the source file. </param>
        /// <param name="fileName">         Filename of the file. </param>
        /// <param name="azureStoragePath"> Full pathname of the azure storage file. </param>
        /// <param name="azureStorageKey">  The azure storage key. </param>
        /// <param name="sourceFilePath">   Full pathname of the source file. </param>
        ///
        /// <returns>   A TransferSummary. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static TransferSummary AzCopy(string sourceFilePath, string fileName, string azureStoragePath, string azureStorageKey)
        {
            string strCmdText;
            strCmdText = @"/C C:&cd C:\Program Files (x86)\Microsoft SDKs\Azure\AzCopy&" +
                                    @"AzCopy /Source:" + sourceFilePath + " " +
                                    @"/Dest:" + azureStoragePath + " " +
                                    @"/DestKey:" + azureStorageKey + " /Pattern:" + fileName + " " +
                                    @"/V:" + sourceFilePath + @"\AzCopy\Log\" + DateTime.Now.Year + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + @"_AzCopyLog.txt " +
                                    @"/Z:" + sourceFilePath + @"\AzCopy\Journal /Y /NC:2";

            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", strCmdText);

            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;

            Process p = Process.Start(processInfo);

            //retrieve any errors
            string output = p.StandardOutput.ReadToEnd();
            string err = p.StandardError.ReadToEnd();
            p.WaitForExit();
            TransferSummary ts = ParseTransferSummary(output);

            if (err != "") throw new Exception(err);

            return ts;
        }

        public static TransferSummary AzCopyDownload(string destFilePath, string fileName, string azureStoragePath, string azureStorageKey)
        {
            string strCmdText;
            strCmdText = @"/C C:&cd C:\Program Files (x86)\Microsoft SDKs\Azure\AzCopy&" +
                                    @"AzCopy /Source:" + azureStoragePath + " " +
                                    @"/Dest:" + destFilePath + " " +
                                    @"/SourceKey:" + azureStorageKey + " /Pattern:" + fileName + " " +
                                    @"/V:" + destFilePath + @"\AzCopy\Log\" + DateTime.Now.Year + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + @"_AzCopyLog.txt " +
                                    @"/Z:" + destFilePath + @"\AzCopy\Journal /Y /NC:2";

            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", strCmdText);

            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;

            Process p = Process.Start(processInfo);

            //retrieve any errors
            string output = p.StandardOutput.ReadToEnd();
            string err = p.StandardError.ReadToEnd();
            p.WaitForExit();
            TransferSummary ts = ParseTransferSummary(output);

            if (err != "") throw new Exception(err);

            return ts;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parse transfer summary. </summary>
        ///
        /// <remarks>   Rphilavanh, 20160520. </remarks>
        ///
        /// <param name="text"> The text. </param>
        ///
        /// <returns>   A TransferSummary. </returns>
        ///-------------------------------------------------------------------------------------------------
        private static TransferSummary ParseTransferSummary(string text)
        {
            TransferSummary ts = new TransferSummary();
            foreach (string ln in text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
            {

                string[] tsOut = ln.Split(':');
                switch (tsOut[0].ToString())
                {
                    case "Transfer summary":
                        break;

                    case "Total files transferred":
                        ts.TotalFilesTransferred = int.Parse(tsOut[1]);
                        break;

                    case "Transfer successfully":
                        ts.TransferSuccessfully = int.Parse(tsOut[1]);
                        break;

                    case "Transfer skipped":
                        ts.TransferSkipped = int.Parse(tsOut[1]);
                        break;

                    case "Transfer failed":
                        ts.TransferFailed = int.Parse(tsOut[1]);
                        break;

                    case "Elapsed time":
                        ts.ElapsedTime = tsOut[1].ToString() + ":" + tsOut[2].ToString() + ":" + tsOut[3].ToString();
                        break;
                }
            }
            return ts;
        }
    }
}
