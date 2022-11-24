// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Concurrent;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Logging
{
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly string _log4NetConfigFile;

        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers =
            new ConcurrentDictionary<string, Log4NetLogger>();


        public Log4NetProvider()
        {
            var s = typeof(Log4NetProvider).Assembly.Location;
            // ReSharper disable once AssignNullToNotNullAttribute
            s = Path.Combine(new FileInfo(s).DirectoryName, "log4net.config");
            _log4NetConfigFile = s;
        }


        public Log4NetProvider(string log4NetConfigFile)
        {
            _log4NetConfigFile = log4NetConfigFile;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var impl = CreateLoggerImplementation(categoryName);

            impl.IsEnabled(LogLevel.Trace);


            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (disposing)
            {
                //#pragma warning disable 
                try
                {
                    _loggers.Clear();
                    
                }
#pragma warning disable CA1031
                catch //(Exception e)
                {
                    // ignored
                }
#pragma warning restore CA1031
            }
        }


        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            var l = new Log4NetLogger(name, Parselog4NetConfigFile(_log4NetConfigFile));
            return l;
        }

        private static XmlElement Parselog4NetConfigFile(string filename)
        {
            var log4NetConfig = new XmlDocument();

            using (var s = File.OpenRead(filename))
            {
                log4NetConfig.Load(s);
            }

            return log4NetConfig["log4net"];
        }

    }
}