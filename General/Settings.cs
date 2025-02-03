public struct Settings
{
    public int ResolutionX;
    public int ResolutionY;
    public readonly float MouseSensitivity;
    public readonly float MasterVolume;
    public readonly float SoundEffectsVolume;
    public readonly float DialogVolume;
    public readonly int ResolutionIndex;
    
    
    public Settings(int resolutionIndex, int resolutionX, int resolutionY, float mouseSensitivity, float masterVolume, float soundEffectsVolume, float dialogVolume)
    {
        ResolutionIndex = resolutionIndex;
        ResolutionX = resolutionX;
        ResolutionY = resolutionY;
        MouseSensitivity = mouseSensitivity;
        MasterVolume = masterVolume;
        SoundEffectsVolume = soundEffectsVolume;
        DialogVolume = dialogVolume;
    }
    
    public Settings( float mouseSensitivity, float masterVolume, float soundEffectsVolume, float dialogVolume)
    {
        ResolutionIndex = 0;
        ResolutionX = 1920;
        ResolutionY = 1080;
        MouseSensitivity = mouseSensitivity;
        MasterVolume = masterVolume;
        SoundEffectsVolume = soundEffectsVolume;
        DialogVolume = dialogVolume;
    }
}