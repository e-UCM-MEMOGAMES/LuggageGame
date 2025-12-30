public static class Defs
{
    /// <summary>
    /// Nombre de la escena de seleccion de idiomas
    /// </summary>
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
    /// Id de la configuracion para saber si cargar partida o no
    /// </summary>
    NEW_GAME_PREFS_KEY = "newGame",
    /// <summary>
    /// Id de la configuracion para el genero del jugador
    /// </summary>
    GENDER_PREFS_KEY = "gender",
    /// <summary>
    /// Id de la configuracion para el volumen de la musica de fondo en las preferencias
    /// </summary>
    BGM_VOLUME_PREFS_KEY = "bgmVolume",
    /// <summary>
    /// Id de la configuracion para el volumen de los efectos de sonido en las preferencias
    /// </summary>
    SFX_VOLUME_PREFS_KEY = "sfxVolume",
    /// <summary>
    /// Id de la configuracion para el idioma en las preferencias
    /// </summary>
    LANGUAGE_PREFS_KEY = "language",
    /// <summary>
    /// Id de la configuracion para la informacion de cada nivel 
    /// (la id empieza con este string y cambia dependiendo del nivel o el clima)
    /// </summary>
    LEVEL_NAME_PREFS_KEY = "Level";

    /// <summary>
    /// Valores posibles para la configuracion para saber si cargar partida o no
    /// </summary>
    public enum LoadGameValues { NEW_GAME = 0, LOAD_GAME = 1 };

    /// <summary>
    /// Valores posibles para el genero del jugador
    /// </summary>
    public enum Gender { NEUTRAL = 0, MALE = 1, FEMALE = 2, };

    /// <summary>
    /// Climas en los que se puede jugar
    /// </summary>
    public enum Climate { BOTH, WARM, COLD };

    /// <summary>
    /// Categorias de las que pueden ser los objetos
    /// </summary>
    public enum ItemCategory { CLOTHING, FOOTWEAR, OTHER };

    /// <summary>
    /// Maneras en las que pueden aparecer los objetos segun su punto de aparicion
    /// </summary>
    public enum SpawnType { REGULAR, STORED, CABINET };
    /// <summary>
    /// Pivote/origen vertical con el que aparecen los objetos segun su punto de aparicion
    /// </summary>
    public enum SpawnPivot { TOP, MIDDLE, BOTTOM };

    /// <summary>
    /// Devuelve la Id de la configuracion para el nivel indicado en el clima indicado
    /// </summary>
    public static string GetLevelSaveKey(int levelNumber, Climate climate)
    {
        return $"{LEVEL_NAME_PREFS_KEY}_{levelNumber.ToString()}_{climate.ToString()}";
    }
}