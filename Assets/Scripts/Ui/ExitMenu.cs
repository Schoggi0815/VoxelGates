using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ui
{
    public class ExitMenu : MonoBehaviour
    {
        public GameObject child;
        public RectTransform canvas;

        private RectTransform _rectTransform;
    
        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            MoveUp();
        }

        private void MoveDown()
        {
            child.SetActive(true);
            
            _rectTransform.MoveUI(new Vector2(.5f, .5f), canvas, 1);
        }

        private void MoveUp()
        {
            _rectTransform.MoveUI(new Vector2(.5f, 1.5f), canvas, 1).SetOnComplete(() => child.SetActive(false));
        }

        public void Yes()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void No()
        {
            MoveUp();
        }

        public void Open()
        {
            MoveDown();
        }
    }
}
