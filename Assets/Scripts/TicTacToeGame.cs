using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TicTacToeGame : MonoBehaviour
{
    [Header("Game Elements")]
    public Image[] cellImages;
    public Sprite xSprite; 
    public Sprite oSprite; 
    public GameObject gameBackground;
    public Text gameStatusText;

    [Header("Game Settings")]
    public float robotDelay = 1f;

    [Header("Game Settings")]
    public RobotDialog robotDialog;

    private int[] board = new int[9];
    private bool isPlayerTurn = true;
    private bool gameActive = false;

    void Start()
    {
        gameBackground.SetActive(false);
        if (gameStatusText != null) gameStatusText.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        gameBackground.SetActive(true);
        if (gameStatusText != null)
        {
            gameStatusText.gameObject.SetActive(true);
            gameStatusText.text = "Ваш ход (X)";
        }
        gameActive = true;
        ResetBoard();
    }

    public void OnCellClicked(int cellIndex)
    {
        if (!gameActive || board[cellIndex] != 0 || !isPlayerTurn)
            return;

        MakeMove(cellIndex, 1, xSprite);

        if (CheckWin(1))
        {
            EndGame("Вы победили!");
            return;
        }

        if (IsBoardFull())
        {
            EndGame("Ничья!");
            return;
        }

        isPlayerTurn = false;
        if (gameStatusText != null) gameStatusText.text = "Ход робота...";
        StartCoroutine(RobotMove());
    }

    IEnumerator RobotMove()
    {
        yield return new WaitForSeconds(robotDelay);

        int randomCell;
        do
        {
            randomCell = Random.Range(0, 9);
        } while (board[randomCell] != 0);

        MakeMove(randomCell, 2, oSprite);

        if (CheckWin(2))
        {
            EndGame("Робот победил!");
            yield break;
        }

        if (IsBoardFull())
        {
            EndGame("Ничья!");
            yield break;
        }

        isPlayerTurn = true;
        if (gameStatusText != null) gameStatusText.text = "Ваш ход (X)";
    }

    void MakeMove(int cellIndex, int player, Sprite sprite)
    {
        board[cellIndex] = player;
        cellImages[cellIndex].sprite = sprite;
        cellImages[cellIndex].gameObject.SetActive(true);
        cellImages[cellIndex].color = Color.white;
    }

    bool CheckWin(int player)
    {
        int[,] winConditions = new int[8, 3] {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8},
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8},
            {0, 4, 8}, {2, 4, 6}
        };

        for (int i = 0; i < 8; i++)
        {
            if (board[winConditions[i, 0]] == player &&
                board[winConditions[i, 1]] == player &&
                board[winConditions[i, 2]] == player)
            {
                return true;
            }
        }
        return false;
    }

    bool IsBoardFull()
    {
        foreach (int cell in board)
        {
            if (cell == 0) return false;
        }
        return true;
    }

    void ResetBoard()
    {
        board = new int[9];
        foreach (Image img in cellImages)
        {
            img.gameObject.SetActive(false);
        }
        isPlayerTurn = true;
        gameActive = true;
    }

    void EndGame(string message)
    {
        gameActive = false;
        if (gameStatusText != null) gameStatusText.text = message;

        if (message.Contains("Вы победили"))
        {
            Invoke("ShowWinDialog", 1f);
        }
        else
        {
            Invoke("RestartGame", 3f);
        }
    }

    void ShowWinDialog()
    {
        CloseGame();
        robotDialog.ShowWinDialog();
    }

    void RestartGame()
    {
        ResetBoard();
        if (gameStatusText != null) gameStatusText.text = "Ваш ход (X)";
    }

    public void CloseGame()
    {
        gameBackground.SetActive(false);
        if (gameStatusText != null) gameStatusText.gameObject.SetActive(false);
        gameActive = false;
    }
}