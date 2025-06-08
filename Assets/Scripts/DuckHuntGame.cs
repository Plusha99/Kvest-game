using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DuckHuntGame : MonoBehaviour, IPointerClickHandler
{
    [Header("Game Settings")]
    public int ducksToKill = 30;
    public float duckSpawnInterval = 0.5f;

    [Header("UI References")]
    public Text ducksCounterText;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject arrowObject;
    public InventorySystem inventorySystem;

    [Header("Game Objects")]
    public GameObject gameBackground;
    public GameObject duckPrefab;
    public Transform[] spawnPoints;

    private int ducksKilled = 0;
    private bool gameActive = false;
    private List<GameObject> activeDucks = new List<GameObject>();

    void Start()
    {
        gameBackground.SetActive(false);
        gameOverPanel.SetActive(false);

        if (arrowObject != null) arrowObject.SetActive(false);

        if (spawnPoints.Length != 6)
        {
            Debug.LogError("Нужно ровно 6 спавн-поинтов!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!gameActive)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameActive = true;
        ducksKilled = 0;
        activeDucks.Clear();

        gameBackground.SetActive(true);
        gameOverPanel.SetActive(false);

        UpdateUI();
        StartCoroutine(SpawnDucks());
    }

    IEnumerator SpawnDucks()
    {
        while (gameActive && ducksKilled < ducksToKill)
        {
            SpawnDuck();
            yield return new WaitForSeconds(duckSpawnInterval);
        }
    }

    void SpawnDuck()
    {
        if (spawnPoints.Length != 6 || duckPrefab == null) return;

        int startPoint = Random.Range(0, spawnPoints.Length);
        int targetPoint = (startPoint + 3) % 6;

        if (spawnPoints[startPoint] == null || spawnPoints[targetPoint] == null)
        {
            Debug.LogError("Один из спавн-поинтов не задан!");
            return;
        }

        GameObject duck = Instantiate(
            duckPrefab,
            spawnPoints[startPoint].position,
            Quaternion.identity,
            gameBackground.transform
        );

        DuckController duckController = duck.GetComponent<DuckController>();
        if (duckController != null)
        {
            duckController.Initialize(
                this,
                spawnPoints[targetPoint].position
            );
            activeDucks.Add(duck);
        }
        else
        {
            Destroy(duck);
        }
    }

    public void DuckKilled(GameObject duck)
    {
        if (duck == null || !activeDucks.Contains(duck)) return;

        ducksKilled++;
        activeDucks.Remove(duck);
        Destroy(duck);

        UpdateUI();

        if (ducksKilled >= ducksToKill)
        {
            EndGame();
        }
    }

    void UpdateUI()
    {
        if (ducksCounterText != null)
            ducksCounterText.text = $"{ducksKilled}/{ducksToKill}";
    }

    void EndGame()
    {
        gameActive = false;
        StopAllCoroutines();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (gameOverText != null)
            gameOverText.text = "Ты победил! Все утки убиты!";

        foreach (var duck in activeDucks)
        {
            if (duck != null) 
                Destroy(duck);
        }
        activeDucks.Clear();

        StartCoroutine(CloseGame());
    }

    public IEnumerator CloseGame()
    {
        yield return new WaitForSeconds(3f);
        if (gameBackground != null)
            gameBackground.SetActive(false);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

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

        if (inventorySystem != null)
        {
            inventorySystem.SetItemVisible(1, true);
        }
    }
}