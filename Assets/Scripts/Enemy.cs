using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position)
    {
        Transform pfEnemy = Resources.Load<Transform>("pfEnemy");
        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }

    private Rigidbody2D rb2D;
    private Transform targetTransform;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;
    private HealthSystem healthSystem;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;

        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        HandleMovement();
        HandleTargeting();
        
    }

    private void HandleTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTarget();
        }
    }

    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            float moveSpeed = 6f;
            rb2D.velocity = moveDir * moveSpeed;
        }
        else
        {
            rb2D.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
    
        if(building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(13);
            Destroy(gameObject);
        }
    }

    private void LookForTarget()
    {
        float targetMaxRadius = 10f;
        Collider2D[] colllider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);
    
        foreach(Collider2D collider in colllider2DArray)
        {
            Building building = collider.GetComponent<Building>();

            if(building != null)
            {
                if(targetTransform == null)
                {
                    targetTransform = building.transform;
                }
                else
                {
                    if(Vector3.Distance(transform.position, building.transform.position) < 
                        Vector3.Distance(transform.position, targetTransform.position)){
                        targetTransform = building.transform;
                    }
                }
            }
        }
        if(targetTransform == null){
            // nO TARGETS FOUND!
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
    }
}
