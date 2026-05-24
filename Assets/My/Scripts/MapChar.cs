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
        private Sprite glasses;
        private Collider2D myCollider;
        private Collider2D[] results = new Collider2D[20];
        private void OnEnable()
        {
            EventHolder.OnFadingInToPCStarted += Move;
            EventHolder.OnChoosingStarted += ShowMapNode;
            EventHolder.OnChoosingFinished += HideMapNode;
            EventHolder.OnPCFinished += ShowMapNode;
            EventHolder.OnPCStarted += HideMapNode;
        }
        private void OnDisable()
        {
            EventHolder.OnFadingInToPCStarted -= Move;
            EventHolder.OnChoosingStarted -= ShowMapNode;
            EventHolder.OnChoosingFinished -= HideMapNode;
            EventHolder.OnPCFinished -= ShowMapNode;
            EventHolder.OnPCStarted -= HideMapNode;
        }
        private void Move()
        {
            Hide();
            if (transform.position.y > 2.4f && !isGlasses)
            {
                isGlasses = true;
                SetGlasses();
            }
            else if (transform.position.y < 2.4f)
            {
                SetFlag();
                offScreenPosition = new Vector2(offScreenPosition.x, offScreenPosition.y + 1.25f);
                inScreenPosition = new Vector2(inScreenPosition.x, inScreenPosition.y + 1.25f);
                transform.DOMove(new Vector3(0, transform.position.y + 1.25f, 0), 0).SetEase(Ease.Linear).WaitForCompletion();
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
        protected override void Start()
        {
            base.Start();
            
            var path = "My/My/Sprites/mineswapper";
            var allSprites = Resources.LoadAll<Sprite>(path);
            var targetName = "mineswapper_7";
            var targetSprite = allSprites.FirstOrDefault(s => s.name == targetName);
            glasses = targetSprite;
            
            StartCoroutine(HangingRoutine());
        }
        public void Init()
        {
            myCollider = GetComponent<Collider2D>();
            image = transform.GetChild(0).GetComponent<Image>();
        }
    }
}