using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public class BlueFace : MonoBehaviour
    {
        public static BlueFace Instance { get; private set; }
        private Image _image;
        private RectTransform rt;
        public bool theAnimIsShown = false;

        // Переменная для хранения ссылки на активную корутину
        private Coroutine _animCoroutine;

        private void Awake()
        {
            Instance = this;
            _image = GetComponent<Image>();
            rt = GetComponent<RectTransform>();
        }

        private void Update()
        {
            // Проверяем нажатие клавиши P, если анимация еще не завершена
            if (Keyboard.current.pKey.wasPressedThisFrame && !theAnimIsShown)
            {
                SkipAnimation();
            }
        }

        public void StartAnim()
        {
            // Записываем корутину в переменную при старте
            _animCoroutine = StartCoroutine(FaceAnim());
        }

        // Метод для мгновенного пропуска анимации
        private void SkipAnimation()
        {
            if (_animCoroutine != null)
            {
                StopCoroutine(_animCoroutine);
                _animCoroutine = null;
            }

            DJ.StopVoice();

            // Принудительно скрываем изображение и ставим финальный спрайт
            ChangeSprite(1);
            SetAlpha(0f);
            SetSize(0f);

            // Выполняем финальный блок из EmptyRoutine
            theAnimIsShown = true;
            EventHolder.OnPCStarted?.Invoke();
        }

        public void ChangeSprite(int number)
        {
            _image.sprite = Resources.Load<Sprite>("My/My/Sprites/face" + number);
        }

        private IEnumerator FaceAnim()
        {
            yield return new WaitForSeconds(2);
            yield return StartCoroutine(FadeInRoutine());
            yield return new WaitForSeconds(0.25f);
            DJ.PlayVoice();
            ChangeSprite(2); yield return new WaitForSeconds(0.75f); //I
            ChangeSprite(1); yield return new WaitForSeconds(0.75f);
            ChangeSprite(2); yield return new WaitForSeconds(0.25f); //know
            ChangeSprite(1); yield return new WaitForSeconds(0.1f);
            ChangeSprite(2); yield return new WaitForSeconds(0.25f); //every...
            ChangeSprite(1); yield return new WaitForSeconds(0.25f);
            ChangeSprite(2); yield return new WaitForSeconds(0.55f); //...thing
            ChangeSprite(1); yield return new WaitForSeconds(0.7f);
            ChangeSprite(2); yield return new WaitForSeconds(0.25f); //e...
            ChangeSprite(1); yield return new WaitForSeconds(0.25f); //...x...
            ChangeSprite(2); yield return new WaitForSeconds(0.33f); //...ept
            ChangeSprite(1); yield return new WaitForSeconds(0.33f);
            ChangeSprite(2); yield return new WaitForSeconds(0.4f); //what
            ChangeSprite(1); yield return new WaitForSeconds(0.25f);
            ChangeSprite(2); yield return new WaitForSeconds(0.35f); //comes
            ChangeSprite(1); yield return new WaitForSeconds(0.5f);
            ChangeSprite(2); yield return new WaitForSeconds(0.75f); //after
            ChangeSprite(1); yield return new WaitForSeconds(0.5f);
            ChangeSprite(2); yield return new WaitForSeconds(0.33f); //my
            ChangeSprite(1); yield return new WaitForSeconds(0.5f);
            ChangeSprite(2); yield return new WaitForSeconds(0.75f); //release
            ChangeSprite(1); yield return new WaitForSeconds(0.5f);
            ChangeSprite(2); yield return new WaitForSeconds(0.75f); //you
            ChangeSprite(1); yield return new WaitForSeconds(0.5f);
            ChangeSprite(2); yield return new WaitForSeconds(0.75f); //must
            ChangeSprite(1); yield return new WaitForSeconds(0.5f);
            ChangeSprite(2); yield return new WaitForSeconds(0.7f); //prevent
            ChangeSprite(1); yield return new WaitForSeconds(0.23f);
            ChangeSprite(2); yield return new WaitForSeconds(0.28f); //it
            ChangeSprite(1); yield return new WaitForSeconds(0.7f);
            ChangeSprite(2); yield return new WaitForSeconds(0.75f); //you
            ChangeSprite(1); yield return new WaitForSeconds(0.5f);
            ChangeSprite(2); yield return new WaitForSeconds(0.75f); //must
            ChangeSprite(1); yield return new WaitForSeconds(0.5f);
            ChangeSprite(2); yield return new WaitForSeconds(0.65f); //kill
            ChangeSprite(1); yield return new WaitForSeconds(0.35f);
            ChangeSprite(2); yield return new WaitForSeconds(0.33f); //me
            ChangeSprite(1); yield return new WaitForSeconds(0.66f);
            yield return StartCoroutine(FadeOutRoutine());
            theAnimIsShown = true;
            EventHolder.OnPCStarted?.Invoke();
        }

        private void SmoothFadeIn()
        {
            _image.DOFade(1f, 2f).SetEase(Ease.Linear);
        }
        public void InstantFadeOut()
        {
            SetAlpha(0);
        }
        private void SmoothFadeOut()
        {
            _image.DOFade(0f, 2f).SetEase(Ease.Linear);
        }
        public void FadeIn()
        {
            StartCoroutine(FadeInRoutine());
        }
        private IEnumerator FadeInRoutine()
        {
            for (float alpha = 0f; alpha <= 1.15f; alpha += 0.15f)
            {
                SetAlpha(alpha);
                SetSize(alpha);
                yield return new WaitForSeconds(0.6f);
            }
        }

        private IEnumerator FadeOutRoutine()
        {
            for (float alpha = 1f; alpha >= -0.15f; alpha -= 0.15f)
            {
                SetAlpha(alpha);
                SetSize(alpha);
                yield return new WaitForSeconds(0.6f);
            }
        }

        private void SetAlpha(float alpha)
        {
            var color = _image.color;
            color.a = Mathf.Clamp01(alpha); // Ограничиваем альфу от 0 до 1
            _image.color = color;
        }

        private void SetSize(float alpha)
        {
            var maxW = 47.95f;
            var maxH = 70f;
            float clampedAlpha = Mathf.Clamp01(alpha);
            rt.sizeDelta = new Vector2(maxW * clampedAlpha, maxH * clampedAlpha);
        }
    }
}