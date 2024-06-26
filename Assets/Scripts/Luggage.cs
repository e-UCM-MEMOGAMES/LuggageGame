using RAGE.Analytics;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Luggage : MonoBehaviour
{
    #region Variables de Unity

    [SerializeField]
    private List<LuggageTarget> _targets;
    public Sprite emptyLuggage, fullLuggage;
    [SerializeField]
    private LevelManager levelMng;
    private AudioManager audioMng;
    #endregion

    #region Atributos
    /// <summary>
    /// Objetos que hay que meter en la maleta.
    /// </summary>
    public List<string> ObjetosList { get; set; }

    /// <summary>
    /// Objetos guardados en la maleta y correctos.
    /// </summary>
    public List<string> ObjetosGuardados { get; set; }

    /// <summary>
    /// Objetos guardados en la maleta e incorrectos.
    /// </summary>
    public List<string> ObjetosErroneosGuardados { get; set; }

    /// <summary>
    /// Número de objetos guardados.
    /// </summary>
    private int NumItemsSaved { get; set; } = 0;

    /// <summary>
    /// Maletas en diferentes escenas.
    /// </summary>
    private List<LuggageTarget> Targets { get => _targets; set => _targets = value; }

    /// <summary>
    /// Variable que almacena las estrellas que consigue el usuario.
    /// </summary>
    private int stars = 0;
    public int Stars { get => stars; set => stars = value; }
    #endregion

    #region Eventos

    void Start()
    {
        audioMng = AudioManager.Instance;
        ObjetosGuardados = new List<string>();
        ObjetosErroneosGuardados = new List<string>();
    }
    public void InicializeList()
    {
        ObjetosList = GM.Gm.List;
    }
    void Update() { }

    #endregion

    #region Métodos públicos

    /// <summary>
    /// Guarda un objeto en la maleta.
    /// </summary>
    /// <param name="obj">Objeto a guardar.</param>
    public void SaveObject(Item obj)
    {
        audioMng.Play(GameSound.PutIn);
        NumItemsSaved++;
        obj.gameObject.SetActive(true);
        if (NumItemsSaved == 1)
            Targets.ForEach(target => target.ChangeSprite(fullLuggage));

        if (ObjetosList.Contains(obj.name))
        {
            ObjetosGuardados.Add(obj.name);
            Tracker.T.setVar("Objeto guardado", 1);
            Debug.Log("Correcto");
            levelMng.addToLuggage(obj.name);
        }
        else
        {
            ObjetosErroneosGuardados.Add(obj.name);
            Tracker.T.setVar("Objeto erróneo guardado", 1);
            Debug.Log("Correcto");
        }
    }

    /// <summary>
    /// Quita un objeto de la maleta.
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveObject(Item obj)
    {
        audioMng.Play(GameSound.ThrowOut);
        NumItemsSaved--;
        obj.gameObject.SetActive(false);
        if (NumItemsSaved == 0)
            Targets.ForEach(target => target.ChangeSprite(emptyLuggage));

        if (ObjetosGuardados.Contains(obj.name))
        {
            ObjetosGuardados.Remove(obj.name);
            Tracker.T.setVar("Objeto quitado", 1);
            levelMng.removefromLuggage(obj.name);
        }
        else
        {
            ObjetosErroneosGuardados.Remove(obj.name);
            Tracker.T.setVar("Objeto erróneo quitado", 1);
        }
    }

    #endregion

}
