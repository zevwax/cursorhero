using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class Tooltip : MonoBehaviour
    {
        private void Update()
        {
            if (MainCharacter.Instance != null)
                GetComponent<RectTransform>().position = MainCharacter.Instance.transform.position + new Vector3(1, 1, 0);
        }
    }
}