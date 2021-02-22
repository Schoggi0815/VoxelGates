using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class OverwritePopupUi : MonoBehaviour
    {
        public RectTransform canvas;

        public GameObject child;

        public Text textObject;
        
        private RectTransform _rectTransform;

        private string _text = "A save with the name\n\"{0}\"\nalready exist\n\nDo you want to\noverwrite it?";

        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            MoveLeft();
        }

        public void MoveLeft()
        {
            _rectTransform.MoveUI(new Vector2(-.5f, .5f), canvas, .5f).SetOnComplete(() => child.SetActive(false));
        }

        public void MoveRight(string saveName)
        {
            textObject.text = string.Format(_text, saveName);
            
            child.SetActive(true);
            
            _rectTransform.MoveUI(new Vector2(.5f, .5f), canvas, .5f);
        }

        public void Yes()
        {
            Constants.C.saveHandler.SaveForce();
            
            MoveLeft();
        }

        public void No()
        {
            MoveLeft();
        }
    }
}
