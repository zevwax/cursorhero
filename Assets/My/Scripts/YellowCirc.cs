using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class YellowCirc : MonoBehaviour
    {
        private void Update()
        {
            if (MainCharacter.Instance != null)
                GetComponent<RectTransform>().position = MainCharacter.Instance.transform.position;
        }
    }
}