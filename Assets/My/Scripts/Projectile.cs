using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ZevWaxGames.CursorHero
{
    public abstract class Projectile : MonoBehaviour
    {
        private static Transform psholder;
        private static GameObject bloodSplash;
        private static Texture2D _editableTexture1;
        private static Texture2D _editableTexture2;
        protected float edge;
        protected float size;
        public float speed;
        public Vector3 direction;
        private BoxCollider2D col;
        private void OnEnable()
        {
            EventHolder.OnRunStarted += Clean;
            EventHolder.OnChoosingStarted += Disable;
            EventHolder.OnChoosingFinished += Die;
            EventHolder.OnRunFinished += Disable;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= Clean;
            EventHolder.OnChoosingStarted -= Disable;
            EventHolder.OnChoosingFinished -= Die;
            EventHolder.OnRunFinished -= Disable;
        }
        protected virtual void Start()
        {
            col = GetComponent<BoxCollider2D>();
            psholder = GameObject.Find("PSHolder").transform;
            bloodSplash = Resources.Load<GameObject>("My/My/Prefabs/BloodSplash");
            Setup();
        }
        protected abstract void Setup();
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
            if (other.gameObject.GetComponent<Cursor>() != null)
            {
                other.gameObject.GetComponent<Cursor>().GetDamage(edge);

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                var rotation = Quaternion.Euler(0, 0, angle - 90f);
                Instantiate(bloodSplash, other.transform.position, rotation, psholder);
                
                DrawBloodOnWallpaper1(other.transform.position);
                DrawBloodOnWallpaper2(other.transform.position);
            }
            Die();
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
            var cleanSprite = Resources.Load<Sprite>("My/My/Sprites/blood_canvas");
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
            var cleanSprite = Resources.Load<Sprite>("My/My/Sprites/blood_canvas");
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
        private void Disable()
        {
            col.enabled = false;
        }
        private void Clean()
        {
            Die();
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}