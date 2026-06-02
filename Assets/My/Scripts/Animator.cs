using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
namespace ZevWaxGames.CursorHero
{
    public class Animator : MonoBehaviour
    {
        private int currFrameIndex = -1;
        private Sprite[] sprites;
        private float cycleDuration;
        private Image img;
        public void Init(string spriteSheetName, float cycleDuration)
        {
            var path = "My/My/Sprites/" + spriteSheetName;
            var allSprites = Resources.LoadAll<Sprite>(path);
            sprites = new Sprite[allSprites.Length];
            for (var i = 0; i < sprites.Length; i++)
            {
                var targetName = spriteSheetName + "_" + i;
                var targetSprite = allSprites.FirstOrDefault(s => s.name == targetName);
                sprites[i] = targetSprite;
            }
            
            this.cycleDuration = cycleDuration;
            
            img = transform.GetChild(0).GetComponent<Image>();
            
            StartCoroutine(Animation());
        }
        private IEnumerator Animation()
        {
            while (true)
            {
                currFrameIndex = NextValue();
                img.sprite = sprites[currFrameIndex];
                yield return new WaitForSeconds(cycleDuration/sprites.Length);
            }
        }
        private int NextValue()
        {
            var res = currFrameIndex + 1;
            if (res == sprites.Length)
                res = 0;
            return res;
        }
    }
}