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
        GetComponent<TextMeshProUGUI>().text = string.Format("projectile.png\nEdge: {0:F1} / Size: {1:F1}", mainChar.edge, mainChar.size);
    }
}