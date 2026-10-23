
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;

    private bool isFading;

    public IEnumerator FadeOutIn(float duration)
    {
        if (isFading || fadeImage == null)
            yield break;

        isFading = true;

        float previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        duration = Mathf.Max(0f, duration);

        yield return StartCoroutine(Fade(0f, 1f, duration / 2f));

        yield return new WaitForSecondsRealtime(0.2f);

        yield return StartCoroutine(Fade(1f, 0f, duration / 2f));

        Time.timeScale = previousTimeScale;
        isFading = false;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        Color color = fadeImage.color;

        if (duration <= 0f)
        {
            color.a = endAlpha;
            fadeImage.color = color;
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            color.a = Mathf.Lerp(
                startAlpha,
                endAlpha,
                Mathf.Clamp01(elapsed / duration)
            );

            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }
}
