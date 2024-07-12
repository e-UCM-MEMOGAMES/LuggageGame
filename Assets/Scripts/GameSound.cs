

// Este enum define los tipos de sonido disponibles para reproducir.
// Cada valor del enum puede tener AudioClips asignados en el panel del Inspector del SoundManager.

public enum GameSound
{
    // Es recomendable mantener 'None' como la primera opci�n, ya que ayuda a exponer este enum en el Inspector.
    // Si la primera opci�n ya es un valor real, entonces no hay opci�n de "nada seleccionado".
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

    Default,
    UI,
    BGM,
    SceneSound

}
