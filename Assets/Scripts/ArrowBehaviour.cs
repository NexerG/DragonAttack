using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField] internal float Damage = 1f;
    private float LifeSpan = 2f;
    private float start;
    [SerializeField] internal int Level = 1;
    private void Start()
    {
        start = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        //set the sprite rotation to velocity vector
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
        angle = Mathf.Rad2Deg * angle;
        return Quaternion.Euler(0f, 0f, angle); // Create quaternion for Z-axis rotation
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<SimpleEnemyController>().DoDamage(Damage * Level);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("HealingEnemy"))
        {
            collision.gameObject.GetComponent<HealingEnemyController>().DoDamage(Damage * Level);
            Destroy(gameObject);
        }
    }
}
