using UnityEngine;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public class Cursor : MonoBehaviour
    {
        private Vector2 virtualMousePixels; 
        private Rigidbody2D rb;
        private Vector2 lastMousePos;
        private Vector2 virtualPos;

        void Start()
        {
            virtualMousePixels = Mouse.current.position.ReadValue();
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            
            lastMousePos = GetMousePos();
            virtualPos = rb.position;
        }

        void FixedUpdate()
        {
            Vector2 currentMousePos = GetMousePos();
            Vector2 delta = currentMousePos - lastMousePos;
            lastMousePos = currentMousePos;

            virtualPos += delta;
            rb.MovePosition(virtualPos);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            virtualPos = rb.position;
        }

        private Vector2 GetMousePos()
        {
            virtualMousePixels += Mouse.current.delta.ReadValue();
            virtualMousePixels += Mouse.current.delta.ReadValue();
            virtualMousePixels += Mouse.current.delta.ReadValue();
            virtualMousePixels += Mouse.current.delta.ReadValue();
            virtualMousePixels += Mouse.current.delta.ReadValue();
            virtualMousePixels += Mouse.current.delta.ReadValue();
            virtualMousePixels += Mouse.current.delta.ReadValue();
            virtualMousePixels += Mouse.current.delta.ReadValue();
            virtualMousePixels += Mouse.current.delta.ReadValue();
            virtualMousePixels += Mouse.current.delta.ReadValue();
            virtualMousePixels += Mouse.current.delta.ReadValue();
            virtualMousePixels += Mouse.current.delta.ReadValue();
            return Camera.main.ScreenToWorldPoint((Vector3)virtualMousePixels + Vector3.forward * 10f);
        }
    }
}