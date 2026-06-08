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
            return TabbyPointer.Instance.transform.position; //+ new Vector3(0, 0.1f, 0);
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
            UpdateLayout();
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