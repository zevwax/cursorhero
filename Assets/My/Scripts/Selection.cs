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
            _rectTransform.sizeDelta = Vector2.zero;
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
            var o = GetSelectedObject();
            if (o != null)
                if (o.GetComponent<LayingPieceOfGlass>() != null)
                {
                    MainCharacter.Instance.edge = o.GetComponent<LayingPieceOfGlass>().Edge;
                    MainCharacter.Instance.size = o.GetComponent<LayingPieceOfGlass>().Size;
                    ClipboardTextbox.Instance.UpdateContents();
                    Spawner.NewDamageNumbers(transform.position, "Copied projectile.png");
                }
                else if (o.GetComponent<Apple>() != null)
                {
                    MainCharacter.Instance.HP = MainCharacter.Instance.MaxHP;
                    Spawner.NewDamageNumbers(transform.position, "HP Restored");
                    Destroy(o);
                }
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
        private GameObject GetSelectedObject()
        {
            var allScripts = Object.FindObjectsByType<LayingPieceOfGlass>(FindObjectsSortMode.None);
            var ppu = 30;
            var halfOfW = (_rectTransform.sizeDelta.x / ppu) * 0.5f;
            var halfOfH = (_rectTransform.sizeDelta.y / ppu) * 0.5f;
            foreach (var piece in allScripts)
            {
                var posXOfLeftSideOfThePiece = piece.transform.position.x - piece.transform.lossyScale.x / 2;
                var posXOfRightSideOfThePiece = piece.transform.position.x + piece.transform.lossyScale.x / 2;
                var posYOfBottomSideOfThePiece = piece.transform.position.y - piece.transform.lossyScale.y / 2;
                var posYOfTopSideOfThePiece = piece.transform.position.y + piece.transform.lossyScale.y / 2;
                var posXOfLeftSideOfTheSelection = transform.position.x - halfOfW;
                var posXOfRightSideOfTheSelection = transform.position.x + halfOfW;
                var posYOfBottomSideOfTheSelection = transform.position.y - halfOfH;
                var posYOfTopSideOfTheSelection = transform.position.y + halfOfH;
                if
                (
                    posXOfLeftSideOfTheSelection < posXOfLeftSideOfThePiece &&
                    posXOfRightSideOfThePiece < posXOfRightSideOfTheSelection &&
                    posYOfBottomSideOfTheSelection < posYOfBottomSideOfThePiece &&
                    posYOfTopSideOfThePiece < posYOfTopSideOfTheSelection
                )
                    return piece.gameObject;
            }
            
            var allApples = Object.FindObjectsByType<Apple>(FindObjectsSortMode.None);
            foreach (var apple in allApples)
            {
                var posXOfLeftSideOfThePiece = apple.transform.position.x - apple.transform.lossyScale.x / 2;
                var posXOfRightSideOfThePiece = apple.transform.position.x + apple.transform.lossyScale.x / 2;
                var posYOfBottomSideOfThePiece = apple.transform.position.y - apple.transform.lossyScale.y / 2;
                var posYOfTopSideOfThePiece = apple.transform.position.y + apple.transform.lossyScale.y / 2;
                var posXOfLeftSideOfTheSelection = transform.position.x - halfOfW;
                var posXOfRightSideOfTheSelection = transform.position.x + halfOfW;
                var posYOfBottomSideOfTheSelection = transform.position.y - halfOfH;
                var posYOfTopSideOfTheSelection = transform.position.y + halfOfH;
                if
                (
                    posXOfLeftSideOfTheSelection < posXOfLeftSideOfThePiece &&
                    posXOfRightSideOfThePiece < posXOfRightSideOfTheSelection &&
                    posYOfBottomSideOfTheSelection < posYOfBottomSideOfThePiece &&
                    posYOfTopSideOfThePiece < posYOfTopSideOfTheSelection
                )
                    return apple.gameObject;
            }
            
            return null;
        }
    }
}