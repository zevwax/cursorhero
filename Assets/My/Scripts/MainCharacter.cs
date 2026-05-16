using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class MainCharacter : Cursor
    {
        public static MainCharacter Instance { get; private set; }
        
        public float edge = 1f;
        public float size = 1f;
        
        public bool is_trackable = true;
        public float WeightBuff = 0f;
        public float ProjectileSpeed = 6f;
        public float Sensitivity = 0.3f;
        public float MaxHP = 10f;
        public GameObject SkinSetter => skinSetter;
        private GameObject skinSetter = null;
        private GameObject currentShield;
        private void OnEnable()
        {
            EventHolder.OnRunStarted += Born;
            EventHolder.OnChoosingStarted += Disable;
            EventHolder.OnChoosingFinished += Enable;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= Born;
            EventHolder.OnChoosingStarted -= Disable;
            EventHolder.OnChoosingFinished -= Enable;
        }
        private void Start()
        {
            Instance = this;
            
            HP = 10f;
            
            gun = Guns.Library[GunName.Yellow];
            gameObject.layer = LayerMask.NameToLayer("MainCharacter");
            
            virtualMousePixels = Mouse.current.position.ReadValue();
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            
            lastMousePos = GetMousePos();
            virtualPos = rb.position;

            Spawner.NewHealthBar();
            Spawner.NewYellowCirc();
            Spawner.NewSelection();
            
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
            var closest = GetClosestEnemy();
            if (closest != null)
                targetObj = closest;
        }
        private void FixedUpdate()
        {
            Vector2 currentMousePos = GetMousePos();
            Vector2 delta = currentMousePos - lastMousePos;
            lastMousePos = currentMousePos;

            virtualPos += delta;
            rb.MovePosition(virtualPos);
        }
        private Vector2 GetMousePos()
        {
            for(int i = 0; i < 12; i++) 
            {
                virtualMousePixels += Mouse.current.delta.ReadValue() * Sensitivity;
            }
            return Camera.main.ScreenToWorldPoint((Vector3)virtualMousePixels + Vector3.forward);
        }
        public GameObject GetClosestEnemy()
        {
            Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            if (enemies.Length > 0)
            {
                Enemy closest = null;
                float distance = Mathf.Infinity;
                Vector3 position = transform.position;

                foreach (Enemy enemy in enemies)
                {
                    Vector3 diff = enemy.transform.position - position;
                    float curDistance = diff.sqrMagnitude;

                    if (curDistance < distance)
                    {
                        closest = enemy;
                        distance = curDistance;
                    }
                }
                return closest.gameObject;
            }
            return null;
        }
        public void Born()
        {
            edge = 1f;
            size = 1f;
            MaxHP = 10f;
            HP = 10f;
            WeightBuff = 0f;
            ProjectileSpeed = 6f;
            Sensitivity = 0.3f;
            Guns.Library[GunName.Yellow].Cooldown = 2f;
            Guns.Library[GunName.Yellow].Weight = 1f;
            Guns.Library[GunName.Yellow].Size = 1f;
            Enable();
            Projectile.RefreshWallpapers1();
            Projectile.RefreshWallpapers2();
        }
        protected override void Die()
        {
            if (is_trackable)
            {
                /*GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("My/My/Sprites/default_wait");*/
                Disable();
                Spawner.NewSoul(transform.position);
                EventHolder.OnRunFinished?.Invoke();
            }
        }
        private void Enable()
        {
            is_trackable = true;
            SetGlove();
            StartShooting();
        }
        private void Disable()
        {
            is_trackable = false;
        }
        public void SetGlove(GameObject skinSetter)
        {
            if (!IsAbleForReskinBy(skinSetter)) return;
            SetGlove();
            this.skinSetter = null;
        }
        public void SetCross(GameObject skinSetter)
        {
            if (!IsAbleForReskinBy(skinSetter)) return;
            SetSkin("cross");
            this.skinSetter = skinSetter;
        }
        public void SetTake(GameObject skinSetter)
        {
            if (!IsAbleForReskinBy(skinSetter)) return;
            SetSkin("take");
            this.skinSetter = skinSetter;
        }
        public void SetGrab(GameObject skinSetter)
        {
            if (!IsAbleForReskinBy(skinSetter)) return;
            SetSkin("grab");
            this.skinSetter = skinSetter;
        }
        public void SetButtonLink(GameObject skinSetter)
        {
            if (!IsAbleForReskinBy(skinSetter)) return;
            SetSkin("glove");
            this.skinSetter = skinSetter;
        }
        public void StopBeingSkinSetter(GameObject skinSetter)
        {
            if (skinSetter == this.skinSetter)
                this.skinSetter = null;
        }
        public void SetSkin(string spriteName) => transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("My/My/Sprites/{0}", spriteName));
        public bool IsAbleForReskinBy(GameObject skinSetter) => this.skinSetter == null || skinSetter == this.skinSetter;
        private void SetGlove() => SetSkin("glove");
        public override void GetDamage(float damage)
        {
            if (currentShield != null) return;
            StartCoroutine(CastShield());
            StartCoroutine(DoGlitch());
            base.GetDamage(damage);
            Spawner.NewDamageNumbers(transform.position, false, damage);
            ImpulseSource.Instance.Invoke();
        }

        private IEnumerator DoGlitch()
        {
            for (var i = 0; i < 15; i++)
            {
                Spawner.NewGlitch();
                yield return new WaitForSeconds(0.033f);
            }
        }
        private IEnumerator CastShield()
        {
            currentShield = Spawner.NewShield();
            yield return new WaitForSeconds(2f);
            currentShield.GetComponent<CanvasGroup>().alpha = 0;
            yield return new WaitForSeconds(1f);
            Destroy(currentShield.gameObject);
        }
        public void IncreaseWeightBuff(float diff)
        {
            WeightBuff += diff;
            ClipboardTextbox.Instance.UpdateContents();
        }
    }
}