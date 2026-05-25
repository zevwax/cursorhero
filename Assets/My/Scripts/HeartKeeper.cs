using UnityEngine;
using UnityEngine.UI;
using System.Linq;
namespace ZevWaxGames.CursorHero
{
    public class HeartKeeper : MonoBehaviour
    {
        private static Image[] layers;
        private static Sprite[][] sprites;
        private void Awake()
        {
            if (sprites == null)
            {
                var path = "My/My/Sprites/heart_keeper";
                var allSprites = Resources.LoadAll<Sprite>(path);
                sprites = new Sprite[6][];
                for (var fingers = 0; fingers <= 5; fingers++)
                {
                    sprites[fingers] = new Sprite[3];
                    for (var priority = 0; priority <= 2; priority++)
                    {
                        var targetName = string.Format("heart_keeper_f{0}_p{1}", fingers, priority);
                        var targetSprite = allSprites.FirstOrDefault(s => s.name == targetName);
                        sprites[fingers][priority] = targetSprite;
                    }
                }
            }
        }
        public void Init()
        {
            layers = new Image[3];
            layers[0] = transform.GetChild(0).GetComponent<Image>();
            layers[1] = transform.GetChild(1).GetComponent<Image>();
            // 2 - heart
            layers[2] = transform.GetChild(3).GetComponent<Image>();
            ShowFingers(5);
        }
        public void ShowFingers(int number)
        {
            for (var i = 0; i <= 2; i++)
            {
                var sprite = sprites[number][i];
                if (sprite != null)
                {
                    var img = layers[i];
                    img.color = Color.white;
                    img.sprite = sprite;
                }
                else
                    layers[i].color = new Color(0, 0, 0, 0);
            }
        }
    }
}