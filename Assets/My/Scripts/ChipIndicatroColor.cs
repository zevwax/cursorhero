using TMPro;
using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class ChipIndicatroColor : MonoBehaviour
    {
        public static ChipIndicatroColor Instance { get; private set; }
        public void Init() => Reset();
        public void Reset() => GetComponent<TextMeshProUGUI>().text = "<sprite=6> 0";
        public void UpdateContents() => GetComponent<TextMeshProUGUI>().text = string.Format("<sprite=6> {0}", MainCharacter.Instance.Chips);
        private void Awake() => Instance = this;
    }
}