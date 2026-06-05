using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ZevWaxGames.CursorHero
{
    public class MainCharacter : Cursor
    {
        public static MainCharacter Instance { get; private set; }
        public static readonly Color Yellow = new Color(0, 0.33f, 0.66f, 1f);
        public int Version => version;
        private int version = 1;
        public int Drivers => drivers;
        private int drivers = 0;
        /*public void ResetVersion()
        {
            version = 1;
            VersionIndicator.Instance.Reset();
        }*/
        public void UpdateVersion()
        {
            version++;
            drivers++;
            VersionIndicator.Instance.UpdateContents();
            DriversIndicator.Instance.UpdateContents();
        }
        public void SetGlyph(Glyph glyph) => Guns.Library[GunName.Yellow].Glyph = glyph;
        public void SpendDriver()
        {
            drivers--;
            DriversIndicator.Instance.UpdateContents();
        }
        public float Weight => Guns.Library[GunName.Yellow].Glyph.Weight;
        public float Size => Guns.Library[GunName.Yellow].Glyph.Size;
        
        public bool is_trackable = true;
        public float WeightBuff = 0f;
        public float Sensitivity = 0.25f;
        private const int InitNumOfHeartKeepers = 1;
        private int maxHP;
        private int CurrHP => currHP;
        private int currHP;
        public GameObject SkinSetter => skinSetter;
        private GameObject skinSetter = null;
        private GameObject currentShield;
        private Coroutine currentShieldRoutine;
        
        public GameObject ClipboardGlyph;
        /*private bool initialized = false;*/
        
        public Vector2 BackDirection { get; private set; } = Vector2.left;
        private List<HeartKeeper> heartKeepers;
        private void OnEnable()
        {
            EventHolder.OnRunStarted += Born;
            EventHolder.OnYouWinStarted += Disable;
            EventHolder.OnYouWinFinished += Enable;
            EventHolder.OnBSODStarted += KillHeartKeepers;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= Born;
            EventHolder.OnYouWinStarted -= Disable;
            EventHolder.OnYouWinFinished -= Enable;
            EventHolder.OnBSODStarted -= KillHeartKeepers;
        }
        private void KillHeartKeepers() => AddHeartKeepers(-heartKeepers.Count);
        private void Awake() => Instance = this;
        public void Init()
        {
            gun = Guns.Library[GunName.Yellow];
            gameObject.layer = LayerMask.NameToLayer("MainCharacter");
            
            virtualMousePixels = Mouse.current.position.ReadValue();
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            
            lastMousePos = GetMousePos();
            virtualPos = rb.position;
            
            Spawner.NewYellowCirc();
            Spawner.NewSelection();
            Spawner.NewVersionIndicator();
            MaxHPInit();
            RestoreFullHP();
            
            base.Start();
        }
        protected override void Update()
        {
            /*if (!initialized) return;*/
            base.Update();
            var closest = GetClosestEnemy();
            if (closest != null)
                targetObj = closest;
        }
        private void FixedUpdate()
        {
            /*if (!initialized) return;*/
            var currentMousePos = GetMousePos();
            var delta = currentMousePos - lastMousePos;
            lastMousePos = currentMousePos;
            
            if (delta.sqrMagnitude > 0.001f)
                BackDirection = -delta.normalized;
            
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
            /*if (!initialized)
            {
                Init();
                initialized = true;
            }*/
            ResetMaxHP();
            RestoreFullHP();
            //WeightBuff = 0f;
            //Sensitivity = 0.25f;
            
            var gun = Guns.Library[GunName.Yellow];
            /*gun.BurstSize = 1;
            gun.BurstInterval = 2f;
            gun.ShotInterval = 0.2f;*/
            gun.Glyph = new Glyph(true, 1, 1, false, false, 6);
            
            Enable();
            Projectile.RefreshWallpapers1();
            Projectile.RefreshWallpapers2();
            SummonShield();
            
            if (ClipboardGlyph != null)
                Destroy(ClipboardGlyph);
            ClipboardGlyph = Spawner.NewClipboardGlyph(new Vector2(-2f, -4f));
            ClipboardTextbox.Instance.UpdateContents();
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
            var dmg = (int)Math.Round(damage);
            DJ.Instance.HandleGettingDamage();
            SummonShield();
            StartCoroutine(DoGlitch());
            IncreaseCurrHPByValue(-dmg);
            Spawner.NewDamageNumbers(transform.position, false, dmg);
            ImpulseSource.Instance.Invoke();
        }
        public void IncreaseCurrHPByValue(int value)
        {
            Debug.Log("IncreaseCurrHPByValue " + value);
            currHP = Math.Clamp(currHP + value, 0, maxHP);
            UpdateHeartKeepers();
            if (currHP == 0)
                Die();
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
        public void AddHeartKeepers(int number)
        {
            if (number == 0) return;
            if (number < 0)
                for (var i = 0; i < -number; i++)
                {
                    maxHP -= 5;
                    if (CurrHP > maxHP)
                        RestoreFullHP();
                    var index = heartKeepers.Count - 1;
                    var hk = heartKeepers[index];
                    heartKeepers.RemoveAt(index);
                    Destroy(hk.gameObject);
                }
            if (number > 0)
                for (var i = 0; i < number; i++)
                    NewTamedHeartKeeper();
        }
        private HeartKeeper NewTamedHeartKeeper()
        {
            var hk = Spawner.NewHeartKeeper(transform.position).GetComponent<HeartKeeper>();
            TameAHeartKeeper(hk);
            return hk;
        }
        public void TameAHeartKeeper(HeartKeeper hk)
        {
            heartKeepers.Add(hk);
            hk.isOwned = true;
            maxHP += 5;
            UpdateQueueIndexes();
        }
        private void UpdateQueueIndexes()
        {
            for (var i = 0; i < heartKeepers.Count; i++)
                heartKeepers[i].GetComponent<Follower>().UpdateQueueIndex(i, heartKeepers.Count);
        }
        private void UpdateHeartKeepers()
        {
            Debug.Log("UpdateHeartKeepers " + heartKeepers.Count);
            var fingersToShow = CurrHP;
            foreach (var hk in heartKeepers)
            {
                if (fingersToShow > 5)
                {
                    hk.ShowFingers(5);
                    fingersToShow -= 5;
                }
                else
                {
                    hk.ShowFingers(fingersToShow);
                    fingersToShow = 0;
                }
            }
        }
        private void MaxHPInit()
        {
            heartKeepers = new List<HeartKeeper>();
            AddHeartKeepers(InitNumOfHeartKeepers);
        }
        private void ResetMaxHP()
        {
            AddHeartKeepers(-heartKeepers.Count+InitNumOfHeartKeepers);
        }
        public void RestoreFullHP()
        {
            IncreaseCurrHPByValue(-CurrHP+maxHP);
        }
    }
}