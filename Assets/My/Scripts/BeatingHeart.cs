using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ZevWaxGames.CursorHero
{
    [RequireComponent(typeof(Image))]
    public class BeatingHeart : MonoBehaviour
    {
        private float targetScale = 0.8f;
        private float beatDuration = 0.5f;
        private Ease pulseEase = Ease.InOutQuad;

        private Image heartImage;
        private Tweener pulseTweener;

        private void Awake()
        {
            heartImage = GetComponent<Image>();
        }

        private void Start()
        {
            StartBeating();
        }

        private void StartBeating()
        {
            pulseTweener = transform.DOScale(Vector3.one * targetScale, beatDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(pulseEase);
        }

        private void OnDestroy()
        {
            pulseTweener?.Kill();
        }
    }
}