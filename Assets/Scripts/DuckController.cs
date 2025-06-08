using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class DuckController : MonoBehaviour, IPointerClickHandler
{
    [Header("Settings")]
    public float speed = 500f;
    public bool showDebugLines = true;
    public Color debugLineColor = Color.cyan;

    private DuckHuntGame gameManager;
    private Vector3 targetPosition;
    private bool isDead = false;

    public void Initialize(DuckHuntGame manager, Vector3 target)
    {
        if (manager == null)
        {
            Debug.LogError("GameManager не задан!");
            Destroy(gameObject);
            return;
        }

        gameManager = manager;
        targetPosition = target;

        UpdateRotation();
    }

    void Update()
    {
        if (isDead || gameManager == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        UpdateRotation();

        if (showDebugLines)
        {
            Debug.DrawLine(transform.position, targetPosition, debugLineColor);
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            DestroyDuck();
        }
    }

    void UpdateRotation()
    {
        Vector3 moveDirection = targetPosition - transform.position;
        if (moveDirection.x != 0)
        {
            transform.localScale = new Vector3(
                moveDirection.x < 0 ? -1 : 1,
                1,
                1
            );
        }
    }

    void DestroyDuck()
    {
        if (isDead) return;

        isDead = true;
        if (gameManager != null)
            gameManager.DuckKilled(gameObject);
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDead || gameManager == null) return;

        isDead = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
            renderer.color = Color.red;

        if (gameManager != null)
            gameManager.DuckKilled(gameObject);
        Destroy(gameObject, 0.1f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}