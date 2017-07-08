namespace AudioChanger.AudioApi
{
    /// <summary>
    /// Audio device information.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Input or output audio device.
        /// </summary>
        public AudioType AudioType { get; }

        /// <summary>
        /// Audio device name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Additional information about the audio device.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Audio device identifier.
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// True if the device is the default output audio device.
        /// </summary>
        public bool IsDefaultOutput { get; }

        public Device(string id, AudioType audioType, string name, string description, bool isDefaultOutput)
        {
            ID = id;
            AudioType = audioType;
            Name = name;
            Description = description;
            IsDefaultOutput = isDefaultOutput;
        }

    }
}
