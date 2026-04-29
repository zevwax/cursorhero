using UnityEngine;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public class Cursor : MonoBehaviour
    {
        protected Vector2 virtualMousePixels; 
        protected Rigidbody2D rb;
        protected Vector2 lastMousePos;
        protected Vector2 virtualPos;

        protected virtual void Start()
        {
            virtualMousePixels = Mouse.current.position.ReadValue();
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            
            lastMousePos = GetMousePos();
            virtualPos = rb.position;
        }

        protected virtual void FixedUpdate()
        {
            Vector2 currentMousePos = GetMousePos();
            Vector2 delta = currentMousePos - lastMousePos;
            lastMousePos = currentMousePos;

            virtualPos += delta;
            rb.MovePosition(virtualPos);
        }

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            virtualPos = rb.position;
        }

        private Vector2 GetMousePos()
        {
            for(int i = 0; i < 12; i++) 
            {
                virtualMousePixels += Mouse.current.delta.ReadValue();
            }
            return Camera.main.ScreenToWorldPoint((Vector3)virtualMousePixels + Vector3.forward * 10f);
        }
    }
}