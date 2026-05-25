using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class TabbySelection : Selection
    {
        protected override bool CanStartSelection()
        {
            return TabbyPointer.Instance != null;
        }

        protected override Vector3 GetPointerPos()
        {
            return TabbyPointer.Instance.transform.position + new Vector3(0, 0.1f, 0);
        }

        protected override void OnStartSelecting()
        {
        }

        protected override void OnFinishSelecting()
        {
        }

        protected override void Update()
        {
        }

        public void StartSelectionFromPointer(Vector3 startPos)
        {
            isActive = true;
            _startPos = startPos;
            OnStartSelecting();
            UpdateSelectionArea();
        }

        public void UpdateSelectionArea()
        {
            if (_rectTransform == null) return;

            Vector3 currentPointerPos = GetPointerPos();
            
            transform.position = (_startPos + currentPointerPos) / 2f;
            
            float widthUnits = Mathf.Abs(_startPos.x - currentPointerPos.x);
            float heightUnits = Mathf.Abs(_startPos.y - currentPointerPos.y);

            _rectTransform.sizeDelta = new Vector2(widthUnits * PixelsPerUnit, heightUnits * PixelsPerUnit);
        }

        public void FinishSelectionFromPointer()
        {
            OnFinishSelecting();
            isActive = false;
            if (_rectTransform != null)
            {
                _rectTransform.sizeDelta = Vector2.zero;
            }
        }
    }
}