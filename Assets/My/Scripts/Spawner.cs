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
            var obj = NewEntity<White>(pos, "Enemy", "My/My/Sprites/pointer_1", 17, 22, 0 - 17/2f, 0 - 22/2f);
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
            return obj;
        }
        public static GameObject NewYellow(Vector2 pos)
        {
            var obj = NewEntity<Yellow>(pos, "Enemy", "My/My/Sprites/pointer_2", 17, 22, 0 - 17/2f, 0 - 22/2f);
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
            return obj;
        }
        public static GameObject NewCyan(Vector2 pos)
        {
            var obj = NewEntity<Cyan>(pos, "Enemy", "My/My/Sprites/pointer_3", 17, 22, 0 - 17/2f, 0 - 22/2f);
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
            return obj;
        }
        public static void NewYellowP(Vector2 pos, Vector2 direction)
        {
            var projectile = NewEntity<YellowP>(pos,"YellowP", "My/My/Sprites/glass", 11, 15, 0 - 11/2f, 0 - 20/2f);
            var sr = projectile.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "Projectiles";
            sr.color = new Color(1, 1, 1, 0.75f);
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
            var obj = new GameObject(name);
            obj.transform.position = new Vector3(pos.x, pos.y, 0);
            
            var sr = obj.AddComponent<SpriteRenderer>();
            var sprite = Resources.Load<Sprite>(spritePath);
            sr.sprite = sprite;
            sr.sortingOrder = 0;
            
            var rb = obj.AddComponent<Rigidbody2D>();
            rb.sharedMaterial = CreateIceMaterial();
            rb.gravityScale = 0;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            var box = obj.AddComponent<BoxCollider2D>();
            var ppu = sprite.pixelsPerUnit;
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
            obj.layer = LayerMask.NameToLayer("GUI");
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
            obj.AddComponent<CanvasGroup>();
            var worldSpaceCanvasScaler = obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
            var tooltip = obj.AddComponent<Tooltip>();
            
            var imageObj = new GameObject("Image");
            imageObj.layer = LayerMask.NameToLayer("GUI");
            imageObj.transform.SetParent(obj.transform, false);
            var imageRt = imageObj.AddComponent<RectTransform>();
            imageRt.anchorMin = Vector2.zero;
            imageRt.anchorMax = Vector2.one;
            imageRt.offsetMin = Vector2.zero;
            imageRt.offsetMax = Vector2.zero;
            var image = imageObj.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>("My/My/Sprites/tooltip");
            image.type = Image.Type.Sliced;
            if (imageObj.GetComponent<CanvasRenderer>() == null)
                imageObj.AddComponent<CanvasRenderer>();
            
            var textObj = new GameObject("Text");
            textObj.layer = LayerMask.NameToLayer("GUI");
            textObj.transform.SetParent(imageObj.transform, false);
            var textRt = textObj.AddComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = new Vector2(4, 4);
            textRt.offsetMax = new Vector2(-4, -4);
            var textTxt = textObj.AddComponent<TextMeshProUGUI>();
            textTxt.font = Resources.Load<TMP_FontAsset>("My/My/Fonts/tahoma08pt_raster_hinted");
            textTxt.text = text;
            textTxt.alignment = TextAlignmentOptions.Center;
            textTxt.color = Color.black;
            textTxt.enableAutoSizing = true;
            textTxt.fontSizeMin = ushort.MinValue;
            textTxt.fontSizeMax = ushort.MaxValue;
            textTxt.raycastTarget = false;
            
            return obj;
        }
        public static GameObject NewDamageNumbers(Vector2 pos, bool isGood, float damageAmount)
        {
            var obj = new GameObject("DamageNumbers");
            obj.layer = LayerMask.NameToLayer("GUI");
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "DamageNumbers";
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.position = pos;
            rectTransform.sizeDelta = new Vector2(30, 15);
            rectTransform.localScale = new Vector3(0, 0, 1);
            var canvasScaler = obj.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 30f;
            canvasScaler.referencePixelsPerUnit = 30f;
            var raycaster = obj.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            raycaster.blockingMask = -1;
            obj.AddComponent<CanvasGroup>();
            var worldSpaceCanvasScaler = obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
            var tooltip = obj.AddComponent<DamageNumbers>();
            
            var textObj = new GameObject("Text");
            textObj.layer = LayerMask.NameToLayer("GUI");
            textObj.transform.SetParent(obj.transform, false);
            var textRt = textObj.AddComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = Vector2.zero;
            textRt.offsetMax = Vector2.zero;
            var textTxt = textObj.AddComponent<TextMeshProUGUI>();
            textTxt.font = Resources.Load<TMP_FontAsset>("My/My/Fonts/tahoma08pt_raster_hinted");
            textTxt.alignment = TextAlignmentOptions.Center;
            if (isGood)
            {
                textTxt.text = "";
                textTxt.color = Color.white;
            }
            else
            {
                textTxt.text = "-";
                textTxt.color = Color.red;
            }
            textTxt.text += string.Format("{0:F1}", damageAmount);
            textTxt.enableAutoSizing = true;
            textTxt.fontSizeMin = ushort.MinValue;
            textTxt.fontSizeMax = ushort.MaxValue;
            textTxt.raycastTarget = false;
            
            return obj;
        }
        public static GameObject NewDamageNumbers(Vector2 pos, string text)
        {
            var obj = new GameObject("DamageNumbers");
            obj.layer = LayerMask.NameToLayer("GUI");
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "DamageNumbers";
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.position = pos;
            rectTransform.sizeDelta = new Vector2(80, 25);
            rectTransform.localScale = new Vector3(0, 0, 1);
            var canvasScaler = obj.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 30f;
            canvasScaler.referencePixelsPerUnit = 30f;
            var raycaster = obj.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            raycaster.blockingMask = -1;
            obj.AddComponent<CanvasGroup>();
            var worldSpaceCanvasScaler = obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
            var tooltip = obj.AddComponent<DamageNumbers>();
            
            var textObj = new GameObject("Text");
            textObj.layer = LayerMask.NameToLayer("GUI");
            textObj.transform.SetParent(obj.transform, false);
            var textRt = textObj.AddComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = Vector2.zero;
            textRt.offsetMax = Vector2.zero;
            var textTxt = textObj.AddComponent<TextMeshProUGUI>();
            textTxt.font = Resources.Load<TMP_FontAsset>("My/My/Fonts/tahoma08pt_raster_hinted");
            textTxt.alignment = TextAlignmentOptions.Center;
            textTxt.color = new Color32(0, 170, 0, 255);
            textTxt.text = text;
            textTxt.enableAutoSizing = true;
            textTxt.fontSizeMin = ushort.MinValue;
            textTxt.fontSizeMax = ushort.MaxValue;
            textTxt.raycastTarget = false;
            
            return obj;
        }
        public static GameObject NewSelection()
        {
            var obj = new GameObject("Selection");
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "Selection";
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(0, 0);
            rectTransform.localScale = new Vector3(0, 0, 1);
            var canvasScaler = obj.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 30f;
            canvasScaler.referencePixelsPerUnit = 30f;
            var raycaster = obj.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            raycaster.blockingMask = -1;
            var worldSpaceCanvasScaler = obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
            var script = obj.AddComponent<Selection>();
            
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
            if (imageObj.GetComponent<CanvasRenderer>() == null)
                imageObj.AddComponent<CanvasRenderer>();
            
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
            return obj;
        }
        public static GameObject NewBinButton(Vector2 pos)
        {
            var obj = CreateBaseButton<RecycleBinButton>(pos, "BinButton");
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
        public static GameObject NewBottle(Vector2 pos)
        {
            var bottleFullness = 0.25f;
            var w = 26f;
            var h = 60f;
            var obj = NewEntity<Bottle>(pos, "Bottle", "My/My/Sprites/bottle", w, h, 0 - w / 2f, 0 - h / 2f);

            Object.DestroyImmediate(obj.GetComponent<SpriteRenderer>());

            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "Bottles";
            obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();

            var rootRt = obj.GetComponent<RectTransform>();
            rootRt.sizeDelta = new Vector2(w, h);
            rootRt.localScale = new Vector3(0, 0, 1);

            var bottleSprite = Resources.Load<Sprite>("My/My/Sprites/bottle");

            var maskObj = new GameObject("MaskContainer");
            maskObj.transform.SetParent(obj.transform, false);
            var maskImg = maskObj.AddComponent<Image>();
            maskImg.sprite = bottleSprite;
            maskObj.AddComponent<Mask>().showMaskGraphic = false; 

            var maskRt = maskObj.GetComponent<RectTransform>();
            maskRt.anchorMin = Vector2.zero;
            maskRt.anchorMax = Vector2.one;
            maskRt.sizeDelta = Vector2.zero;

            var water = new GameObject("Background_Water");
            water.transform.SetParent(maskObj.transform, false);
            var waterImg = water.AddComponent<Image>();
            waterImg.color = new Color(0.5f, 0.25f, 0f, 0.8f);

            var waterRt = water.GetComponent<RectTransform>();
            waterRt.anchorMin = new Vector2(0, 0);
            waterRt.anchorMax = new Vector2(1, bottleFullness); 
            waterRt.sizeDelta = new Vector2(w * 10f, w);

            var foreground = new GameObject("Foreground");
            foreground.transform.SetParent(obj.transform, false);
            var fgImage = foreground.AddComponent<Image>();
            fgImage.sprite = bottleSprite;
            fgImage.color = new Color(1f, 1f, 1f, 0.75f);

            var fgRt = foreground.GetComponent<RectTransform>();
            fgRt.anchorMin = Vector2.zero;
            fgRt.anchorMax = Vector2.one;
            fgRt.sizeDelta = Vector2.zero;

            var box = obj.GetComponent<BoxCollider2D>();
            box.isTrigger = true;
            
            box.size = new Vector2(w, h);
            box.offset = Vector2.zero;

            var bottleScript = obj.GetComponent<Bottle>();
            bottleScript.waterRect = waterRt;

            return obj;
        }
        public static GameObject NewLayingPieceOfGlass(Vector2 pos)
        {
            var w = 11f;
            var h = 15f;
            var obj = NewEntity<LayingPieceOfGlass>(pos, "LayingPieceOfGlass", "My/My/Sprites/glass", w, h, 0 - w / 2f, 0 - h / 2f);
            obj.GetComponent<Rigidbody2D>().freezeRotation = false;
            
            Object.DestroyImmediate(obj.GetComponent<SpriteRenderer>());
            
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "Bottles";
            obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
            obj.AddComponent<CanvasGroup>();

            var rootRt = obj.GetComponent<RectTransform>();
            rootRt.sizeDelta = new Vector2(w, h);
            rootRt.localScale = new Vector3(0, 0, 1);

            var sprite = Resources.Load<Sprite>("My/My/Sprites/glass");

            var foreground = new GameObject("Foreground");
            foreground.transform.SetParent(obj.transform, false);
            var fgImage = foreground.AddComponent<Image>();
            fgImage.sprite = sprite;
            fgImage.color = new Color(1f, 1f, 1f, 0.75f);

            var fgRt = foreground.GetComponent<RectTransform>();
            fgRt.anchorMin = Vector2.zero;
            fgRt.anchorMax = Vector2.one;
            fgRt.sizeDelta = Vector2.zero;

            var box = obj.GetComponent<BoxCollider2D>();
            box.isTrigger = true;
            
            box.size = new Vector2(w, h);
            box.offset = Vector2.zero;

            return obj;
        }
        public static GameObject NewApple(Vector2 pos)
        {
            var w = 16f;
            var h = 24f;
            var obj = NewEntity<Apple>(pos, "Apple", "My/My/Sprites/apple", w, h, 0 - w / 2f, 0 - h / 2f);
            obj.GetComponent<Rigidbody2D>().freezeRotation = false;
            
            Object.DestroyImmediate(obj.GetComponent<SpriteRenderer>());
            
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "Bottles";
            obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
            obj.AddComponent<CanvasGroup>();

            var rootRt = obj.GetComponent<RectTransform>();
            rootRt.sizeDelta = new Vector2(w, h);
            rootRt.localScale = new Vector3(0, 0, 1);

            var sprite = Resources.Load<Sprite>("My/My/Sprites/apple");

            var foreground = new GameObject("Foreground");
            foreground.transform.SetParent(obj.transform, false);
            var fgImage = foreground.AddComponent<Image>();
            fgImage.sprite = sprite;
            fgImage.color = new Color(1f, 1f, 1f, 1f);

            var fgRt = foreground.GetComponent<RectTransform>();
            fgRt.anchorMin = Vector2.zero;
            fgRt.anchorMax = Vector2.one;
            fgRt.sizeDelta = Vector2.zero;

            var box = obj.GetComponent<BoxCollider2D>();
            box.isTrigger = true;
            
            box.size = new Vector2(w, h);
            box.offset = Vector2.zero;

            return obj;
        }
    }
}