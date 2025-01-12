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
using AudioSwitcher.AudioApi;

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
            volumeController.VolumeChanged += OnVolumeChanged;
            volumeController.MuteChanged += OnMuteStateChanged;

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

        private void OnVolumeChanged(object? sender, VolumeChangedEventArgs e)
        {
            // Update UI, for example, update the slider value
            DispatcherQueue.TryEnqueue(() =>
            {
                volumeSlider.Value = e.NewVolume; // Assume there's a Slider named volumeSlider
            });
        }
        private void OnMuteStateChanged(object? sender, MuteChangedEventArgs e)
        {
            // Update UI, for example, update the mute icon
            DispatcherQueue.TryEnqueue(() =>
            {
                volumeIcon.Text = e.NewMute ? FontAwesome.FontAwesomeIcons.VolumeXmark : GetUnmutedVolumeIcon();
            });
        }

        private void UpdateVolumeButtonIcon()
        {
            volumeIcon.Text = isMuted ? FontAwesome.FontAwesomeIcons.VolumeXmark : GetUnmutedVolumeIcon();
        }

        private string GetUnmutedVolumeIcon()
        {
            string retval = FontAwesome.FontAwesomeIcons.VolumeHigh;

            if (volumeSlider.Value == 0)
            {
                retval = FontAwesome.FontAwesomeIcons.VolumeXmark;
            }
            else if (volumeSlider.Value < 50)
            {
                retval = FontAwesome.FontAwesomeIcons.VolumeLow;
            }
            return retval;
        }
    }
}
