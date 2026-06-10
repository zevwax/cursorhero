using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace ZevWaxGames.CursorHero
{
    public class PowerIsOut : MonoBehaviour
    {
        public static PowerIsOut Instance { get; private set; }
        private Sequence fadeSequence;
        private RectTransform rectTransform;

        private void Awake()
        {
            Instance = this;
            rectTransform = GetComponent<RectTransform>();
        }

        public IEnumerator StartFadeOutAnimation()
        {
            var dur = 0.5f;
            
            fadeSequence?.Kill();
            fadeSequence = DOTween.Sequence();

            var startSize = rectTransform.sizeDelta;
            var text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            var image = transform.GetChild(3).GetComponent<Image>();

            var targetSide = startSize.y / 30f;
            var targetColor = new Color(1f, 1f, 1f, 1f);
            
            fadeSequence.Append(text.DOColor(new Color(1f, 0f, 0f, 1f), dur));
            fadeSequence.Append(text.DOColor(new Color(1f, 0f, 0f, 0f), dur));
            fadeSequence.Append(text.DOColor(new Color(1f, 0f, 0f, 1f), dur));
            fadeSequence.Append(text.DOColor(new Color(1f, 0f, 0f, 0f), dur));
            fadeSequence.Append(text.DOColor(new Color(1f, 0f, 0f, 1f), dur));
            fadeSequence.Append(rectTransform.DOSizeDelta(new Vector2(startSize.x, targetSide), dur)); fadeSequence.Join(image.DOColor(targetColor, dur));
            fadeSequence.Append(rectTransform.DOSizeDelta(new Vector2(targetSide, targetSide), dur));
            fadeSequence.Append(rectTransform.DOSizeDelta(Vector2.zero, dur));
            fadeSequence.SetEase(Ease.OutQuad);
            
            yield return new WaitForSeconds(dur * 8);
        }
        public void StartInstantFadeInAnimation()
        {
            var text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            var image = transform.GetChild(3).GetComponent<Image>();
            var targetColor = new Color(1f, 1f, 1f, 0f);
            text.color = new Color(1f, 0f, 0f, 0f);
            image.color = targetColor;
            rectTransform.sizeDelta = new Vector2(480, 270);
        }
        private void OnDestroy()
        {
            fadeSequence?.Kill();
        }
    }
}