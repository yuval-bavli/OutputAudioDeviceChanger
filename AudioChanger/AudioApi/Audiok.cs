using AudioSwitch.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioChanger.AudioApi
{
    internal class Audiok : IAudioApi
    {
        private static readonly MMDeviceEnumerator s_deviceEnumerator = new MMDeviceEnumerator();
        private static readonly PolicyConfigClient s_pPolicyConfig = new PolicyConfigClient();

        public IReadOnlyList<Device> GetDevices(AudioType? audioType = null)
        {
            IEnumerable<Device> inputDevices;
            IEnumerable<Device> outputDevices;

            try
            {
                inputDevices = GetDevicesInternal(AudioType.Input);
                outputDevices = GetDevicesInternal(AudioType.Output);
            }
            catch(Exception ex)
            {
                throw new AudioException("Failed to get audio devices", ex);
            }            

            if(!audioType.HasValue)
            {
                return inputDevices.Concat(outputDevices).ToList().AsReadOnly();
            }
            else if(audioType == AudioType.Input)
            {
                return inputDevices.ToList().AsReadOnly();
            }
            else
            {
                return outputDevices.ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// Get audio devices
        /// </summary>
        /// <param name="audioType">Input or output devices.</param>
        /// <returns>Set of devices.</returns>
        private IEnumerable<Device> GetDevicesInternal(AudioType audioType)
        {
            var devices = new List<Device>();
            EDataFlow renderType = audioType == AudioType.Input ? EDataFlow.eCapture : EDataFlow.eRender;

            MMDeviceCollection pDevices = s_deviceEnumerator.EnumerateAudioEndPoints(renderType, EDeviceState.Active);
            string defualtDeviceID = s_deviceEnumerator.GetDefaultAudioEndpoint(renderType, ERole.eMultimedia).ID;
            int devCount = pDevices.Count;

            for (int i = 0; i < devCount; i++)
            {
                MMDevice mmDevice = pDevices[i];
                bool isDefaultOutput = audioType == AudioType.Output && mmDevice.ID == defualtDeviceID;
                Device device = new Device(mmDevice.ID, audioType, mmDevice.FriendlyName, mmDevice.Description, isDefaultOutput);
                devices.Add(device);
            }

            return devices;
        }

        public void SetDefaultOutputDevice(string deviceID)
        {
            if(deviceID == null)
            {
                throw new ArgumentNullException(nameof(deviceID));
            }

            try
            {
                // Validation:
                bool deviceFound = GetDevicesInternal(AudioType.Output).Any(d => d.ID == deviceID);
                if(!deviceFound)
                {
                    throw new ArgumentException($"Could not find an output device with ID {deviceID}");
                }
                
                s_pPolicyConfig.SetDefaultEndpoint(deviceID, ERole.eMultimedia);
            }
            catch(Exception ex)
            {
                throw new AudioException("Failed to set default output device", ex);
            }
        }
    }
}
