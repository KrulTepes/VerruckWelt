using System;
using UnityEngine;

namespace UI
{
    public class UIUtility : MonoBehaviour
    {
        private static UIUtility _instance;
        private void Awake()
        {
            if (_instance != null) Destroy(_instance.gameObject);
            _instance = this;
        }
        
        public static UIUtility GetInstance()
        {
            if (_instance == null) {
                throw new Exception("UIUtility not be initialized!");
            }
            return _instance;
        }
        public void StopTime()
        {
            Time.timeScale = 0f;
        }
        
        public void StartTime()
        {
            Time.timeScale = 1f;
        }
    }
}
