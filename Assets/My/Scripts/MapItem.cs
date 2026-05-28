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
        /*private void OnEnable()
        {
            EventHolder.OnFadingInToPCStarted += Hide;
            EventHolder.OnChoosingStarted += ShowMapNode;
            EventHolder.OnChoosingFinished += HideMapNode;
            EventHolder.OnPCFinished += ShowMapNode;
            EventHolder.OnPCStarted += HideMapNode;
        }
        private void OnDisable()
        {
            EventHolder.OnFadingInToPCStarted -= Hide;
            EventHolder.OnChoosingStarted -= ShowMapNode;
            EventHolder.OnChoosingFinished -= HideMapNode;
            EventHolder.OnPCFinished -= ShowMapNode;
            EventHolder.OnPCStarted -= HideMapNode;
        }*/
        protected virtual void Start()
        {
            offScreenPosition = new Vector2(8.5f, transform.position.y);
            inScreenPosition = new Vector2(7.35f, transform.position.y);
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
        protected void ShowMapNode()
        {
            transform.DOMove(inScreenPosition, 1).SetEase(Ease.Linear);
        }
        protected void HideMapNode()
        {
            transform.DOMove(offScreenPosition, 1).SetEase(Ease.Linear);
        }
        protected void Hide()
        {
            transform.DOMove(offScreenPosition, 0).SetEase(Ease.Linear);
        }
    }
}