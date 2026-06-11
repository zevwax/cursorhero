using System.Collections;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using TMPro;
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
        [SerializeField] private GameObject BIOS;
        public bool forThe1stTime = true;
        private GameObject[] btns;
        private GameObject[] biosBtns;
        private void Awake()
        {
            Instance = this;
            if (tryAgainWindow != null)
                tryAgainWindow.SetActive(false);
            btns = new GameObject[3];
            biosBtns = new GameObject[3+7+1];
        }
        private void Start()
        {
            StartCoroutine(FadeIn());
        }
        private void OnEnable()
        {
            EventHolder.OnRunStarted += HideStartGameNTryAgainWindows;
            EventHolder.OnYouWinStarted += HandleChoosingStarted;
            EventHolder.OnYouWinFinished += HideYouWinNChooseAnUpgradeWindows;
            EventHolder.OnRunFinished += HandleRunFinished;
            EventHolder.OnBIOSStarted += HandleBIOSStarted;
            EventHolder.OnBIOSFinished += HandleBIOSFinished;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= HideStartGameNTryAgainWindows;
            EventHolder.OnYouWinStarted -= HandleChoosingStarted;
            EventHolder.OnYouWinFinished -= HideYouWinNChooseAnUpgradeWindows;
            EventHolder.OnRunFinished -= HandleRunFinished;
            EventHolder.OnBIOSStarted -= HandleBIOSStarted;
            EventHolder.OnBIOSFinished -= HandleBIOSFinished;
        }
        public void ShowStartGameWindow()
        {
            startGameWindow.SetActive(true);
            btns[0] = Spawner.NewPlayButton(Vector2.zero);
        }
        private void HandleRunFinished()
        {
            StartCoroutine(CHandleRunFinished());
        }
        private IEnumerator CHandleRunFinished()
        {
            if (Power.Instance.CurrentLeftTime <= 0)
            {
                yield return StartCoroutine(PowerIsOut.Instance.StartFadeOutAnimation());
                PowerIsOut.Instance.StartInstantFadeInAnimation();
            }
            else
                yield return StartCoroutine(StartBSOD());
            EventHolder.OnBIOSStarted?.Invoke();
        }
        private void HandleBIOSStarted()
        {
            BIOS.GetComponent<CanvasGroup>().alpha = 1f;
            var index = 0;

            ShowChooseAnUpgradeWindow();
            
            if (biosBtns[index] != null)
                Destroy(biosBtns[index]);
            biosBtns[index] = Spawner.NewDriverButton(new Vector2(-4f, 4f));
            index++;
            
            if (biosBtns[index] != null)
                Destroy(biosBtns[index]);
            biosBtns[index] = Spawner.NewSkillTreeButton(new Vector2(0, 4f));
            index++;
            
            /*if (biosBtns[index] != null)
                Destroy(biosBtns[index]);
            biosBtns[index] = Spawner.NewInventoryButton(new Vector2(4f, 4f));
            index++;*/ //TEMP
            
            if (biosBtns[index] != null)
                Destroy(biosBtns[index]);
            biosBtns[index] = Spawner.NewFightButton(new Vector2(4f, -4f));
            index++;
            
            StartCoroutine(FastFadeIn());
        }
        private void HandleBIOSFinished()
        {
            StartCoroutine(FromBIOSToOS());
        }
        private IEnumerator FromBIOSToOS()
        {
            yield return StartCoroutine(FastFadeOut());
            
            BIOS.GetComponent<CanvasGroup>().alpha = 0;
            var index = 0;

            for (var i = 0; i < biosBtns.Length; i++)
                if (biosBtns[i] != null)
                    Destroy(biosBtns[i]);
            
            tryAgainWindow.SetActive(true);
            btns[0] = Spawner.NewPlayButton(Vector2.zero);
            
            yield return new WaitForSeconds(0.75f);
            StartCoroutine(FadeIn());
        }
        private IEnumerator StartBSOD()
        {
            yield return new WaitForSeconds(2f);
            BSOD.SetActive(true);
            yield return new WaitForSeconds(1.25f);
            InstantFadeOut();
            
            EventHolder.OnBSODStarted?.Invoke();
            BSOD.SetActive(false);
        }
        public void ShowYouWinWindow()
        {
            youWinWindow.SetActive(true);
        }
        private void HandleChoosingStarted()
        {
            ShowYouWinWindow();
            StartCoroutine(ShowTheEnd());
        }
        public void ShowChooseAnUpgradeWindow()
        {
            if (MainCharacter.Instance.Drivers <= 0) return;
            
            var chooseAnUpgradeWindowName =
                chooseAnUpgradeWindow.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            chooseAnUpgradeWindowName.text = string.Format("<sprite=5> Select Driver");
            chooseAnUpgradeWindow.SetActive(true);
            var buttons = GetThreeRandom
            (
                "Damage",
                "FireRate",
                "Speed",
                "Sensitivity"/*,
                "Heart",
                "HeartKeeper"*/
            );
            btns[0] = Spawner.NewUpgradeButton(buttons[0], new Vector2(-2.75f, 0f));
            btns[1] = Spawner.NewUpgradeButton(buttons[1], Vector2.zero);
            btns[2] = Spawner.NewUpgradeButton(buttons[2], new Vector2(2.75f, 0f));
        }
        public void ShowRecycleBinWindow()
        {
            var pos1 = new Vector2(0f, 1f);
            var pos2 = new Vector2(-2f, -1f);
            var pos3 = new Vector2(2f, -1f);
            if (forThe1stTime)
            {
                forThe1stTime = false;
                Zipporah.Instance.SwitchActionTo(7);
                
                Spawner.NewTutorialApple(pos1);
                Spawner.NewTutorialStaticGlyph(pos2);
            }
            else
            {
                switch (Random.Range(0, 4))
                {
                    case 0:
                        Spawner.NewApple(pos1);
                        break;
                    case 1:
                        Spawner.NewStaticGlyph(pos1);
                        break;
                    case 2:
                        Spawner.NewBottle(pos1);
                        break;
                    case 3:
                        //Nothing
                        break;
                }
                switch (Random.Range(0, 4))
                {
                    case 0:
                        Spawner.NewApple(pos2);
                        break;
                    case 1:
                        Spawner.NewStaticGlyph(pos2);
                        break;
                    case 2:
                        Spawner.NewBottle(pos2);
                        break;
                    case 3:
                        //Nothing
                        break;
                }
                switch (Random.Range(0, 4))
                {
                    case 0:
                        Spawner.NewApple(pos3);
                        break;
                    case 1:
                        Spawner.NewStaticGlyph(pos3);
                        break;
                    case 2:
                        Spawner.NewBottle(pos3);
                        break;
                    case 3:
                        //Nothing
                        break;
                }
            }
            recycleBinWindow.SetActive(true);
            EventHolder.OnBinStarted?.Invoke();
        }
        public void HideRecycleBinWindow()
        {
            recycleBinWindow.SetActive(false);
        }
        public void ShowSkillTreeTab()
        {
            var index = 4;

            if (DirPower.Unlocked)
            {
                if (biosBtns[index] != null)
                    Destroy(biosBtns[index]);
                biosBtns[index] = Spawner.NewDirPower(new Vector2(-2.75f-2.75f, 1f));
                index++;
            }

            if (DirMainCharDamage.Unlocked)
            {
                if (biosBtns[index] != null)
                    Destroy(biosBtns[index]);
                biosBtns[index] = Spawner.NewDirMainCharDamage(new Vector2(-2.75f, 2f));
                index++;
            }

            if (DirPetQuantity.Unlocked)
            {
                if (biosBtns[index] != null)
                    Destroy(biosBtns[index]);
                biosBtns[index] = Spawner.NewDirPetQuantity(new Vector2(-2.75f, 0f));
                index++;
            }

            if (DirPetHealth.Unlocked)
            {
                if (biosBtns[index] != null)
                    Destroy(biosBtns[index]);
                biosBtns[index] = Spawner.NewDirPetHealth(new Vector2(0f, 1f));
                index++;
            }

            if (DirPetDamage.Unlocked)
            {
                if (biosBtns[index] != null)
                    Destroy(biosBtns[index]);
                biosBtns[index] = Spawner.NewDirPetDamage(new Vector2(0, -1f));
                index++;
            }

            if (DirPetMovementSpeed.Unlocked)
            {
                if (biosBtns[index] != null)
                    Destroy(biosBtns[index]);
                biosBtns[index] = Spawner.NewDirPetMovementSpeed(new Vector2(2.75f, 1f));
                index++;
            }

            if (DirPetAttackInterval.Unlocked)
            {
                if (biosBtns[index] != null)
                    Destroy(biosBtns[index]);
                biosBtns[index] = Spawner.NewDirPetAttackInterval(new Vector2(2.75f, -1f));
                index++;
            }
        }
        public void HideSkillTreeTab()
        {
            for (var i = 0; i < 7; i++)
                if (biosBtns[i+4] != null)
                    Destroy(biosBtns[i+4]);
        }
        public void HideYouWinNChooseAnUpgradeWindows()
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
            Zipporah.Instance.SwitchActionTo(10);
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
        private string wallpaperDirectory = "My/My/Sprites/wallpaper";
        private void ResetWallpapers()
        {
            var wallpapersBG = GameObject.Find("Wallpapers BG").GetComponent<Image>();
            
            var allWallpapers = Resources.LoadAll<Sprite>(wallpaperDirectory);
            var targetName = "wallpaper_16";
            var specificSprite = System.Array.Find(allWallpapers, s => s.name == targetName);
            
            if (specificSprite != null)
                wallpapersBG.sprite = specificSprite;
            else
                Debug.LogWarning($"Sprite with name {targetName} not found in {wallpaperDirectory}");
        }
        private void RefreshWallpapers()
        {
            Projectile.RefreshWallpapers1();
            Projectile.RefreshWallpapers2();
            
            var allWallpapers = Resources.LoadAll<Sprite>(wallpaperDirectory);
            var subSprites = System.Array.FindAll(allWallpapers, s => s.name != "wallpaper");

            if (subSprites != null && subSprites.Length > 0)
            {
                var randomIndex = Random.Range(0, subSprites.Length);
                var newWallpaper = subSprites[randomIndex];
                var wallpapersBG = GameObject.Find("Wallpapers BG").GetComponent<Image>();
                wallpapersBG.sprite = newWallpaper;
            }
            else
                Debug.LogWarning($"No sliced sprites found for texture at Resources/{wallpaperDirectory}");
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