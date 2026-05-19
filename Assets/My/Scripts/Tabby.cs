using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace ZevWaxGames.CursorHero
{
    public class Tabby : MonoBehaviour
    {
        public static Tabby Instance { get; private set; }
        private Sprite[] blinkSprites;
        private Sprite[] walkSprites;
        private float frameRate = 1/8f;
        private float walkDuration = 4f;

        private Image uiImage;
        private Vector2 offScreenPosition;
        private Vector2 inScreenPosition;
        private bool isWalkingLooping;
        
        private Coroutine waitNGo;
        
        private bool messageIsUpdated = false;
        public bool hasToGo = false;
        private void Awake() => Instance = this;
        private void OnEnable() => EventHolder.OnFadingInToPCStarted += Die;
        private void OnDisable() => EventHolder.OnFadingInToPCStarted -= Die;
        public void Init()
        {
            offScreenPosition = new Vector2(8.5f, -3.5f);
            inScreenPosition = new Vector2(5.5f, -3.5f);
            
            transform.position = offScreenPosition;
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            
            uiImage = transform.GetChild(0).GetComponent<Image>();

            Sprite[] allSprites = Resources.LoadAll<Sprite>("My/My/Sprites/tabby");
            
            System.Array.Sort(allSprites, (a, b) => {
                int indexA = int.Parse(a.name.Substring(a.name.LastIndexOf('_') + 1));
                int indexB = int.Parse(b.name.Substring(b.name.LastIndexOf('_') + 1));
                return indexA.CompareTo(indexB);
            });

            blinkSprites = new Sprite[8];
            walkSprites = new Sprite[8];

            System.Array.Copy(allSprites, 0, blinkSprites, 0, 8);
            System.Array.Copy(allSprites, 8, walkSprites, 0, 8);
        }

        public void PlayAnimation()
        {
            StartCoroutine(CPlayAnimation());
        }

        private IEnumerator CPlayAnimation()
        {
            yield return new WaitForSeconds(1f);
            
            transform.position = offScreenPosition;
            uiImage.transform.localRotation = Quaternion.Euler(0, 0, 0);

            StartCoroutine(LoopWalkAnimation(true));
            WalkIn();
            yield return new WaitForSeconds(walkDuration);

            StartCoroutine(LoopWalkAnimation(false));
            yield return StartCoroutine(CBlink());
            
            yield return StartCoroutine(MessageScaleUp(195f));
            
            yield return new WaitUntil(() => hasToGo);
            
            yield return StartCoroutine(MessageScaleDown());
            
            uiImage.transform.localRotation = Quaternion.Euler(0, 180, 0);
            StartCoroutine(LoopWalkAnimation(true));
            WalkOut();
            yield return new WaitForSeconds(walkDuration);

            StartCoroutine(LoopWalkAnimation(false));
        }

        private void WalkIn()
        {
            transform.DOMove(inScreenPosition, walkDuration).SetEase(Ease.Linear);
        }

        private void WalkOut()
        {
            transform.DOMove(offScreenPosition, walkDuration).SetEase(Ease.Linear);
        }

        private IEnumerator CBlink()
        {
            for (int i = 0; i < blinkSprites.Length; i++)
            {
                uiImage.sprite = blinkSprites[i];
                yield return new WaitForSeconds(frameRate);
            }
        }

        private IEnumerator LoopWalkAnimation(bool start)
        {
            isWalkingLooping = start;
            int currentFrame = 0;

            while (isWalkingLooping)
            {
                uiImage.sprite = walkSprites[currentFrame];
                currentFrame = (currentFrame + 1) % walkSprites.Length;
                yield return new WaitForSeconds(frameRate);
            }
        }
        private IEnumerator MessageScaleUp(float height)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            var msg1 = transform.GetChild(1).GetComponent<RectTransform>();
            var duration = 0.3f;
            yield return msg1.DOSizeDelta(new Vector2(130f, height), duration).SetEase(Ease.OutBack).WaitForCompletion();
            transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
        private IEnumerator MessageScaleDown()
        {
            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            var msg1 = transform.GetChild(1).GetComponent<RectTransform>();
            var duration = 0.3f;
            yield return msg1.DOSizeDelta(new Vector2(130f, 21f), duration).SetEase(Ease.OutBack).WaitForCompletion();
            transform.GetChild(1).gameObject.SetActive(false);
        }
        public IEnumerator UpdateMessage()
        {
            if (messageIsUpdated) yield break;
            yield return StartCoroutine(MessageScaleDown());
            var msg1 = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            msg1.text =
                "Good job, boss!!\n\n" +
                "<color=#FF00FF>Hit the 'Switch PC' shortcut</color> when you're done looting, okay??";
            yield return StartCoroutine(MessageScaleUp(65f));
            yield return new WaitForSeconds(2f);
            G.Instance.net.EnableButton();
            messageIsUpdated = true;
        }
        private void Die() => Destroy(gameObject);
    }
}