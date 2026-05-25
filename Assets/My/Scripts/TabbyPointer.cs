using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ZevWaxGames.CursorHero
{
    public class TabbyPointer : MonoBehaviour
    {
        public static TabbyPointer Instance { get; private set; }

        private Vector3 initPos;
        private Image pointerImage;
        private Coroutine loopCoroutine;
        
        private TabbySelection currentSelection;
        private CanvasGroup selectionCanvasGroup;

        private const string IdleSpritePath = "My/My/Sprites/tabby_idle";
        private const string CopySpritePath = "My/My/Sprites/tabby_copy";

        private Sprite idleSprite;
        private Sprite copySprite;

        private float alpha = 1;
        private float selectionSquareSide = 2.25f;

        private void Awake() => Instance = this;

        public void Init(Vector3 startPosition)
        {
            GetComponent<CanvasGroup>().alpha = alpha;
            
            if (transform.childCount > 0)
            {
                pointerImage = transform.GetChild(0).GetComponent<Image>();
            }
            
            idleSprite = Resources.Load<Sprite>(IdleSpritePath);
            copySprite = Resources.Load<Sprite>(CopySpritePath);
            
            initPos = startPosition + new Vector3(-selectionSquareSide/2f, selectionSquareSide/2f, 0f);

            var selectionObj = Spawner.NewTabbySelection();
            currentSelection = selectionObj.GetComponent<TabbySelection>();
            selectionCanvasGroup = selectionObj.GetComponent<CanvasGroup>();
            
            SetSelectionAlpha(0f);
            
            if (loopCoroutine != null)
            {
                StopCoroutine(loopCoroutine);
            }
            loopCoroutine = StartCoroutine(PointerLoopRoutine());
        }

        private IEnumerator PointerLoopRoutine()
        {
            while (true)
            {
                transform.position = initPos;
                if (pointerImage != null) pointerImage.sprite = idleSprite;
                
                SetSelectionAlpha(0f);

                yield return new WaitForSeconds(0.9f);

                pointerImage.sprite = copySprite;
                
                if (currentSelection != null)
                {
                    SetSelectionAlpha(alpha);
                    currentSelection.StartSelectionFromPointer(initPos);
                }
                
                yield return new WaitForSeconds(0.9f);

                Vector3 targetPos = initPos + new Vector3(selectionSquareSide, -selectionSquareSide, 0f);
                bool tweenCompleted = false;

                transform.DOMove(targetPos, 2f)
                    .SetEase(Ease.Linear)
                    .OnUpdate(() =>
                    {
                        if (currentSelection != null)
                        {
                            currentSelection.UpdateSelectionArea();
                        }
                    })
                    .OnComplete(() => tweenCompleted = true);

                yield return new WaitUntil(() => tweenCompleted);

                yield return new WaitForSeconds(1);
                
                if (currentSelection != null)
                {
                    currentSelection.FinishSelectionFromPointer();
                    SetSelectionAlpha(0f);
                }

                if (pointerImage != null) pointerImage.sprite = idleSprite;

                yield return new WaitForSeconds(1f);
            }
        }

        private void SetSelectionAlpha(float alpha)
        {
            if (selectionCanvasGroup != null)
            {
                selectionCanvasGroup.alpha = alpha;
            }
        }

        private void OnDestroy()
        {
            if (loopCoroutine != null) StopCoroutine(loopCoroutine);
            Destroy(currentSelection.gameObject);
        }
    }
}