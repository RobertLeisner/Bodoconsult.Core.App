﻿// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

namespace Bodoconsult.Core.App.Interfaces;

/// <summary>
/// Interface for application starter classes
/// </summary>
public interface IAppStarterUi: IDisposable
{
    /// <summary>
    /// The current app start process handler
    /// </summary>
    IAppStarterProcessHandler AppStarterProcessHandler { get; }


    /// <summary>
    /// Is already another instance started?
    /// </summary>
    bool IsAnotherInstance { get;  }


    /// <summary>
    /// Start the app
    /// </summary>
    void Start();

    /// <summary>
    /// Wait while the application runs
    /// </summary>
    void Wait();

    /// <summary>
    /// Show a message and then terminate the app
    /// </summary>
    /// <param name="message">Message to show before app termination</param>
    /// <param name="appTitle">App title to set</param>
    void TerminateAppWithMessage(string message, string appTitle);


    void HandleException(Exception ex);
}