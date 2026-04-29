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
        public static GameObject NewCursorHero()
        {
            GameObject cursorObj = new GameObject("CursorHero");

            SpriteRenderer sr = cursorObj.AddComponent<SpriteRenderer>();
            Sprite arrowSprite = Resources.Load<Sprite>("My/WinXp/Cursor/default_arrow");
            sr.sprite = arrowSprite;

            Rigidbody2D rb = cursorObj.AddComponent<Rigidbody2D>();
            rb.sharedMaterial = CreateIceMaterial();
            rb.gravityScale = 0;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            BoxCollider2D box = cursorObj.AddComponent<BoxCollider2D>();

            float ppu = arrowSprite.pixelsPerUnit;

            float pixelWidth = 9f;
            float pixelHeight = 18f;
            float pixelLeft = 11f - 16f;
            float pixelTop = 12f - 16f;

            box.size = new Vector2(pixelWidth / ppu, pixelHeight / ppu);

            float offsetX = (pixelLeft + (pixelWidth / 2f)) / ppu;
            float offsetY = (-pixelTop - (pixelHeight / 2f)) / ppu;
            
            box.offset = new Vector2(offsetX, offsetY);
            
            cursorObj.AddComponent<Cursor>();

            return cursorObj;
        }
    }
}