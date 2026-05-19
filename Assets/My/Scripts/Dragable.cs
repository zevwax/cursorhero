using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening; 
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class Dragable : MonoBehaviour
    {
        private BoxCollider2D myCollider;
        private bool isHeld;
        private Vector2 grabOffset;
        private Rigidbody2D rb;
        public void Init()
        {
            myCollider = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.None;
        }
        private void Update()
        {
            HandleDragNDropLogic();
        }
        private void HandleDragNDropLogic()
        {
            var mainChar = MainCharacter.Instance;
            var collision = myCollider.IsTouching(mainChar.gameObject.GetComponent<BoxCollider2D>());
            
            var mouseHold = Mouse.current.leftButton.isPressed;
            var mouseDown = Mouse.current.leftButton.wasPressedThisFrame;
            
            //OnStarted
            if (collision && mouseDown)
            {
                isHeld = true;
                grabOffset = (Vector2)transform.position - (Vector2)mainChar.transform.position;
                
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.simulated = false;
                
                mainChar.SetGrab(gameObject);
                
                // Hide tooltip immediately when grabbed
                /*if (_currentTooltip != null) _currentTooltip.GetComponent<Tooltip>().Die();*/
            }
            
            //OnFinished
            if (!mouseHold && isHeld)
            {
                rb.simulated = true; 
                isHeld = false;
            }
            
            if (isHeld)
                transform.position = mainChar.transform.position + (Vector3)grabOffset;
            else if (collision) mainChar.SetTake(gameObject);
            else mainChar.SetGlove(gameObject);
        }
        private void OnDestroy()
        {
            MainCharacter.Instance.SetGlove(gameObject);
        }
    }
}