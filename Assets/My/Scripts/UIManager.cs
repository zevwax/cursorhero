using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        private GameObject playBtn;
        private void Awake()
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);
        }
        private void OnEnable()
        {
            EventHolder.OnPlayerDie += ShowGameOver;
            EventHolder.OnRunStarted += HideGameOver;
        }
        private void OnDisable()
        {
            EventHolder.OnPlayerDie -= ShowGameOver;
            EventHolder.OnRunStarted -= HideGameOver;
        }
        private void ShowGameOver()
        {
            gameOverPanel.SetActive(true);
            playBtn = Spawner.NewPlayButton(Vector2.zero);
        }
        private void HideGameOver()
        {
            gameOverPanel.SetActive(false);
            Destroy(playBtn);
        }
    }
}