using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEnemyController : MonoBehaviour
{
    [SerializeField] float movespeed = 0.2f;
    [SerializeField] internal float HP = 10f;
    [SerializeField] private GameObject AttackPrefab;
    [SerializeField] float FireRate = 0.5f;
    [SerializeField] float Speed = 2.5f;
    internal int index;
    private float nextFire = 0f;
    [SerializeField] float HealingDelay = 3f;
    [SerializeField] float nextHeal = 0f;
    [SerializeField] float Healing = 1f;
    private GameObject healingTarget;

    [SerializeField] internal GameObject AttackPoint;
    internal GameManager manager;
    private bool isMoving = true;
    private bool isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        gameObject.GetComponent<Rigidbody2D>().velocityX = -movespeed;
        gameObject.GetComponent<Rigidbody2D>().velocityY = 0f;
        gameObject.GetComponent<SpriteChanger>().PlayMovement();
    }

    // Update is called once per frame
    void Update()
    {
        //move + heal -> attack transition + heal
        if (isMoving && gameObject.transform.position.x < -4)
        {
            gameObject.GetComponent<SpriteChanger>().StopMovement();
            isMoving = false;
            gameObject.GetComponent<Rigidbody2D>().velocityX = 0f;
            isAttacking = true;
        }
        //Attacking
        if (isAttacking)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + FireRate;
                shoot();
            }
        }
        if (Time.time > nextHeal)
        {
            nextHeal = Time.time + HealingDelay;
            Heal();
        }
    }

    private void shoot()
    {
        float vx = AttackPoint.transform.position.x - gameObject.transform.position.x;
        float vy = AttackPoint.transform.position.y - gameObject.transform.position.y;
        float magnitude = Mathf.Sqrt(vx * vx + vy * vy);
        vx = (vx / magnitude);
        vy = (vy / magnitude);
        Vector2 Vv = Speed * new Vector2(vx, vy);

        GameObject attack;
        attack = Instantiate(AttackPrefab, gameObject.transform);
        attack.GetComponent<Rigidbody2D>().velocity = Vv;
    }

    internal void DoDamage(float d)
    {
        HP -= d;
        CheckHP();
    }
    private void CheckHP()
    {
        if (HP <= 0)
        {
            for (int i = 0; i < manager.enemies.Count; i++)
            {
                if (manager.enemies[i] != null)
                    if (manager.enemies[i].TryGetComponent<HealingEnemyController>(out HealingEnemyController HEC))
                    {
                        if (HEC.index == index)
                        {
                            manager.enemies.RemoveAt(i);
                            manager.EndRound();
                            break;
                        }
                    }
            }
            manager.addMoney(10);
            Destroy(gameObject);
        }
    }
    private void Heal()
    {
        healingTarget = manager.enemies[0];
        if(healingTarget!=null)
            if (healingTarget.TryGetComponent<SimpleEnemyController>(out SimpleEnemyController SEC))
                SEC.HP += Healing;
            else
                healingTarget.GetComponent<HealingEnemyController>().HP = healingTarget.GetComponent<HealingEnemyController>().HP + Healing;
    }
}
