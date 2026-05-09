using UnityEngine;
using UnityEngine.UI;
namespace ZevWaxGames.CursorHero
{
    public class BloodCanvas : MonoBehaviour
    {
        public void Refresh()
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("My/My/Sprites/blood_canvas");
        }
    }
}