using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace ZevWaxGames.CursorHero
{
    public abstract class Button : MonoBehaviour
    {
        protected string boxPath = "My/My/Sprites/btn";
        public string iconPath;
        public string tooltipText = "Default Tooltip";
        
        private SpriteRenderer boxSr;
        private SpriteRenderer iconSr;
        private GameObject currentTooltip;
        private bool isHovered;
        private Tween idleTween;
        private Collider2D myCollider;

        protected virtual void Start()
        {
            boxSr = GetComponent<SpriteRenderer>();
            boxSr.sprite = Resources.Load<Sprite>(boxPath);
            
            myCollider = GetComponent<Collider2D>();

            var iconObj = new GameObject("Icon");
            iconObj.transform.SetParent(transform);
            iconObj.transform.localPosition = new Vector3(-0.045f, 0.065f, 0);
            
            iconSr = iconObj.AddComponent<SpriteRenderer>();
            iconSr.sortingLayerName = "Buttons";
            iconSr.sprite = Resources.Load<Sprite>(iconPath);
            iconSr.sortingOrder = boxSr.sortingOrder + 1;

            StartHangedAnimation();
            SetStateIdle();
        }

        private void StartHangedAnimation()
        {
            idleTween?.Kill();
            transform.localRotation = Quaternion.Euler(0, 0, -5f);
            idleTween = transform.DORotate(new Vector3(0, 0, 5f), 2f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        protected virtual void Update()
        {
            if (MainCharacter.Instance == null) return;
            
            var mainChar = MainCharacter.Instance;
            
            Collider2D charCol = mainChar.GetComponent<Collider2D>();
            bool collision = myCollider.IsTouching(charCol);

            if (collision)
            {
                mainChar.SetButtonLink(gameObject);
                
                if (!isHovered) OnHoverEnter();

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    SetStatePushed();
                }
                else if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    SetStateHover();
                    if (currentTooltip != null) currentTooltip.GetComponent<Tooltip>().Die();
                    ButtonAction();
                }
            }
            else
            {
                if (isHovered)
                {
                    mainChar.SetGlove(gameObject);
                    OnHoverExit();
                }
            }
        }

        private void OnHoverEnter()
        {
            isHovered = true;
            SetStateHover();
            currentTooltip = Spawner.NewTooltip(tooltipText);
        }

        private void OnHoverExit()
        {
            isHovered = false;
            SetStateIdle();
            if (currentTooltip != null) currentTooltip.GetComponent<Tooltip>().Die();
        }

        private void SetStateIdle()
        {
            transform.DOScale(1f, 0.2f);
        }

        private void SetStateHover()
        {
            transform.DOScale(1.1f, 0.2f);
        }

        private void SetStatePushed()
        {
            transform.DOScale(0.9f, 0.1f);
        }

        private void OnDestroy() => idleTween?.Kill();

        public abstract void ButtonAction();
    }
}