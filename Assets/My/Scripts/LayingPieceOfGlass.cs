using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening; 
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class LayingPieceOfGlass : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float slideForce = 15f;
        [SerializeField] private float torqueForce = 30f;
        [SerializeField] private float drag = 5f;

        private Vector2 _grabOffset;
        private bool _isHeld = false;

        private Collider2D myCollider;
        private Rigidbody2D rb;
        private CanvasGroup canvasGroup;

        private void Start()
        {
            myCollider = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            canvasGroup = GetComponent<CanvasGroup>();

            rb.linearDamping = drag;
            rb.angularDamping = drag; 
            rb.constraints = RigidbodyConstraints2D.None;
            
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDir * slideForce, ForceMode2D.Impulse);
            
            float randomTorque = Random.Range(-torqueForce, torqueForce);
            rb.AddTorque(randomTorque, ForceMode2D.Impulse);

            StartCoroutine(BlinkRoutine());
        }

        private IEnumerator BlinkRoutine()
        {
            canvasGroup.DOFade(0.2f, 0.2f)
                .SetLoops(10, LoopType.Yoyo)
                .OnComplete(() => {
                    canvasGroup.DOFade(1f, 0.1f);
                });
            yield return null;
        }

        private void Update()
        {
            HandleDragNDropLogic();
        }

        private void HandleDragNDropLogic()
        {
            if (MainCharacter.Instance == null) return;
            var mainChar = MainCharacter.Instance;
            
            bool collision = myCollider.IsTouching(mainChar.GetComponent<Collider2D>());
            
            bool mouseHold = Mouse.current.leftButton.isPressed;
            bool mouseDown = Mouse.current.leftButton.wasPressedThisFrame;

            if (collision && mouseDown)
            {
                _isHeld = true;
                _grabOffset = (Vector2)transform.position - (Vector2)mainChar.transform.position;
                
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.simulated = false; 
            }

            if (!mouseHold)
            {
                if (_isHeld) rb.simulated = true; 
                _isHeld = false;
            }

            if (_isHeld)
            {
                transform.position = mainChar.transform.position + (Vector3)_grabOffset;
                mainChar.SetGrab();
            }
            else if (collision) mainChar.SetTake();
            else mainChar.SetRealLink();
        }
    }
}