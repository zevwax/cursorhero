using TMPro;
using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class MMIndicatorColor : MonoBehaviour
    {
        public static MMIndicatorColor Instance { get; private set; }
        public void Init() => Reset();
        public void Reset() => GetComponent<TextMeshProUGUI>().text = "<sprite=6> 0";
        public void UpdateContents() => GetComponent<TextMeshProUGUI>().text = string.Format("<sprite=6> {0}", MainCharacter.Instance.MMs);
        private void Awake() => Instance = this;
    }
}