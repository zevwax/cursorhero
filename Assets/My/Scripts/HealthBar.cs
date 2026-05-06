using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class HealthBar : MonoBehaviour
    {
        private RectTransform foregroundRT;
        private float fullWidth;

        public void Setup(RectTransform fgRect)
        {
            foregroundRT = fgRect;
            fullWidth = GetComponent<RectTransform>().sizeDelta.x;
        }

        private void Update()
        {
            if (MainCharacter.Instance != null && foregroundRT != null)
            {
                transform.position = MainCharacter.Instance.transform.position + new Vector3(0, -0.5f, 0);
                
                float healthRatio = Mathf.Clamp01(MainCharacter.Instance.HP / MainCharacter.Instance.MaxHP);
                float rightMargin = (1f - healthRatio) * -fullWidth;

                foregroundRT.offsetMax = new Vector2(rightMargin, foregroundRT.offsetMax.y);
            }
        }
    }
}