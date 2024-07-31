using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyProjectileBehaviour : MonoBehaviour
{
    private float Damage = 1f;
    private float LifeSpan = 2f;
    private float start;

    // Start is called before the first frame update
    void Start()
    {
        start = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        Vector3 pos = gameObject.transform.position;
        gameObject.transform.SetPositionAndRotation(pos, ConvertToRotation(velocity));
        if (Time.time > start + LifeSpan)
        {
            Destroy(gameObject);
        }
    }
    private Quaternion ConvertToRotation(Vector2 direction)
    {
        // Assuming direction represents the desired rotation
        float angle = Mathf.Atan2(direction.y, direction.x); // Calculate angle in radians
        return Quaternion.Euler(0f, 0f, angle); // Create quaternion for Z-axis rotation
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Castle"))
        {
            collision.gameObject.GetComponent<Castle>().DoDamage(Damage);
            Destroy(gameObject);
        }
    }

}
