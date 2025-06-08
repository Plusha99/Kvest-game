using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Image))]
public class InteractiveDog : MonoBehaviour, IPointerClickHandler
{
    [Header("Dog Settings")]
    public AudioClip[] barkSounds;
    public Sprite heartSprite;
    public Transform heartSpawnPoint;

    [Header("Heart Settings")]
    public Vector2 heartSize = new Vector2(100, 100);
    public float heartScale = 1f;

    [Header("Timing Settings")]
    public float actionDelay = 3f;
    public float heartDisplayTime = 1f;
    public float fadeDuration = 0.3f;

    [Header("Probabilities")]
    [Range(0, 100)] public int heartChance = 70;

    private AudioSource audioSource;
    private bool canInteract = true;
    private Image dogImage;

    void Awake()
    {
        dogImage = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canInteract)
        {
            StartCoroutine(PerformAction());
        }
    }

    private IEnumerator PerformAction()
    {
        canInteract = false;

        bool showHeart = Random.Range(0, 100) < heartChance;

        if (showHeart)
        {
            ShowHeart();
        }
        else
        {
            Bark();
        }

        yield return new WaitForSeconds(actionDelay);
        canInteract = true;
    }

    private void Bark()
    {
        if (barkSounds.Length > 0)
        {
            AudioClip randomBark = barkSounds[Random.Range(0, barkSounds.Length)];
            audioSource.PlayOneShot(randomBark);
        }
    }

    private void ShowHeart()
    {
        if (heartSprite != null && heartSpawnPoint != null)
        {
            GameObject heartObj = new GameObject("Heart");
            heartObj.transform.SetParent(transform.parent);
            heartObj.transform.position = heartSpawnPoint.position;
            heartObj.transform.localScale = Vector3.one * heartScale;

            Image heartImage = heartObj.AddComponent<Image>();
            heartImage.sprite = heartSprite;

            RectTransform rt = heartObj.GetComponent<RectTransform>();
            rt.sizeDelta = heartSize;

            CanvasGroup canvasGroup = heartObj.AddComponent<CanvasGroup>();
            StartCoroutine(FadeAndDestroyHeart(heartObj));
        }
    }

    private IEnumerator FadeAndDestroyHeart(GameObject heartObj)
    {
        CanvasGroup canvasGroup = heartObj.GetComponent<CanvasGroup>();
        float timer = 0f;

        yield return new WaitForSeconds(heartDisplayTime - fadeDuration);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f - (timer / fadeDuration);
            }
            yield return null;
        }

        if (heartObj != null)
        {
            Destroy(heartObj);
        }
    }
}