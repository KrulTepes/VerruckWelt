using System;
using UnityEngine;

namespace SceneManagement
{
    public class SceneController : MonoBehaviour
    {
        private static SceneController _instance;

        public GameObject startScene;
        private GameObject _currentSceneInstanse;

        private void Awake()
        {
            if (_instance != null) Destroy(this);
            _instance = this;
        }

        public static SceneController GetInstance()
        {
            if (_instance == null)
            {
                throw new Exception("SceneController not be initialized!");
            }
            return _instance;
        }

        void Start()
        {
            if (startScene == null) {
                throw new Exception("Scene Controller: start scene not be initialized!");
            }
            Load(startScene);
        }

        public void Load(GameObject scene)
        {
            if (_currentSceneInstanse != null)
            {
                Destroy(_currentSceneInstanse);
                _currentSceneInstanse = null;
            }
            _currentSceneInstanse = Instantiate(scene, transform.parent);
        }
    }
}
