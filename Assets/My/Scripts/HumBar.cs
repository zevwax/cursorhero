using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace ZevWaxGames.CursorHero
{
    public class HumBar : MonoBehaviour
    {
        public float speed = 0.6f;
        public float pixelOffsetY = 0f;
        public RawImage refRawImage;
        public RawImage rawImage;

        private RectTransform canvasRect;
        private RectTransform refCanvasRect;
        
        private const float TopY = 4.5f + (1f / 6f);
        private const float BottomY = -(4.5f + (1f / 6f));
        private const float ScreenHeight = 270f;
        private const float GlitchHeight = 10f;
        
        private bool isWaiting = false;

        public void Init()
        {
            refRawImage = GameObject.Find("DitheringRawImage").GetComponent<RawImage>();
            if (refRawImage != null)
            {
                refCanvasRect = refRawImage.GetComponent<RectTransform>();
            }
            
            rawImage = transform.GetChild(1).GetComponent<RawImage>();
            canvasRect = GetComponent<RectTransform>();
            
            if (refRawImage != null && refRawImage.texture != null && rawImage != null)
                rawImage.texture = refRawImage.texture;
        }

        void Update()
        {
            if (canvasRect == null || rawImage == null || refRawImage == null || refCanvasRect == null || isWaiting) return;

            // 1. Движение самой UI полоски
            Vector3 pos = canvasRect.localPosition;
            pos.y -= speed * Time.deltaTime;

            if (pos.y <= BottomY)
            {
                StartCoroutine(Waiter());
                return;
            }
            canvasRect.localPosition = pos;

            // 2. Честный расчёт UV через мировые координаты (убирает любой рассинхрон и непостоянство)
            Vector3[] refCorners = new Vector3[4];
            refCanvasRect.GetWorldCorners(refCorners);
            float refBottomWorld = refCorners[0].y;
            float refTopWorld = refCorners[1].y;
            float refWorldHeight = refTopWorld - refBottomWorld;

            Vector3[] myCorners = new Vector3[4];
            canvasRect.GetWorldCorners(myCorners);
            float myCenterWorld = (myCorners[0].y + myCorners[1].y) * 0.5f;

            // Находим точное положение полосы на экране от 0 до 1 без привязки к локальным TopY/BottomY
            float absoluteNormalizedY = (myCenterWorld - refBottomWorld) / refWorldHeight;

            // Переводим пиксельный оффсет в UV координату
            float uvOffsetY = pixelOffsetY / ScreenHeight;
            float uvHeight = GlitchHeight / ScreenHeight;
            
            // Финальный расчёт координаты Y для UV
            float calculatedUVY = absoluteNormalizedY - (uvHeight * 0.5f) + uvOffsetY;

            // 3. Применение параметров в UV Rect
            Rect masterUV = refRawImage.uvRect;
            Rect rect = rawImage.uvRect;
            
            rect.height = uvHeight * masterUV.height;
            rect.y = masterUV.y + (calculatedUVY * masterUV.height);
            rect.x = masterUV.x;
            rect.width = masterUV.width;

            rawImage.uvRect = rect;
        }
        private IEnumerator Waiter()
        {
            // ИЗМЕНЕНИЕ: Включаем режим ожидания
            isWaiting = true;
            
            yield return new WaitForSeconds(120f);
            
            // ИЗМЕНЕНИЕ: Исправили ошибку компиляции (создаем локальный resetPos, так как pos из Update тут недоступен)
            Vector3 resetPos = canvasRect.localPosition;
            resetPos.y = TopY;
            canvasRect.localPosition = resetPos;
            
            // ИЗМЕНЕНИЕ: Выключаем режим ожидания, позволяя Update снова двигать полосу
            isWaiting = false;
        }
    }
}