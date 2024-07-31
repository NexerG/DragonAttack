using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireBoltWizardController : MonoBehaviour
{
    [SerializeField] GameObject FireboltRef;
    [SerializeField] Transform start;
    [SerializeField] GameObject target;
    [SerializeField] float Speed;
    [SerializeField] internal GameManager manager;
    [SerializeField] float FireRate = 0.1f*11f;
    private float nextFire = 0f;
    internal int level = 1;

    SpriteChanger changer;

    // Start is called before the first frame update
    void Start()
    {
        changer = GetComponent<SpriteChanger>();
    }

    internal void LevelUp()
    {
        level++;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            DetectTarget();
        }
        if (target != null && Time.time > nextFire)
        {
            nextFire = Time.time + FireRate;
            shoot();
        }
    }

    private void shoot()
    {
        changer.PlayAnim();
        StartCoroutine(ShootAfterDelay());
    }

    IEnumerator ShootAfterDelay()
    {
        yield return new WaitForSeconds(0.1f * 5);
        if(target != null)
        {
            float vx = target.transform.position.x - start.position.x;
            float vy = target.transform.position.y - start.position.y;

            float magnitude = Mathf.Sqrt(vx * vx + vy * vy);
            vx = (vx / magnitude);
            vy = (vy / magnitude);

            Vector2 Vv = Speed * new Vector2(vx, vy);

            GameObject firebolt;
            firebolt = Instantiate(FireboltRef, start);
            firebolt.GetComponent<Rigidbody2D>().velocity = Vv;
            firebolt.GetComponent<ArrowBehaviour>().Level = level;
        }
    }

    private void DetectTarget()
    {
        if (manager.enemies.Count > 0 && target == null)
        {
            int randomIndex = UnityEngine.Random.Range(0, manager.enemies.Count);
            target = manager.enemies[randomIndex];
        }
    }

    internal void setLevel(int level)
    {
        FireboltRef.GetComponent<ArrowBehaviour>().Level = level;
    }
}
