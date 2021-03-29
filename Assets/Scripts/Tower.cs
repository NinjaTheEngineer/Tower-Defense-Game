using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Enemy targetEnemy;
    private Vector3 projectileSpawnPosition;

    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;
    private float shootTimer;
    [SerializeField] private float shootTimerMax = .2f;

    private void Awake()
    {
        projectileSpawnPosition = transform.Find("ProjectileSpawnPosition").position;
    }

    private void Start()
    {
        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void Update()
    {
        HandleTargeting();
        HandleShooting();
    }

    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0f)
        {
            shootTimer += shootTimerMax;

            if (targetEnemy != null)
            {
                ArrowProjectile.Create(projectileSpawnPosition, targetEnemy);
            }

        }
    }

    private void HandleTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;

        if(lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }
    private void LookForTargets()
    {
        float targetMaxRadius = 20f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach(Collider2D collider in collider2DArray)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                if(targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if(Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }

    }
}
