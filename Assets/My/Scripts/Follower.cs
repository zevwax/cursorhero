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

        // === MY CHANGES START ===
        private Rigidbody2D rb;
        private Vector3 calculatedTargetPosition;
        public void Init() => rb = GetComponent<Rigidbody2D>();
        // === MY CHANGES END ===

        public void UpdateQueueIndex(int index, int total)
        {
            myIndex = index;
            totalCount = total;
        }

        private void Update()
        {
            if (MainCharacter.Instance == null) return;

            var targetPosition = MainCharacter.Instance.transform.position;
            // === MY CHANGES START ===
            // Removed currentPosition local variable from Update
            // === MY CHANGES END ===

            transform.DOKill();

            float targetAngle = 0f;
            if (totalCount > 1)
            {
                float startAngle = -((totalCount - 1) * stepAngle) / 2f;
                targetAngle = startAngle + (myIndex * stepAngle);
            }

            smoothBackDirection = Vector2.Lerp(smoothBackDirection, MainCharacter.Instance.BackDirection, Time.deltaTime * rotationSmoothSpeed);

            Vector2 directionOffset = Quaternion.Euler(0, 0, targetAngle) * smoothBackDirection;
            // === MY CHANGES START ===
            calculatedTargetPosition = targetPosition + (Vector3)(directionOffset * minDistance);
            // Removed all movement and distance calculations from Update
            // === MY CHANGES END ===
        }

        // === MY CHANGES START ===
        private void FixedUpdate()
        {
            if (MainCharacter.Instance == null) return;
            if (rb == null) return;

            Vector3 currentPosition = rb.position;
            float distanceToTarget = Vector3.Distance(currentPosition, calculatedTargetPosition);

            if (distanceToTarget <= 0.01f)
            {
                rb.MovePosition(calculatedTargetPosition);
                return;
            }

            float dynamicSpeed = distanceToTarget * speedMultiplier;
            
            if (dynamicSpeed < 1f) dynamicSpeed = 1f;

            Vector2 nextPosition = Vector3.MoveTowards(currentPosition, calculatedTargetPosition, dynamicSpeed * Time.fixedDeltaTime);
            rb.MovePosition(nextPosition);
        }
        // === MY CHANGES END ===

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}