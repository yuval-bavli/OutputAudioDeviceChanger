using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioChanger.AudioApi
{
    /// <summary>
    /// Simplified version of the CoreAudioApi. Provides simpler APIs.
    /// </summary>
    public interface IAudioApi
    {
        /// <summary>
        /// Get devices.
        /// </summary>
        /// <param name="audioType">Specify input to get only input devices. Specify output to get only output devices. If null all devices will be returned.</param>
        /// <returns>Set of devices</returns>
        /// <exception cref="AudioException">Any Failure in native API.</exception>
        IReadOnlyList<Device> GetDevices(AudioType? audioType = null);

        /// <summary>
        /// Set default audio device.
        /// </summary>
        /// <param name="deviceID">Audio device ID to set</param>
        /// <exception cref="AudioException">Any Failure in native API.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="deviceID"/> is null.</exception>
        /// <exception cref="ArgumentException">Invalid ID or Device is not an output device.</exception>
        void SetDefaultOutputDevice(string deviceID);
    }
}
