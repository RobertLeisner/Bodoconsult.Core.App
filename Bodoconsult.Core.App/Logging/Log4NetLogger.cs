// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Reflection;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.Core.App.Logging
{
    public class Log4NetLogger : Microsoft.Extensions.Logging.ILogger
    {
        ////private readonly string _name;
        ////private readonly XmlElement _xmlElement;
        private ILog _log;




        #region Ctors

        /// <summary>
        /// Default ctor: setup for a Log4Net logger with default name logfile.log for the log file
        /// </summary>
        public Log4NetLogger()
        {
            var assPath = typeof(Log4NetLogger).Assembly.Location;

            var dir = new FileInfo(assPath).DirectoryName;

            var fileName = Path.Combine(dir, "logfile.log");

            InitLoggerFromCode(fileName);
        }


        /// <summary>
        /// Setup for a Log4Net logger
        /// </summary>
        /// <param name="fileName">Full file path to the log file</param>
        public Log4NetLogger(string fileName)
        {
            InitLoggerFromCode(fileName);
        }

        private void InitLoggerFromCode(string fileName)
        {
            var fi = new FileInfo(fileName);

            var plainFileName = fi.Name.Replace(fi.Extension, "");

            var layout =
                new PatternLayout("%message%newline");
            var filter = new LevelMatchFilter { LevelToMatch = Level.All };
            filter.ActivateOptions();

            var appender = new RollingFileAppender
            {
                File = fileName,
                ImmediateFlush = true,
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Date,
                DatePattern = "-yyyy - MM - dd",
                LockingModel = new FileAppender.MinimalLock(),
                Name = $"FileAppender_{plainFileName}"
            };
            appender.AddFilter(filter);
            appender.Layout = layout;

            try
            {
                appender.ActivateOptions();
            }
            catch //(Exception e)
            {
                // Do nothing: may throw on Android due not supported mutex
            }
            
            var repositoryName = $"{plainFileName}Repository";
            var loggerName = $"{plainFileName} Logger";
            try
            {
                var repository = LoggerManager.CreateRepository(repositoryName);
                BasicConfigurator.Configure(repository, appender);
            }
            catch (Exception e)
            {
                //logger.LogError("Error when initializing monitor logger: {0}",e);
            }
            _log = LogManager.GetLogger(repositoryName, loggerName);

        }

        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="configFileName"></param>

        public Log4NetLogger(string name, string configFileName)
        {

            var type = typeof(Log4NetLogger);

            var filePath = Path.Combine(new FileInfo(type.Assembly.Location).DirectoryName, configFileName);

            var xmlElement = Parselog4NetConfigFile(filePath);

            InitLoggerFromXml(name, xmlElement);

        }

        public Log4NetLogger(string name, XmlElement xmlElement)
        {
            InitLoggerFromXml(name, xmlElement);
            //_log.Fatal("log4net init successful");
        }

        private void InitLoggerFromXml(string name, XmlElement xmlElement)
        {
            var loggerRepository = LogManager.CreateRepository(
                Assembly.GetCallingAssembly(), typeof(Hierarchy));
            XmlConfigurator.Configure(loggerRepository, xmlElement);

            _log = LogManager.GetLogger(loggerRepository.Name, name);
        }

        #endregion 




        public static XmlElement Parselog4NetConfigFile(string filename)
        {
            var log4NetConfig = new XmlDocument();

            using (var s = File.OpenRead(filename))
            {
                log4NetConfig.Load(s);
            }

            var node = log4NetConfig["log4net"];

            if (node != null)
            {
                return node;
            }

            node = log4NetConfig["configuration"];

            if (node == null)
            {
                return null;
            }

            node = node["log4net"];


            return node;
        }


        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _log.IsDebugEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message) && exception == null)
            {
                return;
            }

            switch (logLevel)
            {
                case LogLevel.Critical:
                    _log.Fatal(message);
                    break;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    _log.Debug(message);
                    break;
                case LogLevel.Error:
                    _log.Error(message);
                    break;
                case LogLevel.Information:
                    _log.Info(message);
                    break;
                case LogLevel.Warning:
                    _log.Warn(message);
                    break;
                default:
                    _log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                    _log.Info(message, exception);
                    break;
            }
        }
    }
}
