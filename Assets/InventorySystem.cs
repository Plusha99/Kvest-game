using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<string> items = new List<string>();

    public void AddItem(string item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            Debug.Log("Добавлен предмет в инвентарь: " + item);
        }
    }
}