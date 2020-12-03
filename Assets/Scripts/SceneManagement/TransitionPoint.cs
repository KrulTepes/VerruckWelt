using System;
using UnityEngine;

namespace SceneManagement
{
    public class TransitionPoint : MonoBehaviour
    {
        [SerializeField]
        public GameObject loadScene;

        public void OnLoadScene()
        {
            if (loadScene == null) {
                throw new Exception("Transition Point: load scene not be initialized!");
            }
            SceneController.GetInstance().Load(loadScene);
        }
    }
}
