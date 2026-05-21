using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class ItemApple : MonoBehaviour
    {
        private void OnEnable()
        {
            EventHolder.OnFadingInToPCStarted += Die;
        }
        private void OnDisable()
        {
            EventHolder.OnFadingInToPCStarted -= Die;
        }
        private void Die()
        {
            Destroy(gameObject);
        }
        public void Init()
        {
            var tooltip = string.Format("apple.png\nCut to eat");
            GetComponent<TooltipHolder>().SetText(tooltip);
        }
    }
}