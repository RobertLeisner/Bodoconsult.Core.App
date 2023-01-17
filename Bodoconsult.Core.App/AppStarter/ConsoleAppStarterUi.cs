// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.AppStarter;

/// <summary>
/// Implementation of <see cref="IAppStarterUi"/> for console app
/// </summary>
public class ConsoleAppStarterUi : BaseAppStarterUi
{
    /// <summary>
    /// Message shown during console is waiting
    /// </summary>
    public string MsgConsoleWait { get; set; }

    /// <summary>
    /// Message "how to shitdon server app"
    /// </summary>
    public string MsgHowToShutdownServer { get; set; }

    /// <summary>
    /// Default ctor
    /// </summary>
    public ConsoleAppStarterUi(IAppStarterProcessHandler appStarterProcessHandler): base(appStarterProcessHandler)
    {

        // App is a WinForms app, therefore the console is normally hidden.
        // We access the hidden console here and make it visible 
        AllocConsole();

        _consoleHandle = GetConsoleWindow();
        ShowWindow(_consoleHandle, ShowWindow_Show);


    }


    /// <summary>
    /// Wait while the application runs
    /// </summary>
    public override void Wait()
    {
        try
        {
            var msg = MsgConsoleWait;

            AppStarterProcessHandler.AppLogger.LogInformation(msg);
            Console.WriteLine(msg);
            Console.WriteLine(MsgHowToShutdownServer);
            while (true)
            {
                //var key = 
                Console.ReadKey();
            }
        }
        catch (Exception e)
        {
            HandleException(e);
        }
    }


    public override void TerminateAppWithMessage(string message, string appTitle)
    {
        Console.WriteLine(message);
        Thread.Sleep(5000);
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

            // ToDo: correct handling of line breaks
            Console.WriteLine(msg);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }

        Thread.Sleep(5000);
        Environment.Exit(0);
    }
}