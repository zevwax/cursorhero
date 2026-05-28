using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
namespace ZevWaxGames.CursorHero
{
    public class MapChar : MapItem
    {
        private bool isGlasses = false;
        private Image image;
        private Sprite initSprite;
        private Sprite glasses;
        private Collider2D myCollider;
        private Collider2D[] results = new Collider2D[20];
        private void OnEnable()
        {
            EventHolder.OnFadingOutFromPCStarted += Move;
            EventHolder.OnBSODStarted += Reset;
            /*EventHolder.OnChoosingStarted += ShowMapNode;
            EventHolder.OnChoosingFinished += HideMapNode;
            EventHolder.OnPCFinished += ShowMapNode;
            EventHolder.OnPCStarted += HideMapNode;*/
        }
        private void OnDisable()
        {
            EventHolder.OnFadingOutFromPCStarted -= Move;
            EventHolder.OnBSODStarted -= Reset;
            /*EventHolder.OnChoosingStarted -= ShowMapNode;
            EventHolder.OnChoosingFinished -= HideMapNode;
            EventHolder.OnPCFinished -= ShowMapNode;
            EventHolder.OnPCStarted -= HideMapNode;*/
        }
        private void Move()
        {
            if (transform.position.y > 2.4f && !isGlasses)
            {
                isGlasses = true;
                SetGlasses();
            }
            else if (transform.position.y < 2.4f)
            {
                SetFlag();
                var diff = 1.25f;
                offScreenPosition = new Vector2(offScreenPosition.x, offScreenPosition.y + diff);
                inScreenPosition = new Vector2(inScreenPosition.x, inScreenPosition.y + diff);
                transform.DOMove(inScreenPosition, 3);
            }
        }
        private void SetFlag()
        {
            if (myCollider == null) return;
            var filter = new ContactFilter2D().NoFilter();
            var count = myCollider.Overlap(filter, results);
            for (var i = 0; i < count; i++)
            {
                if (results[i].TryGetComponent<MapNode>(out MapNode mapNode))
                {
                    mapNode.SetFlag();
                    break; 
                }
            }
        }
        private void SetGlasses() => image.sprite = glasses;
        private void Reset()
        {
            image.sprite = initSprite;
            offScreenPosition = new Vector2(offScreenPosition.x, -2.5f);
            inScreenPosition = new Vector2(inScreenPosition.x, -2.5f);
            transform.position = inScreenPosition;
        }
        public void Init()
        {
            myCollider = GetComponent<Collider2D>();
            image = transform.GetChild(0).GetComponent<Image>();

            initSprite = image.sprite;
            
            var path = "My/My/Sprites/mineswapper";
            var allSprites = Resources.LoadAll<Sprite>(path);
            var targetName = "mineswapper_7";
            var targetSprite = allSprites.FirstOrDefault(s => s.name == targetName);
            glasses = targetSprite;
            
            StartCoroutine(HangingRoutine());
        }
    }
}