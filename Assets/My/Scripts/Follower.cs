using UnityEngine;
using DG.Tweening;

namespace ZevWaxGames.CursorHero
{
    public class Follower : MonoBehaviour
    {
        private float minDistance = 1f;
        private float speedMultiplier = 3f;

        private void Update()
        {
            if (MainCharacter.Instance == null) return;

            var targetPosition = MainCharacter.Instance.transform.position;
            var currentPosition = transform.position;

            float distance = Vector2.Distance(currentPosition, targetPosition);

            if (distance <= minDistance)
            {
                transform.DOKill();
                return;
            }

            float dynamicSpeed = (distance - minDistance) * speedMultiplier;
            float duration = distance / dynamicSpeed;

            transform.DOMove(targetPosition, duration)
                .SetEase(Ease.Linear);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}