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
        var currChar = MainCharacter.Instance.ClipboardGlyph.GetComponent<Projectile>().CurrChar();
        GetComponent<TextMeshProUGUI>().text =
            string.Format(
                "{0}.glyph\nWeight: {1:F1} + {2:F1} / Size: {3:F1}",
                currChar, mainChar.weight, MainCharacter.Instance.WeightBuff, mainChar.size);
    }
}