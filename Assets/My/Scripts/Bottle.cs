using UnityEngine;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public class Bottle : MonoBehaviour
    {
        public RectTransform waterRect;

        [Header("Spring Physics")]
        [Tooltip("Force pulling water back to center. Higher = faster splashes.")]
        public float springStiffness = 110f;
        
        [Tooltip("Friction. Increase this (e.g., 1.5 - 2.5) if the water jitters too much. Нужно оставить очень низкой чтобы вода быстро не затухала")]
        public float damping = 0.1f;
        
        [Header("Impulse Settings")]
        [Tooltip("How sensitive the water is to movement. Нужно оставить очень высокой чтобы вода сильнее плескалась")]
        public float impulseSensitivity = 150f;

        [Tooltip("Max force applied per frame. Prevents 'sticking' to walls.")]
        public float maxImpulseForce = 20f;

        [Header("Visual Limits")]
        public float maxSlosh = 30f;
        public float maxAngle = 25f;

        [Header("Advanced")]
        [Range(0, 1)]
        [Tooltip("How much velocity is kept when hitting the bottle wall. 0 = stop, 1 = perfect bounce.")]
        public float wallBounciness = 0.1f;

        private Vector3 lastPosition;
        private Vector2 _grabOffset;
        private bool _isHeld = false;

        private float _waterVelocity;
        private float _waterDisplacement;

        private Collider2D myCollider;

        void Start()
        {
            lastPosition = transform.position;
            myCollider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (deltaTime <= 0) return;

            // 1. Calculate Impulse from movement
            Vector3 worldVelocity = (transform.position - lastPosition) / deltaTime;
            float lateralImpulse = worldVelocity.x * impulseSensitivity;
            lateralImpulse = Mathf.Clamp(lateralImpulse, -maxImpulseForce, maxImpulseForce);

            // 2. Physics Simulation (Spring-Mass-Damper)
            float force = (-springStiffness * _waterDisplacement) - (damping * _waterVelocity);
            _waterVelocity += force * deltaTime;
            _waterVelocity -= lateralImpulse; 

            // 3. Update Displacement
            _waterDisplacement += _waterVelocity * deltaTime;

            // 4. EDGE BOUNCE LOGIC (Fixes the 'Delay' after stop)
            // If displacement exceeds maxSlosh, we clamp it and invert/dampen the velocity
            if (_waterDisplacement > maxSlosh)
            {
                _waterDisplacement = maxSlosh;
                if (_waterVelocity > 0) _waterVelocity *= -wallBounciness; 
            }
            else if (_waterDisplacement < -maxSlosh)
            {
                _waterDisplacement = -maxSlosh;
                if (_waterVelocity < 0) _waterVelocity *= -wallBounciness;
            }

            // 5. Apply Visuals
            waterRect.anchoredPosition = new Vector2(_waterDisplacement, waterRect.anchoredPosition.y);
            
            float rotationZ = (_waterDisplacement / maxSlosh) * maxAngle;
            waterRect.localRotation = Quaternion.Euler(0, 0, rotationZ);

            lastPosition = transform.position;
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
                _grabOffset = transform.position - mainChar.transform.position;
            }

            if (!mouseHold) _isHeld = false;

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