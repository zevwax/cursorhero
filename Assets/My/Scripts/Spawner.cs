using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public static class Spawner
    {
        public static PhysicsMaterial2D CreateIceMaterial()
        {
            var ice = new PhysicsMaterial2D("Ice");
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
            var obj = NewEntity<MainCharacter>(pos, "MainCharacter", "My/My/Sprites/glove", 17, 22, 0 - 17/2f, 0 - 22/2f);
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Pointer";
            return obj;
        }
        public static void NewSoul(Vector2 pos)
        {
            GameObject obj = new GameObject("Soul");
            obj.transform.position = new Vector3(pos.x, pos.y, 0);
            var sr = obj.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Soul";
            sr.sprite = Resources.Load<Sprite>("My/WinXP/Cursor/default_arrow");
            obj.AddComponent<Soul>();
        }
        public static GameObject NewWhite(Vector2 pos)
        {
            var obj = NewEntity<White>(pos, "Enemy", "My/My/Sprites/pointer", 17, 22, 0 - 17/2f, 0 - 22/2f);
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
            return obj;
        }
        public static GameObject NewYellow(Vector2 pos)
        {
            var obj = NewEntity<Yellow>(pos, "Enemy", "My/My/Sprites/pointer", 17, 22, 0 - 17/2f, 0 - 22/2f);
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
            return obj;
        }
        public static GameObject NewCyan(Vector2 pos)
        {
            var obj = NewEntity<Cyan>(pos, "Enemy", "My/My/Sprites/pointer", 17, 22, 0 - 17/2f, 0 - 22/2f);
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
            return obj;
        }
        public static void NewYellowP(Vector2 pos, Vector2 direction)
        {
            var projectile = NewEntity<YellowP>(pos,"YellowP", "My/My/Sprites/glass", 11, 20, 0 - 11/2f, 0 - 20/2f);
            projectile.GetComponent<SpriteRenderer>().sortingLayerName = "Projectiles";
            projectile.GetComponent<BoxCollider2D>().isTrigger = true;
            projectile.GetComponent<Projectile>().direction = direction;
        }
        public static void NewRedP(Vector2 pos, Vector2 direction)
        {
            var projectile = NewEntity<RedP>(pos,"RedP", "My/My/Sprites/arrow", 11, 20, 0 - 11/2f, 0 - 20/2f);
            projectile.GetComponent<SpriteRenderer>().sortingLayerName = "Projectiles";
            projectile.GetComponent<BoxCollider2D>().isTrigger = true;
            projectile.GetComponent<Projectile>().direction = direction;
        }
        public static void NewRingP(Vector2 pos, Vector2 direction)
        {
            var projectile = NewEntity<RingP>(pos,"RingP", "My/My/Sprites/arrow", 11, 20, 0 - 11/2f, 0 - 20/2f);
            projectile.GetComponent<SpriteRenderer>().sortingLayerName = "Projectiles";
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
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "RealDisks";
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            return obj;
        }
        
        public static GameObject NewFallingDisk(Vector2 pos)
        {
            var obj = new GameObject("FallingDisk");
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "FallingDisks";
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(15, 15);
            rectTransform.localScale = new Vector3(0, 0, 1);
            var canvasScaler = obj.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 30f;
            canvasScaler.referencePixelsPerUnit = 30f;
            var raycaster = obj.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            raycaster.blockingMask = -1;
            var worldSpaceCanvasScaler = obj.AddComponent<WorldSpaceCanvasOnInitScaler>();
            var fallingDisk = obj.AddComponent<FallingDisk>();
            
            var imageObj = new GameObject("Image");
            imageObj.transform.SetParent(obj.transform, false);
            var imageRt = imageObj.AddComponent<RectTransform>();
            imageRt.anchorMin = Vector2.zero;
            imageRt.anchorMax = Vector2.one;
            imageRt.offsetMin = Vector2.zero;
            imageRt.offsetMax = Vector2.zero;
            var image = imageObj.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>("My/My/Sprites/disk");
            if (imageObj.GetComponent<CanvasRenderer>() == null)
                imageObj.AddComponent<CanvasRenderer>();
            
            return obj;
        }
        
        public static GameObject NewYellowCirc()
        {
            var obj = new GameObject("Yellow Circ");
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "YellowCirc";
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(32, 32);
            rectTransform.localScale = new Vector3(0, 0, 1);
            var canvasScaler = obj.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 30f;
            canvasScaler.referencePixelsPerUnit = 30f;
            var raycaster = obj.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            raycaster.blockingMask = -1;
            var worldSpaceCanvasScaler = obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
            var yellowCirc = obj.AddComponent<YellowCirc>();
            
            var imageObj = new GameObject("Image");
            imageObj.transform.SetParent(obj.transform, false);
            var imageRt = imageObj.AddComponent<RectTransform>();
            imageRt.anchorMin = Vector2.zero;
            imageRt.anchorMax = Vector2.one;
            imageRt.offsetMin = Vector2.zero;
            imageRt.offsetMax = Vector2.zero;
            var image = imageObj.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>("My/My/Sprites/yellow_circ");
            imageObj.AddComponent<CanvasRenderer>();
            
            return obj;
        }
        public static GameObject NewTooltip(string text)
        {
            var obj = new GameObject("Tooltip");
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "Tooltips";
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(90, 30);
            rectTransform.localScale = new Vector3(0, 0, 1);
            var canvasScaler = obj.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 30f;
            canvasScaler.referencePixelsPerUnit = 30f;
            var raycaster = obj.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            raycaster.blockingMask = -1;
            var worldSpaceCanvasScaler = obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
            var tooltip = obj.AddComponent<Tooltip>();
            
            var imageObj = new GameObject("Image");
            imageObj.transform.SetParent(obj.transform, false);
            var imageRt = imageObj.AddComponent<RectTransform>();
            imageRt.anchorMin = Vector2.zero;
            imageRt.anchorMax = Vector2.one;
            imageRt.offsetMin = Vector2.zero;
            imageRt.offsetMax = Vector2.zero;
            var image = imageObj.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>("My/My/Sprites/tooltip");
            image.type = Image.Type.Sliced;
            imageObj.AddComponent<CanvasRenderer>();
            
            var textObj = new GameObject("Text");
            textObj.transform.SetParent(imageObj.transform, false);
            var textRt = textObj.AddComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = new Vector2(4, 4);
            textRt.offsetMax = new Vector2(-4, -4);
            
            var textTxt = textObj.AddComponent<TextMeshProUGUI>();
            textTxt.font = Resources.Load<TMP_FontAsset>("My/My/Fonts/tahoma08pt");
            textTxt.text = text;
            textTxt.alignment = TextAlignmentOptions.Center;
            textTxt.color = Color.black;
            textTxt.enableAutoSizing = true;
            textTxt.fontSizeMin = ushort.MinValue;
            textTxt.fontSizeMax = ushort.MaxValue;
            textTxt.raycastTarget = false;
            
            return obj;
        }
        public static GameObject NewHealthBar()
        {
            var obj = new GameObject("HealthBar");
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "Pointer";

            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(11, 1);
            rectTransform.localScale = new Vector3(0, 0, 1); 

            obj.AddComponent<CanvasScaler>();
            obj.AddComponent<GraphicRaycaster>();
            obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
            var healthBar = obj.AddComponent<HealthBar>();
    
            var bgObj = new GameObject("BG");
            bgObj.transform.SetParent(obj.transform, false);
            var bgRt = bgObj.AddComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero;
            bgRt.anchorMax = Vector2.one;
            bgRt.offsetMin = Vector2.zero;
            bgRt.offsetMax = Vector2.zero;
            bgObj.AddComponent<Image>().color = Color.black;
    
            var fgObj = new GameObject("FG");
            fgObj.transform.SetParent(obj.transform, false);
            var fgRt = fgObj.AddComponent<RectTransform>();
            fgRt.anchorMin = Vector2.zero;
            fgRt.anchorMax = Vector2.one;
            fgRt.offsetMin = Vector2.zero;
            fgRt.offsetMax = Vector2.zero;
            fgObj.AddComponent<Image>().color = Color.red;
    
            healthBar.Setup(fgRt);

            return obj;
        }

        public static GameObject NewPlayButton(Vector2 pos)
        {
            var obj = CreateBaseButton<Play>(pos, "PlayButton");
            var script = obj.GetComponent<Play>();
            return obj;
        }
        public static GameObject NewEndlessModeButton(Vector2 pos)
        {
            var obj = CreateBaseButton<EndlessMode>(pos, "EndlessModeButton");
            var script = obj.GetComponent<EndlessMode>();
            return obj;
        }
        public static GameObject NewUpgradeButton(string upgradeName, Vector2 pos)
        {
            GameObject obj;
            switch (upgradeName)
            {
                case "Damage": obj = CreateBaseButton<ProjectileDamage>(pos, "UpgradeDamage"); break;
                case "FireRate": obj = CreateBaseButton<Firerate>(pos, "UpgradeFireRate"); break;
                case "Speed": obj = CreateBaseButton<ProjectileSpeed>(pos, "UpgradeSpeed"); break;
                case "Sensitivity": obj = CreateBaseButton<Sensitivity>(pos, "UpgradeSensitivity"); break;
                case "Heart": obj = CreateBaseButton<Heart>(pos, "UpgradeHeart"); break;
                default: throw new System.NotImplementedException();
            }
            return obj;
        }

        private static GameObject CreateBaseButton<T>(Vector2 pos, string name) where T : Button
        {
            GameObject obj = NewEntity<T>(pos, name, "My/My/Sprites/btn", 32f, 32f, -16f, -16f);
            //obj.transform.localScale = new Vector3(32f/30f, 32f/30f, 1f);
            obj.layer = LayerMask.NameToLayer("Button");
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "Buttons";
            var col = obj.GetComponent<BoxCollider2D>();
            col.isTrigger = true;
            return obj;
        }
    }
}