using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RabbitController : MonoBehaviour, IPointerClickHandler
{
    [Header("Settings")]
    public Image heartPrefab;
    public float spawnHeight = 100f;
    public float fallSpeed = 50f;
    public int heartsPerClick = 2;
    public float spawnInterval = 0.3f;
    public float heartLifetime = 2f;
    public float fadeDuration = 0.5f;

    private Canvas parentCanvas;
    private RectTransform rabbitRect;

    void Start()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas == null)
            Debug.LogError("No Canvas found in parents!");

        rabbitRect = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(SpawnHearts());
    }

    IEnumerator SpawnHearts()
    {
        for (int i = 0; i < heartsPerClick; i++)
        {
            Image newHeart = Instantiate(heartPrefab, parentCanvas.transform);
            newHeart.color = Color.white;

            RectTransform heartRect = newHeart.GetComponent<RectTransform>();

            heartRect.anchoredPosition = GetHeartStartPosition(rabbitRect);

            heartRect.anchoredPosition += new Vector2(
                Random.Range(-100f, 100f),
                Random.Range(150f, 170f));

            StartCoroutine(AnimateHeart(newHeart));
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector2 GetHeartStartPosition(RectTransform rabbitRect)
    {
        return new Vector2(
            rabbitRect.anchoredPosition.x,
            rabbitRect.anchoredPosition.y + spawnHeight);
    }

    IEnumerator AnimateHeart(Image heart)
    {
        RectTransform rectTransform = heart.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = heart.gameObject.AddComponent<CanvasGroup>();
        float timer = 0f;

        rectTransform.localScale = Vector3.zero;

        while (timer < 0.2f)
        {
            timer += Time.deltaTime;
            rectTransform.localScale = Vector3.one * (timer / 0.2f);
            yield return null;
        }

        timer = 0f;
        Vector2 startPosition = rectTransform.anchoredPosition;

        while (timer < heartLifetime)
        {
            timer += Time.deltaTime;

            rectTransform.anchoredPosition = startPosition +
                Vector2.down * fallSpeed * timer;

            if (timer > heartLifetime - fadeDuration)
            {
                float fadeProgress = (timer - (heartLifetime - fadeDuration)) / fadeDuration;
                canvasGroup.alpha = 1f - fadeProgress;
            }

            yield return null;
        }

        Destroy(heart.gameObject);
    }

    private void Reset()
    {
        if (GetComponent<Collider2D>() == null)
            gameObject.AddComponent<BoxCollider2D>();
    }
}