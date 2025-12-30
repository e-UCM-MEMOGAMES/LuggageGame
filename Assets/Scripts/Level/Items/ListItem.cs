using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class ListItem : MonoBehaviour
{
    /// <summary>
    /// Icono del tick verde
    /// </summary>
    [SerializeField]
    GameObject check,
    /// <summary>
    /// Icono del X roja
    /// </summary>
    cross;

    /// <summary>
    /// Evento de localizar string del texto
    /// </summary>
    [SerializeField]
    LocalizeStringEvent localization;

    // Start is called before the first frame update
    void Start()
    {
        check.SetActive(false);
        cross.SetActive(false);
    }

    /// <summary>
    /// Llamado por el LevelManager. Indica que referencia de la tabla de localizaciones usar
    /// </summary>
    public void SetName(LocalizedString localized)
    {
        localization.StringReference = localized;
    }
    /// <summary>
    /// Llamado por el LevelManager al meter/sacar un objeto de la maleta
    /// </summary>
    public void SetObtained(bool obtained)
    {
        // Oculta la X y muestra el tick si el objeto ha sido obtenido (o lo oculta en caso contrario)
        cross.SetActive(false);
        check.SetActive(obtained);
    }
    /// <summary>
    /// Llamado por el LevelManager al terminar el nivel
    /// </summary>
    public void SetMissed(bool missed)
    {
        // Muestra la X si el objeto no ha sido obtenido (o la oculta en caso contrario) y oculta el tick
        cross.SetActive(missed);
        check.SetActive(false);
    }
}
