using UnityEngine;
using DG.Tweening;
using System.Collections;

public class TweenEntity : MonoBehaviour
{
    public void Init(Vector2 to, float duration) => StartCoroutine(CInit(to, duration));
    private IEnumerator CInit(Vector2 to, float duration)
    {
        yield return transform.DOMove(to, duration).SetEase(Ease.OutQuad).WaitForCompletion();
        Destroy(gameObject);
    }
}