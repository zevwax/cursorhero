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

        private WorldSpaceCanvasRealtimeScaler scaler;
        private Tween sizeTween;

        private void Awake()
        {
            scaler = GetComponent<WorldSpaceCanvasRealtimeScaler>();
        }
        protected virtual void Start()
        {
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
            
            var charCol = mainChar.GetComponent<BoxCollider2D>();
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

        public abstract void ButtonAction();
    }
}