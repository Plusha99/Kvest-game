using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour
{
    public InventoryItem[] inventorySlots = new InventoryItem[3];
    public LockController lockController;

    void Awake()
    {
        InitializeInventoryItems();
    }

    private void InitializeInventoryItems()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] != null)
            {
                inventorySlots[i].Initialize(i, this);
            }
        }
    }

    public void OnItemClicked(int itemId)
    {
        lockController?.OnInventoryItemClicked(itemId);
    }

    public Sprite GetItemSprite(int itemId)
    {
        return IsValidItemId(itemId) ? inventorySlots[itemId].ItemSprite : null;
    }

    public void SetItemVisible(int itemId, bool visible)
    {
        if (IsValidItemId(itemId))
        {
            inventorySlots[itemId].gameObject.SetActive(visible);
        }
    }

    public void SetItemsInteractable(bool interactable)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot != null) slot.SetInteractable(interactable);
        }
    }

    private bool IsValidItemId(int itemId) => itemId >= 0 && itemId < inventorySlots.Length;
}