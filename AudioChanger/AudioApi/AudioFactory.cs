namespace AudioChanger.AudioApi
{
    public static class AudioFactory
    {
        public static IAudioApi Create()
        {
            return new Audiok();
        }
    }
}
