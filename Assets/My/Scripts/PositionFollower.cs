using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class PositionFollower : MonoBehaviour
    {
        private bool initialized = false;
        public Transform target;
        public Vector2 offset;

        private void LateUpdate()
        {
            if (target != null)
            {
                initialized = true;
                transform.position = target.position + new Vector3(offset.x, offset.y, 0);
            }

            if (initialized && target == null)
                Destroy(gameObject);
        }
    }
}