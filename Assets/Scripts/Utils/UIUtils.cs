using UnityEngine;
using System.Collections;

public static class UIUtils
{
    // CanvasGroup의 Alpha 값을 부드럽게 변경하여 페이드 효과를 줍니다.
    // 'this' 키워드를 사용하여 모든 CanvasGroup에 .Fade() 메서드를 '확장'합니다.
    // 사용 예: myPanelCanvasGroup.Fade(1f, 0.3f); // 0.3초 동안 나타나기
    public static Coroutine Fade(this CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        // Coroutine을 시작하려면 MonoBehaviour 인스턴스가 필요하므로, GameManager 등을 활용
        return GameManager.Instance.StartCoroutine(FadeRoutine(canvasGroup, targetAlpha, duration));
    }

    private static IEnumerator FadeRoutine(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}