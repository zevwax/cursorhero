using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

namespace ZevWaxGames.CursorHero
{
    public abstract class Button : MonoBehaviour
    {
        protected string boxPath = "My/My/Sprites/btn";
        public string iconPath;
        public string tooltipText = "Default Tooltip";
        
        private GameObject currentTooltip;
        private bool isHovered;
        private Tween idleTween;
        private BoxCollider2D myCollider;
        private Tween alphaTween;

        private WorldSpaceCanvasRealtimeScaler scaler;
        private Tween sizeTween;
        private CanvasGroup canvasGroup;
        
        public bool IsEnabled { get; private set; } = true;
        protected virtual void Start()
        {
            scaler = GetComponent<WorldSpaceCanvasRealtimeScaler>();
            
            canvasGroup = GetComponent<CanvasGroup>();
            
            transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(boxPath);
            
            myCollider = GetComponent<BoxCollider2D>();
            //here add something that will let me find the instance in hierarchy
            if (myCollider == null)
            {
                Debug.LogError($"[Missing Collider] This GameObject is missing BoxCollider2D!", gameObject);
                #if UNITY_EDITOR
                UnityEditor.EditorGUIUtility.PingObject(gameObject);
                #endif
            }

            StartHangedAnimation();
            if (!IsEnabled)
            {
                idleTween?.Pause();
                transform.localRotation = Quaternion.identity;
                canvasGroup.alpha = 0.5f;
                AnimateMult(1f, 0f);
            }
            else
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
            if (!IsEnabled) return;
            if (MainCharacter.Instance == null) return;
            
            var mainChar = MainCharacter.Instance;
            
            var charCol = mainChar.GetComponent<BoxCollider2D>();
            bool collision = myCollider.IsTouching(charCol);

            if (collision)
            {
                if (!Mouse.current.leftButton.isPressed)
                    mainChar.SetButtonLink(gameObject);
                
                if (!isHovered) OnHoverEnter();

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    mainChar.SetPush(gameObject);
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
            AnimateMult(1.0f, 0.2f);
        }
        private void SetStateHover()
        {
            AnimateMult(1.1f, 0.2f);
        }
        private void SetStatePushed()
        {
            AnimateMult(0.9f, 0.1f);
        }
        private void AnimateMult(float targetValue, float duration)
        {
            sizeTween?.Kill();
            sizeTween = DOTween.To(() => scaler.mult, x => scaler.mult = x, targetValue, duration)
                .SetEase(Ease.OutQuad);
        }
        private void OnDestroy() => idleTween?.Kill();

        public virtual void ButtonAction()
        {
            DisableButton();
            MainCharacter.Instance.SetGlove(gameObject);
        }
        private void AnimateAlpha(float targetValue, float duration)
        {
            alphaTween?.Kill();
            alphaTween = canvasGroup.DOFade(targetValue, duration)
                .SetEase(Ease.InOutQuad);
        }
        public void DisableButton()
        {
            if (!IsEnabled) return;
            IsEnabled = false;
            
            if (isHovered) OnHoverExit();
            idleTween?.Pause();
            transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutQuad);
            AnimateMult(1f, 0.3f);
            AnimateAlpha(0.5f, 0.3f);
        }

        public void EnableButton()
        {
            if (IsEnabled) return;
            IsEnabled = true;
            SetStateIdle();
            idleTween?.Play();
            AnimateAlpha(1f, 0.3f);
        }
    }
}