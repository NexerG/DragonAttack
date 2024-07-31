using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArcherBehaviour : MonoBehaviour
{
    [SerializeField] GameObject ArrowRef;
    [SerializeField] Transform start;
    [SerializeField] GameObject target;
    [SerializeField] float Speed;
    [SerializeField] internal GameManager manager;
    [SerializeField] float FireRate=0.5f;
    private float nextFire = 0f;
    SpriteChanger changer;
    internal int Level = 1;

    // Start is called before the first frame update
    void Start()
    {
        changer = GetComponent<SpriteChanger>();
    }

    internal void LevelUp()
    {
        Level++;
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
        yield return new WaitForSeconds(0.04f * 5);
        if(target != null)
        {
            Vector2 Vv = CalcSpeedVector(target.transform, start);
            GameObject Arrow;
            Arrow = Instantiate(ArrowRef, start.position, quaternion.Euler(0, 0, 0));
            Arrow.GetComponent<Rigidbody2D>().velocity = Vv;
            Arrow.GetComponent<ArrowBehaviour>().Level = Level;
        }
    }

    private void DetectTarget()
    {
        if (manager.enemies.Count > 0 && target == null)
        {
            int randomIndex = UnityEngine.Random.Range(0,manager.enemies.Count);
            target = manager.enemies[randomIndex];
        }
    }

    private Vector2 CalcSpeedVector(Transform target, Transform start)
    {
        float horizontalDistance = (target.position.x - start.position.x);
        
        float minAngle = 0f;
        float maxAngle = Mathf.PI / 2f;
        float tolerance = 0.001f;

        float angle = (minAngle + maxAngle) / 2f;
        float vx = Mathf.Cos(angle) * Speed;
        float vy = Mathf.Sin(angle) * Speed;

        while ((maxAngle - minAngle) > tolerance)
        {
            float timeFlying = horizontalDistance / vx;
            float Ft = vy * timeFlying - 0.5f * Physics2D.gravity.magnitude * timeFlying * timeFlying;

            if (Mathf.Abs(Ft - target.position.y) <= 0.05f)
            {
                //success
                return new Vector2(vx, vy);
            }
            else if (Ft < target.position.y)
            {
                //undershoot
                minAngle = angle;
                angle = (minAngle + maxAngle) / 2f;
                vx = Mathf.Cos(angle) * Speed;
                vy = Mathf.Sin(angle) * Speed;
            }
            else
            {
                maxAngle = angle;
                angle = (minAngle + maxAngle) / 2f;
                vx = Mathf.Cos(angle) * Speed;
                vy = Mathf.Sin(angle) * Speed;
            }
        }

        vx = Mathf.Cos(Mathf.Deg2Rad * 30) * Speed;
        vy = Mathf.Sin(Mathf.Deg2Rad * 30) * Speed;
        return new Vector2(vx, vy);
    }
}