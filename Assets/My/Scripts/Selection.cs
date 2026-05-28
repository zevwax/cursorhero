using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public abstract class Selection : MonoBehaviour
    {
        protected RectTransform _rectTransform;
        private Image _image;
        protected Vector3 _startPos;
        private Sprite[] _sprites;
        protected const float PixelsPerUnit = 30f;
        protected bool isActive = false;

        protected abstract bool CanStartSelection();
        protected abstract Vector3 GetPointerPos();
        protected abstract void OnStartSelecting();
        protected abstract void OnFinishSelecting();

        protected virtual void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.sizeDelta = Vector2.zero;
            _image = transform.GetChild(0).GetComponent<Image>();
            _startPos = GetPointerPos();
            
            _sprites = new Sprite[3];
            _sprites[0] = Resources.Load<Sprite>("My/My/Sprites/s1");
            _sprites[1] = Resources.Load<Sprite>("My/My/Sprites/s2");
            _sprites[2] = Resources.Load<Sprite>("My/My/Sprites/s3");
            
            StartCoroutine(AnimateSprites());
        }

        protected virtual void Update()
        {
            if (!CanStartSelection()) return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
                StartSelecting();

            if (isActive)
                UpdateLayout();

            if (Mouse.current.leftButton.wasReleasedThisFrame)
                FinishSelecting();
        }

        private void UpdateLayout()
        {
            var currPos = GetPointerPos();
            transform.position = (_startPos + currPos) / 2f;
            float widthUnits = Mathf.Abs(currPos.x - _startPos.x);
            float heightUnits = Mathf.Abs(currPos.y - _startPos.y);
            _rectTransform.sizeDelta = new Vector2(widthUnits * PixelsPerUnit, heightUnits * PixelsPerUnit);
        }

        private IEnumerator AnimateSprites()
        {
            int currentIndex = 0;
            while (true)
            {
                if (_sprites[currentIndex] != null)
                {
                    _image.sprite = _sprites[currentIndex];
                }
                currentIndex = (currentIndex + 1) % _sprites.Length;
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void StartSelecting()
        {
            isActive = true;
            _startPos = GetPointerPos();
            OnStartSelecting();
        }

        public void FinishSelecting()
        {
            OnFinishSelecting();
            isActive = false;
            _rectTransform.sizeDelta = Vector2.zero;
        }

        protected GameObject GetSelectedObject()
        {
            var halfOfW = (_rectTransform.sizeDelta.x / PixelsPerUnit) * 0.5f;
            var halfOfH = (_rectTransform.sizeDelta.y / PixelsPerUnit) * 0.5f;

            var posXOfLeftSideOfTheSelection = transform.position.x - halfOfW;
            var posXOfRightSideOfTheSelection = transform.position.x + halfOfW;
            var posYOfBottomSideOfTheSelection = transform.position.y - halfOfH;
            var posYOfTopSideOfTheSelection = transform.position.y + halfOfH;

            var allScripts = Object.FindObjectsByType<Projectile>(FindObjectsSortMode.None);
            foreach (var proj in allScripts)
            {
                if (IsInsideSelection(proj.transform, posXOfLeftSideOfTheSelection, posXOfRightSideOfTheSelection, posYOfBottomSideOfTheSelection, posYOfTopSideOfTheSelection))
                    return proj.gameObject;
            }
            
            var allApples = Object.FindObjectsByType<ItemApple>(FindObjectsSortMode.None);
            foreach (var apple in allApples)
            {
                if (IsInsideSelection(apple.transform, posXOfLeftSideOfTheSelection, posXOfRightSideOfTheSelection, posYOfBottomSideOfTheSelection, posYOfTopSideOfTheSelection))
                    return apple.gameObject;
            }
            
            return null;
        }

        private bool IsInsideSelection(Transform target, float left, float right, float bottom, float top)
        {
            var targetLeft = target.position.x - target.lossyScale.x / 2;
            var targetRight = target.position.x + target.lossyScale.x / 2;
            var targetBottom = target.position.y - target.lossyScale.y / 2;
            var targetTop = target.position.y + target.lossyScale.y / 2;

            return left < targetLeft && targetRight < right && bottom < targetBottom && targetTop < top;
        }
    }
}