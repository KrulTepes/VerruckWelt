using UnityEngine;

namespace Utility
{
    public class ApplicationSet : MonoBehaviour
    { 
        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}
