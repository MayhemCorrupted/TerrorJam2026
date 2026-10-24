using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Necesario para el Vignette de URP

// Estructura para manejar los 4 usos de GameObjects
[System.Serializable]
public class BlinkStage
{
    public string stageName = "Fase de Parpadeo";
    public GameObject objectToActivate;
    public GameObject[] objectsToDeactivate;
}

public class CameraBlink : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionAsset actionAsset;
    private InputAction blinkAction;

    [Header("UI & Post-Processing")]
    [SerializeField] private CanvasGroup imageAlpha;
    [SerializeField] private Volume postProcessVolume;
    private Vignette vignette;

    [Header("Blink Settings")]
    [SerializeField] private float blinkDuration = 0.5f;
    [SerializeField] private float maxVignetteIntensity = 0.6f;

    [Header("Fases")]
    [SerializeField] private BlinkStage[] blinkStages = new BlinkStage[4];
    private int currentStageIndex = 0;

    private Coroutine currentCoroutine;
    private bool eyesAreClosed = false;
    private void OnEnable()
    {
        blinkAction = actionAsset.FindAction("Blink");
        blinkAction.Enable();

        blinkAction.started += ctx => CloseEyes();
        blinkAction.canceled += ctx => OpenEyes();
    }

    private void OnDisable()
    {
        blinkAction.Disable();
        blinkAction.started -= ctx => CloseEyes();
        blinkAction.canceled -= ctx => OpenEyes();
    }
    private void Start()
    {
        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            postProcessVolume.profile.TryGet(out vignette);
        }
    }
    private void CloseEyes()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(AnimateBlink(1f, maxVignetteIntensity));
        eyesAreClosed = true;
    }
    private void OpenEyes()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(AnimateBlink(0f, 0f));

        if (eyesAreClosed)
        {
            TriggerStageEvents();
            eyesAreClosed = false;
        }
    }
    private void TriggerStageEvents()
    {
        if (blinkStages.Length == 0) return;

        BlinkStage currentStage = blinkStages[currentStageIndex];

        if (currentStage.objectToActivate != null) currentStage.objectToActivate.SetActive(true);

        for (int i = 0; i > currentStage.objectsToDeactivate.Length; i++)
        {
           if (currentStage.objectsToDeactivate[i] != null) currentStage.objectsToDeactivate[i].SetActive(false);
        }

        currentStageIndex = (currentStageIndex + 1) % blinkStages.Length;
    }

    private IEnumerator AnimateBlink(float targetAlpha, float targetVignette)
    {
        float startAlpha = imageAlpha.alpha;
        float startVignette = vignette != null ? (float)vignette.intensity : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / blinkDuration;

            imageAlpha.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(startVignette, targetVignette, t);
            }

            yield return null;
        }
        imageAlpha.alpha = targetAlpha;
        if (vignette != null) vignette.intensity.value = targetVignette;
    }
}