using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class DigitalRain : MonoBehaviour
    {
        private Image rawImage;
        private Material targetMaterial;
        private Texture2D[] ditherTextures;
        private int currentTextureIndex = 0;
        private const string ShaderTextureParam = "_Dither"; 

        void Awake()
        {
            rawImage = GetComponent<Image>();
            if (rawImage != null)
            {
                targetMaterial = rawImage.material;
            }
        }

        void Start()
        {
            LoadDitherTextures();
            if (targetMaterial != null && ditherTextures != null && ditherTextures.Length > 0)
            {
                StartCoroutine(AnimateDitherRoutine());
            }
        }

        private void LoadDitherTextures()
        {
            ditherTextures = new Texture2D[6];
            for (int i = 0; i < 6; i++)
            {
                string path = $"My/My/Dither/my_dither_{i + 1}";
                ditherTextures[i] = Resources.Load<Texture2D>(path);
            }
        }

        private IEnumerator AnimateDitherRoutine()
        {
            WaitForSeconds waitTime = new WaitForSeconds(0.5f);
            while (true)
            {
                targetMaterial.SetTexture(ShaderTextureParam, ditherTextures[currentTextureIndex]);
                currentTextureIndex = (currentTextureIndex + 1) % ditherTextures.Length;
                yield return waitTime;
            }
        }
    }
}