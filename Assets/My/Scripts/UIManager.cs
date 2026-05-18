using System.Collections;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        [SerializeField] private GameObject startGameWindow;
        [SerializeField] private GameObject BSOD;
        [SerializeField] private GameObject tryAgainWindow;
        [SerializeField] private GameObject chooseAnUpgradeWindow;
        [SerializeField] private GameObject recycleBinWindow;
        [SerializeField] private GameObject youWinWindow;
        private GameObject[] btns;
        private void Awake()
        {
            Instance = this;
            if (tryAgainWindow != null)
                tryAgainWindow.SetActive(false);
            btns = new GameObject[3];
        }
        private void Start()
        {
            StartCoroutine(FadeIn());
        }
        private void OnEnable()
        {
            EventHolder.OnRunStarted += HideStartGameNTryAgainWindows;
            EventHolder.OnChoosingStarted += HandleChoosingStarted;
            EventHolder.OnChoosingFinished += HideYouWinNChooseAnUpgradeWindows;
            EventHolder.OnRunFinished += ShowTryAgainWindow;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= HideStartGameNTryAgainWindows;
            EventHolder.OnChoosingStarted -= HandleChoosingStarted;
            EventHolder.OnChoosingFinished -= HideYouWinNChooseAnUpgradeWindows;
            EventHolder.OnRunFinished -= ShowTryAgainWindow;
        }
        public void ShowStartGameWindow()
        {
            startGameWindow.SetActive(true);
            btns[0] = Spawner.NewPlayButton(Vector2.zero);
        }
        private void ShowTryAgainWindow()
        {
            StartCoroutine(StartBSOD());
        }
        private IEnumerator StartBSOD()
        {
            yield return new WaitForSeconds(2f);
            BSOD.SetActive(true);
            yield return new WaitForSeconds(1.25f);
            InstantFadeOut();
            
            BSOD.SetActive(false);
            tryAgainWindow.SetActive(true);
            btns[0] = Spawner.NewPlayButton(Vector2.zero);
            
            yield return new WaitForSeconds(0.75f);
            StartCoroutine(FadeIn());
        }
        public void ShowYouWinWindow()
        {
            youWinWindow.SetActive(true);
        }
        private void HandleChoosingStarted()
        {
            if (ProgressBar.Instance.Value >= 1f)
                ShowChooseAnUpgradeWindow();
            else
            {
                ShowYouWinWindow();
                StartCoroutine(ShowTheEnd());
            }
        }
        private void ShowChooseAnUpgradeWindow()
        {
            chooseAnUpgradeWindow.SetActive(true);
            var buttons = GetThreeRandom
            (
                "Damage",
                "FireRate",
                "Speed",
                "Sensitivity",
                "Heart"
            );
            btns[0] = Spawner.NewUpgradeButton(buttons[0], new Vector2(-2.75f, 0f));
            btns[1] = Spawner.NewUpgradeButton(buttons[1], Vector2.zero);
            btns[2] = Spawner.NewUpgradeButton(buttons[2], new Vector2(2.75f, 0f));
        }
        public void ShowRecycleBinWindow()
        {
            recycleBinWindow.SetActive(true);
            Spawner.NewBottle(new Vector2(-2f, -1f));
            Spawner.NewApple(new Vector2(0f, 1f));
            Spawner.NewNewspaper(new Vector2(2f, -1f));
            EventHolder.OnBinStarted?.Invoke();
        }
        public void HideRecycleBinWindow()
        {
            recycleBinWindow.SetActive(false);
        }
        private void HideYouWinNChooseAnUpgradeWindows()
        {
            youWinWindow.SetActive(false);
            chooseAnUpgradeWindow.SetActive(false);
            if (btns[0] != null)
                Destroy(btns[0]);
            if (btns[1] != null)
                Destroy(btns[1]);
            if (btns[2] != null)
                Destroy(btns[2]);
        }
        public void HideStartGameNTryAgainWindows()
        {
            startGameWindow.SetActive(false);
            tryAgainWindow.SetActive(false);
            Destroy(btns[0]);
            ResetWallpapers();
        }
        public T[] GetThreeRandom<T>(params T[] source)
        {
            if (source.Length <= 3) return source;
            return source.OrderBy(x => UnityEngine.Random.value).Take(3).ToArray();
        }
        private IEnumerator ShowTheEnd()
        {
            var endCanvas = GameObject.Find("TheEndCanvas").GetComponent<CanvasGroup>();
            
            yield return new WaitForSeconds(5.5f);
            
            var alpha = 0.2f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha += 0.2f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha += 0.2f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha += 0.2f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha += 0.2f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            
            btns[0] = Spawner.NewEndlessModeButton(Vector2.zero);
            
            yield return new WaitForSeconds(5.5f);
            
            alpha -= 0.2f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha -= 0.2f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha -= 0.2f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha -= 0.2f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha -= 0.2f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
        }
        private IEnumerator FadeIn()
        {
            var endCanvas = GameObject.Find("FadeCanvas").GetComponent<CanvasGroup>();
            yield return new WaitForSeconds(1f);
            var alpha = 0.95f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha -= 0.1f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha -= 0.15f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha -= 0.2f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha -= 0.25f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            alpha -= 0.25f;
            yield return endCanvas.DOFade(alpha, 0).WaitForCompletion();
        }
        public void SwitchPC() => StartCoroutine(CSwitchPC());
        private IEnumerator CSwitchPC()
        {
            yield return FastFadeOut();
            EventHolder.OnFadingInToPCStarted?.Invoke();
            RefreshWallpapers();
            G.Instance.bin.DisableButton();
            G.Instance.net.DisableButton();
            HideRecycleBinWindow();
            yield return FastFadeIn();
            EventHolder.OnBinFinished?.Invoke();
            EventHolder.OnPCStarted?.Invoke();
        }
        private void InstantFadeOut()
        {
            var endCanvas = GameObject.Find("FadeCanvas").GetComponent<CanvasGroup>();
            endCanvas.DOFade(1f, 0);
        }
        private IEnumerator FastFadeOut()
        {
            var endCanvas = GameObject.Find("FadeCanvas").GetComponent<CanvasGroup>();
            yield return endCanvas.DOFade(0.25f, 0).WaitForCompletion(); yield return new WaitForSeconds(0.2f);
            yield return endCanvas.DOFade(0.5f, 0).WaitForCompletion(); yield return new WaitForSeconds(0.2f);
            yield return endCanvas.DOFade(0.7f, 0).WaitForCompletion(); yield return new WaitForSeconds(0.2f);
            yield return endCanvas.DOFade(0.85f, 0).WaitForCompletion(); yield return new WaitForSeconds(0.2f);
            yield return endCanvas.DOFade(0.95f, 0).WaitForCompletion(); yield return new WaitForSeconds(0.2f);
            yield return endCanvas.DOFade(1f, 0).WaitForCompletion(); yield return new WaitForSeconds(0.2f);
        }
        private string wallpaperDirectory = "My/My/Sprites/Wallpapers";
        private void ResetWallpapers()
        {
            var wallpapersBG = GameObject.Find("Wallpapers BG").GetComponent<Image>();
            wallpapersBG.sprite = Resources.Load<Sprite>(wallpaperDirectory + "/Bliss");
        }
        private void RefreshWallpapers()
        {
            Sprite[] allWallpapers = Resources.LoadAll<Sprite>(wallpaperDirectory);

            if (allWallpapers != null && allWallpapers.Length > 0)
            {
                var randomIndex = Random.Range(0, allWallpapers.Length);
                var newWallpapers = allWallpapers[randomIndex];
                var wallpapersBG = GameObject.Find("Wallpapers BG").GetComponent<Image>();
                wallpapersBG.sprite = newWallpapers;
            }
            else
                Debug.LogWarning($"No sprites found in Resources/{wallpaperDirectory}");
        }
        private IEnumerator FastFadeIn()
        {
            var endCanvas = GameObject.Find("FadeCanvas").GetComponent<CanvasGroup>();
            yield return endCanvas.DOFade(0.95f, 0).WaitForCompletion(); yield return new WaitForSeconds(0.2f);
            yield return endCanvas.DOFade(0.85f, 0).WaitForCompletion(); yield return new WaitForSeconds(0.2f);
            yield return endCanvas.DOFade(0.7f, 0).WaitForCompletion(); yield return new WaitForSeconds(0.2f);
            yield return endCanvas.DOFade(0.5f, 0).WaitForCompletion(); yield return new WaitForSeconds(0.2f);
            yield return endCanvas.DOFade(0.25f, 0).WaitForCompletion(); yield return new WaitForSeconds(0.2f);
            yield return endCanvas.DOFade(0, 0).WaitForCompletion();
        }
    }
}