using UnityEngine;
using DG.Tweening;
using System.Collections;
namespace ZevWaxGames.CursorHero
{
    public class RedArrow : MonoBehaviour
    {
        public Vector2 position;
        public Vector2 direction;
        private RectTransform imageHolder;
        private float floatAmplitudePx = 2f;
        public void Init()
        {
            imageHolder = transform.GetChild(0).GetComponent<RectTransform>();
            var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
            transform.position = new Vector3(8f + 0.75f, -(4.5f + 0.75f), 0);
            transform.localRotation = Quaternion.Euler(0, 0, angle);
            StartCoroutine(Anim());
        }
        private IEnumerator Anim()
        {
            yield return new WaitForSeconds(0.9f);
            yield return transform.DOMove(position, 0.9f).WaitForCompletion();
            StartFlexAnimation();
        }
        private void StartFlexAnimation()
        {
            imageHolder.DOAnchorPosY(-floatAmplitudePx, 0.5f)
                .SetRelative(true)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDestroy() => imageHolder.DOKill();
    }
}