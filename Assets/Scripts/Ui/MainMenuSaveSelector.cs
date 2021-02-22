using SaveObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ui
{
    public class MainMenuSaveSelector : MonoBehaviour
    {
        public TMP_Dropdown dropdown;
    
        void Start()
        {
            var allSaves = SaveHandler.GetAllSaves();

            dropdown.options.Clear();
        
            foreach (var save in allSaves)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(save));
            }

            dropdown.RefreshShownValue();
        }

        public void NewGame()
        {
            LoadGameScene();
        }

        public void Load()
        {
            if (dropdown.options.Count > 0)
            {
                var saveName = dropdown.options[dropdown.value].text;
            
                SaveHandler.Load(saveName);
            
                LoadGameScene();
            }
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
