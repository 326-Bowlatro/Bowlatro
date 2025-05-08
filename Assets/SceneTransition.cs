using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneSwirlTransition : MonoBehaviour
{
    public string targetScene = "GameScene"; // Set your target scene name
    public float duration = 1.5f;
    public AudioClip transitionSound;

    private Image swirlImage;
    private AudioSource audioSource;

    public void StartTransition()
    {
        // ✅ Start gameplay music BEFORE loading the scene
        if (AudioManager.Instance != null && AudioManager.Instance.gameplayMusic != null)
        {
            StartCoroutine(AudioManager.Instance.FadeToMusic(AudioManager.Instance.gameplayMusic));
        }

        StartCoroutine(SwirlAndLoad());
    }

    private IEnumerator SwirlAndLoad()
    {
        // Create Canvas
        GameObject canvasGO = new GameObject("TransitionCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        // Create Image
        GameObject imageGO = new GameObject("SwirlImage");
        imageGO.transform.SetParent(canvasGO.transform, false);
        swirlImage = imageGO.AddComponent<Image>();

        // Full-screen rect
        RectTransform rt = swirlImage.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        swirlImage.color = new Color(0f, 0f, 0f, 0f);
        swirlImage.raycastTarget = false;

        // Add and play sound
        audioSource = canvasGO.AddComponent<AudioSource>();
        if (transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }

        // Animate
        float t = 0f;
        Vector3 startScale = Vector3.one;
        Vector3 endScale = new Vector3(4f, 4f, 1f);

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;
            swirlImage.rectTransform.localScale = Vector3.Lerp(startScale, endScale, p);
            swirlImage.color = new Color(0f, 0f, 0f, p);
            yield return null;
        }

        swirlImage.color = new Color(0f, 0f, 0f, 1f);
        SceneManager.LoadScene(targetScene);
    }
}
