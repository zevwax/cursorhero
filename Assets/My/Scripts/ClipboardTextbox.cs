using TMPro;
using UnityEngine;
using ZevWaxGames.CursorHero;

public class ClipboardTextbox : MonoBehaviour
{
    public static ClipboardTextbox Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }
    public void UpdateContents()
    {
        var mainChar = MainCharacter.Instance;
        GetComponent<TextMeshProUGUI>().text =
            string.Format(
                "A.glyph\nWeight: {0:F1} + {1:F1} / Size: {2:F1}",
                mainChar.edge, MainCharacter.Instance.WeightBuff, mainChar.size);
    }
}