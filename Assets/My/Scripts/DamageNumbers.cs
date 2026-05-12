using UnityEngine;
using DG.Tweening;
using TMPro;

namespace ZevWaxGames.CursorHero
{
    public class DamageNumbers : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float floatDistance = 1.5f;

        [SerializeField] private float duration = 0.8f;
        [SerializeField] private Ease moveEase = Ease.OutQuint;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            Animate();
        }

        private void Animate()
        {
            transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
            
            transform.DOMoveY(transform.position.y + floatDistance, duration)
                .SetEase(moveEase);
            
            canvasGroup.DOFade(0f, duration)
                .SetEase(Ease.InExpo)
                .OnComplete(() => 
                {
                    Destroy(gameObject);
                });
        }
    }
}