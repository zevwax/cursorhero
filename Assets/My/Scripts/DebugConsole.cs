using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ZevWaxGames.CursorHero
{
    public class DebugConsole : MonoBehaviour
    {
        private TextMeshProUGUI debugText;

        void Start()
        {
            debugText = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            string info = "DEBUG DATA\n";
            /*info += $"Target World Pos: {GameObject.Find("CursorHero")?.GetComponent<Cursor>().targetWorldPos}\n";*/
            info += $"Lock State: {UnityEngine.Cursor.lockState}\n";
            info += $"Resolution: {Screen.width}x{Screen.height}";

            debugText.text = info;
        }
    }
}