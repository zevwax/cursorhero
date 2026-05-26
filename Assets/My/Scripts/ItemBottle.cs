using UnityEngine;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public class ItemBottle : MonoBehaviour
    {
        private Transform psholder;
        private GameObject beerSplash;
        public RectTransform waterRect;
        public HeartKeeper prisoner;
        public Color glassColor;
        public Color liquidColor;

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

        private BoxCollider2D myCollider;
        
        private ContactFilter2D _wallFilter;
        private readonly BoxCollider2D[] _results = new BoxCollider2D[1];
        
        private void OnEnable()
        {
            EventHolder.OnFadingInToPCStarted += Die;
        }
        private void OnDisable()
        {
            EventHolder.OnFadingInToPCStarted -= Die;
        }
        public void Init()
        {
            var tooltip = string.Format("bottle.png\nTotally useless");
            GetComponent<TooltipHolder>().SetText(tooltip);
            
            lastPosition = transform.position;
            myCollider = GetComponent<BoxCollider2D>();
            psholder = GameObject.Find("PSHolder").transform;
            beerSplash = Resources.Load<GameObject>("My/My/Prefabs/BeerSplash");
            
            _wallFilter = new ContactFilter2D();
            _wallFilter.SetLayerMask(LayerMask.GetMask("Wall"));
            _wallFilter.useTriggers = true;
        }
        private void Update()
        {
            UpdatePosition();
            WallCheck();
        }
        private void UpdatePosition()
        {
            if (waterRect == null) return;
            
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
        }
        private void WallCheck()
        {
            int count = myCollider.Overlap(_wallFilter, _results);
    
            if (count > 0)
            {
                Vector2 collisionPoint = _results[0].ClosestPoint(transform.position);
                Vector2 awayFromWall = (Vector2)transform.position - collisionPoint;
                float angle = Mathf.Atan2(awayFromWall.y, awayFromWall.x) * Mathf.Rad2Deg;
                
                var newParticleObject = Instantiate(beerSplash, transform.position, Quaternion.Euler(0, 0, angle - 90), psholder);
                var ps = newParticleObject.transform.GetChild(0).GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    var mainModule = ps.main;
                    mainModule.startColor = liquidColor;
                }
                
                var piecesToDrop = UnityEngine.Random.Range(2, 3+1);
                for (int i = 0; i < piecesToDrop; i++)
                    Spawner.NewLayingPieceOfGlass(transform.position, glassColor);
                
                var mainChar = MainCharacter.Instance;
                mainChar.SetGlove(gameObject);
                if (prisoner != null)
                    mainChar.TameAHeartKeeper(prisoner);
                
                Die();
            }
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}