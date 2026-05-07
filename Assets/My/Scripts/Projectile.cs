using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ZevWaxGames.CursorHero
{
    public abstract class Projectile : MonoBehaviour
    {
        private static GameObject bloodSplash;
        private static Texture2D _editableTexture;
        private static Transform psholder;
        protected float damage;
        public float speed;
        public Vector3 direction;
        private Collider2D col;

        private void OnEnable()
        {
            EventHolder.OnRunStarted += Clean;
            EventHolder.OnChoosingStarted += Disable;
            EventHolder.OnChoosingFinished += Enable;
            EventHolder.OnPlayerDie += Disable;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= Clean;
            EventHolder.OnChoosingStarted -= Disable;
            EventHolder.OnChoosingFinished -= Enable;
            EventHolder.OnPlayerDie -= Disable;
        }

        protected virtual void Start()
        {
            col = GetComponent<Collider2D>();
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
                float offset = -28.8f;
                transform.rotation = Quaternion.Euler(0, 0, angle + offset);
            }
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Cursor>() != null)
            {
                other.gameObject.GetComponent<Cursor>().HP -= damage;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                var rotation = Quaternion.Euler(0, 0, angle - 90f);
                Instantiate(bloodSplash, other.transform.position, rotation, psholder);
                
                Debug.Log(direction);
                
                DrawBloodOnWallpaper(other.transform.position);
            }
            Die();
        }
        private void DrawBloodOnWallpaper(Vector3 hitPosition)
        {
            var wallObj = GameObject.Find("Blood Canvas");
            if (wallObj == null) return;

            Image wallImage = wallObj.GetComponent<Image>();
            RectTransform rectTransform = wallObj.GetComponent<RectTransform>();

            if (_editableTexture == null)
            {
                Texture2D originalTex = wallImage.sprite.texture;
                _editableTexture = Instantiate(originalTex);
                
                wallImage.sprite = Sprite.Create(
                    _editableTexture, 
                    new Rect(0, 0, _editableTexture.width, _editableTexture.height), 
                    new Vector2(0.5f, 0.5f)
                );
            }

            Texture2D tex = _editableTexture;

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
        public void RefreshWallpapers()
        {
            // 1. Load the clean data source
            var cleanSprite = Resources.Load<Sprite>("My/My/Sprites/blood_canvas");
            if (cleanSprite == null) return;

            var wallObj = GameObject.Find("Blood Canvas");
            if (wallObj == null) return;
            var wallImage = wallObj.GetComponent<Image>();

            // 2. If we don't have an editable texture yet, create it now
            if (_editableTexture == null)
            {
                _editableTexture = Instantiate(cleanSprite.texture);
            }
            else
            {
                // 3. Otherwise, copy the clean pixels into our existing editable texture
                Graphics.CopyTexture(cleanSprite.texture, _editableTexture);
                _editableTexture.Apply();
            }

            // 4. IMPORTANT: Ensure the UI Image is using a sprite created FROM our editable texture
            // If we just set it to cleanSprite, we can't draw on it anymore.
            wallImage.sprite = Sprite.Create(
                _editableTexture, 
                new Rect(0, 0, _editableTexture.width, _editableTexture.height), 
                new Vector2(0.5f, 0.5f)
            );
        }
        private void Enable()
        {
            col.enabled = true;
        }
        private void Disable()
        {
            col.enabled = false;
        }
        private void Clean()
        {
            RefreshWallpapers();
            Die();
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}