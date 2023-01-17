// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

namespace Bodoconsult.Core.App.Interfaces;

/// <summary>
/// Interface for concrete implementations of the app start process required for a certain app
/// </summary>
public interface IAppStarterProcessHandler
{
    /// <summary>
    /// Clear text name of the app to show in windows and message boxes
    /// </summary>
    string AppName { get; }

    /// <summary>
    /// Current <see cref="IAppStarterUi"/> instance
    /// </summary>
    IAppStarterUi AppStarterUi { get; }

    /// <summary>
    /// String with the current app version
    /// </summary>
    string AppVersion { get; }

    /// <summary>
    /// Current software version
    /// </summary>
    Version SoftwareRevision { get; }

    /// <summary>
    /// Current app logger
    /// </summary>
    IAppLoggerProxy AppLogger { get; }

    /// <summary>
    /// Load the current app starter in the start process
    /// </summary>
    /// <param name="appStarterUi">Current <see cref="IAppStarterUi"/> instance</param>
    void SetAppStarterUi(IAppStarterUi appStarterUi);

    /// <summary>
    /// Activates an instance of <see cref="IAppLoggerProxy"/> for logging during app start process. <see cref="AppLogger"/> may NOT be null after calling this method
    /// </summary>
    void ActivateLogger();

    /// <summary>
    /// Starts the application
    /// </summary>
    void StartApplication();

    /// <summary>
    /// Stops the application
    /// </summary>
    void StopApplication();

    /// <summary>
    /// Checks if the database is available
    /// </summary>
    void CheckDatabaseServerConnection();

    /// <summary>
    /// Handles an exception an returns a string to use in UI
    /// </summary>
    /// <param name="e">Raised exception</param>
    /// <returns>String with information for the raised exception</returns>
    string HandleException(Exception e);

}