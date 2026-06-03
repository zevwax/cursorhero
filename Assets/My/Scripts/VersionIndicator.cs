using TMPro;
using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class VersionIndicator : MonoBehaviour
    {
        public static VersionIndicator Instance { get; private set; }
        public void Init() => Reset();
        public void Reset() => GetComponent<TextMeshProUGUI>().text = "v.1";
        public void UpdateContents() => GetComponent<TextMeshProUGUI>().text = string.Format("v.{0}", MainCharacter.Instance.Version);
        private void Awake() => Instance = this;
    }
}