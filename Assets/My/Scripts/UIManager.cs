using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        [SerializeField] private GameObject tryAgainWindow;
        [SerializeField] private GameObject chooseAnUpgradeWindow;
        private GameObject[] btns;
        private void Awake()
        {
            if (tryAgainWindow != null)
                tryAgainWindow.SetActive(false);
            btns = new GameObject[3];
        }
        private void Start()
        {
            Instance = this;
        }
        private void OnEnable()
        {
            EventHolder.OnPlayerDie += ShowTryAgainWindow;
            EventHolder.OnChoosingStarted += ShowChooseAnUpgradeWindow;
            EventHolder.OnChoosingFinished += HideChooseAnUpgradeWindow;
            EventHolder.OnRunStarted += HideTryAgainWindow;
        }
        private void OnDisable()
        {
            EventHolder.OnPlayerDie -= ShowTryAgainWindow;
            EventHolder.OnChoosingStarted -= ShowChooseAnUpgradeWindow;
            EventHolder.OnChoosingFinished -= HideChooseAnUpgradeWindow;
            EventHolder.OnRunStarted -= HideTryAgainWindow;
        }
        private void ShowTryAgainWindow()
        {
            tryAgainWindow.SetActive(true);
            btns[0] = Spawner.NewPlayButton(Vector2.zero);
        }
        private void ShowChooseAnUpgradeWindow()
        {
            chooseAnUpgradeWindow.SetActive(true);
            btns[0] = Spawner.NewUpgradeButton("Damage", new Vector2(-2f, 0f));
            btns[1] = Spawner.NewUpgradeButton("FireRate", Vector2.zero);
            btns[2] = Spawner.NewUpgradeButton("Speed", new Vector2(2f, 0f));
        }
        private void HideChooseAnUpgradeWindow()
        {
            chooseAnUpgradeWindow.SetActive(false);
            Destroy(btns[0]);
            Destroy(btns[1]);
            Destroy(btns[2]);
        }
        private void HideTryAgainWindow()
        {
            tryAgainWindow.SetActive(false);
            Destroy(btns[0]);
        }
    }
}