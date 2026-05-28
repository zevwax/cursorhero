using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ZevWaxGames.CursorHero
{
    public class Projectile : MonoBehaviour
    {
        public enum ProjectileType
        {
            Projectile,
            RecycleBinKey,
            ClipboardGlyph
        }
        private ProjectileType pType = ProjectileType.Projectile;
        public bool TeamIsAlly => teamIsAlly;
        public float Weight => weight;
        public float Size => size;
        public bool IsBouncyV => isBouncy;
        public bool IsPiercingV => isPiercing;
        public float Speed => speed;
        private static Transform psholder;
        private static GameObject bloodSplash;
        private static Texture2D _editableTexture1;
        private static Texture2D _editableTexture2;
        protected bool teamIsAlly;
        protected float weight;
        protected float size;
        protected bool isBouncy = false;
        protected bool isPiercing = false;
        public float speed;
        public Vector3 direction;
        private BoxCollider2D col;
        public void Init(Glyph glyph)
        {
            col = GetComponent<BoxCollider2D>();
            psholder = GameObject.Find("PSHolder").transform;
            bloodSplash = Resources.Load<GameObject>("My/My/Prefabs/BloodSplash");
            SetTeam(glyph.IsAlly);
            SetWeight(glyph.Weight);
            SetSize(glyph.Size);
            IsBouncy(glyph.IsBouncy);
            IsPiercing(glyph.IsPiercing);
            speed = glyph.Speed;
            
            var tooltip = string.Format("A.glyph\nWeight: {0:F1} / Size: {1:F1}", Weight, Size);
            GetComponent<TooltipHolder>().SetText(tooltip);
            var rb = GetComponent<Rigidbody2D>();
            var drag = 5f;
            rb.linearDamping = drag;
            rb.angularDamping = drag;
            rb.constraints = RigidbodyConstraints2D.None;
        }
        private void OnEnable()
        {
            EventHolder.OnBSODStarted += Clean;
            EventHolder.OnChoosingStarted += HandleChoosingStarted;
            EventHolder.OnChoosingFinished += HandleChoosingFinished;
            EventHolder.OnBinStarted += Disable;
            EventHolder.OnFadingInToPCStarted += HandleFadingInToPCStarted;
            EventHolder.OnRunFinished += Disable;
        }
        private void OnDisable()
        {
            EventHolder.OnBSODStarted -= Clean;
            EventHolder.OnChoosingStarted -= HandleChoosingStarted;
            EventHolder.OnChoosingFinished -= HandleChoosingFinished;
            EventHolder.OnBinStarted -= Disable;
            EventHolder.OnFadingInToPCStarted -= HandleFadingInToPCStarted;
            EventHolder.OnRunFinished -= Disable;
        }
        public void Launch(Vector3 launchDirection)
        {
            direction = launchDirection.normalized;
        }
        protected virtual void Update()
        {
            transform.position += direction * speed * Time.deltaTime;
            
            if (direction != Vector3.zero)
            {
                float angle = Vector2.SignedAngle(Vector2.up, direction);
                float offset = 0f;
                transform.rotation = Quaternion.Euler(0, 0, angle + offset);
            }
        }
        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (pType == ProjectileType.Projectile)
            {
                if (other.gameObject.GetComponent("Wall") != null)
                {
                    if (isBouncy)
                    {
                        Vector2 closestPoint = other.ClosestPoint(transform.position);
                        Vector2 normal = ((Vector2)transform.position - closestPoint).normalized;
                        direction = Vector2.Reflect(direction, normal).normalized;
                        if (Random.Range(0, 4) == 0)
                            isBouncy = false;
                        return;
                    }
                    else
                    {
                        Die();
                        return;
                    }
                }
                if (other.gameObject.GetComponent<Cursor>() != null)
                {
                    var dmg = (float)default;
                    if (teamIsAlly)
                        dmg = weight + MainCharacter.Instance.WeightBuff;
                    else
                        dmg = weight;
                    other.gameObject.GetComponent<Cursor>().GetDamage(dmg);

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    var rotation = Quaternion.Euler(0, 0, angle - 90f);
                    Instantiate(bloodSplash, other.transform.position, rotation, psholder);
                
                    DrawBloodOnWallpaper1(other.transform.position);
                    DrawBloodOnWallpaper2(other.transform.position);
                
                    if (!isPiercing)
                        Die();
                }
            }
        }
        private void DrawBloodOnWallpaper1(Vector3 hitPosition)
        {
            var wallObj = GameObject.Find("Background Blood Canvas");
            if (wallObj == null) return;

            Image wallImage = wallObj.GetComponent<Image>();
            RectTransform rectTransform = wallObj.GetComponent<RectTransform>();

            if (_editableTexture1 == null)
            {
                Texture2D originalTex = wallImage.sprite.texture;
                _editableTexture1 = Instantiate(originalTex);
                
                wallImage.sprite = Sprite.Create(
                    _editableTexture1, 
                    new Rect(0, 0, _editableTexture1.width, _editableTexture1.height), 
                    new Vector2(0.5f, 0.5f)
                );
            }

            Texture2D tex = _editableTexture1;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, 
                Camera.main.WorldToScreenPoint(hitPosition), 
                Camera.main, 
                out localPoint
            );

            float xInsideRect = localPoint.x + (rectTransform.rect.width * rectTransform.pivot.x);
            float yInsideRect = localPoint.y + (rectTransform.rect.height * rectTransform.pivot.y);
            
            int dropletsCount = Random.Range(12, 16);
            Color bloodColor = new Color(0.7f, 0, 0, 1f);
            
            for (int i = 0; i < dropletsCount; i++)
            {
                float forwardShift = i * Random.Range(6f, 7f);
                float sideShift = Random.Range(-12f, 12f);

                Vector2 tangent = new Vector2(-direction.y, direction.x);
                
                Vector2 dropPos = new Vector2(xInsideRect, yInsideRect) 
                                  + (new Vector2(direction.x, direction.y) * forwardShift) 
                                  + (tangent * sideShift);

                int pxCenter = (int)((dropPos.x / rectTransform.rect.width) * tex.width);
                int pyCenter = (int)((dropPos.y / rectTransform.rect.height) * tex.height);

                int radius = Random.Range(2, 7 - (i / 2));
                if (radius < 1) radius = 1;

                DrawCircle(tex, pxCenter, pyCenter, radius, bloodColor);
            }

            tex.Apply();
        }
        private void DrawBloodOnWallpaper2(Vector3 hitPosition)
        {
            var wallObj = GameObject.Find("Progress Bar Blood Canvas");
            if (wallObj == null) return;

            Image wallImage = wallObj.GetComponent<Image>();
            RectTransform rectTransform = wallObj.GetComponent<RectTransform>();

            if (_editableTexture2 == null)
            {
                Texture2D originalTex = wallImage.sprite.texture;
                _editableTexture2 = Instantiate(originalTex);
                
                wallImage.sprite = Sprite.Create(
                    _editableTexture2, 
                    new Rect(0, 0, _editableTexture2.width, _editableTexture2.height), 
                    new Vector2(0.5f, 0.5f)
                );
            }

            Texture2D tex = _editableTexture2;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, 
                Camera.main.WorldToScreenPoint(hitPosition), 
                Camera.main, 
                out localPoint
            );

            float xInsideRect = localPoint.x + (rectTransform.rect.width * rectTransform.pivot.x);
            float yInsideRect = localPoint.y + (rectTransform.rect.height * rectTransform.pivot.y);
            
            int dropletsCount = Random.Range(12, 16);
            Color bloodColor = new Color(0.7f, 0, 0, 1f);
            
            for (int i = 0; i < dropletsCount; i++)
            {
                float forwardShift = i * Random.Range(6f, 7f);
                float sideShift = Random.Range(-12f, 12f);

                Vector2 tangent = new Vector2(-direction.y, direction.x);
                
                Vector2 dropPos = new Vector2(xInsideRect, yInsideRect) 
                                  + (new Vector2(direction.x, direction.y) * forwardShift) 
                                  + (tangent * sideShift);

                int pxCenter = (int)((dropPos.x / rectTransform.rect.width) * tex.width);
                int pyCenter = (int)((dropPos.y / rectTransform.rect.height) * tex.height);

                int radius = Random.Range(2, 7 - (i / 2));
                if (radius < 1) radius = 1;

                DrawCircle(tex, pxCenter, pyCenter, radius, bloodColor);
            }

            tex.Apply();
        }
        private void DrawCircle(Texture2D tex, int cx, int cy, int r, Color color)
        {
            for (int x = -r; x < r; x++)
            {
                for (int y = -r; y < r; y++)
                {
                    if (x * x + y * y <= r * r)
                    {
                        int px = cx + x;
                        int py = cy + y;

                        if (px >= 0 && px < tex.width && py >= 0 && py < tex.height)
                        {
                            Color currentColor = tex.GetPixel(px, py);
                            tex.SetPixel(px, py, Color.Lerp(currentColor, color, 0.85f));
                        }
                    }
                }
            }
        }
        public static void RefreshWallpapers1()
        {
            // 1. Load the clean data source
            var cleanSprite = Resources.Load<Sprite>("My/My/Sprites/SizeHolders/blood_canvas");
            if (cleanSprite == null) return;

            var wallObj = GameObject.Find("Background Blood Canvas");
            if (wallObj == null) return;
            var wallImage = wallObj.GetComponent<Image>();

            // 2. If we don't have an editable texture yet, create it now
            if (_editableTexture1 == null)
            {
                _editableTexture1 = Instantiate(cleanSprite.texture);
            }
            else
            {
                // 3. Otherwise, copy the clean pixels into our existing editable texture
                Graphics.CopyTexture(cleanSprite.texture, _editableTexture1);
                _editableTexture1.Apply();
            }

            // 4. IMPORTANT: Ensure the UI Image is using a sprite created FROM our editable texture
            // If we just set it to cleanSprite, we can't draw on it anymore.
            wallImage.sprite = Sprite.Create(
                _editableTexture1, 
                new Rect(0, 0, _editableTexture1.width, _editableTexture1.height), 
                new Vector2(0.5f, 0.5f)
            );
            Debug.Log("RefreshWallpapers1");
        }
        public static void RefreshWallpapers2()
        {
            // 1. Load the clean data source
            var cleanSprite = Resources.Load<Sprite>("My/My/Sprites/SizeHolders/blood_canvas");
            if (cleanSprite == null) return;

            var wallObj = GameObject.Find("Progress Bar Blood Canvas");
            if (wallObj == null) return;
            var wallImage = wallObj.GetComponent<Image>();

            // 2. If we don't have an editable texture yet, create it now
            if (_editableTexture2 == null)
            {
                _editableTexture2 = Instantiate(cleanSprite.texture);
            }
            else
            {
                // 3. Otherwise, copy the clean pixels into our existing editable texture
                Graphics.CopyTexture(cleanSprite.texture, _editableTexture2);
                _editableTexture2.Apply();
            }

            // 4. IMPORTANT: Ensure the UI Image is using a sprite created FROM our editable texture
            // If we just set it to cleanSprite, we can't draw on it anymore.
            wallImage.sprite = Sprite.Create(
                _editableTexture2,
                new Rect(0, 0, _editableTexture2.width, _editableTexture2.height),
                new Vector2(0.5f, 0.5f)
            );
            Debug.Log("RefreshWallpapers2");
        }
        private void HandleChoosingStarted()
        {
            if (pType == ProjectileType.Projectile)
                col.enabled = false;
            
            if (pType == ProjectileType.RecycleBinKey)
                GetComponent<Canvas>().sortingLayerName = "MainCharacterProjectiles";
        }
        private void HandleChoosingFinished()
        {
            if (pType == ProjectileType.Projectile)
                Destroy(gameObject);
            
            if (pType == ProjectileType.RecycleBinKey)
                GetComponent<Canvas>().sortingLayerName = "Bottles";
        }
        private void Disable()
        {
            if (pType == ProjectileType.Projectile)
                col.enabled = false;
        }
        private void Clean()
        {
            if (pType != ProjectileType.ClipboardGlyph)
                Die();
        }
        private void Die()
        {
            Destroy(gameObject);
        }
        private void Clear()
        {
            if (pType == ProjectileType.Projectile)
                Destroy(gameObject);
        }
        private void HandleFadingInToPCStarted()
        {
            if (pType != ProjectileType.ClipboardGlyph)
                Destroy(gameObject);
        }
        public void SetTeam(bool isAlly)
        {
            teamIsAlly = isAlly;
            var objLayer = (string)default;
            var color = (Color)default;
            var text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (isAlly)
            {
                objLayer = "MainCharacterProjectile";
                color = MainCharacter.Yellow;
            }
            else
            {
                objLayer = "EnemyProjectile";
                color = new Color(1f, 0f, 0f, 1f);
            }
            gameObject.layer = LayerMask.NameToLayer(objLayer);
            for (var i = 0; i < 8; i++)
                transform.GetChild(i + 1).GetComponent<TextMeshProUGUI>().color = color;
        }
        public void SetWeight(float w)
        {
            var minWeight = 1f;
            var maxWeight = 3f;
            if (w < minWeight || maxWeight < w)
            {
                Debug.LogError("Set weight must be between 1 and 3");
                w = Math.Clamp(w, minWeight, maxWeight);
            }
            weight = w;
            var weightNormalized = (w - minWeight) / (maxWeight - minWeight);
            var text = transform.GetChild(9).GetComponent<TextMeshProUGUI>();
            text.fontSizeMax = ushort.MaxValue;
            text.ForceMeshUpdate();
            var maxSizeAtAll = text.fontSize;
            var maxSizeInFractionsOfMaxVal = 0.9f;
            var maxSize = maxSizeAtAll * maxSizeInFractionsOfMaxVal;
            var minSizeOf8CharsInFractionsOfMaxVal = 0.85f; //0.833f n 0.845f - too small
            var minSizeOf8Chars = maxSizeAtAll * minSizeOf8CharsInFractionsOfMaxVal;
            var minSizeOf1CharInFractionsOfMaxVal = 0.666f; //0.333f n 0.5f - too small
            var minSizeOf1Char = maxSizeAtAll * minSizeOf1CharInFractionsOfMaxVal;
            var newSizeOf1Char = Mathf.Lerp(minSizeOf1Char, maxSize, weightNormalized);
            var newSizeOf8Chars = Mathf.Lerp(minSizeOf8Chars, maxSize, weightNormalized);
            for (var i = 0; i < 8; i++)
                transform.GetChild(i + 1).GetComponent<TextMeshProUGUI>().fontSizeMax = newSizeOf8Chars;
            transform.GetChild(9).GetComponent<TextMeshProUGUI>().fontSizeMax = newSizeOf1Char;
        }
        public void SetSize(float s)
        {
            size = s;
            GetComponent<WorldSpaceCanvasRealtimeScaler>().mult = s;
        }
        public void IsBouncy (bool bouncy) => isBouncy = bouncy;
        public void IsPiercing (bool piercing) => isPiercing = piercing;

        public void MakeItBeAKey()
        {
            SetTeam(true);
            pType = ProjectileType.RecycleBinKey;
            GetComponent<TooltipHolder>().enabled = true;
            GetComponent<Dragable>().enabled = true;
            direction = Vector2.zero;
            GetComponent<Canvas>().sortingLayerName = "Bottles";
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        public void MakeItBeAClipboardItem()
        {
            SetTeam(true);
            pType = ProjectileType.ClipboardGlyph;
            GetComponent<TooltipHolder>().enabled = true;
            GetComponent<Dragable>().enabled = false;
            direction = Vector2.zero;
            GetComponent<Canvas>().sortingLayerName = "Bottles";
            gameObject.layer = LayerMask.NameToLayer("Default");
            MainCharacter.Instance.ClipboardGlyph = gameObject;
        }
    }
}