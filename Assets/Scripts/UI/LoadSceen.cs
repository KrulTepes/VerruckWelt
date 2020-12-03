using System;
using SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoadSceen : MonoBehaviour
    {
        private Button _button;
        public GameObject loadScene;
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClickHandler);
        }

        private void OnClickHandler()
        {
            if (loadScene == null) {
                throw new Exception("New Game Button: load scene not be initialized!");
            }
            SceneController.GetInstance().Load(loadScene);
        }
    }
}
