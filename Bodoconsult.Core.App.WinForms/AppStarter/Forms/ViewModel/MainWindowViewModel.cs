// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Text;
using Bodoconsult.Core.App.Helpers;
using Bodoconsult.Core.App.Interfaces;
using Bodoconsult.Core.App.Logging;

namespace Bodoconsult.Core.App.WinForms.AppStarter.Forms.ViewModel;

/// <summary>
/// ViewModel for MainWindow form
/// </summary>
public class MainWindowViewModel : INotifyPropertyChanged
{



    private const int MaxNumberOfLogEntries = 100;

    private readonly AppEventListener _listener;

    private readonly IList<string> _logData = new List<string>();

    private EventLevel _logEventLevel;

    /// <summary>
    /// Current app start process handler
    /// </summary>
    public IAppStarterProcessHandler AppStarterProcessHandler { get; }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="listener">Current EventSource listener: neede to bring logging entries to UI</param>
    /// <param name="appStarterProcessHandler">Current app start process handler</param>
    public MainWindowViewModel(AppEventListener listener, IAppStarterProcessHandler appStarterProcessHandler)
    {
        _listener = listener;
        AppStarterProcessHandler = appStarterProcessHandler;
    }


    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Message shown during console is waiting
    /// </summary>
    public string MsgConsoleWait { get; set; }

    /// <summary>
    /// Message "how to shitdon server app"
    /// </summary>
    public string MsgHowToShutdownServer { get; set; }

    /// <summary>
    /// Message to exit the app
    /// </summary>
    public string MsgExit { get; set; } = "Exit the app?";


    /// <summary>
    /// Current app version
    /// </summary>
    public string AppVersion
    {
        get => AppStarterProcessHandler.AppVersion;
        set
        {
            if (value == AppStarterProcessHandler.AppVersion)
            {
                return;
            }
            //_appVersion = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Current application context
    /// </summary>
    public TaskTrayApplicationContext ApplicationContext { get; set; }



    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    /// <summary>
    /// Shutdown for app
    /// </summary>
    public void ShutDown()
    {
        AppStarterProcessHandler.StopApplication();
    }

    /// <summary>
    /// Check if there are new log entries
    /// </summary>
    public void CheckLogs()
    {

        if (_listener==null)
        {
            return;
        }

        var count = _listener.Messages.Count;

        if (count == 0)
        {
            return;
        }

        // Keep maximum log data length equal to MaxNumberOfLogEntries
        if (_logData.Count > 0 && _logData.Count + count > MaxNumberOfLogEntries)
        {
            for (var i = _logData.Count - MaxNumberOfLogEntries - 2; i >= 0; i--)
            {
                _logData.Remove(_logData[i]);
            }
        }

        // Add the received messages to log data
        for (var i = 0; i < count; i++)
        {
            var logMsg = GeneralHelper.DequeueFromQueue(_listener.Messages);

            if (_logData.Count > MaxNumberOfLogEntries)
            {
                continue;
            }

            _logData.Add(logMsg);
        }

        // If there are to much entries
        for (var i = _logData.Count - MaxNumberOfLogEntries - 2; i >= 0; i--)
        {
            _logData.Remove(_logData[i]);
        }


        OnPropertyChanged(nameof(LogData));
    }


    /// <summary>
    /// Log data as string to show on UI
    /// </summary>
    public string LogData
    {
        get
        {
            var x = new StringBuilder();
            foreach (var message in _logData)
            {
                x.AppendLine(message);
            }

            return x.ToString();

        }
    }

    /// <summary>
    /// Event level
    /// </summary>
    public EventLevel LogEventLevel
    {
        get => _logEventLevel;
        set
        {
            if (value == _logEventLevel)
            {
                return;
            }
            _logEventLevel = value;
            _listener.EventLevel = _logEventLevel;
            OnPropertyChanged();
        }
    }
}