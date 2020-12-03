using UnityEditor;
using UnityEngine;

namespace Utility
{
    public class ApplicationClose : MonoBehaviour
    {
        public void Close()
        {
            Application.Quit();
        }
    }
}