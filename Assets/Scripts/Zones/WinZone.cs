using UI;
using UnityEngine;

namespace Zones
{
    [RequireComponent(typeof(Collider2D))]
    public class WinZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                //UIUtility.GetInstance().StopTime();
                StartCoroutine(UIController.FadeSceneOut(UIController.FadeType.Win));
            }
        }
    }
}
