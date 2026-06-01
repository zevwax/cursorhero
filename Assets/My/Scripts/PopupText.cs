using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class PopupText : MonoBehaviour
    {
        private float floatDistance = 1.5f;
        private float duration;
        private Ease moveEase = Ease.OutQuint;
        
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Init(float duration)
        {
            this.duration = duration;
            StartCoroutine(CInit());
        }
        private IEnumerator CInit()
        {
            //проверка ниже нужна так как, по неизвестной причине, несмотря на обнуление canvas obj создаётся огромным: scale = 0.2695548
            yield return new WaitUntil(() => transform.localScale.x > 0f && transform.localScale.x < 0.1f);
            
            GetComponent<CanvasGroup>().alpha = 1;
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