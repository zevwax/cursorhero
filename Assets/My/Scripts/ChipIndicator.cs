using TMPro;
using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class ChipIndicator : MonoBehaviour
    {
        public static ChipIndicator Instance { get; private set; }
        public void Init() => Reset();
        public void Reset() => GetComponent<TextMeshProUGUI>().text = "<sprite=7> 0";
        public void UpdateContents() => GetComponent<TextMeshProUGUI>().text = string.Format("<sprite=7> {0}", MainCharacter.Instance.Chips);
        private void Awake() => Instance = this;
    }
}