using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
namespace ZevWaxGames.CursorHero
{
    public class MapItem : MonoBehaviour
    {
        protected Vector2 offScreenPosition;
        protected Vector2 inScreenPosition;
        private CanvasGroup cg;
        protected virtual void OnEnable()
        {
            EventHolder.OnFadingInToPCStarted += HideInstantly;
            EventHolder.OnChoosingStarted += ShowMapNode;
            EventHolder.OnChoosingFinished += HideMapNode;
            EventHolder.OnPCFinished += ShowMapNode;
            EventHolder.OnPCStarted += HideMapNode;
        }
        protected virtual void OnDisable()
        {
            EventHolder.OnFadingInToPCStarted -= HideInstantly;
            EventHolder.OnChoosingStarted -= ShowMapNode;
            EventHolder.OnChoosingFinished -= HideMapNode;
            EventHolder.OnPCFinished -= ShowMapNode;
            EventHolder.OnPCStarted -= HideMapNode;
        }
        protected virtual void Start()
        {
            offScreenPosition = new Vector2(8.5f, transform.position.y);
            inScreenPosition = new Vector2(7.35f, transform.position.y);
            cg = GetComponent<CanvasGroup>();
        }
        protected IEnumerator HangingRoutine()
        {
            yield return new WaitForSeconds(((transform.position.y + 2.5f) / 5f) * 1.5f);
            while (true)
            {
                yield return transform.DORotate(new Vector3(0, 0, -10), 1.5f).SetEase(Ease.InOutQuad).WaitForCompletion();
                yield return transform.DORotate(new Vector3(0, 0, 10), 1.5f).SetEase(Ease.InOutQuad).WaitForCompletion();
            }
        }
        protected virtual void ShowMapNode() => cg.DOFade(1, 1);
        protected virtual void HideMapNode() => cg.DOFade(0.25f, 1);
        protected virtual void HideInstantly() => cg.DOFade(0.25f, 0);
        /*protected void ShowMapNode()
        {
            transform.DOMove(inScreenPosition, 1).SetEase(Ease.Linear);
        }
        protected void HideMapNode()
        {
            transform.DOMove(offScreenPosition, 1).SetEase(Ease.Linear);
        }
        protected void HideInstantly()
        {
            transform.DOMove(offScreenPosition, 0).SetEase(Ease.Linear);
        }*/
    }
}