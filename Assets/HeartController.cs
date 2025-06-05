using UnityEngine;

public class HeartController : MonoBehaviour
{
    public float fallSpeed = 1f;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
}