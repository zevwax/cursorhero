using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class BottlePrisoner : MonoBehaviour
    {
        public Transform leftWall;
        public Transform rightWall;
        public Transform topWall;
        public Transform bottomWall;
        private Rigidbody2D rb;
        private void Start() => rb = GetComponent<Rigidbody2D>();
        private void LateUpdate()
        {
            if (leftWall != null && rightWall != null && topWall != null && bottomWall != null)
            {
                var ppu = 30f;
                var minX = leftWall.position.x + (7f/ppu);
                var maxX = rightWall.position.x - (9f/ppu);
                var minY = bottomWall.position.y + (11.5f/ppu);
                var maxY = topWall.position.y - (6.5f/ppu);

                Vector3 currentPos = transform.position;

                var clampedX = Mathf.Clamp(currentPos.x, minX, maxX);
                var clampedY = Mathf.Clamp(currentPos.y, minY, maxY);

                if (rb != null)
                {
                    Vector2 velocity = rb.linearVelocity;
                    if ((currentPos.x >= maxX && velocity.x > 0) || (currentPos.x <= minX && velocity.x < 0)) velocity.x = 0;
                    if ((currentPos.y >= maxY && velocity.y > 0) || (currentPos.y <= minY && velocity.y < 0)) velocity.y = 0;
                    rb.linearVelocity = velocity;
                }

                transform.position = new Vector3(clampedX, clampedY, currentPos.z);
            }
        }
    }
}