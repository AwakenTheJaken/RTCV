﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.LayoutRenderers;

namespace RTCV.Common
{
    public static class Logging
    {
        public static Logger GlobalLogger = LogManager.GetLogger("Global");

        public static void StartLogging(string filename)
        {
            int logsToKeep = 5;

            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var errorLayout = new NLog.Layouts.SimpleLayout("${longdate}|${level:uppercase=true}|${logger}|${message}${onexception:|${newline}EXCEPTION OCCURRED\\:${exception:format=type,message,method:maxInnerExceptionLevel=5:innerFormat=shortType,message,method}${newline}");

            for (int i = logsToKeep; i >= 0; i--)
            {
                var _filename = getFormattedLogFilename(filename, i - 1);
                if (File.Exists(_filename))
                {
                    var newName = getFormattedLogFilename(filename, i);

                    if (String.IsNullOrEmpty(newName)) //If something went wrong generating the name, just give up
                        break;

                    File.Copy(_filename, newName, true);
                }
            }

            try
            {
                File.Delete(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to delete old log!\n" + e);
            }

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = filename , Layout = errorLayout};
            var logconsole = new NLog.Targets.ColoredConsoleTarget("logconsole") {Layout = errorLayout};
            

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;

            GlobalLogger = LogManager.GetLogger("Global");
        }

        private static string getFormattedLogFilename(string path, int num)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                var filename = Path.GetFileNameWithoutExtension(path);
                var ext = Path.GetExtension(path);

                return Path.Combine(dir, num == 0 ? $"{filename}{ext}" : $"{filename}.{num}{ext}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"getFormattedLogFilename failed {e}");
                return String.Empty;
            }
        }
    }


    public class ExtendedRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            throw new NotImplementedException();
        }
    }
}