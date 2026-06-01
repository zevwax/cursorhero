using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.UI;
namespace ZevWaxGames.CursorHero
{
    public class MapLine : MapItem
    {
        private Sprite[] frames;
        private Image image;
        private int currFrameIndex = 0;
        private const float FastAnimDuration = 1f;
        private const float SlowAnimDuration = 2f;
        private float currAnimDuration = FastAnimDuration;
        private int numOfFrames = 16;
        public void Init()
        {
            var path = "My/My/Sprites/map_line";
            var allSprites = Resources.LoadAll<Sprite>(path);
            frames = new Sprite[numOfFrames];
            for (var i = 0; i < numOfFrames; i++)
            {
                var targetName = "map_line_" + i;
                Debug.Log(targetName);
                var targetSprite = allSprites.FirstOrDefault(s => s.name == targetName);
                frames[i] = targetSprite;
            }
            
            image = transform.GetChild(0).GetComponent<Image>();
            
            StartCoroutine(Animation());
        }
        private IEnumerator Animation()
        {
            while (true)
            {
                NextFrame();
                yield return new WaitForSeconds(currAnimDuration/numOfFrames);
            }
        }
        private void NextFrame()
        {
            if (currFrameIndex == frames.Length - 1)
                currFrameIndex = 0;
            else
                currFrameIndex++;
            image.sprite = frames[currFrameIndex];
        }
        protected override void ShowMapNode()
        {
            base.ShowMapNode();
            currAnimDuration = FastAnimDuration;
        }
        protected override void HideMapNode()
        {
            base.HideMapNode();
            currAnimDuration = SlowAnimDuration;
        }
        protected override void HideInstantly()
        {
            base.HideInstantly();
            currAnimDuration = SlowAnimDuration;
        }
    }
}