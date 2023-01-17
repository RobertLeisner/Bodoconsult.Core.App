// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using System.Diagnostics;
using System.Runtime.InteropServices;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.AppStarter;

/// <summary>
/// Base class for <see cref="IAppStarterUi"/> implementations
/// </summary>
public class BaseAppStarterUi : IAppStarterUi
{

    /// <summary>
    /// Default ctor
    /// </summary>
    public BaseAppStarterUi(IAppStarterProcessHandler appStarterProcessHandler)
    {
        AppStarterProcessHandler = appStarterProcessHandler;
    }

    /// <summary>
    /// The current app start process handler
    /// </summary>
    public IAppStarterProcessHandler AppStarterProcessHandler { get; }

    /// <summary>
    /// Is already another instance started?
    /// </summary>
    public bool IsAnotherInstance
    {

        get
        {
            // Check if StSys is already started
            var currentProcess = Process.GetCurrentProcess();
            var runningProcess = (from process in Process.GetProcesses()
                where
                    process.Id != currentProcess.Id &&
                    process.ProcessName.Equals(
                        currentProcess.ProcessName,
                        StringComparison.Ordinal)
                select process).FirstOrDefault();
                
            if (runningProcess == null)
            {
                return false;
            }

            _consoleHandle = GetConsoleWindow();
            ShowWindow(_consoleHandle, ShowWindow_Show);
            Console.WriteLine($"{AppStarterProcessHandler.AppName} is already started. Current instance terminates now. Please press any key now.");
            Console.Read();
            return true;

        }


    }



    protected IntPtr _consoleHandle;

    [DllImport("user32.dll")]
    protected static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    protected static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    protected static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    protected static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    protected const int ShowWindow_Hide = 0;
    protected const int ShowWindow_Show = 5;


    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool FreeConsole();


    /// <summary>
    /// Start the app
    /// </summary>
    public virtual void Start()
    {
        try
        {
            AppStarterProcessHandler.StartApplication();
        }
        catch (Exception e)
        {
            AppStarterProcessHandler.AppLogger.LogError($"{AppStarterProcessHandler.AppName} is closed due to error", e);
            HandleException(e);
            Environment.Exit(0);
        }
    }

    /// <summary>
    /// Wait while the application runs
    /// </summary>
    public virtual void Wait()
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Show a message and then terminate the app
    /// </summary>
    /// <param name="message">Message to show before app termination</param>
    /// <param name="appTitle">App title to set</param>
    public virtual void TerminateAppWithMessage(string message, string appTitle)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Central handling for exceptions
    /// </summary>
    /// <param name="e"></param>
    public virtual void HandleException(Exception e)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Current implementation of Dispose()
    /// </summary>
    /// <param name="disposing">Disposing required: true or false</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
                
        }
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        // Dispose of unmanaged resources.
        Dispose(true);
        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Finalizer
    /// </summary>
    ~BaseAppStarterUi()
    {
        Dispose(false);
    }
}