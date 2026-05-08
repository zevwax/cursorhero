using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
namespace ZevWaxGames.CursorHero
{
    public class Selection : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Image _image;
        private Vector3 _startPos;
        private Sprite[] _sprites;
        private const float PixelsPerUnit = 30f;
        private bool isActive = false;
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = transform.GetChild(0).GetComponent<Image>();
            _startPos = GetPointerPos();
            _sprites = new Sprite[3];
            _sprites[0] = Resources.Load<Sprite>("My/My/Sprites/s1");
            _sprites[1] = Resources.Load<Sprite>("My/My/Sprites/s2");
            _sprites[2] = Resources.Load<Sprite>("My/My/Sprites/s3");
            StartCoroutine(AnimateSprites());
        }
        private void Update()
        {
            if (!MainCharacter.Instance.IsAbleForReskinBy(gameObject)) return;
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

        public void FinishSelecting()
        {
            isActive = false;
            _rectTransform.sizeDelta = Vector2.zero;
            MainCharacter.Instance.SetGlove(gameObject);
        }
        public void StartSelecting()
        {
            isActive = true;
            _startPos = GetPointerPos();
            MainCharacter.Instance.SetCross(gameObject);
        }
        private Vector3 GetPointerPos() => MainCharacter.Instance.transform.position + new Vector3(0, 0.1f, 0);
    }
}