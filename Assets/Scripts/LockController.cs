using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LockController : MonoBehaviour, IPointerClickHandler
{
    [Header("Settings")]
    public int correctCode = 138;
    public float resetDelay = 2f;

    [Header("References")]
    public GameObject lockPanel;
    public Image[] codeSlots = new Image[3];
    public InventorySystem inventorySystem;
    public GameObject zamok;
    public GameObject arrowObject;

    private int[] enteredDigits = new int[3] { -1, -1, -1 };
    private int currentSlotIndex = 0;
    private bool isLockActive = false;

    private Dictionary<int, int> itemIdToDigitMap = new Dictionary<int, int>()
    {
        {0, 1},
        {1, 3},
        {2, 8}
    };

    private void Awake()
    {
        if (arrowObject != null) arrowObject.SetActive(false);
        lockPanel.SetActive(false);
        InitializeCodeSlots();
    }

    private void InitializeCodeSlots()
    {
        foreach (var slot in codeSlots)
        {
            if (slot != null)
            {
                slot.gameObject.SetActive(true);
                slot.sprite = null;
                slot.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isLockActive) OpenLock();
    }

    private void OpenLock()
    {
        isLockActive = true;
        lockPanel.SetActive(true);
        inventorySystem.SetItemsInteractable(true);
        currentSlotIndex = 0;
    }

    private void CloseLock()
    {
        isLockActive = false;
        lockPanel.SetActive(false);
        StartCoroutine(CloseGame());
        zamok.SetActive(false);
        inventorySystem.SetItemsInteractable(false);
    }

    public IEnumerator CloseGame()
    {
        if (arrowObject != null)
        {
            arrowObject.SetActive(true);

            CanvasGroup arrowCanvas = arrowObject.GetComponent<CanvasGroup>();
            if (arrowCanvas != null)
            {
                arrowCanvas.alpha = 0;
                float fadeTime = 1f;
                float timer = 0;

                while (timer < fadeTime)
                {
                    timer += Time.deltaTime;
                    arrowCanvas.alpha = Mathf.Lerp(0, 1, timer / fadeTime);
                    yield return null;
                }
            }
        }
    }

    public void OnInventoryItemClicked(int itemId)
    {
        if (!isLockActive || currentSlotIndex >= codeSlots.Length) return;

        var sprite = inventorySystem.GetItemSprite(itemId);
        if (sprite == null) return;

        codeSlots[currentSlotIndex].gameObject.SetActive(true);
        codeSlots[currentSlotIndex].sprite = sprite;
        codeSlots[currentSlotIndex].color = Color.white;

        enteredDigits[currentSlotIndex] = itemIdToDigitMap[itemId];
        inventorySystem.SetItemVisible(itemId, false);

        currentSlotIndex++;

        if (currentSlotIndex >= codeSlots.Length)
        {
            CheckCode();
        }
    }

    private void CheckCode()
    {
        int enteredCode = enteredDigits[0] * 100 + enteredDigits[1] * 10 + enteredDigits[2];

        if (enteredCode == correctCode)
        {
            Debug.Log("Code correct!");
            CloseLock();
        }
        else
        {
            Debug.Log("Wrong code!");
            StartCoroutine(ResetLock());
        }
    }

    private IEnumerator ResetLock()
    {
        yield return new WaitForSeconds(resetDelay);

        for (int i = 0; i < enteredDigits.Length; i++)
        {
            if (enteredDigits[i] != -1)
            {
                foreach (var pair in itemIdToDigitMap)
                {
                    if (pair.Value == enteredDigits[i])
                    {
                        inventorySystem.SetItemVisible(pair.Key, true);
                        break;
                    }
                }
                enteredDigits[i] = -1;
            }
        }

        InitializeCodeSlots();
        currentSlotIndex = 0;
    }
}