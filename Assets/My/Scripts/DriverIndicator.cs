using TMPro;
using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class DriverIndicator : MonoBehaviour
    {
        public static DriverIndicator Instance { get; private set; }
        public void Init() => Reset();
        public void Reset() => GetComponent<TextMeshProUGUI>().text = "<sprite=5> 0";
        public void UpdateContents() => GetComponent<TextMeshProUGUI>().text = string.Format("<sprite=5> {0}", MainCharacter.Instance.Drivers);
        private void Awake() => Instance = this;
    }
}