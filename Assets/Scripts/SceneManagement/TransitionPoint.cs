using System;
using UnityEngine;

namespace SceneManagement
{
    public class TransitionPoint : MonoBehaviour
    {
        [SerializeField]
        public GameObject loadScene;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) {
                OnLoadScene();
            }
        }

        public void OnLoadScene()
        {
            if (loadScene == null) {
                throw new Exception("Transition Point: load scene not be initialized!");
            }
            SceneController.GetInstance().Load(loadScene);
        }
    }
}
