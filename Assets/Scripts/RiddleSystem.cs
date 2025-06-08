using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class RiddleSystem : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Image riddleBackground;
    public Text riddleText;
    public Button[] answerButtons = new Button[3];
    public Text[] answerTexts = new Text[3];
    public GameObject riddlePanel;
    public GameObject arrowObject;
    public float arrowShowDelay = 1f;
    public InventorySystem inventorySystem;

    [Header("Settings")]
    public float fadeDuration = 0.5f;
    public Color backgroundColor = new Color(0, 0, 0, 0.8f);

    private string[,] riddles = new string[,]
    {
        {"Что можно увидеть с закрытыми глазами?", "Сон", "Тьму", "Мечты", "0"},
        {"Что становится влажным, пока сохнет?", "Полотенце", "Зонт", "Мыло", "0"},
        {"Что идет вверх и вниз, но не двигается?", "Лестница", "Температура", "Лифт", "0"},
        {"Чем больше берешь, тем больше оставляешь. Что это?", "Следы", "Долги", "Фотографии", "0"},
        {"Что принадлежит вам, но другие используют это чаще?", "Имя", "Телефон", "Одежда", "0"}
    };

    private int currentRiddleIndex;
    private bool isRiddleActive = false;

    void Start()
    {
        if (riddlePanel != null)
        {
            riddlePanel.SetActive(false);
        }

        InitializeAnswerButtons();
    }

    private void InitializeAnswerButtons()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isRiddleActive)
        {
            ShowRandomRiddle();
        }
    }

    private void ShowRandomRiddle()
    {
        currentRiddleIndex = Random.Range(0, riddles.GetLength(0));
        ShowRiddle(currentRiddleIndex);
    }

    private void ShowRiddle(int riddleIndex)
    {
        isRiddleActive = true;
        riddlePanel.SetActive(true);

        riddleText.text = riddles[riddleIndex, 0];

        int answersCount = Mathf.Min(answerTexts.Length, riddles.GetLength(1) - 2);
        for (int i = 0; i < answersCount; i++)
        {
            answerTexts[i].text = riddles[riddleIndex, i + 1];
            answerButtons[i].gameObject.SetActive(true);
        }

        for (int i = answersCount; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(false);
        }

        StartCoroutine(FadeIn());
    }

    private void OnAnswerSelected(int answerIndex)
    {
        if (!isRiddleActive) return;

        int correctAnswer = int.Parse(riddles[currentRiddleIndex, 4]);
        bool isCorrect = (answerIndex == correctAnswer);

        Debug.Log(isCorrect ? "Правильно!" : "Неправильно!");

        if (isCorrect)
        {
            StartCoroutine(HideRiddleWithBackground());
        }
        else
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        if (riddleBackground != null)
        {
            riddleBackground.color = Color.clear;
            float elapsed = 0;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                riddleBackground.color = Color.Lerp(Color.clear, backgroundColor, elapsed / fadeDuration);
                yield return null;
            }
        }
    }

    private IEnumerator FadeOut()
    {
        if (riddleBackground != null)
        {
            Color startColor = riddleBackground.color;
            float elapsed = 0;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                riddleBackground.color = Color.Lerp(startColor, Color.clear, elapsed / fadeDuration);
                yield return null;
            }
        }

        HideRiddle();
    }

    private IEnumerator HideRiddleWithBackground()
    {
        if (riddleBackground != null)
        {
            riddleBackground.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.1f);

        HideRiddle();
    }

    private void HideRiddle()
    {
        isRiddleActive = false;
        riddlePanel.SetActive(false);
        if (arrowObject != null)
        {
            StartCoroutine(ShowArrowAfterDelay());
        }

        if (inventorySystem != null)
        {
            inventorySystem.SetItemVisible(2, true);
        }
    }

    IEnumerator ShowArrowAfterDelay()
    {
        yield return new WaitForSeconds(arrowShowDelay);
        arrowObject.SetActive(true);
    }
}