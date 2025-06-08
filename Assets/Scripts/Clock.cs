using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Image))]
public class Clock : MonoBehaviour, IPointerClickHandler
{
    [Header("Clock Settings")]
    public Sprite normalClock;
    public Sprite brokenClock;
    public float shakeDuration = 0.3f;
    public float shakeIntensity = 10f;

    private Image clockImage;
    private bool isBroken = false;
    private RectTransform rectTransform;
    private Vector3 originalPosition;

    void Awake()
    {
        clockImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.localPosition;
        clockImage.sprite = normalClock;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isBroken)
        {
            clockImage.sprite = brokenClock;
            isBroken = true;
        }
        else
        {
            StartCoroutine(ShakeAndBreakEffect());
        }
    }

    private IEnumerator ShakeAndBreakEffect()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            Vector2 shakeOffset = Random.insideUnitCircle * shakeIntensity;
            rectTransform.localPosition = originalPosition + (Vector3)shakeOffset;
            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.localPosition = originalPosition;

    }
}