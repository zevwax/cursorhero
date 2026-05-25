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
        public float Sensitivity = 0.25f;
        public float MaxHP = 5f;
        public GameObject SkinSetter => skinSetter;
        private GameObject skinSetter = null;
        private GameObject currentShield;
        private Coroutine currentShieldRoutine;
        
        private HeartKeeper hk;

        // === NEW ===
        public Vector2 BackDirection { get; private set; } = Vector2.left;
        // ===========

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
            
            HP = 5f;
            
            gun = Guns.Library[GunName.Yellow];
            gameObject.layer = LayerMask.NameToLayer("MainCharacter");
            
            virtualMousePixels = Mouse.current.position.ReadValue();
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            
            lastMousePos = GetMousePos();
            virtualPos = rb.position;

            //Spawner.NewHealthBar();
            Spawner.NewYellowCirc();
            Spawner.NewSelection();
            hk = Spawner.NewHeartKeeper(Vector2.zero).GetComponent<HeartKeeper>();
            
            // === NEW ===
            if (hk != null && hk.GetComponent<Follower>() != null)
            {
                hk.GetComponent<Follower>().UpdateQueueIndex(0, 1);
            }
            // ===========
            
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

            // === NEW ===
            if (delta.sqrMagnitude > 0.001f)
            {
                BackDirection = -delta.normalized;
            }
            // ===========

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
            MaxHP = 5f;
            HP = 5f;
            WeightBuff = 0f;
            ProjectileSpeed = 6f;
            Sensitivity = 0.25f;
            Guns.Library[GunName.Yellow].Cooldown = 2f;
            Guns.Library[GunName.Yellow].Weight = 1f;
            Guns.Library[GunName.Yellow].Size = 1f;
            Enable();
            Projectile.RefreshWallpapers1();
            Projectile.RefreshWallpapers2();
            SummonShield();
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
            SummonShield();
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
            SetSkin("copy");
            this.skinSetter = skinSetter;
        }
        public void SetTake(GameObject skinSetter)
        {
            if (!IsAbleForReskinBy(skinSetter)) return;
            SetSkin("drop");
            this.skinSetter = skinSetter;
        }
        public void SetGrab(GameObject skinSetter)
        {
            if (!IsAbleForReskinBy(skinSetter)) return;
            SetSkin("drag");
            this.skinSetter = skinSetter;
        }
        public void SetButtonLink(GameObject skinSetter)
        {
            if (!IsAbleForReskinBy(skinSetter)) return;
            SetSkin("link");
            this.skinSetter = skinSetter;
        }
        public void SetPush(GameObject skinSetter)
        {
            if (!IsAbleForReskinBy(skinSetter)) return;
            SetSkin("push");
            this.skinSetter = skinSetter;
        }
        public void StopBeingSkinSetter(GameObject skinSetter)
        {
            if (skinSetter == this.skinSetter)
                this.skinSetter = null;
        }
        public void SetSkin(string spriteName) => transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("My/My/Sprites/{0}", spriteName));
        public bool IsAbleForReskinBy(GameObject skinSetter) => this.skinSetter == null || skinSetter == this.skinSetter;
        private void SetGlove() => SetSkin("idle");
        public override void GetDamage(float damage)
        {
            if (currentShield != null) return;
            DJ.Instance.HandleGettingDamage();
            SummonShield();
            StartCoroutine(DoGlitch());
            base.GetDamage(damage);
            hk.ShowFingers((int)System.Math.Round(HP));
            Spawner.NewDamageNumbers(transform.position, false, damage);
            ImpulseSource.Instance.Invoke();
        }
        protected override void Shoot()
        {
            DJ.PlayShoot();
            base.Shoot();
        }
        public IEnumerator DoGlitch()
        {
            for (var i = 0; i < 15; i++)
            {
                Spawner.NewGlitch();
                yield return new WaitForSeconds(0.033f);
            }
        }

        private void SummonShield()
        {
            if (currentShieldRoutine != null)
            {
                StopCoroutine(currentShieldRoutine);
                Destroy(currentShield.gameObject);
            }
            currentShieldRoutine = StartCoroutine(CastShield());
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

        // === NEW ===
        public void AddHeartKeeper()
        {
            Spawner.NewHeartKeeper(transform.position);
            
            Follower[] followers = Object.FindObjectsByType<Follower>(FindObjectsSortMode.None);
            for (int i = 0; i < followers.Length; i++)
            {
                followers[i].UpdateQueueIndex(i, followers.Length);
            }
        }
        // ===========
    }
}