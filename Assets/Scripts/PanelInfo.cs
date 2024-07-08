using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class PanelInfo : MonoBehaviour
    {
        [SerializeField]
        private GameObject _panelInfo;

        private float OFFSET_Z { get { return 10.0f; } }


     
        private void OnMouseEnter()
        {
            if (!Input.GetMouseButtonUp(0))
            {
                _panelInfo.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 50, OFFSET_Z);
                Debug.Log("Posiciones del objeto: " + Input.mousePosition.x + " , " + Input.mousePosition.y + " , " + OFFSET_Z);
                _panelInfo.SetActive(true);
            }
        }
        private void OnMouseExit()
        {
            _panelInfo.SetActive(false);
        }
        private void OnMouseUp()
        {
            _panelInfo.SetActive(false);
        }

        private void OnMouseDrag()
        {
            _panelInfo.SetActive(true);
        }
        private void OnMouseDown()
        {
            _panelInfo.SetActive(true);

        }
    }
}
