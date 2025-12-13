using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField]
    RectTransform canvasRectTr;

    [SerializeField]
    float topOffset = 50,
    bottomOffset = 50,
    scrollSpeed = 100;

    RectTransform rectTr;

    float finalY;

    // Start is called before the first frame update
    void Start()
    {
        rectTr = GetComponent<RectTransform>();
        rectTr.localPosition = new Vector3(rectTr.localPosition.x, - canvasRectTr.sizeDelta.y / 2 - topOffset, rectTr.localPosition.z);

        finalY = canvasRectTr.sizeDelta.y - rectTr.sizeDelta.y + bottomOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (rectTr.localPosition.y + scrollSpeed * Time.deltaTime < finalY)
        {
            rectTr.localPosition += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
        }
        else
        {
            rectTr.localPosition = new Vector3(rectTr.localPosition.x, finalY, rectTr.localPosition.z);
        }
    }
}
