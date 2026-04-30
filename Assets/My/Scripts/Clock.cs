using UnityEngine;
using TMPro;

namespace ZevWaxGames.CursorHero
{
    public class Clock : MonoBehaviour
    {
        private TextMeshProUGUI clockText;
        private float elapsedTime;
        private void Start()
        {
            clockText = GetComponent<TextMeshProUGUI>();
        }
        private void Update()
        {
            elapsedTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);

            clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}