using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemImage;
    private int itemId;
    private InventorySystem inventorySystem;

    public Sprite ItemSprite => itemImage.sprite;

    public void Initialize(int id, InventorySystem system)
    {
        itemId = id;
        inventorySystem = system;
        itemImage = GetComponent<Image>();
        itemImage.raycastTarget = false;
    }

    public void SetInteractable(bool interactable)
    {
        itemImage.raycastTarget = interactable;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventorySystem.OnItemClicked(itemId);
    }
}