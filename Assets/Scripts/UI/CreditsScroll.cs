using UnityEngine;
using UnityEngine.UI;

public class CreditsScroll : MonoBehaviour
{
    /// <summary>
    /// CanvasScaler del canvas para saber sus dimensiones originales
    /// </summary>
    [SerializeField]
    CanvasScaler canvasScaler;
    /// <summary>
    /// Altura original del canvas
    /// </summary>
    float originalHeight;

    /// <summary>
    /// Distancia inicial entre la parte superior del texto y la parte inferior del canvas
    /// </summary>
    [SerializeField]
    float topOffset = 50,
    /// <summary>
    /// Distancia final entre la parte inferior del texto y la parte supeiror del canvas
    /// </summary>
    bottomOffset = 50,
    /// <summary>
    /// Velocidad a la que se mueve el texto cada frame
    /// </summary>
    scrollSpeed = 100;

    /// <summary>
    /// RectTransform del texto para saber sus dimensiones y desplazarlo
    /// </summary>
    RectTransform textRectTr;
    /// <summary>
    /// Posicion y final del texto a partir de la cual no se desplazara mas
    /// </summary>
    float finalY;
    /// <summary>
    /// Posicion del texto que se ira actualizando en cada frame
    /// </summary>
    Vector3 textPos;


    // Start is called before the first frame update
    void Start()
    {
        textRectTr = GetComponent<RectTransform>();

        // Se fuerza la actualizacion del transform del texto para obtener correctamente sus dimensiones
        LayoutRebuilder.ForceRebuildLayoutImmediate(textRectTr);
        textPos = textRectTr.localPosition;

        // Se calculan los limites verticales del canvas
        originalHeight = canvasScaler.referenceResolution.y;
        float canvasBottom = - originalHeight / 2;
        float canvasTop = canvasBottom + originalHeight;

        // Se coloca el texto con su lado superior a la distancia topOffset del lado inferior del canvas
        textPos.y = canvasBottom - (textRectTr.sizeDelta.y * (1 - textRectTr.pivot.y)) - topOffset;
        textRectTr.localPosition = textPos;

        // Se calcula la posicion final del texto
        finalY = canvasTop + (textRectTr.sizeDelta.y * textRectTr.pivot.y) + bottomOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // Si no ha llegado a la posicion final, se desplaza
        if (textRectTr.localPosition.y < finalY)
        {
            textPos.y += scrollSpeed * Time.deltaTime;
            textRectTr.localPosition = textPos;

            // Si se pasa de la posicion final despues de desplazarse,
            // se fuerza a que este justo en la posicion final
            if (textRectTr.localPosition.y > finalY)
            {
                textPos.y = finalY;
                textRectTr.localPosition = textPos;
            }
        }
    }
}
