using TMPro;
using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public static class Spawner
    {
        public static PhysicsMaterial2D CreateIceMaterial()
        {
            PhysicsMaterial2D ice = new PhysicsMaterial2D("Ice");
            ice.friction = 0f;
            return ice;
        }
        public static GameObject NewWall(Wall.WallType type)
        {
            GameObject obj = new GameObject("Wall");
            
            Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
            rb.sharedMaterial = CreateIceMaterial();
            rb.gravityScale = 0;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.bodyType = RigidbodyType2D.Static;
            
            obj.AddComponent<BoxCollider2D>();
            Wall script = obj.AddComponent<Wall>();
            script.type = type;
            return obj;
        }

        public static GameObject NewMainCharacter(Vector2 pos)
        {
            var obj = NewEntity<MainCharacter>(pos, "MainCharacter", "My/My/Sprites/default_arrow", 9f, 18f, 11f - 16f, 7f - 16f);
            obj.GetComponent<SpriteRenderer>().sortingOrder = 100;
            return obj;
        }
        
        public static void NewSoul(Vector2 pos)
        {
            GameObject obj = new GameObject("Soul");
            obj.transform.position = new Vector3(pos.x, pos.y, 0);
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>("My/WinXP/Cursor/default_arrow");
            obj.AddComponent<Soul>();
        }
        
        public static GameObject NewWhite(Vector2 pos) 
            => NewEntity<White>(pos, "Enemy", "My/WinXp/Cursor/3dwarro", 9f, 17f, 0f - 16f, 1f - 16f);
        public static GameObject NewYellow(Vector2 pos) 
            => NewEntity<Yellow>(pos, "Enemy", "My/WinXp/Cursor/3dgarro", 9f, 17f, 0f - 16f, 1f - 16f);
        public static GameObject NewCyan(Vector2 pos) 
            => NewEntity<Cyan>(pos, "Enemy", "My/WinXp/Cursor/3dsarro", 9f, 17f, 0f - 16f, 1f - 16f);

        public static void NewYellowP(Vector2 pos, Vector2 direction)
        {
            var projectile = NewEntity<YellowP>(pos,"YellowP", "My/My/Sprites/main_character_projectile", 6f, 6f, 13f - 16f, 13f - 16f);
            projectile.GetComponent<BoxCollider2D>().isTrigger = true;
            projectile.GetComponent<Projectile>().direction = direction;
        }
        public static void NewRedP(Vector2 pos, Vector2 direction)
        {
            var projectile = NewEntity<RedP>(pos,"RedP", "My/My/Sprites/enemy_projectile", 6f, 6f, 13f - 16f, 13f - 16f);
            projectile.GetComponent<BoxCollider2D>().isTrigger = true;
            projectile.GetComponent<Projectile>().direction = direction;
        }
        public static void NewRingP(Vector2 pos, Vector2 direction)
        {
            var projectile = NewEntity<RingP>(pos,"RingP", "My/My/Sprites/ring_projectile", 20f, 20f, 6f - 16f, 6f - 16f);
            projectile.GetComponent<BoxCollider2D>().isTrigger = true;
            projectile.GetComponent<Projectile>().direction = direction;
        }
        
        private static GameObject NewEntity<T>(Vector2 pos, string name, string spritePath, float w, float h, float left, float top) where T : MonoBehaviour
        {
            GameObject obj = new GameObject(name);
            obj.transform.position = new Vector3(pos.x, pos.y, 0);
            
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            Sprite sprite = Resources.Load<Sprite>(spritePath);
            sr.sprite = sprite;
            sr.sortingOrder = 0;
            
            Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
            rb.sharedMaterial = CreateIceMaterial();
            rb.gravityScale = 0;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            BoxCollider2D box = obj.AddComponent<BoxCollider2D>();
            float ppu = sprite.pixelsPerUnit;
            box.size = new Vector2(w / ppu, h / ppu);
            box.offset = new Vector2((left + (w / 2f)) / ppu, (-top - (h / 2f)) / ppu);
            
            obj.AddComponent<T>();

            return obj;
        }
        
        public static GameObject NewDisk(Vector2 pos)
        {
            var obj = NewEntity<Disk>(pos, "Disk", "My/My/Sprites/disk", 15f, 15f, 0f, 0f);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            return obj;
        }
        
        public static GameObject NewFallingDisk(Vector2 pos)
        {
            GameObject obj = new GameObject("FallingDisk");
            obj.transform.position = new Vector3(pos.x, pos.y, 0);
    
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>("My/My/Sprites/disk");
            sr.sortingOrder = 5; // Behind the UI but above the grass

            obj.AddComponent<FallingDisk>();
            return obj;
        }
        
        public static GameObject NewTooltip(string text, Vector2 pos)
        {
            GameObject canvasObj = GameObject.Find("FG Canvas");
            if (canvasObj == null) return null;
            
            GameObject obj = new GameObject("Tooltip");
            obj.transform.SetParent(canvasObj.transform, false);
            
            RectTransform rt = obj.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(200, 50);
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            
            UnityEngine.UI.Image bgImage = obj.AddComponent<UnityEngine.UI.Image>();
            bgImage.color = new Color(0, 0, 0, 0.6f);
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(obj.transform, false);
            
            RectTransform textRt = textObj.AddComponent<RectTransform>();
            textRt.sizeDelta = new Vector2(200, 50); 
            textRt.anchorMin = new Vector2(0.5f, 0.5f);
            textRt.anchorMax = new Vector2(0.5f, 0.5f);
            textRt.pivot = new Vector2(0.5f, 0.5f);
            textRt.anchoredPosition = Vector2.zero;
            
            TextMeshProUGUI t = textObj.AddComponent<TextMeshProUGUI>();
            t.font = Resources.Load<TMP_FontAsset>("My/My/Fonts/tahoma");
            t.text = text;
            t.alignment = TextAlignmentOptions.Center;
            t.color = Color.white;
            t.fontSize = 18;
            t.raycastTarget = false;
            
            Tooltip tooltip = obj.AddComponent<Tooltip>();
            tooltip.textComponent = t;
            tooltip.rectTransform = rt;
            tooltip.gap = 60f;

            return obj;
        }



        public static GameObject NewPlayButton(Vector2 pos)
        {
            GameObject obj = CreateBaseButton<Play>(pos, "PlayButton");
            Play script = obj.GetComponent<Play>();
            script.tooltipText = "Start Game";
            script.iconPath = "your_play_icon_path";
            return obj;
        }

        public static void NewUpgradeButton(string upgradeName, Vector2 pos)
        {
            GameObject obj;
            switch (upgradeName)
            {
                case "Damage": obj = CreateBaseButton<ProjectileDamage>(pos, "UpgradeDamage"); break;
                case "FireRate": obj = CreateBaseButton<Firerate>(pos, "UpgradeFireRate"); break;
                case "Speed": obj = CreateBaseButton<ProjectileSpeed>(pos, "UpgradeSpeed"); break;
                case "Sensitivity": obj = CreateBaseButton<Sensitivity>(pos, "UpgradeSensitivity"); break;
                default: return;
            }
            obj.GetComponent<Button>().tooltipText = "Upgrade " + upgradeName;
        }

        private static GameObject CreateBaseButton<T>(Vector2 pos, string name) where T : Button
        {
            GameObject obj = NewEntity<T>(pos, name, "My/My/Sprites/btn", 32f, 32f, -16f, -16f);
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 90;
            var col = obj.GetComponent<BoxCollider2D>();
            col.isTrigger = true;
            return obj;
        }
    }
}