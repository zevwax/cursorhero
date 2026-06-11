using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections; // Required for Coroutines

namespace ZevWaxGames.CursorHero
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private float smoothTime = 0.25f;
        private RectTransform _rectTransform;
        private Vector3 _lastTargetOffset;
        private CanvasGroup _canvasGroup;
        private Coroutine _fadeCoroutine;
        private float fadeDuration = 0.33f;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
        }
        private void Start()
        {
            FadeIn();
            
            if (MainCharacter.Instance == null) return;
            var charPos = MainCharacter.Instance.transform.position;
            var xOffset = 1.5f;
            var yOffset = 1f;
            var x = charPos.x < 0 ? xOffset : -xOffset;
            var y = charPos.y < 0 ? yOffset : -yOffset;
            Vector3 targetOffset = new Vector3(x, y, 0);
            Vector3 targetWorldPos = charPos + targetOffset;
            _rectTransform.DOMove(targetWorldPos, 0).SetEase(Ease.OutCubic);
        }
        private void Update()
        {
            _rectTransform.DOMove(GetCurrPos(), smoothTime).SetEase(Ease.OutCubic);
        }
        public void FadeIn() => Fade(1);
        public void FadeOut() => Fade(0);
        private void Fade(float targetAlpha)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(FadeCoroutine(targetAlpha));
        }
        public IEnumerator FadeCoroutine(float targetAlpha)
        {
            float startAlpha = _canvasGroup.alpha;
            float time = 0;
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
                yield return null;
            }
            _canvasGroup.alpha = targetAlpha;
        }
        public void Die()
        {
            StartCoroutine(DieCoroutine());
        }
        private IEnumerator DieCoroutine()
        {
            FadeOut();
            yield return new WaitForSeconds(fadeDuration);
            Destroy(gameObject);
        }
        private Vector3 GetCurrPos()
        {
            var tooltipMaxPosX = 1.5f;
            var tooltipMaxPosY = 1f;
            var mainCharMaxPosX = 7.701668f;
            var mainCharMaxPosY = 4.118333f;
            var mainCharPos = MainCharacter.Instance.transform.position;
            var x = (mainCharPos.x / mainCharMaxPosX) * -tooltipMaxPosX;
            var y = mainCharPos.y < 0 ? tooltipMaxPosY : -tooltipMaxPosY;
            return mainCharPos + new Vector3(x, y, 0);
        }
        private void OnEnable()
        {
            EventHolder.OnBIOSFinished += Die;
        }
        private void OnDisable()
        {
            EventHolder.OnBIOSFinished -= Die;
        }
    }
}