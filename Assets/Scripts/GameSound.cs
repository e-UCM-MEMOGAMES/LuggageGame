
    /// <summary>
    /// This enum defines the sound types available to play.
    /// Each enum value can have AudioClips assigned to it in the SoundManager's Inspector pane.
    /// </summary>
    public enum GameSound
    {
        // It's advisable to keep 'None' as the first option, since it helps exposing this enum in the Inspector.
        // If the first option is already an actual value, then there is no "nothing selected" option.
        None,
        ButtonClicked,
        MenuBGM,
        LevelBGM,
        DrawerOpen,
        DrawerClose,
        MedicineOpen,
        MedicineClose,
        PutIn,
        ThrowOut,
        NoteBook,
        Star,
        AirPlane
    }
    public enum ChannelType
    {
        // It's advisable to keep 'None' as the first option, since it helps exposing this enum in the Inspector.
        // If the first option is already an actual value, then there is no "nothing selected" option.
        Default,
        UI,
        BGM,
        SceneSound

    }
