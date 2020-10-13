using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TF2TextToSpeech
{
    public class LogFile
    {
        private ClassConnector classConnector;
        public LogFile(ClassConnector classConnector)
        {
            this.classConnector = classConnector;
        }

        public string ReadEndOfFile()
        {
            string logFileLine = "Hello World!";
            using (FileStream logFileFileStream = File.Open(classConnector.userSettings.pathToFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader logFileStreamReader = new StreamReader(logFileFileStream))
                {
                    while (!logFileStreamReader.EndOfStream)
                    {
                        logFileLine = logFileStreamReader.ReadLine();
                    }
                    logFileStreamReader.Close();
                }
                logFileFileStream.Close();
            }
            return logFileLine;
        }

        public bool TryConnectLogFile()
        {
            try
            {
                FileStream logFileFileStream = File.Open(classConnector.userSettings.pathToFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                logFileFileStream.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("An Error occured trying to open the tf2 console log file.");
                Console.WriteLine("Error:" + e);
                return false;
            }
        }


    }
}
