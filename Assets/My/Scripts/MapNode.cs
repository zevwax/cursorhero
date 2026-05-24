using UnityEngine;
using UnityEngine.UI;
using System.Linq;
namespace ZevWaxGames.CursorHero
{
    public class MapNode : MapItem
    {
        private Image image;
        private Sprite flag;
        protected override void Start()
        {
            base.Start();
            
            var path = "My/My/Sprites/mineswapper";
            var allSprites = Resources.LoadAll<Sprite>(path);
            var targetName = "mineswapper_0";
            var targetSprite = allSprites.FirstOrDefault(s => s.name == targetName);
            flag = targetSprite;
            
            StartCoroutine(HangingRoutine());
        }
        public void Init() => image = transform.GetChild(0).GetComponent<Image>();
        public void SetFlag() => image.sprite = flag;
    }
}