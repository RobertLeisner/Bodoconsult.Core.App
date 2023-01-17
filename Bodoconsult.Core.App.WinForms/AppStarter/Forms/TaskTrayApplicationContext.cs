// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Core.App.WinForms.AppStarter.Forms.ViewModel;

namespace Bodoconsult.Core.App.WinForms.AppStarter.Forms;

/// <summary>
/// Application context for add the app to system tray bar
/// </summary>
public class TaskTrayApplicationContext : ApplicationContext
{

    private MainWindow _mainWindow;

    private  MainWindowViewModel _viewModel;

    readonly NotifyIcon _notifyIcon = new NotifyIcon();

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="viewModel">Current view model</param>
    public TaskTrayApplicationContext(MainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.ApplicationContext = this;

        _mainWindow = new MainWindow(_viewModel)
        {
            Visible = true,
            WindowState = FormWindowState.Normal
        };

        MainForm = _mainWindow;

        _mainWindow.Show();


        _notifyIcon.Icon = Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
        _notifyIcon.Text = _viewModel.AppVersion;

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Show", null, Show);
        contextMenu.Items.Add("Exit", null, Exit);
        _notifyIcon.ContextMenuStrip = contextMenu;

        _notifyIcon.Visible = true;

    }

    /// <summary>
    /// Reload the window from system tray
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Show(object sender, EventArgs e)
    {
        _mainWindow.WindowState = FormWindowState.Normal;
        _mainWindow.Visible = true;
    }

    /// <summary>
    /// Exit the whole app
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Exit(object sender, EventArgs e)
    {

        if (MessageBox.Show(_viewModel.MsgExit, _viewModel.AppStarterProcessHandler.AppName, MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
        {
            return;
        }

        if (MessageBox.Show(_viewModel.MsgExit, _viewModel.AppStarterProcessHandler.AppName, MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
        {
            return;
        }

        Shutdown();
    }

    /// <summary>
    /// Shut the app down
    /// </summary>
    public void Shutdown()
    {
        try
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            _notifyIcon.Visible = false;
            _viewModel.ShutDown();
            _mainWindow.Close();
            _mainWindow.Dispose();
            _mainWindow = null;
            _viewModel = null;

            ExitThread();
        }
        catch 
        {
            //
        }

    }
}