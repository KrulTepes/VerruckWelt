using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ButtonUtility : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        public delegate void OnDownHandler();
        public delegate void OnUpHandler();
        public delegate void OnClickHandler();

        public event OnDownHandler OnDown;
        public event OnUpHandler OnUp;
        public event OnClickHandler OnClick;
    
        public void OnPointerDown(PointerEventData eventData)
        {
            OnDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnUp?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }
    }
}
