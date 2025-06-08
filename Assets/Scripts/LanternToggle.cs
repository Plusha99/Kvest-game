using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class LanternToggle : MonoBehaviour, IPointerClickHandler
{
    public Sprite lanternOnSprite;
    public Sprite lanternOffSprite;

    private Image lanternImage;
    private bool isOn = false;

    void Awake()
    {
        lanternImage = GetComponent<Image>();
        lanternImage.sprite = lanternOffSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleLantern();
    }

    private void ToggleLantern()
    {
        isOn = !isOn;
        lanternImage.sprite = isOn ? lanternOnSprite : lanternOffSprite;
    }
}