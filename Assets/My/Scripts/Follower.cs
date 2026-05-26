using UnityEngine;
using DG.Tweening;

namespace ZevWaxGames.CursorHero
{
    public class Follower : MonoBehaviour
    {
        private float minDistance = 1f;
        private float speedMultiplier = 3f;
        
        private float stepAngle = 40f;
        public int MyIndex => myIndex;
        private int myIndex = 0;
        private int totalCount = 1;
        
        private Vector2 smoothBackDirection = Vector2.left;
        private float rotationSmoothSpeed = 5f;

        public void UpdateQueueIndex(int index, int total)
        {
            myIndex = index;
            totalCount = total;
        }

        private void Update()
        {
            if (MainCharacter.Instance == null) return;

            var targetPosition = MainCharacter.Instance.transform.position;
            var currentPosition = transform.position;

            // === NEW ===
            transform.DOKill();

            float targetAngle = 0f;
            if (totalCount > 1)
            {
                float startAngle = -((totalCount - 1) * stepAngle) / 2f;
                targetAngle = startAngle + (myIndex * stepAngle);
            }

            smoothBackDirection = Vector2.Lerp(smoothBackDirection, MainCharacter.Instance.BackDirection, Time.deltaTime * rotationSmoothSpeed);

            Vector2 directionOffset = Quaternion.Euler(0, 0, targetAngle) * smoothBackDirection;
            Vector3 calculatedTargetPosition = targetPosition + (Vector3)(directionOffset * minDistance);

            // ИСПРАВЛЕНИЕ: Считаем честное расстояние ДО ЦЕЛЕВОЙ ТОЧКИ, а не до игрока
            float distanceToTarget = Vector3.Distance(currentPosition, calculatedTargetPosition);

            // Если мы уже очень близко к финальной точке — мягко останавливаемся, предотвращая микро-дергания
            if (distanceToTarget <= 0.01f)
            {
                transform.position = calculatedTargetPosition;
                return;
            }

            // ИСПРАВЛЕНИЕ: Скорость зависит от расстояния до цели, гарантируя, что кипер дойдет до конца
            float dynamicSpeed = distanceToTarget * speedMultiplier;
            
            // Задаем минимальный порог скорости, чтобы они не залипали на месте в конце пути
            if (dynamicSpeed < 1f) dynamicSpeed = 1f;

            transform.position = Vector3.MoveTowards(currentPosition, calculatedTargetPosition, dynamicSpeed * Time.deltaTime);
            // ===========
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}