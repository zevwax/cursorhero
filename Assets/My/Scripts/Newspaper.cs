using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening; 
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class Newspaper : MonoBehaviour
    {
        public float Edge => edge;
        public float Size => size;
        private float edge;
        private float size;
        
        [Header("Settings")]
        [SerializeField] private float drag = 5f;
        
        public string tooltipText = "Default Tooltip";

        private Vector2 _grabOffset;
        private bool _isHeld = false;
        private bool _isHovered = false; // Track hover state
        private GameObject _currentTooltip; // Reference to active tooltip

        private BoxCollider2D myCollider;
        private Rigidbody2D rb;

        private void OnEnable()
        {
            EventHolder.OnLimbo += Die;
        }
        private void OnDisable()
        {
            EventHolder.OnLimbo -= Die;
        }
        private void Start()
        {
            size = Random.Range(1f, 3f);
            edge = Random.Range(1f, 3f);
            tooltipText = string.Format("projectile.png\nEdge: {0:F1} / Size: {1:F1}", edge, size);
            
            myCollider = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();

            rb.linearDamping = drag;
            rb.angularDamping = drag; 
            rb.constraints = RigidbodyConstraints2D.None;
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

            // --- Tooltip & Hover Logic ---
            if (collision && !_isHeld)
            {
                if (!_isHovered)
                {
                    _isHovered = true;
                    _currentTooltip = Spawner.NewTooltip(tooltipText);
                }
            }
            else
            {
                if (_isHovered)
                {
                    _isHovered = false;
                    if (_currentTooltip != null) _currentTooltip.GetComponent<Tooltip>().Die();
                }
            }

            // --- Drag Logic ---
            if (collision && mouseDown)
            {
                _isHeld = true;
                _grabOffset = (Vector2)transform.position - (Vector2)mainChar.transform.position;
                
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.simulated = false;

                // Hide tooltip immediately when grabbed
                if (_currentTooltip != null) _currentTooltip.GetComponent<Tooltip>().Die();
            }

            if (!mouseHold)
            {
                if (_isHeld) rb.simulated = true; 
                _isHeld = false;
            }

            if (_isHeld)
            {
                transform.position = mainChar.transform.position + (Vector3)_grabOffset;
                mainChar.SetGrab(gameObject);
            }
            else if (collision) mainChar.SetTake(gameObject);
            else mainChar.SetGlove(gameObject);
        }

        private void OnDestroy()
        {
            if (_currentTooltip != null) _currentTooltip.GetComponent<Tooltip>().Die();
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}