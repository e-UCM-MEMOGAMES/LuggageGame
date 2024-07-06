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

        public GameObject PanelInfor { get => _panelInfo; set => _panelInfo = value; }    

        public TMP_Text TextPanelInfo { get; set; }

        [SerializeField]
        string id;

        void Start()
        {
            TextPanelInfo = PanelInfor.GetComponentInChildren<TMP_Text>();
        }


        private void OnMouseEnter()
        {
            if (!Input.GetMouseButtonUp(0))
            {
                Debug.Log("Posiciones del objeto: " + Input.mousePosition.x + " , " + Input.mousePosition.y + " , " + OFFSET_Z);
                PanelInfor.SetActive(true);         
                PanelInfor.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y+50, OFFSET_Z);
                TextPanelInfo.text = id;
            }
        }
        private void OnMouseExit()
        {
            PanelInfor.SetActive(false);
        }
      
        private void OnMouseDrag()
        {
            PanelInfor.SetActive(true);
        }
        private void OnMouseDown()
        {
            PanelInfor.SetActive(true);

        }
        private void OnMouseUp()
        {
            PanelInfor.SetActive(false);
        }
    }
}
