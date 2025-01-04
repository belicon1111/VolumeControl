using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using FontAwesome;
using WinUIEx;
using Microsoft.UI;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VolumeControl
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WinUIEx.WindowEx
    {
        private bool isMuted;
        private VolumeController volumeController;
        public MainWindow()
        {
            this.InitializeComponent();

            // initialize style
            this.PersistenceId = "MainWindow";
            this.SystemBackdrop = new DesktopAcrylicBackdrop();
            ExtendsContentIntoTitleBar = true;

            if (AppWindowTitleBar.IsCustomizationSupported() is true)
            {
                IntPtr hWnd = WindowNative.GetWindowHandle(this);
                WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
                AppWindow appWindow = AppWindow.GetFromWindowId(wndId);
                appWindow.SetIcon(@"Assets\icon.ico");
            }

            // initialize members
            volumeController = new VolumeController();
            isMuted = volumeController.IsMuted();
            volumeSlider.Value = volumeController.GetVolume();
            UpdateVolumeButtonIcon();

            // subscribe to changes
            volumeSlider.ValueChanged += Slider_ValueChanged;
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle mute state
            isMuted = !isMuted;
            volumeController.SetMuted(isMuted);

            // Update the button icon
            UpdateVolumeButtonIcon();
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            UpdateVolumeButtonIcon();
            volumeController.SetVolume(volumeSlider.Value);
        }

        private void UpdateVolumeButtonIcon()
        {
            volumeIcon.Text = isMuted ? FontAwesome.FontAwesomeIcons.VolumeXmark : GetUnmutedVolumeIcon();
        }

        private string GetUnmutedVolumeIcon()
        {
            return volumeSlider.Value < 50 ? FontAwesome.FontAwesomeIcons.VolumeLow : FontAwesome.FontAwesomeIcons.VolumeHigh;
        }
    }
}
