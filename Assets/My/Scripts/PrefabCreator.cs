using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public static class PrefabCreator
    {
        public static PhysicsMaterial2D CreateIceMaterial()
        {
            PhysicsMaterial2D ice = new PhysicsMaterial2D("Ice");
            ice.friction = 0f;
            return ice;
        }
        public static GameObject NewBackground()
        {
            GameObject obj = new GameObject("Background");
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>("My/WinXp/Wallpapers/Bliss");
            sr.sortingOrder = -10;
            obj.AddComponent<Background>();
            return obj;
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
            => NewEntity<MainCharacter>(pos, "MainCharacter", "My/WinXp/Cursor/default_arrow", 9f, 18f, 11f - 16f, 12f - 16f);
        
        public static GameObject NewWhite(Vector2 pos) 
            => NewEntity<White>(pos, "Enemy", "My/WinXp/Cursor/3dwarro", 9f, 17f, 0f - 16f, 1f - 16f);
        public static GameObject NewYellow(Vector2 pos) 
            => NewEntity<Yellow>(pos, "Enemy", "My/WinXp/Cursor/3dgarro", 9f, 17f, 0f - 16f, 1f - 16f);
        public static GameObject NewCyan(Vector2 pos) 
            => NewEntity<Cyan>(pos, "Enemy", "My/WinXp/Cursor/3dsarro", 9f, 17f, 0f - 16f, 1f - 16f);

        public static GameObject NewYellowP(Vector2 pos, Vector2 direction)
        {
            var projectile = NewEntity<YellowP>(pos,"YellowP", "My/main_character_projectile", 6f, 6f, 13f - 16f, 13f - 16f);
            projectile.GetComponent<BoxCollider2D>().isTrigger = true;
            projectile.GetComponent<Projectile>().direction = direction;
            return projectile;
        }
        public static GameObject NewRedP(Vector2 pos, Vector2 direction)
        {
            var projectile = NewEntity<RedP>(pos,"RedP", "My/enemy_projectile", 6f, 6f, 13f - 16f, 13f - 16f);
            projectile.GetComponent<BoxCollider2D>().isTrigger = true;
            projectile.GetComponent<Projectile>().direction = direction;
            return projectile;
        }
        public static GameObject NewRingP(Vector2 pos, Vector2 direction)
        {
            var projectile = NewEntity<RingP>(pos,"RingP", "My/ring_projectile", 20f, 20f, 6f - 16f, 6f - 16f);
            projectile.GetComponent<BoxCollider2D>().isTrigger = true;
            projectile.GetComponent<Projectile>().direction = direction;
            return projectile;
        }
        
        private static GameObject NewEntity<T>(Vector2 pos, string name, string spritePath, float w, float h, float left, float top) where T : MonoBehaviour
        {
            GameObject obj = new GameObject(name);
            obj.transform.position = new Vector3(pos.x, pos.y, 0);
            
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            Sprite sprite = Resources.Load<Sprite>(spritePath);
            sr.sprite = sprite;
            
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
    }
}