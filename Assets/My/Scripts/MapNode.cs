using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
namespace ZevWaxGames.CursorHero
{
    public class MapNode : MapItem
    {
        private Image image;
        private Sprite initSprite;
        private Sprite flag;
        public void Init()
        {
            image = transform.GetChild(0).GetComponent<Image>();
            
            initSprite = image.sprite;
            
            var path = "My/My/Sprites/mineswapper";
            var allSprites = Resources.LoadAll<Sprite>(path);
            var targetName = "mineswapper_0";
            var targetSprite = allSprites.FirstOrDefault(s => s.name == targetName);
            flag = targetSprite;
            
            StartCoroutine(HangingRoutine());
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            EventHolder.OnBIOSStarted += ResetSprite;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventHolder.OnBIOSStarted -= ResetSprite;
        }
        public void SetFlag() => image.sprite = flag;
        public void ResetSprite() => image.sprite = initSprite;
    }
}