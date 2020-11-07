using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public enum FadeType
        {
            Win, Death, Menu
        }
        
        private static UIController _instance;
        private void Awake()
        {
            if (_instance != null) Destroy(_instance.gameObject);
            _instance = this;
        }

        public static UIController GetInstance()
        {
            if (_instance == null) {
                throw new Exception("UIController not be initialized!");
            }
            return _instance;
        }
        
        public CanvasGroup winCanvasGroup;
        public CanvasGroup deathCanvasGroup;
        public CanvasGroup menuCanvasGroup;
        public float fadeDuration = 1f;

        protected IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup)
        {
            canvasGroup.blocksRaycasts = false;
            float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuration;
            while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha,
                    fadeSpeed * Time.deltaTime);
                yield return null;
            }
            canvasGroup.alpha = finalAlpha;
            canvasGroup.blocksRaycasts = true;
        }
        
        public static IEnumerator FadeSceneIn ()
        {
            CanvasGroup canvasGroup;
            if (_instance.winCanvasGroup.alpha > 0.1f)
                canvasGroup = _instance.winCanvasGroup;
            else if (_instance.deathCanvasGroup.alpha > 0.1f)
                canvasGroup = _instance.deathCanvasGroup;
            else
                canvasGroup = _instance.menuCanvasGroup;
            
            yield return _instance.StartCoroutine(_instance.Fade(0f, canvasGroup));

            canvasGroup.gameObject.SetActive (false);
        }

        public static IEnumerator FadeSceneOut (FadeType fadeType)
        {
            CanvasGroup canvasGroup;
            switch (fadeType)
            {
                case FadeType.Win:
                    canvasGroup = _instance.winCanvasGroup;
                    break;
                case FadeType.Death:
                    canvasGroup = _instance.deathCanvasGroup;
                    break;
                default:
                    canvasGroup = _instance.menuCanvasGroup;
                    break;
            }
            
            canvasGroup.gameObject.SetActive (true);
            yield return _instance.StartCoroutine(_instance.Fade(1f, canvasGroup));
        }
    }
}
