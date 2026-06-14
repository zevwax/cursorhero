using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class ChallengeIndication : MonoBehaviour
    {
        private Vector2 startPosition;
        private Tweener pulseTweener;
        private float currMult = 1f;

        public void Init()
        {
            startPosition = transform.position;
            StartCoroutine(StartAnimation());
        }
        private IEnumerator StartAnimation()
        {
            var directionToCenter = ((Vector2)Vector3.zero - (Vector2)transform.position).normalized;
            var offsetInUnits = 1f;
            var targetPosition = (Vector2)transform.position + directionToCenter * offsetInUnits;
            
            yield return transform.DOMove(targetPosition, 0.4f).SetEase(Ease.OutBack).WaitForCompletion();

            var maxSize = 1.6f;
            StartBeating(maxSize);
        }
        private void StartBeating(float maxSize)
        {
            DOTween.To(() => currMult, x => currMult = x, maxSize, 0.25f)
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Yoyo);
        }
        public void Remove() => StartCoroutine(CRemove());
        private IEnumerator CRemove()
        {
            transform.DOKill();

            transform.DOMove(startPosition, 0.3f)
                .SetEase(Ease.InBack);
                
            transform.DOScale(Vector3.zero, 0.3f)
                .SetEase(Ease.InBack);

            yield return new WaitForSeconds(0.3f);

            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            pulseTweener?.Kill();
            transform.DOKill();
        }

        private void Update()
        {
            GetComponent<WorldSpaceCanvasRealtimeScaler>().mult = currMult;
        }
    }
}