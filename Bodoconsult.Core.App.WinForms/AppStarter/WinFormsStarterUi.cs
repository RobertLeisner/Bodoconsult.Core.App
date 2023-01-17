// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using System.Diagnostics.Tracing;
using Bodoconsult.Core.App.AppStarter;
using Bodoconsult.Core.App.Interfaces;
using Bodoconsult.Core.App.Logging;
using Bodoconsult.Core.App.WinForms.AppStarter.Forms;
using Bodoconsult.Core.App.WinForms.AppStarter.Forms.ViewModel;

namespace Bodoconsult.Core.App.WinForms.AppStarter;

/// <summary>
/// Implementation of <see cref="IAppStarterUi"/> for WinForms app
/// </summary>
internal class WinFormsStarterUi : BaseAppStarterUi
{
    private readonly AppEventListener _listener;

    private TaskTrayApplicationContext _context;

    private MainWindowViewModel _viewModel;

    public WinFormsStarterUi(IAppStarterProcessHandler appStarterProcessHandler): base(appStarterProcessHandler)
    {
        //var minimumLogLevel = Globals.GetLoggingConfiguration().MinimumLogLevel;

        //MinimumLogLevel = LogLevel.Debug;

        //var eventLevel = MapLogLevelToEventLevel(minimumLogLevel);

        var eventLevel = EventLevel.Error;

        _listener = new AppEventListener(eventLevel);
    }

    ///// <summary>
    ///// Map the log level to EventSource event level
    ///// </summary>
    ///// <param name="logLevel">Log level</param>
    ///// <returns>Event level for EventSource</returns>
    //private static EventLevel MapLogLevelToEventLevel(LogLevel logLevel)
    //{
    //    switch (logLevel)
    //    {
    //        case LogLevel.Trace:
    //            return EventLevel.LogAlways;
    //        case LogLevel.Debug:
    //            return EventLevel.Verbose;
    //        case LogLevel.Information:
    //            return EventLevel.Informational;
    //        case LogLevel.Warning:
    //            return EventLevel.Warning;
    //        case LogLevel.Error:
    //            return EventLevel.Error;
    //        case LogLevel.Critical:
    //            return EventLevel.Critical;
    //        case LogLevel.None:
    //            return EventLevel.Error;
    //        default:
    //            throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
    //    }
    //}


    /// <summary>
    /// Wait while the application runs
    /// </summary>
    public override void Wait()
    {

        _consoleHandle = GetConsoleWindow();
        ShowWindow(_consoleHandle, ShowWindow_Hide);

        _viewModel  = new MainWindowViewModel(_listener, AppStarterProcessHandler)
        {
            AppVersion = AppStarterProcessHandler.AppVersion
        };

            

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        _context = new TaskTrayApplicationContext(_viewModel);
        Application.Run(_context);
    }

    public override void TerminateAppWithMessage(string message, string appTitle)
    {
        MessageBox.Show(message, appTitle);

        _context.Shutdown();

        Environment.Exit(0);
    }


    /// <summary>
    /// Central handling for exceptions
    /// </summary>
    /// <param name="e"></param>
    public override void HandleException(Exception e)
    {
        if (e == null)
        {
            return;
        }

        try
        {
            var msg = AppStarterProcessHandler.HandleException(e);
            MessageBox.Show(msg, AppStarterProcessHandler.AppName);
        }
        catch (Exception exception)
        {
            MessageBox.Show($"{exception.Message} {exception.StackTrace}", AppStarterProcessHandler.AppName);

        }

        _viewModel?.ShutDown();
        Environment.Exit(0);
    }


    /// <summary>
    /// Current implementation of Dispose()
    /// </summary>
    /// <param name="disposing">Disposing required: true or false</param>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        try
        {
            _context.Dispose();
            _listener.Dispose();
        }
        catch //(Exception e)
        {
            // Do nothing
        }
    }
}