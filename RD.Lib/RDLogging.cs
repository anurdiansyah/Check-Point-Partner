using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.Lib
{
    public class RDLogging
    {
        public static void WriteToFile(
              string sPath,
              string sMessage,
              bool bIsNeedToSeparateLargeFile)
        {
            try
            {
                if (sPath != null &&
                    sPath.Trim().Length > 0)
                {
                    FileInfo oFile = new System.IO.FileInfo(sPath);

                    // New
                    if (!oFile.Exists)
                    {
                        oFile.Directory.Create();
                        System.IO.File.WriteAllText(oFile.FullName, sMessage);
                    }

                    // Existing
                    else
                    {
                        System.IO.FileInfo fiLogFile = new FileInfo(sPath);

                        if (bIsNeedToSeparateLargeFile
                            && fiLogFile.Length > 10485760)
                        {
                            // Rename
                            fiLogFile.CopyTo(string.Format("{0}_{1}.Bak",
                                sPath,
                                DateTime.Now.ToString("yyyyMMddHHmmss")), true);
                            fiLogFile.Delete();

                            // Create new
                            using (StreamWriter sw = System.IO.File.CreateText(sPath))
                            {
                                sw.WriteLine(sMessage);
                                sw.Close();
                            }
                        }
                        else
                        {
                            using (StreamWriter sw = System.IO.File.AppendText(sPath))
                            {
                                sw.WriteLine(sMessage);
                                sw.Close();
                            }
                        }

                        fiLogFile = null;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }

        public static void WriteToEventLog(
            string sSource,
            string sMessage)
        {
            try
            {
                EventLog oLog = new EventLog();
                if (!EventLog.SourceExists(sSource))
                {
                    EventLog.CreateEventSource(sSource, "RD");
                }
                oLog.Source = sSource;
                oLog.WriteEntry(String.Format("Source : {0}{1}{0}{0}Message : {0}{2}{0}",
                        System.Environment.NewLine,
                        sSource,
                        sMessage),
                    EventLogEntryType.Information);
                oLog.Dispose();
            }
            catch
            {
                // Do nothing
            }
        }
    }
}
