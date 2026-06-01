using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using DG.Tweening;

namespace ZevWaxGames.CursorHero
{
    public class HeartKeeper : MonoBehaviour
    {
        public bool isOwned = false;
        private Image[] layers;
        private static Sprite[][] sprites;
        private int currNumOfFingers = -1;
        private WorldSpaceCanvasRealtimeScaler scaler;
        private Coroutine zoom;

        private void OnEnable() => EventHolder.OnFadingInToPCStarted += DieIfNeeded;
        private void OnDisable() => EventHolder.OnFadingInToPCStarted -= DieIfNeeded;
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
            scaler = GetComponent<WorldSpaceCanvasRealtimeScaler>();
            
            layers = new Image[3];
            layers[0] = transform.GetChild(0).GetComponent<Image>();
            layers[1] = transform.GetChild(1).GetComponent<Image>();
            // 2 - heart
            layers[2] = transform.GetChild(3).GetComponent<Image>();
            ShowFingers(0);
        }
        public void ShowFingers(int number)
        {
            if (currNumOfFingers == number) return;
            currNumOfFingers = number;
            if (zoom != null)
                StopCoroutine(zoom);
            zoom = StartCoroutine(CShowFingers(number));
        }
        private IEnumerator CShowFingers(int numOfFingers)
        {
            yield return new WaitForSeconds(0.8f);
            yield return StartCoroutine(CZoom(1.5f));
            GetComponent<Canvas>().sortingLayerName = "HeartKeeperFG";
            yield return StartCoroutine(CZoom(2f));
            yield return new WaitForSeconds(.25f);
            UpdateSprites(numOfFingers);
            yield return new WaitForSeconds(.25f);
            yield return StartCoroutine(CZoom(1.5f));
            GetComponent<Canvas>().sortingLayerName = "HeartKeeperBG";
            yield return StartCoroutine(CZoom(1f));
        }
        private IEnumerator CZoom(float endValue)
        {
            var startValue = scaler.mult;
            var duration = 0.33f;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                scaler.mult = Mathf.Lerp(startValue, endValue, elapsed / duration);
                yield return null; 
            }

            scaler.mult = endValue; 
        }
        private void UpdateSprites(int numOfFingers)
        {
            for (var i = 0; i <= 2; i++)
            {
                var sprite = sprites[numOfFingers][i];
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
        private void DieIfNeeded()
        {
            if (!isOwned)
                Destroy(gameObject);
        }
    }
}