using UnityEngine;
using DG.Tweening;

namespace ZevWaxGames.CursorHero
{
    public class RedArrow : MonoBehaviour
    {
        public Vector2 direction;
        private RectTransform imageHolder;
        private float floatAmplitudePx = 2f;

        private void Start()
        {
            imageHolder = transform.GetChild(0).GetComponent<RectTransform>();
            var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
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