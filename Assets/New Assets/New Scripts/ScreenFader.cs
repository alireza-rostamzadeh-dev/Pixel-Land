using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;

    public IEnumerator FadeOutIn(float duration)
    {
        Time.timeScale = 0f; 

        yield return StartCoroutine(Fade(0f, 1f, duration / 2));

        yield return new WaitForSecondsRealtime(0.2f); 

        yield return StartCoroutine(Fade(1f, 0f, duration / 2));

        Time.timeScale = 1f; 
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;  
            c.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            fadeImage.color = c;
            yield return null;
        }
    }
}
