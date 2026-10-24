using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraBlink : MonoBehaviour
{
    [SerializeField] InputActionAsset action;
    [SerializeField] CanvasGroup imageAlpha;
    void Blink()
    {
        if (action.FindAction("Blink").IsPressed())
        {
            StartCoroutine(BlinkAlpha(0, 1));
            return;
        }

        if (action.FindAction("Blink").WasReleasedThisFrame())
        {
            StartCoroutine(UnblinkAlpha(0, 1));
            return;
        }
    }
    IEnumerator UnblinkAlpha(float initialAlpha, float endingAlpha)
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        imageAlpha.alpha = initialAlpha;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            imageAlpha.alpha = Mathf.Lerp(initialAlpha, endingAlpha, elapsedTime / duration);
            yield return null;
        }
        imageAlpha.alpha = endingAlpha;
    }
    IEnumerator BlinkAlpha(float initialAlpha, float endingAlpha)
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        imageAlpha.alpha = initialAlpha;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            imageAlpha.alpha = Mathf.Lerp(initialAlpha, endingAlpha, elapsedTime / duration);
            yield return null;
        }
        imageAlpha.alpha = endingAlpha; 
    }
    void Update()
    {
        Blink();   
    }
}
