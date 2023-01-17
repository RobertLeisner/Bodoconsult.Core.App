// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.Core.App.WinForms.AppStarter.Forms.ViewModel;

namespace Bodoconsult.Core.App.WinForms.AppStarter.Forms;

/// <summary>
/// Main window of the app
/// </summary>
public sealed partial class MainWindow : Form
{

    private readonly MainWindowViewModel _viewModel;

    private const int CP_NOCLOSE_BUTTON = 0x200;

    private readonly System.Windows.Forms.Timer _timer;

    private bool isClosing;


    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="viewModel">Current view model to use</param>
    public MainWindow(MainWindowViewModel viewModel)
    {

        _viewModel = viewModel;

        InitializeComponent();

        Text = _viewModel.AppVersion;

        AppTitle.Text = _viewModel.AppStarterProcessHandler.AppName;

        MsgServerIsListeningOnPort.Text = _viewModel.MsgConsoleWait;
        MsgHowToShutdownServer.Text = _viewModel.MsgHowToShutdownServer;
        MsgServerProcessId.Text = $"Process ID: {Process.GetCurrentProcess().Id}";


        LogWindow.DataBindings.Add(new Binding
            (nameof(LogWindow.Text), _viewModel, "LogData"));


        _timer = new System.Windows.Forms.Timer
        {
            Interval = 1000
        };
        _timer.Tick += TimerOnTick;
        _timer.Enabled = true;
        _timer.Start();
    }

    private void TimerOnTick(object sender, EventArgs e)
    {
        _timer.Stop();

        _viewModel.CheckLogs();

        _timer.Start();
    }


    /// <summary>
    /// Hide the Close button
    /// </summary>
    protected override CreateParams CreateParams
    {
        get
        {
            var myCp = base.CreateParams;
            myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
            return myCp;
        }
    }


    private void MainWindow_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Check if STRG+C is pressed: if no break here else shutdown
        if (ModifierKeys != Keys.Control || e.KeyChar != '\u0003')
        {
            return;
        }

        isClosing = true;

        _viewModel.ShutDown();

        Close();
    }

    private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
    {

        if (isClosing)
        {
            return;
        }

        // Turn off closing via UI: only STRG+C is allowed
        e.Cancel = true;
    }

    private void LogWindow_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Check if STRG+C is pressed: if no break here else shutdown
        if (ModifierKeys != Keys.Control || e.KeyChar != '\u0003')
        {
            return;
        }

        isClosing = true;

        _viewModel.ApplicationContext.Shutdown();

        Close();
    }
}