using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ZevWaxGames.CursorHero
{
    public class Tabby : MonoBehaviour
    {
        private Sprite[] blinkSprites;
        private Sprite[] walkSprites;
        private float frameRate = 1/8f;
        private float walkDuration = 2f;

        private Image uiImage;
        private Vector2 offScreenPosition;
        private Vector2 inScreenPosition;
        private bool isWalkingLooping;

        public void Init()
        {
            offScreenPosition = new Vector2(8.5f, -3.5f);
            inScreenPosition = new Vector2(7, -3.5f);
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
            yield return new WaitForSeconds(60);
            
            transform.position = offScreenPosition;
            uiImage.transform.localRotation = Quaternion.Euler(0, 0, 0);

            StartCoroutine(LoopWalkAnimation(true));
            WalkIn();
            yield return new WaitForSeconds(walkDuration);

            StartCoroutine(LoopWalkAnimation(false));
            yield return StartCoroutine(CBlink());
            
            yield return new WaitForSeconds(2);
            
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
    }
}