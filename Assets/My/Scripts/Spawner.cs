using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

        public static GameObject NewHeartKeeper(Vector2 pos)
        {
            var obj = NewEntity<HeartKeeper>(
                Vector2.zero, "Heart Keeper", "idle", "Selection", false,
                "Default", false);

            obj.AddComponent<Follower>();
            
            var imageObj2 = new GameObject("Layer 2");
            imageObj2.transform.SetParent(obj.transform, false);
            var imageRt2 = imageObj2.AddComponent<RectTransform>();
            imageRt2.anchorMin = Vector2.zero;
            imageRt2.anchorMax = Vector2.one;
            imageRt2.offsetMin = Vector2.zero;
            imageRt2.offsetMax = Vector2.zero;
            var image2 = imageObj2.AddComponent<Image>();
            image2.sprite = Resources.Load<Sprite>("My/My/Sprites/idle");
            if (imageObj2.GetComponent<CanvasRenderer>() == null)
                imageObj2.AddComponent<CanvasRenderer>();
            
            var imageObj3 = new GameObject("Layer 3");
            imageObj3.transform.SetParent(obj.transform, false);
            var imageRt3 = imageObj3.AddComponent<RectTransform>();
            imageRt3.anchorMin = Vector2.zero;
            imageRt3.anchorMax = Vector2.one;
            imageRt3.offsetMin = Vector2.zero;
            imageRt3.offsetMax = Vector2.zero;
            var image3 = imageObj3.AddComponent<Image>();
            image3.sprite = Resources.Load<Sprite>("My/My/Sprites/heart");
            if (imageObj3.GetComponent<CanvasRenderer>() == null)
                imageObj3.AddComponent<CanvasRenderer>();

            imageObj3.AddComponent<BeatingHeart>();
            
            var imageObj4 = new GameObject("Layer 4");
            imageObj4.transform.SetParent(obj.transform, false);
            var imageRt4 = imageObj4.AddComponent<RectTransform>();
            imageRt4.anchorMin = Vector2.zero;
            imageRt4.anchorMax = Vector2.one;
            imageRt4.offsetMin = Vector2.zero;
            imageRt4.offsetMax = Vector2.zero;
            var image4 = imageObj4.AddComponent<Image>();
            image4.sprite = Resources.Load<Sprite>("My/My/Sprites/idle");
            if (imageObj4.GetComponent<CanvasRenderer>() == null)
                imageObj4.AddComponent<CanvasRenderer>();
            
            obj.GetComponent<HeartKeeper>().Init();
            return obj;
        }
        public static GameObject NewTabbyPointer(Vector2 pos)
        {
            var obj = NewEntity<TabbyPointer>(
                Vector2.zero, "Tabby Pointer", "tabby_idle", "Selection", false,
                "Default", false);
            obj.GetComponent<TabbyPointer>().Init(new Vector3(pos.x, pos.y, 0));
            return obj;
        }
        public static GameObject NewTabbySelection()
        {
            var obj = NewEntity<TabbySelection>(Vector2.zero, "Tabby Selection", "s1", "Selection", false, "Default", false);
            var image = obj.transform.GetChild(0).GetComponent<Image>();
            image.type = Image.Type.Sliced;
            image.color = new Color(1f, 0.8f, 0.8f, 1f);
            return obj;
        }
        public static void NewMapLine(Vector2 pos, Sprite sprite)
        {
            var obj = NewEntity<MapLine>(
                pos, "Map Line", sprite, "MapLine", false, "GUI", false);
            obj.GetComponent<MapLine>().Init();
        }
        public static void NewMapNode(Vector2 pos, Sprite sprite)
        {
            var obj = NewEntity<MapNode>(
                pos, "Map Node", sprite, "MapNode", true, "GUI", false);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            obj.GetComponent<MapNode>().Init();
        }
        public static void NewMapChar(Vector2 pos, Sprite sprite)
        {
            var obj = NewEntity<MapChar>(
                pos, "Map Char", sprite, "MapChar", true, "GUI", false);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            obj.GetComponent<MapChar>().Init();
        }
        public static void NewSoul(Vector2 pos)
        {
            GameObject obj = new GameObject("Soul");
            obj.transform.position = new Vector3(pos.x, pos.y, 0);
            var sr = obj.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Soul";
            sr.sprite = Resources.Load<Sprite>("My/My/Sprites/idle");
            obj.AddComponent<Soul>();
        }
        public static GameObject NewWhite(Vector2 pos) => NewEntity<EnemyHand>(
            pos, "White Enemy", "pointer_1", "Enemies", true, "Enemy", false);
        public static GameObject NewYellow(Vector2 pos) => NewEntity<EnemyBlackGlove>(
            pos, "Yellow Enemy", "pointer_2", "Enemies", true, "Enemy", false);
        public static GameObject NewCyan(Vector2 pos) => NewEntity<EnemyGoat>(
            pos, "Cyan Enemy", "pointer_3", "Enemies", true, "Enemy", false);

        public static void NewProjectileBlue(Vector2 pos, Vector2 direction, Gun g)
        {
            var obj = CreateBaseProjectile<ProjectileBlue>(pos, direction, "Blue Projectile", "MainCharacterProjectile", "MainCharacterProjectiles");
            obj.GetComponent<Projectile>().SetTeam(true);
            obj.GetComponent<Projectile>().SetWeight(g.Weight);
            obj.GetComponent<Projectile>().SetSize(g.Size);
        }

        public static void NewProjectileRedRegular(Vector2 pos, Vector2 direction, Gun g)
        {
            var obj = CreateBaseProjectile<ProjectileRedRegular>(pos, direction, "Red Projectile", "EnemyProjectile", "EnemyProjectiles");
            obj.GetComponent<Projectile>().SetTeam(false);
            obj.GetComponent<Projectile>().SetWeight(g.Weight);
            obj.GetComponent<Projectile>().SetSize(g.Size);
        }

        public static void NewProjectileRedLarge(Vector2 pos, Vector2 direction, Gun g)
        {
            var obj = CreateBaseProjectile<ProjectileRedLarge>(pos, direction, "Ring Projectile", "EnemyProjectile", "EnemyProjectiles");
            obj.GetComponent<Projectile>().SetTeam(false);
            obj.GetComponent<Projectile>().SetWeight(g.Weight);
            obj.GetComponent<Projectile>().SetSize(g.Size);
        }
        private static GameObject CreateBaseProjectile<T>(Vector2 pos, Vector2 direction, string name, string objLayer, string sortLayer) where T : Projectile
        {
            var obj = NewEntity<T>(pos, name, "SizeHolders/projectile", sortLayer, true, objLayer, false);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            obj.GetComponent<Projectile>().direction = direction;
            
            NewText(obj.transform, objLayer, TextAlignmentOptions.BottomLeft, new Color(1f, 0.25f, 0.25f, 1f));
            NewText(obj.transform, objLayer, TextAlignmentOptions.MidlineLeft, new Color(1f, 0.25f, 0.25f, 1f));
            NewText(obj.transform, objLayer, TextAlignmentOptions.TopLeft, new Color(1f, 0.25f, 0.25f, 1f));
            NewText(obj.transform, objLayer, TextAlignmentOptions.Bottom, new Color(1f, 0.25f, 0.25f, 1f));
            NewText(obj.transform, objLayer, TextAlignmentOptions.Top, new Color(1f, 0.25f, 0.25f, 1f));
            NewText(obj.transform, objLayer, TextAlignmentOptions.BottomRight, new Color(1f, 0.25f, 0.25f, 1f));
            NewText(obj.transform, objLayer, TextAlignmentOptions.MidlineRight, new Color(1f, 0.25f, 0.25f, 1f));
            NewText(obj.transform, objLayer, TextAlignmentOptions.TopRight, new Color(1f, 0.25f, 0.25f, 1f));
            NewText(obj.transform, objLayer, TextAlignmentOptions.Center, new Color(1f, 1f, 1f, 1f));
            
            return obj;
        }

        private static GameObject NewText(Transform parent, string objLayer, TextAlignmentOptions alignment, Color color)
        {
            var textObj = new GameObject("Text");
            textObj.layer = LayerMask.NameToLayer(objLayer);
            textObj.transform.SetParent(parent, false);
            var textRt = textObj.AddComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = Vector2.zero;
            textRt.offsetMax = Vector2.zero;
            var textTxt = textObj.AddComponent<TextMeshProUGUI>();
            var fontAsset = Resources.Load<TMP_FontAsset>("My/My/Fonts/pxp_glyphs_raster_hinted");
            textTxt.font = fontAsset;
            textTxt.text = "A";
            textTxt.alignment = alignment;
            textTxt.color = color;
            textTxt.enableAutoSizing = true;
            textTxt.fontSizeMin = ushort.MinValue;
            textTxt.fontSizeMax = ushort.MaxValue;
            textTxt.raycastTarget = false;
            return textObj;
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
        
        public static GameObject NewEntity<T>(Vector2 pos, string name, Sprite sprite, string sortingLayerName, bool collider, string objectLayer, bool isStatic) where T : MonoBehaviour
        {
            var obj = new GameObject(name);
            obj.layer = LayerMask.NameToLayer(objectLayer);
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            if (sortingLayerName != null)
                canvas.sortingLayerName = sortingLayerName;
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.position = new Vector3(pos.x, pos.y, 0);
            var size = new Vector2(sprite.rect.width, sprite.rect.height);
            rectTransform.sizeDelta = size;
            rectTransform.localScale = new Vector3(0, 0, 1);
            var canvasScaler = obj.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 30f;
            var ppu = sprite.pixelsPerUnit;
            canvasScaler.referencePixelsPerUnit = ppu;
            var raycaster = obj.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            raycaster.blockingMask = -1;
            var WSCScaler = obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
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
        public static GameObject NewEntity<T>(Vector2 pos, string name, string spriteName, string sortingLayerName, bool collider, string objectLayer, bool isStatic) where T : MonoBehaviour
        {
            var obj = new GameObject(name);
            obj.layer = LayerMask.NameToLayer(objectLayer);
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
            var ppu = sprite.pixelsPerUnit;
            canvasScaler.referencePixelsPerUnit = ppu;
            var raycaster = obj.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            raycaster.blockingMask = -1;
            var WSCScaler = obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
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
            var obj = NewEntity<Disc>(pos, "Disk", "disc15", "RealDisks", true, "Disk", false);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            return obj;
        }

        public static GameObject NewRedArrow(Vector2 pos, Vector2 direction)
        {
            var obj = NewEntity<RedArrow>(
                pos, "Red Arrow", "red_arrow", "RedArrow", false, "GUI", false);
            obj.GetComponent<RedArrow>().direction = direction;
            return obj;
        }
        public static GameObject NewFallingDisk(Vector2 pos)
        {
            var isLarge = (bool)default;
            if (Random.Range(0, 2) == 0)
                isLarge = false;
            else
                isLarge = true;
            
            var obj = new GameObject("FallingDisk");
            var canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "FallingDisks";
            var rectTransform = obj.GetComponent<RectTransform>();
            if (isLarge)
                rectTransform.sizeDelta = new Vector2(26, 26);
            else
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
            if (isLarge)
                image.sprite = Resources.Load<Sprite>("My/My/Sprites/disc26");
            else
                image.sprite = Resources.Load<Sprite>("My/My/Sprites/disc15");
            if (imageObj.GetComponent<CanvasRenderer>() == null)
                imageObj.AddComponent<CanvasRenderer>();
            
            return obj;
        }
        public static GameObject NewYellowCirc()
        {
            var obj = NewEntity<YellowCirc>(Vector2.zero, "Yellow Circle", "yellow_circ", "YellowCirc", false, "Default", false);
            obj.GetComponent<CanvasGroup>().alpha = 1/3f;
            obj.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 0, 1);
            
            return obj;
        }
        public static GameObject NewShield()
        {
            var obj = NewEntity<YellowCirc>(Vector2.zero, "Shield", "shield32", "Shield", true, "Shield", false);
            obj.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 1, 1);
            
            var imageObj = new GameObject("Fill");
            imageObj.transform.SetParent(obj.transform, false);
            var imageRt = imageObj.AddComponent<RectTransform>();
            imageRt.anchorMin = Vector2.zero;
            imageRt.anchorMax = Vector2.one;
            imageRt.offsetMin = Vector2.zero;
            imageRt.offsetMax = Vector2.zero;
            var image = imageObj.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>("My/My/Sprites/shield30");
            if (imageObj.GetComponent<CanvasRenderer>() == null)
                imageObj.AddComponent<CanvasRenderer>();
            
            return obj;
        }
        public static GameObject NewGlitch()
        {
            var xMax = 8f;
            var yMax = 4.5f;
            var x = Random.Range(-xMax, xMax);
            var y = Random.Range(-yMax, yMax);
            var color = (Color)default;
            switch (Random.Range(0, 6+1))
            {
                case 0:
                    color = Color.red;
                    break;
                case 1:
                    color = Color.green;
                    break;
                case 2:
                    color = Color.blue;
                    break;
                case 3:
                    color = Color.magenta;
                    break;
                case 4:
                    color = Color.yellow;
                    break;
                case 5:
                    color = Color.cyan;
                    break;
                case 6:
                    color = Color.white;
                    break;
            }
            var spriteName = "g" + Convert.ToString(Random.Range(1, 3+1));
            var obj = NewEntity<Glitch>(new Vector2(x, y), "Glitch", spriteName, "Glitch", false, "GUI", false);
            obj.transform.GetChild(0).GetComponent<Image>().color = color;
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
            var obj = NewEntity<MainCharacterSelection>(Vector2.zero, "Selection", "s1", "Selection", false, "Default", false);
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
            rectTransform.sizeDelta = new Vector2(17, 3);
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
        public static GameObject NewPlayButton(Vector2 pos) => CreateBaseButton<ButtonPlay>(pos, "PlayButton", "btn_play");
        public static GameObject NewBinButton(Vector2 pos) => CreateBaseButton<ButtonRecycleBin>(pos, "BinButton", "bin");
        public static GameObject NewNetButton(Vector2 pos) => CreateBaseButton<ButtonSwitchPC>(pos, "NetButton", "btn_net");
        public static GameObject NewEndlessModeButton(Vector2 pos) => CreateBaseButton<ButtonEndlessMode>(pos, "EndlessModeButton", "btn_endless_mode");
        public static GameObject NewUpgradeButton(string upgradeName, Vector2 pos)
        {
            GameObject obj;
            switch (upgradeName)
            {
                case "Damage": obj = CreateBaseButton<ProjectileDamage>(pos, "UpgradeDamage", "btn_upgrade_dmg"); break;
                case "FireRate": obj = CreateBaseButton<UpgradeFirerate>(pos, "UpgradeFireRate", "btn_upgrade_cdn"); break;
                case "Speed": obj = CreateBaseButton<UpgradeProjectileSpeed>(pos, "UpgradeSpeed", "btn_upgrade_spd"); break;
                case "Sensitivity": obj = CreateBaseButton<UpgradeSensitivity>(pos, "UpgradeSensitivity", "btn_upgrade_sen"); break;
                case "Heart": obj = CreateBaseButton<UpgradeHeart>(pos, "UpgradeHeart", "btn_upgrade_hrt"); break;
                case "HeartKeeper": obj = CreateBaseButton<UpgradeHeartKeeper>(pos, "UpgradeHeartKeeper", "btn_upgrade_mxh"); break;
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
            imageRt.offsetMin = Vector2.zero;
            imageRt.offsetMax = Vector2.zero;
            var image = imageObj.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>(string.Format("My/My/Sprites/{0}", spriteName));
            if (imageObj.GetComponent<CanvasRenderer>() == null)
                imageObj.AddComponent<CanvasRenderer>();
            
            return obj;
        }
        public static GameObject NewBottle(Vector2 pos)
        {
            var bottleFullness = 0.25f;
            var obj = NewEntity<ItemBottle>(pos, "Bottle", "bottle", "Bottles", true, "Default", false);

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
            var bottleScript = obj.GetComponent<ItemBottle>();
            bottleScript.waterRect = waterImageRt;
            
            var tooltipHolder = obj.AddComponent<TooltipHolder>();
            var dragable = obj.AddComponent<Dragable>();
            var bottle = obj.GetComponent<ItemBottle>();
            tooltipHolder.Init();
            dragable.Init();
            bottle.Init();
            
            return obj;
        }
        public static GameObject NewLayingPieceOfGlass(Vector2 pos)
        {
            var obj = NewEntity<ItemLayingPieceOfGlass>(pos, "LayingPieceOfGlass", "glass", "Bottles", true, "Default", false);
            obj.GetComponent<CanvasGroup>().alpha = 0.75f;
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            return obj;
        }
        public static GameObject NewTabby()
        {
            var obj = NewEntity<Tabby>(Vector2.zero, "Tabby", "tabby", "Bottles", false, "Default", false);
            
            var imageObj = new GameObject("Tabby Textbox");
            imageObj.transform.SetParent(obj.transform, false);
            //
            var imageRt = imageObj.AddComponent<RectTransform>();
            imageRt.anchorMin = new Vector2(0.5f, 1f);
            imageRt.anchorMax = new Vector2(0.5f, 1f);
            imageRt.pivot = new Vector2(0.5f, 0f);
            imageRt.anchoredPosition3D = new Vector3(-60f, 0f, 0f);
            imageRt.sizeDelta = new Vector2(130f, 21f);
            imageRt.localScale = Vector3.one;
            imageRt.offsetMin = Vector2.zero;
            imageRt.offsetMax = Vector2.zero;
            //
            var image = imageObj.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>("My/My/Sprites/tabby_textbox");
            image.type = Image.Type.Sliced;
            if (imageObj.GetComponent<CanvasRenderer>() == null)
                imageObj.AddComponent<CanvasRenderer>();
            
            //==========
            
            var textObj = new GameObject("Text");
            textObj.layer = LayerMask.NameToLayer("GUI");
            textObj.transform.SetParent(imageObj.transform, false);
            //
            var textRt = textObj.AddComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = new Vector2(8, 24);
            textRt.offsetMax = new Vector2(-8, -8);
            //
            var textTxt = textObj.AddComponent<TextMeshProUGUI>();
            textTxt.font = Resources.Load<TMP_FontAsset>("My/My/Fonts/tahoma08pt_raster_hinted");
            textTxt.text = "Default text";
            textTxt.alignment = TextAlignmentOptions.Left;
            textTxt.color = Color.black;
            textTxt.enableAutoSizing = true;
            textTxt.fontSizeMin = ushort.MinValue;
            textTxt.fontSizeMax = ushort.MaxValue;
            textTxt.raycastTarget = false;
            
            obj.GetComponent<Tabby>().Init();
            
            return obj;
        }
        public static GameObject NewKey(Vector2 pos)
        {
            var obj = NewEntity<ItemKey>(pos, "Key", "key", "Bottles", true, "Default", false);
            var tooltipHolder = obj.AddComponent<TooltipHolder>();
            var dragable = obj.AddComponent<Dragable>();
            var key = obj.GetComponent<ItemKey>();
            tooltipHolder.Init();
            dragable.Init();
            key.Init();
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            return obj;
        }
        public static GameObject NewApple(Vector2 pos)
        {
            var obj = NewEntity<ItemApple>(pos, "Apple", "apple", "Bottles", true, "Default", false);
            var tooltipHolder = obj.AddComponent<TooltipHolder>();
            var dragable = obj.AddComponent<Dragable>();
            var apple = obj.GetComponent<ItemApple>();
            tooltipHolder.Init();
            dragable.Init();
            apple.Init();
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            return obj;
        }
    }
}