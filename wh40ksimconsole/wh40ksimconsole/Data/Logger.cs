using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace wh40ksimconsole.Data
{
    /// <summary>
    /// Customisable lightweight logger
    /// </summary>
    class Logger
    {
        /// <summary>
        /// The current instance of the logger (for easy use)
        /// </summary>
        public static Logger instance;

        /// <summary>
        /// The highest level of log that will be shown to screen
        /// </summary>
        public LogType logLevel { set; get; }

        /// <summary>
        /// How often the logs will be written to text
        /// </summary>
        public int writeThreshold { set; get; }
        
        /// <summary>
        /// The location of the logfile
        /// </summary>
        String fileLocation;

        /// <summary>
        /// The currently stored contents of the log, emptied into the file every writeThreshold logs
        /// </summary>
        String logBuffer;

        /// <summary>
        /// The open file the logger is writing to
        /// </summary>
        FileStream fileStream;

        int logsSinceLastWrite;

        /// <summary>
        /// Customisable lightweight logger
        /// </summary>
        /// <param name="logLevel">The highest level of log that will be shown to screen</param>
        /// <param name="folderLocation">The folder to write the logs to</param>
        /// <param name="writeThreshold">How often the logs with be written to text</param>
        public Logger(LogType logLevel, String folderLocation, int writeThreshold)
        {
            this.logLevel = logLevel;
            // Build the log file name
            this.fileLocation = folderLocation + "log_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Hour + DateTime.Now.Minute + ".txt";
            this.writeThreshold = writeThreshold;
            logBuffer = "";
            logsSinceLastWrite = 0;

            try
            {
                fileStream = File.Create(fileLocation);
            } catch (Exception)
            {
                Console.WriteLine("FATAL: Log file failed to open/create");
                throw new ArgumentException("Log File failed to generate");
            }
        }

        /// <summary>
        /// Takes a log and stores it in a buffer, if the log is high enough level displays it to the screen,
        /// writes the log to a file after a predefined number to logs
        /// </summary>
        /// <param name="type">The type of log</param>
        /// <param name="message">The content</param>
        public void log(LogType type, object message)
        {
            // Generate the log text with date and its type
            String logText = "[" + DateTime.Now + "] [" + type.ToString() + "] " + message.ToString();
            // Store it in the buffer
            logBuffer += (logText + "\n");

            // If its a high enough level, print it
            if (type >= logLevel)
            {
                Console.WriteLine(logText);
            }

            logsSinceLastWrite++;

            // If we've reached the threshold, write the logs to the file
            if (logsSinceLastWrite >= writeThreshold)
            {
                byte[] data = new UTF8Encoding().GetBytes(logBuffer);
                //fileStream.Write(data, 0, data.Length);
                fileStream.WriteAsync(data, 0, data.Length);
                logBuffer = "";
                logsSinceLastWrite = 0;
            }
        }
    }

    public enum LogType
    {
        INFO,
        NOTE,
        WARNING,
        RESULT,
        ERROR,
        FATAL
    }
}
