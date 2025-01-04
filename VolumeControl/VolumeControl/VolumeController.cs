using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi.Observables;

namespace VolumeControl
{
    class VolumeController
    {
        private CoreAudioController controller;
        private CoreAudioDevice defaultDevice;

        //public event Action<double> VolumeChanged;  // Event to notify UI of volume changes

        public VolumeController()
        {
            controller = new CoreAudioController();
            defaultDevice = controller.DefaultPlaybackDevice;

            if (defaultDevice == null)
            {
                throw new Exception("No default playback device found. Ensure a playback device is connected and set as default.");
            }

            // Subscribe to the AudioDeviceChanged event using .Subscribe
            controller.AudioDeviceChanged.Subscribe(OnAudioDeviceChanged);
        }

        // Method that handles device change events
        private void OnAudioDeviceChanged(DeviceChangedArgs e)
        {
            // Check if the changed device is the default playback device
            if (e.Device.IsDefaultDevice && e.Device.DeviceType == AudioSwitcher.AudioApi.DeviceType.Playback)
            {
                defaultDevice = (CoreAudioDevice)e.Device;
            }

            // Check if volume changed

            // Check if mute status changed


        }

        public double GetVolume()
        {
            return defaultDevice.Volume;
        }

        public void SetVolume(double volume)
        {
            if (volume < 0 || volume > 100)
            {
                throw new ArgumentException(nameof(volume), "Volume must be between 0 and 100");
            }

            defaultDevice.Volume = volume;
        }

        public bool IsMuted()
        {
            return defaultDevice.IsMuted;
        }

        public void SetMuted(bool mute)
        {
            defaultDevice.Mute(mute);
        }
    }
}
