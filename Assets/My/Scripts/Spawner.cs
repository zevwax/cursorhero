using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
                pos, "Heart Keeper", "idle", "HeartKeeperBG", true,
                "HeartKeeper");
            var follower = obj.AddComponent<Follower>();
            NewImage(obj.transform, "idle");
            var imageObj3 = NewImage(obj.transform, "SizeHolders/17x22");
            imageObj3.AddComponent<BeatingHeart>();
            var anim = imageObj3.AddComponent<Animator>();
            anim.Init("heart", 1f);
            NewImage(obj.transform, "idle");
            obj.GetComponent<HeartKeeper>().Init();
            follower.Init();
            return obj;
        }
        public static GameObject NewTabbyPointer(Vector2 pos)
        {
            var obj = NewEntity<TabbyPointer>(
                Vector2.zero, "Tabby Pointer", "tabby_idle", "Selection", false,
                "Default");
            obj.GetComponent<TabbyPointer>().Init(new Vector3(pos.x, pos.y, 0));
            return obj;
        }
        public static GameObject NewHumBar()
        {
            var objLayer = "DitheringCameraOutput";
            var obj = NewEntity<HumBar>(
                new Vector2(0f, 4.5f + (1f / 6f)), "Hum Bar", "SizeHolders/480x10", "HumBar", false,
                objLayer, true);
            var child0 = NewImage(obj.transform, "480x10");
            child0.transform.SetAsFirstSibling();
            var child1 = obj.transform.GetChild(1);
            child1.AddComponent<SkewEffect>();
            obj.GetComponent<HumBar>().Init();
            return obj;
        }
        public static GameObject NewVersionIndicator()
        {
            var objLayer = "GUI";
            var obj = NewEntity<Plug>(
                new Vector2(0, 4), "Version Indicator", "SizeHolders/480x30", "Selection", false,
                objLayer);
            var txt = NewText(obj.transform, objLayer, TextAlignmentOptions.Right, new Color(.5f, .5f, .5f, 1f),
                "tahoma_8px_raster_hinted", "v.1");
            txt.AddComponent<VersionIndicator>().Init();
            return obj;
        }
        public static GameObject NewTabbySelection()
        {
            var obj = NewEntity<TabbySelection>(Vector2.zero, "Tabby Selection", "s1", "Selection", false, "Default");
            var image = obj.transform.GetChild(0).GetComponent<Image>();
            image.type = Image.Type.Sliced;
            image.color = new Color(1f, 0.8f, 0.8f, 1f);
            return obj;
        }
        public static void NewMapLine(Vector2 pos, Sprite sprite)
        {
            var obj = NewEntity<MapLine>(
                pos, "Map Line", sprite, "MapLine", false, "GUI");
            obj.GetComponent<MapLine>().Init();
        }
        public static void NewMapNode(Vector2 pos, Sprite sprite)
        {
            var obj = NewEntity<MapNode>(
                pos, "Map Node", sprite, "MapNode", true, "GUI");
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            obj.GetComponent<MapNode>().Init();
        }
        public static void NewMapChar(Vector2 pos, Sprite sprite)
        {
            var obj = NewEntity<MapChar>(
                pos, "Map Char", sprite, "MapChar", true, "GUI");
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
        public static GameObject NewEnemyGrabber(Vector2 pos) => NewEntity<EnemyGrabber>(
            pos, "Enemy", "pointer_0", "Enemies", true, "Enemy");
        public static GameObject NewEnemyPointer(Vector2 pos) => NewEntity<EnemyPointer>(
            pos, "Enemy", "pointer_1", "Enemies", true, "Enemy");
        public static GameObject NewEnemyGlove(Vector2 pos) => NewEntity<EnemyBlackGlove>(
            pos, "Enemy", "pointer_2", "Enemies", true, "Enemy");
        public static GameObject NewEnemyGoat(Vector2 pos) => NewEntity<EnemyGoat>(
            pos, "Enemy", "pointer_3", "Enemies", true, "Enemy");
        public static GameObject NewEnemyBoss(Vector2 pos)
        {
            var obj = NewEntity<EnemyBoss>(pos, "Enemy", "enemy_fuck", "Enemies", true, "Enemy", RigidbodyType2D.Kinematic);
            return obj;
        }
        public static GameObject NewDriverTweenEntity()
        {
            var pos = MainCharacter.Instance.transform.position;
            var sprite = GetSprite("text_icons", "text_icons_1");
            var obj = NewEntity<TweenEntity>(
                pos, "Entity", sprite, "Enemies", false, "GUI");
            obj.GetComponent<TweenEntity>().Init(new Vector2(3.5f, -4), 2.5f);
            return obj;
        }
        public static GameObject NewFloppyDiskTweenEntity()
        {
            var pos = MainCharacter.Instance.transform.position;
            var sprite = GetSprite("spinning_floppy_disk", "spinning_floppy_disk_0");
            var obj = NewEntity<TweenEntity>(
                pos, "Entity", sprite, "HeartKeeperFG", false, "GUI");
            obj.GetComponent<TweenEntity>().Init(new Vector2(0, 4), 1.2f);
            return obj;
        }
        public static GameObject NewRover()
        {
            var pos = Vector3.zero;
            var sprite = GetSprite("rover", "rover_l0");
            var obj = NewEntity<Rover>(
                pos, "Entity", sprite, "Enemies", true, "Rover");
            obj.GetComponent<Rover>().Init();
            return obj;
        }
        public static Sprite GetSprite(string spriteSheetName, string spriteName)
        {
            var path = "My/My/Sprites/" + spriteSheetName;
            var allSprites = Resources.LoadAll<Sprite>(path);
            var sprite = allSprites.FirstOrDefault(s => s.name == spriteName);
            return sprite;
        }
        public static GameObject NewProjectile(Vector2 pos, Vector2 direction, Glyph glyph, bool isDragable = true)
        {
            var obj = CreateBaseProjectile<Projectile>(pos, direction, "Projectile", "MainCharacterProjectile", "MainCharacterProjectiles");
            var tooltipHolder = obj.AddComponent<TooltipHolder>();
            var dragable = (Dragable)default;
            if (isDragable)
                dragable = obj.AddComponent<Dragable>();
            tooltipHolder.Init();
            if (isDragable)
                dragable.Init();
            tooltipHolder.enabled = false;
            if (isDragable)
                dragable.enabled = false;
            obj.GetComponent<Projectile>().Init(glyph);
            return obj;
        }
        /*public static void NewProjectileBlue(Vector2 pos, Vector2 direction, Gun g)
        {
            var obj = CreateBaseProjectile<ProjectileBlue>(pos, direction, "Blue Projectile", "MainCharacterProjectile", "MainCharacterProjectiles");
            obj.GetComponent<Projectile>().SetTeam(true);
            obj.GetComponent<Projectile>().SetWeight(g.ProjectileWeight);
            obj.GetComponent<Projectile>().SetSize(g.ProjectileSize);
        }
        public static void NewProjectileRedRegular(Vector2 pos, Vector2 direction, Gun g)
        {
            var obj = CreateBaseProjectile<ProjectileRedRegular>(pos, direction, "Red Projectile", "EnemyProjectile", "EnemyProjectiles");
            obj.GetComponent<Projectile>().SetTeam(false);
            obj.GetComponent<Projectile>().SetWeight(g.ProjectileWeight);
            obj.GetComponent<Projectile>().SetSize(g.ProjectileSize);
        }
        public static void NewProjectileRedLarge(Vector2 pos, Vector2 direction, Gun g)
        {
            var obj = CreateBaseProjectile<ProjectileRedLarge>(pos, direction, "Ring Projectile", "EnemyProjectile", "EnemyProjectiles");
            obj.GetComponent<Projectile>().SetTeam(false);
            obj.GetComponent<Projectile>().SetWeight(g.ProjectileWeight);
            obj.GetComponent<Projectile>().SetSize(g.ProjectileSize);
        }*/
        private static GameObject CreateBaseProjectile<T>(Vector2 pos, Vector2 direction, string name, string objLayer, string sortLayer) where T : Projectile
        {
            var obj = NewEntity<T>(pos, name, "SizeHolders/projectile", sortLayer, true, objLayer);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            obj.GetComponent<Projectile>().direction = direction;
            
            NewGlyphText(obj.transform, objLayer, TextAlignmentOptions.BottomLeft, new Color(1f, 0.25f, 0.25f, 1f));
            NewGlyphText(obj.transform, objLayer, TextAlignmentOptions.MidlineLeft, new Color(1f, 0.25f, 0.25f, 1f));
            NewGlyphText(obj.transform, objLayer, TextAlignmentOptions.TopLeft, new Color(1f, 0.25f, 0.25f, 1f));
            NewGlyphText(obj.transform, objLayer, TextAlignmentOptions.Bottom, new Color(1f, 0.25f, 0.25f, 1f));
            NewGlyphText(obj.transform, objLayer, TextAlignmentOptions.Top, new Color(1f, 0.25f, 0.25f, 1f));
            NewGlyphText(obj.transform, objLayer, TextAlignmentOptions.BottomRight, new Color(1f, 0.25f, 0.25f, 1f));
            NewGlyphText(obj.transform, objLayer, TextAlignmentOptions.MidlineRight, new Color(1f, 0.25f, 0.25f, 1f));
            NewGlyphText(obj.transform, objLayer, TextAlignmentOptions.TopRight, new Color(1f, 0.25f, 0.25f, 1f));
            NewGlyphText(obj.transform, objLayer, TextAlignmentOptions.Center, new Color(1f, 1f, 1f, 1f));
            
            return obj;
        }
        private static GameObject NewImage(Transform parent, string spriteName)
        {
            var sprite = Resources.Load<Sprite>("My/My/Sprites/" + spriteName);
            return NewImage(parent, sprite);
        }
        private static GameObject NewImage(Transform parent, Sprite sprite)
        {
            var obj = new GameObject("Image");
            obj.transform.SetParent(parent, false);
            var rt = obj.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            var image = obj.AddComponent<Image>();
            image.sprite = sprite;
            if (obj.GetComponent<CanvasRenderer>() == null)
                obj.AddComponent<CanvasRenderer>();
            return obj;
        }
        private static GameObject NewGlyphText(Transform parent, string objLayer, TextAlignmentOptions alignment, Color color)
        {
            return NewText(parent, objLayer, alignment, color, "pxp_glyphs_raster_hinted", "A");
        }
        private static GameObject NewText(Transform parent, string objLayer, TextAlignmentOptions alignment, Color color, string fontName, string contents)
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
            var fontAsset = Resources.Load<TMP_FontAsset>("My/My/Fonts/" + fontName);
            textTxt.font = fontAsset;
            textTxt.text = contents;
            textTxt.alignment = alignment;
            textTxt.color = color;
            textTxt.enableAutoSizing = true;
            textTxt.fontSizeMin = ushort.MinValue;
            textTxt.fontSizeMax = ushort.MaxValue;
            textTxt.raycastTarget = false;
            return textObj;
        }
        private const RigidbodyType2D RbTypeByDefault = RigidbodyType2D.Dynamic;
        public static GameObject NewEntity<T>(
            Vector2 pos, string name, Sprite sprite, string sortingLayerName,
            bool collider, string objectLayer, RigidbodyType2D rbType, bool isRaw = false
            ) where T : MonoBehaviour
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
            if (isRaw)
                imageObj.AddComponent<RawImage>();
            else
            {
                var image = imageObj.AddComponent<Image>();
                image.sprite = sprite;
            }
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
                rb.bodyType = rbType;
                var box = obj.AddComponent<BoxCollider2D>();
                box.size = size;
                box.offset = Vector2.zero;
            }
            
            return obj;
        }
        public static GameObject NewEntity<T>(
            Vector2 pos, string name, Sprite sprite, string sortingLayerName,
            bool collider, string objectLayer, bool isRaw = false
            ) where T : MonoBehaviour => NewEntity<T>(pos, name, sprite, sortingLayerName, collider, objectLayer, RbTypeByDefault, isRaw);
        public static GameObject NewEntity<T>(
            Vector2 pos, string name, string spriteName, string sortingLayerName,
            bool collider, string objectLayer, RigidbodyType2D rbType, bool isRaw = false
        ) where T : MonoBehaviour
        {
            var sprite = Resources.Load<Sprite>($"My/My/Sprites/{spriteName}");
            return NewEntity<T>(pos, name, sprite, sortingLayerName, collider, objectLayer, rbType, isRaw);
        }
        public static GameObject NewEntity<T>(
            Vector2 pos, string name, string spriteName, string sortingLayerName,
            bool collider, string objectLayer, bool isRaw = false
            ) where T : MonoBehaviour => NewEntity<T>(pos, name, spriteName, sortingLayerName, collider, objectLayer, RbTypeByDefault, isRaw);
        public static GameObject NewFloppyDisk(Vector2 pos)
        {
            var obj = NewEntity<FloppyDisk>(pos, "Disk", "SizeHolders/16x16", "RealDisks", true, "Disk");
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            var anim = obj.transform.GetChild(0).gameObject.AddComponent<Animator>();
            anim.Init("spinning_floppy_disk", 2f);
            return obj;
        }
        public static GameObject NewRedArrow(Vector2 pos, Vector2 direction)
        {
            var obj = NewEntity<RedArrow>(
                pos, "Red Arrow", "red_arrow", "RedArrow", false, "GUI");
            var ra = obj.GetComponent<RedArrow>();
            ra.position = pos;
            ra.direction = direction;
            ra.Init();
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
            var obj = NewEntity<YellowCirc>(Vector2.zero, "Yellow Circle", "yellow_circ", "YellowCirc", false, "Default");
            obj.GetComponent<CanvasGroup>().alpha = 1/3f;
            obj.transform.GetChild(0).GetComponent<Image>().color = MainCharacter.Yellow;
            
            return obj;
        }
        public static GameObject NewShield()
        {
            var obj = NewEntity<YellowCirc>(Vector2.zero, "Shield", "shield32", "Shield", true, "Shield");
            obj.transform.GetChild(0).GetComponent<Image>().color = MainCharacter.Yellow;
            
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
            var obj = NewEntity<Glitch>(new Vector2(x, y), "Glitch", spriteName, "Glitch", false, "GUI");
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
            textTxt.font = Resources.Load<TMP_FontAsset>("My/My/Fonts/tahoma_8px_raster_hinted");
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
            var text = "";
            var color = (Color)default;
            
            if (isGood)
            {
                text = "";
                color = Color.white;
            }
            else
            {
                text = "-";
                color = Color.red;
            }
            text += string.Format("{0:F1}", damageAmount);

            var obj = NewPopUpText(pos, text, color, .8f);
            
            return obj;
        }
        //public static GameObject NewDamageNumbers(Vector2 pos, string text) => NewPopUpText(pos, text, Color.white);
        public static GameObject NewPopUpText(Vector2 pos, string text, Color color, float duration)
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
            var cg = obj.AddComponent<CanvasGroup>();
            cg.alpha = 0;
            var worldSpaceCanvasScaler = obj.AddComponent<WorldSpaceCanvasRealtimeScaler>();
            var tooltip = obj.AddComponent<PopupText>();
            
            var textObj = new GameObject("Text");
            textObj.layer = LayerMask.NameToLayer("GUI");
            textObj.transform.SetParent(obj.transform, false);
            var textRt = textObj.AddComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = Vector2.zero;
            textRt.offsetMax = Vector2.zero;
            var textTxt = textObj.AddComponent<TextMeshProUGUI>();
            textTxt.font = Resources.Load<TMP_FontAsset>("My/My/Fonts/tahoma_8px_raster_hinted");
            textTxt.alignment = TextAlignmentOptions.Center;
            textTxt.color = color;
            textTxt.text = text;
            textTxt.enableAutoSizing = true;
            textTxt.fontSizeMin = ushort.MinValue;
            textTxt.fontSizeMax = ushort.MaxValue;
            textTxt.raycastTarget = false;
            
            tooltip.Init(duration);
            
            return obj;
        }
        public static GameObject NewSelection()
        {
            var obj = NewEntity<MainCharacterSelection>(Vector2.zero, "Selection", "s1", "Selection", false, "Default");
            obj.transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced;
            return obj;
        }
        /*public static GameObject NewHealthBar()
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
        }*/
        public static GameObject NewDriverButton(Vector2 pos)
        {
            var obj = CreateBaseButton<ButtonDrivers>(pos, "PlayButton", "bios_btns", "bios_btns_0");
            obj.GetComponent<ButtonDrivers>().DisableButton();
            return obj;
        }
        public static GameObject NewSkillTreeButton(Vector2 pos) => CreateBaseButton<ButtonSkillTree>(pos, "PlayButton", "bios_btns", "bios_btns_2");
        public static GameObject NewInventoryButton(Vector2 pos) => CreateBaseButton<ButtonInventory>(pos, "PlayButton", "bios_btns", "bios_btns_4");
        public static GameObject NewFightButton(Vector2 pos) => CreateBaseButton<ButtonFight>(pos, "PlayButton", "bios_btns", "bios_btns_6");
        public static GameObject NewPlayButton(Vector2 pos) => CreateBaseButton<ButtonPlay>(pos, "PlayButton", "btn_play");
        public static GameObject NewBinButton(Vector2 pos) => CreateBaseButton<ButtonRecycleBin>(pos, "BinButton", "bin");
        public static GameObject NewNetButton(Vector2 pos) => CreateBaseButton<ButtonSwitchPC>(pos, "NetButton", "btn_net");
        public static GameObject NewEndlessModeButton(Vector2 pos) => CreateBaseButton<ButtonEndlessMode>(pos, "EndlessModeButton", "btn_endless_mode");
        public static GameObject NewDirPetQuantity(Vector2 pos) => CreateBaseButton<DirPetQuantity>(pos, "Entity", "directories", "directories_0");
        public static GameObject NewDirPetHealth(Vector2 pos) => CreateBaseButton<DirPetHealth>(pos, "Entity", "directories", "directories_1");
        public static GameObject NewDirPetMovementSpeed(Vector2 pos) => CreateBaseButton<DirPetMovementSpeed>(pos, "Entity", "directories", "directories_2");
        public static GameObject NewDirPetDamage(Vector2 pos) => CreateBaseButton<DirPetDamage>(pos, "Entity", "directories", "directories_3");
        public static GameObject NewDirPetAttackInterval(Vector2 pos) => CreateBaseButton<DirPetAttackInterval>(pos, "Entity", "directories", "directories_4");
        public static GameObject NewUpgradeButton(string upgradeName, Vector2 pos)
        {
            GameObject obj;
            switch (upgradeName)
            {
                case "Damage": obj = CreateBaseButton<ProjectileDamage>(pos, "UpgradeDamage", "drivers", "drivers_2"); break;
                case "FireRate": obj = CreateBaseButton<UpgradeBurstSize>(pos, "UpgradeFireRate", "drivers", "drivers_1"); break;
                case "Speed": obj = CreateBaseButton<UpgradeProjectileSpeed>(pos, "UpgradeSpeed", "drivers", "drivers_3"); break;
                case "Sensitivity": obj = CreateBaseButton<UpgradeSensitivity>(pos, "UpgradeSensitivity", "drivers", "drivers_5"); break;
                /*case "Heart": obj = CreateBaseButton<UpgradeHeart>(pos, "UpgradeHeart", "btn_upgrade_hrt"); break;
                case "HeartKeeper": obj = CreateBaseButton<UpgradeHeartKeeper>(pos, "UpgradeHeartKeeper", "btn_upgrade_mxh"); break;*/
                default: throw new System.NotImplementedException();
            }
            return obj;
        }
        private static GameObject CreateBaseButton<T>(Vector2 pos, string name, Sprite sprite) where T : Button
        {
            var obj = NewEntity<T>(pos, name, sprite, "ButtonsFG", true, "Button");
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            return obj;
        }
        private static GameObject CreateBaseButton<T>(Vector2 pos, string name, string spriteName) where T : Button
        {
            var sprite = Resources.Load<Sprite>(string.Format("My/My/Sprites/{0}", spriteName));
            var obj = CreateBaseButton<T>(pos, name, sprite);
            return obj;
        }
        private static GameObject CreateBaseButton<T>(Vector2 pos, string name, string spriteSheetName, string spriteName) where T : Button
        {
            var path = "My/My/Sprites/" + spriteSheetName;
            var allSprites = Resources.LoadAll<Sprite>(path);
            var sprite = allSprites.FirstOrDefault(s => s.name == spriteName);
            var obj = CreateBaseButton<T>(pos, name, sprite);
            return obj;
        }
        public static GameObject NewBottle(Vector2 pos)
        {
            var path = "My/My/Sprites/bottle";
            var allSprites = Resources.LoadAll<Sprite>(path);
            var targetName0 = "bottle_cork";
            var targetSprite0 = allSprites.FirstOrDefault(s => s.name == targetName0);
            var targetName1 = "bottle_fill";
            var targetSprite1 = allSprites.FirstOrDefault(s => s.name == targetName1);
            var targetName2 = "bottle_glare";
            var targetSprite2 = allSprites.FirstOrDefault(s => s.name == targetName2);
            var targetName3 = "bottle_stroke";
            var targetSprite3 = allSprites.FirstOrDefault(s => s.name == targetName3);
            
            var bottleFullness = Random.Range(0.1f, 0.25f);
            var obj = NewEntity<ItemBottle>(pos, "Bottle", targetSprite1, "Bottles", true, "Default");
            
            var maskObj = obj.transform.GetChild(0).gameObject;
            maskObj.AddComponent<Mask>().showMaskGraphic = false;
            
            var liquidColors = new List<Color>()
            {
                new Color(0.4f, 0.2f, 0.05f, 0.75f),
                new Color(0.76f, 0.11f, 0.22f, 0.8f),
                new Color(0.5f, 0.8f, 0.9f, 0.5f)
            };
            for (var i = 0; i < liquidColors.Count; i++)
            {
                var c = liquidColors[i];
                c.r *= 0.666f;
                c.g *= 0.666f;
                c.b *= 0.666f;
                liquidColors[i] = c;
            }
            var liquidColor = GetRandomElement(liquidColors);
            
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
            waterImage.color = liquidColor;
            
            var glassColors = new List<Color>()
            {
                new Color(0.9f, 0.95f, 1f, 0.75f),
                new Color(0.45f, 0.25f, 0.05f, 0.75f),
                new Color(0.2f, 0.4f, 0.15f, 0.75f),
                new Color(0.05f, 0.35f, 0.15f, 0.75f),
                new Color(0.1f, 0.3f, 0.6f, 0.75f)
            };
            for (var i = 0; i < glassColors.Count; i++)
            {
                var c = glassColors[i];
                c.r *= 0.666f;
                c.g *= 0.666f;
                c.b *= 0.666f;
                glassColors[i] = c;
            }
            var fillColor = GetRandomElement(glassColors);
            
            NewImage(obj.transform, targetSprite0);
            var fill = NewImage(obj.transform, targetSprite1);
            fill.GetComponent<Image>().color = fillColor;
            NewImage(obj.transform, targetSprite2);
            var stroke = NewImage(obj.transform, targetSprite3);
            stroke.GetComponent<Image>().color = fillColor;
            
            obj.GetComponent<ItemBottle>().glassColor = fillColor;
            obj.GetComponent<ItemBottle>().liquidColor = liquidColor;
            
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            var bottleScript = obj.GetComponent<ItemBottle>();
            bottleScript.waterRect = waterImageRt;
            
            var tooltipHolder = obj.AddComponent<TooltipHolder>();
            var dragable = obj.AddComponent<Dragable>();
            var bottle = obj.GetComponent<ItemBottle>();
            tooltipHolder.Init();
            dragable.Init();
            bottle.Init();

            var ppu = 30f;
            var bottleWalls = new List<GameObject>();
            bottleWalls.Add(Spawner.NewEntity<PositionFollower>(
                new Vector2(pos.x-(14.5f/ppu), pos.y-(15f/ppu)), "Left Bottle Wall", "SizeHolders/bottle_v_wall",
                null, true, "BottleWall", RigidbodyType2D.Kinematic));
            bottleWalls.Add(Spawner.NewEntity<PositionFollower>(
                new Vector2(pos.x+(14.5f/ppu), pos.y-(15f/ppu)), "Right Bottle Wall", "SizeHolders/bottle_v_wall",
                null, true, "BottleWall", RigidbodyType2D.Kinematic));
            bottleWalls.Add(Spawner.NewEntity<PositionFollower>(
                new Vector2(pos.x, pos.y-(29.5f/ppu)), "Bottom Bottle Wall", "SizeHolders/bottle_h_wall",
                null, true, "BottleWall", RigidbodyType2D.Kinematic));
            bottleWalls.Add(Spawner.NewEntity<PositionFollower>(
                new Vector2(pos.x, pos.y-(0.5f/ppu)), "Top Bottle Wall", "SizeHolders/bottle_h_wall",
                null, true, "BottleWall", RigidbodyType2D.Kinematic));
            for (int i = 0; i < bottleWalls.Count; i++)
                bottleWalls[i].GetComponent<PositionFollower>().target = obj.transform;
            bottleWalls[0].GetComponent<PositionFollower>().offset = new Vector2(-(14.5f/ppu), -(15f/ppu));
            bottleWalls[1].GetComponent<PositionFollower>().offset = new Vector2((14.5f/ppu), -(15f/ppu));
            bottleWalls[2].GetComponent<PositionFollower>().offset = new Vector2(0, -(29.5f/ppu));
            bottleWalls[3].GetComponent<PositionFollower>().offset = new Vector2(0, -(0.5f/ppu));
            
            var hkPos = new Vector2(pos.x, pos.y - (26f/ppu));
            if (Random.Range(0, 2) == 0)
            {
                var prisoner = NewHeartKeeper(hkPos);
                obj.GetComponent<ItemBottle>().prisoner = prisoner.GetComponent<HeartKeeper>();
                var bpComp = prisoner.AddComponent<BottlePrisoner>();
                bpComp.leftWall = bottleWalls[0].transform;
                bpComp.rightWall = bottleWalls[1].transform;
                bpComp.bottomWall = bottleWalls[2].transform;
                bpComp.topWall = bottleWalls[3].transform;
            }
            return obj;
        }
        public static T GetRandomElement<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default(T);
            }

            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }
        public static GameObject NewLayingPieceOfGlass(Vector2 pos, Color color)
        {
            var obj = NewEntity<ItemLayingPieceOfGlass>(pos, "LayingPieceOfGlass", "glass", "Bottles", true, "Default");
            obj.transform.GetChild(0).GetComponent<Image>().color = color;
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            return obj;
        }
        public static GameObject NewZipporah()
        {
            var obj = NewEntity<Zipporah>(Vector2.zero, "Zipporah", "SizeHolders/480x270", "Tabby", false, "Default");
            
            var imageObj = new GameObject("Zipporah Textbox");
            imageObj.transform.SetParent(obj.transform, false);
            //
            var imageRt = imageObj.AddComponent<RectTransform>();
            imageRt.anchorMin = new Vector2(1, 0);
            imageRt.anchorMax = new Vector2(1, 0);
            imageRt.pivot = new Vector2(1, 0);
            imageRt.anchoredPosition3D = new Vector3(-60f, 15f, 0);
            imageRt.sizeDelta = new Vector2(130f, 32f);
            imageRt.localScale = Vector3.one;
            //
            var image = imageObj.AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>("My/My/Sprites/zipporah_textbox");
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
            textRt.offsetMin = new Vector2(8, 8);
            textRt.offsetMax = new Vector2(-8, -23);
            //
            var textTxt = textObj.AddComponent<TextMeshProUGUI>();
            textTxt.font = Resources.Load<TMP_FontAsset>("My/My/Fonts/tahoma_8px_raster_hinted");
            textTxt.text = "Default text";
            textTxt.alignment = TextAlignmentOptions.Left;
            textTxt.color = Color.black;
            textTxt.enableAutoSizing = true;
            textTxt.fontSizeMin = ushort.MinValue;
            textTxt.fontSizeMax = ushort.MaxValue;
            textTxt.raycastTarget = false;
            
            obj.GetComponent<Zipporah>().Init();
            
            return obj;
        }
        public static GameObject NewTutorialStaticGlyph(Vector2 pos) => NewStaticGlyph(pos, false);
        public static GameObject NewStaticGlyph(Vector2 pos, bool IsDragable = true)
        {
            var glyph = new Glyph(
                true,
                Random.Range(1f, 3f),
                Random.Range(1f, 3f),
                Random.value < 0.33f,
                Random.value < 0.33f,
                Guns.Library[GunName.Yellow].Glyph.Speed
            );
            var obj = NewProjectile(pos, Vector2.zero, glyph, IsDragable);
            obj.GetComponent<Projectile>().MakeItBeAKey();
            return obj;
        }
        public static GameObject NewClipboardGlyph(Vector2 pos)
        {
            var glyph = new Glyph(
                true,
                1,
                1,
                false,
                false,
                Guns.Library[GunName.Yellow].Glyph.Speed
            );
            var obj = NewProjectile(pos, Vector2.zero, glyph);
            obj.GetComponent<Projectile>().MakeItBeAClipboardItem();
            return obj;
        }
        public static GameObject NewTutorialApple(Vector2 pos) => NewApple(pos, false);
        public static GameObject NewApple(Vector2 pos, bool isDragable = true)
        {
            var obj = NewEntity<ItemApple>(pos, "Apple", "apple", "Bottles", true, "Default");
            var tooltipHolder = obj.AddComponent<TooltipHolder>();
            var dragable = (Dragable)default;
            if (isDragable)
                dragable = obj.AddComponent<Dragable>();
            var apple = obj.GetComponent<ItemApple>();
            tooltipHolder.Init();
            if (isDragable)
                dragable.Init();
            apple.Init();
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            return obj;
        }
    }
}