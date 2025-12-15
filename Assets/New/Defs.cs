public static class Defs
{
    public static string
    LANGUAGE_SCENE_NAME = "Language",
    /// <summary>
    /// Nombre de la escena del menu principal
    /// </summary>
    MENU_SCENE_NAME = "Intro",
    /// <summary>
    /// Nombre de la escena de configuracion
    /// </summary>
    SETTINGS_SCENE_NAME = "Settings",
    /// <summary>
    /// Nombre de la escena de creditos
    /// </summary>
    CREDITS_SCENE_NAME = "Credits",
    /// <summary>
    /// Nombre de la escena de opciones del nivel
    /// </summary>
    LEVEL_SETTINGS_SCENE_NAME = "LevelSelector",
    /// <summary>
    /// Nombre de la escena de juego
    /// </summary>
    GAME_SCENE_NAME = "Level",
    /// <summary>
    /// Nombre de la escena de juego
    /// </summary>
    TUTORIAL_SCENE_NAME = "Tutorial",

    NEW_GAME_KEY = "newGame",
    GENDER_KEY = "gender",
    /// <summary>
    /// Id de la configuracion para el volumen de la musica de fondo en las preferencias
    /// </summary>
    BGM_VOLUME_KEY = "bgmVolume",
    /// <summary>
    /// Id de la configuracion para el volumen de los efectos de sonido en las preferencias
    /// </summary>
    SFX_VOLUME_KEY = "sfxVolume",
    /// <summary>
    /// Id de la configuracion para el idioma en las preferencias
    /// </summary>
    LANGUAGE_KEY = "language";

    public enum LoadGameValues { NEW_GAME = 0, LOAD_GAME = 1 };

    /// <summary>
    /// Posibles generos del jugador
    /// </summary>
    public enum Gender { MALE = 0, FEMALE = 1, NEUTRAL };

    /// <summary>
    /// Climas en los que se puede jugar
    /// </summary>
    public enum Climate { BOTH = 0, WARM = 1, COLD = 2 };
}