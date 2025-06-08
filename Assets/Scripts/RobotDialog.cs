using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class RobotDialog : MonoBehaviour, IPointerClickHandler
{
    private string[] dialogPhrases = {
        "Приветствую, игрок!",
        "Я - страж этой комнаты...",
        "Чтобы пройти дальше, тебе нужно...",
        "Победить меня в игре!",
        "Готов начать?"
    };

    private string[] winPhrases = {
        "Не может быть!",
        "Ты победил меня...",
        "Вот твоя награда...",
        "Цифра для кода: ",
        "Проход свободен."
    };

    public float textDisplaySpeed = 0.05f;
    public int codeDigit = 1;

    [Header("UI References")]
    public GameObject dialogPanel;
    public Text dialogText;
    public GameObject arrowObject;
    public float arrowShowDelay = 1f;

    [Header("Game References")]
    public GameObject gameBackground;
    public TicTacToeGame ticTacToeGame;
    public InventorySystem inventorySystem;

    private int currentPhraseIndex = 0;
    private bool isDialogActive = false;
    private bool isTextAnimating = false;
    private Coroutine textAnimation;
    private bool gameStarted = false;
    private bool gameCompleted = false;

    private void Awake()
    {
        dialogPanel.SetActive(false);
        gameBackground.SetActive(false);
        gameCompleted = false;
        gameStarted = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameCompleted) return;

        if (!isDialogActive && !gameStarted)
        {
            StartDialog();
        }
        else if (!isTextAnimating)
        {
            ShowNextPhrase();
        }
        else
        {
            SkipTextAnimation();
        }
    }

    void StartDialog()
    {
        if (dialogPhrases.Length == 0) return;

        currentPhraseIndex = 0;
        isDialogActive = true;
        dialogPanel.SetActive(true);
        DisplayCurrentPhrase();
    }

    public void ShowWinDialog()
    {
        currentPhraseIndex = 0;
        isDialogActive = true;
        dialogPanel.SetActive(true);

        winPhrases[3] = "Цифра для кода: " + codeDigit;

        dialogPhrases = winPhrases;
        DisplayCurrentPhrase();
    }

    void DisplayCurrentPhrase()
    {
        if (textAnimation != null)
            StopCoroutine(textAnimation);

        textAnimation = StartCoroutine(AnimateText(dialogPhrases[currentPhraseIndex]));
    }

    IEnumerator AnimateText(string text)
    {
        isTextAnimating = true;
        dialogText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(textDisplaySpeed);
        }

        isTextAnimating = false;
    }

    void SkipTextAnimation()
    {
        StopCoroutine(textAnimation);
        dialogText.text = dialogPhrases[currentPhraseIndex];
        isTextAnimating = false;
    }

    void ShowNextPhrase()
    {
        currentPhraseIndex++;

        if (currentPhraseIndex < dialogPhrases.Length)
        {
            DisplayCurrentPhrase();
        }
        else
        {
            EndDialog();
        }
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);
        isDialogActive = false;

        if (dialogPhrases == winPhrases)
        {
            gameCompleted = true;
            if (inventorySystem != null)
            {
                inventorySystem.SetItemVisible(0, true);
            }

            if (arrowObject != null)
            {
                StartCoroutine(ShowArrowAfterDelay());
            }
        }
        else
        {
            gameStarted = true;
            StartCoroutine(StartGameAfterDialog());
        }
    }

    IEnumerator StartGameAfterDialog()
    {
        yield return new WaitForSeconds(0.1f);
        gameBackground.SetActive(true);
        ticTacToeGame.StartGame();
    }

    IEnumerator ShowArrowAfterDelay()
    {
        yield return new WaitForSeconds(arrowShowDelay);
        arrowObject.SetActive(true);
    }

    private void Reset()
    {
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }
}