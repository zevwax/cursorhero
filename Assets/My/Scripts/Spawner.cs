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
        public static void NewSoul(Vector2 pos)
        {
            GameObject obj = new GameObject("Soul");
            obj.transform.position = new Vector3(pos.x, pos.y, 0);
            var sr = obj.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Soul";
            sr.sprite = Resources.Load<Sprite>("My/WinXP/Cursor/default_arrow");
            obj.AddComponent<Soul>();
        }
        public static GameObject NewWhite(Vector2 pos) => NewEntity<White>(
            pos, "White Enemy", "pointer_1", "Enemies", true, "Enemy", false);
        public static GameObject NewYellow(Vector2 pos) => NewEntity<Yellow>(
            pos, "Yellow Enemy", "pointer_2", "Enemies", true, "Enemy", false);
        public static GameObject NewCyan(Vector2 pos) => NewEntity<Cyan>(
            pos, "Cyan Enemy", "pointer_3", "Enemies", true, "Enemy", false);
        public static void NewYellowP(Vector2 pos, Vector2 direction)
        {
            var obj = NewEntity<YellowP>(pos, "Yellow Projectile", "glass", "Projectiles", true, "MainCharacterProjectile", false);
            obj.GetComponent<CanvasGroup>().alpha = 0.75f;
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            obj.GetComponent<Projectile>().direction = direction;
        }
        public static void NewRedP(Vector2 pos, Vector2 direction)
        {
            var obj = NewEntity<RedP>(pos, "Red Projectile", "arrow", "Projectiles", true, "EnemyProjectile", false);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            obj.GetComponent<Projectile>().direction = direction;
        }
        public static void NewRingP(Vector2 pos, Vector2 direction)
        {
            var obj = NewEntity<RingP>(pos, "Ring Projectile", "arrow", "Projectiles", true, "EnemyProjectile", false);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            obj.GetComponent<Projectile>().direction = direction;
        }
        
        /*private static GameObject NewEntityLegacy<T>(Vector2 pos, string name, string spritePath, float w, float h, float left, float top) where T : MonoBehaviour
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
        }*/
        
        public static GameObject NewEntity<T>(Vector2 pos, string name, string spriteName, string sortingLayerName, bool collider, string objectLayer, bool isStatic) where T : MonoBehaviour
        {
            var obj = new GameObject(name);
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            if (sortingLayerName != null)
                canvas.sortingLayerName = sortingLayerName;
            var rectTransform = obj.GetComponent<RectTransform>();
            var sprite = Resources.Load<Sprite>(string.Format("My/My/Sprites/{0}", spriteName));
            rectTransform.position = new Vector3(pos.x, pos.y, 0);
            var size = new Vector2(sprite.rect.width, sprite.rect.height);
            rectTransform.sizeDelta = size;
            rectTransform.localScale = new Vector3(0, 0, 1);
            var canvasScaler = obj.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 30f;
            canvasScaler.referencePixelsPerUnit = 30f;
            var raycaster = obj.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            raycaster.blockingMask = -1;
            obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
            obj.AddComponent<T>();
            obj.AddComponent<CanvasGroup>();
            
            var imageObj = new GameObject("Image");
            imageObj.transform.SetParent(obj.transform, false);
            var imageRt = imageObj.AddComponent<RectTransform>();
            imageRt.anchorMin = Vector2.zero;
            imageRt.anchorMax = Vector2.one;
            imageRt.offsetMin = Vector2.zero;
            imageRt.offsetMax = Vector2.zero;
            var image = imageObj.AddComponent<Image>();
            image.sprite = sprite;
            if (imageObj.GetComponent<CanvasRenderer>() == null)
                imageObj.AddComponent<CanvasRenderer>();
            
            if (collider)
            {
                obj.layer = LayerMask.NameToLayer(objectLayer);
                var rb = obj.AddComponent<Rigidbody2D>();
                rb.sharedMaterial = CreateIceMaterial();
                rb.gravityScale = 0;
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (isStatic)
                    rb.bodyType = RigidbodyType2D.Static;
                var box = obj.AddComponent<BoxCollider2D>();
                box.size = size;
                box.offset = Vector2.zero;
            }
            
            return obj;
        }
        
        public static GameObject NewDisk(Vector2 pos)
        {
            var obj = NewEntity<Disk>(pos, "Disk", "disk", "RealDisks", true, "Disk", false);
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
            var obj = NewEntity<Selection>(Vector2.zero, "Selection", "s1", "Selection", false, "Default", false);
            obj.transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced;
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
        public static GameObject NewPlayButton(Vector2 pos) => CreateBaseButton<Play>(pos, "PlayButton", "btn_play");
        public static GameObject NewBinButton(Vector2 pos) => CreateBaseButton<RecycleBinButton>(pos, "BinButton", "bin");
        public static GameObject NewNetButton(Vector2 pos) => CreateBaseButton<BtnNet>(pos, "NetButton", "btn_net");
        public static GameObject NewEndlessModeButton(Vector2 pos) => CreateBaseButton<EndlessMode>(pos, "EndlessModeButton", "btn_endless_mode");
        public static GameObject NewUpgradeButton(string upgradeName, Vector2 pos)
        {
            GameObject obj;
            switch (upgradeName)
            {
                case "Damage": obj = CreateBaseButton<ProjectileDamage>(pos, "UpgradeDamage", "btn_upgrade_dmg"); break;
                case "FireRate": obj = CreateBaseButton<Firerate>(pos, "UpgradeFireRate", "btn_upgrade_cdn"); break;
                case "Speed": obj = CreateBaseButton<ProjectileSpeed>(pos, "UpgradeSpeed", "btn_upgrade_spd"); break;
                case "Sensitivity": obj = CreateBaseButton<Sensitivity>(pos, "UpgradeSensitivity", "btn_upgrade_sen"); break;
                case "Heart": obj = CreateBaseButton<Heart>(pos, "UpgradeHeart", "btn_upgrade_hrt"); break;
                default: throw new System.NotImplementedException();
            }
            return obj;
        }
        private static GameObject CreateBaseButton<T>(Vector2 pos, string name, string spriteName) where T : Button
        {
            var obj = NewEntity<T>(pos, name, "btn", "ButtonsFG", true, "Button", false);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            
            var imageObj = new GameObject("Icon Image");
            imageObj.transform.SetParent(obj.transform, false);
            var imageRt = imageObj.AddComponent<RectTransform>();
            imageRt.anchorMin = Vector2.zero;
            imageRt.anchorMax = Vector2.one;
            imageRt.offsetMin = new Vector2(6f, 13f);
            imageRt.offsetMax = new Vector2(-9f, -8f);
            var image = imageObj.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>(string.Format("My/My/Sprites/{0}", spriteName));
            if (imageObj.GetComponent<CanvasRenderer>() == null)
                imageObj.AddComponent<CanvasRenderer>();
            
            return obj;
        }
        public static GameObject NewBottle(Vector2 pos)
        {
            var bottleFullness = 0.25f;
            var obj = NewEntity<Bottle>(pos, "Bottle", "bottle", "Bottles", true, "Default", false);

            var maskObj = obj.transform.GetChild(0).gameObject;
            maskObj.AddComponent<Mask>().showMaskGraphic = false;
            
            var waterImageObj = new GameObject("Water");
            waterImageObj.transform.SetParent(maskObj.transform, false);
            var waterImageRt = waterImageObj.AddComponent<RectTransform>();
            waterImageRt.anchorMin = Vector2.zero;
            waterImageRt.anchorMax = new Vector2(1, bottleFullness);
            waterImageRt.offsetMin = Vector2.zero;
            waterImageRt.offsetMax = Vector2.zero;
            waterImageRt.sizeDelta = new Vector2(260f, 26f);
            var waterImage = waterImageObj.AddComponent<Image>();
            if (waterImageObj.GetComponent<CanvasRenderer>() == null)
                waterImageObj.AddComponent<CanvasRenderer>();
            waterImage.color = new Color(0.5f, 0.25f, 0f, 0.8f);
            
            var glassImageObj = new GameObject("Glass");
            glassImageObj.transform.SetParent(obj.transform, false);
            var glassImageRt = glassImageObj.AddComponent<RectTransform>();
            glassImageRt.anchorMin = Vector2.zero;
            glassImageRt.anchorMax = Vector2.one;
            glassImageRt.offsetMin = Vector2.zero;
            glassImageRt.offsetMax = Vector2.zero;
            glassImageRt.sizeDelta = Vector2.zero;
            var glassImage = glassImageObj.AddComponent<Image>();
            if (glassImageObj.GetComponent<CanvasRenderer>() == null)
                glassImageObj.AddComponent<CanvasRenderer>();
            glassImage.sprite = maskObj.GetComponent<Image>().sprite;
            glassImage.color = new Color(1f, 1f, 1f, 0.75f);
            
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            var bottleScript = obj.GetComponent<Bottle>();
            bottleScript.waterRect = waterImageRt;
            return obj;
        }
        public static GameObject NewLayingPieceOfGlass(Vector2 pos)
        {
            var obj = NewEntity<LayingPieceOfGlass>(pos, "LayingPieceOfGlass", "glass", "Bottles", true, "Default", false);
            obj.GetComponent<CanvasGroup>().alpha = 0.75f;
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            return obj;
        }
        public static GameObject NewApple(Vector2 pos)
        {
            var obj = NewEntity<Apple>(pos, "Apple", "apple", "Bottles", true, "Default", false);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            return obj;
        }
    }
}