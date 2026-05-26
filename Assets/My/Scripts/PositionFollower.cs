using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class PositionFollower : MonoBehaviour
    {
        public Transform target;
        public Vector2 offset;

        private void LateUpdate()
        {
            if (target != null)
            {
                transform.position = target.position + new Vector3(offset.x, offset.y, 0);
            }
        }
    }
}