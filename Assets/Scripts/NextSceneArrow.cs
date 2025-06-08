using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class NextSceneArrow : MonoBehaviour, IPointerClickHandler
{
    [Header("Settings")]
    public string nextSceneName;

    private bool isTransitioning = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isTransitioning && !string.IsNullOrEmpty(nextSceneName))
        {
            TransitionToNextScene();
        }
    }

    private void TransitionToNextScene()
    {
        isTransitioning = true;

        SceneManager.LoadScene(nextSceneName);
    }
}