using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position)
    {
        //Transform pfEnemy = Resources.Load<Transform>("pfEnemy");
        Transform enemyTransform = Instantiate(GameAssets.Instance.pfEnemy, position, Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }

    private Rigidbody2D rb2D;
    private Transform targetTransform;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;
    private HealthSystem healthSystem;
    private Vector3 moveDir;
    [SerializeField] private float speed;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if(BuildingManager.Instance.GetHQBuilding() != null)
        {
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
            moveDir = (targetTransform.position - transform.position).normalized;
        }
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
        CinemachineShake.Instance.ShakeCamera(2f, .1f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        CinemachineShake.Instance.ShakeCamera(4f, .15f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, GameAssets.Instance.pfEnemyDieParticles.rotation);
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
            //moveDir = (targetTransform.position - transform.position).normalized;

            float moveSpeed = 6f;
            rb2D.velocity = moveDir * moveSpeed;
            if(rb2D.velocity.magnitude < 4.0f)
            {
                rb2D.velocity = moveDir * moveSpeed * 1.5f;
            }
            if (rb2D.velocity.magnitude < 3.0f)
            {
                rb2D.velocity = moveDir * moveSpeed * 2f;
            }
        }
        else
        {
            rb2D.velocity = Vector2.zero;
        }

        speed = rb2D.velocity.magnitude;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
    
        if(building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(13);

            this.healthSystem.Damage(999);
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
                    moveDir = (targetTransform.position - transform.position).normalized;
                }
                else
                {
                    if(Vector3.Distance(transform.position, building.transform.position) < 
                        Vector3.Distance(transform.position, targetTransform.position)){
                        targetTransform = building.transform;
                    moveDir = (targetTransform.position - transform.position).normalized;
                    }
                }
            }
        }
        if(targetTransform == null){
            // nO TARGETS FOUND!
            if(BuildingManager.Instance.GetHQBuilding() != null)
            {
                targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
                moveDir = (targetTransform.position - transform.position).normalized;
            }
        }
    }
}
