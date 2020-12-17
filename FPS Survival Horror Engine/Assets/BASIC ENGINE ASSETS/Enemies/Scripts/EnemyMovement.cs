using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : EnemyStatus
{
    private GameObject player;
    private Vector3 birthPoint;
    private NavMeshAgent nav;

    [Header("Movement Speed")]
    public float minSpeed = 1f;
    public float maxSpeed = 2f;

    [Header("Attacks")]
    public int damageOutput;

    // w e a k p o i n t s //
    [Header("Weakpoint Stats")]
    public float damageMult;
    public float weakpointHealth;

    public bool vitalWeakpoint;

    public GameObject weakpoint;
    private Animator animator;
   
    [SerializeField]
    private Collider visionCone;
    public bool playerSeen;


    // Start is called before the first frame update
    void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        birthPoint = transform.position;
        nav.speed = Random.Range(minSpeed, maxSpeed);
        isAlive = true;
        playerSeen = false;
        animator = this.gameObject.GetComponent<Animator>();
    }

// Update is called once per frame
void Update()
    {
        if (isAlive == false)
        {
            animator.SetBool("playerSeen", false);
            nav.enabled = false;
            return;
        }



        NavMeshPath navMeshPath = new NavMeshPath();
        // create path and check if navMeshAgent can reach its target
        if (nav.CalculatePath(player.transform.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete && playerSeen == true)
        {
            nav.SetPath(navMeshPath);
        }

        else
        {
            animator.SetBool("playerSeen", false);
            playerSeen = false;
            nav.SetDestination(birthPoint);
        }
    }



    public void TakeAHit(float firearmDamage)
    {
        TakeDamage(firearmDamage);
    }


    public void DamageMultiplier(float firearmDamage)
    {
        Debug.Log("Weakpoint Hit");
        firearmDamage *= damageMult;
        Debug.Log(firearmDamage);
        weakpointHealth -= firearmDamage;

        if (weakpointHealth <= 0f)
        {
            Debug.Log("Weakpoint Death");
            Destroy(weakpoint, 0f);
            weakGibs.Play();
        }
        if (weakpointHealth <= 0 && vitalWeakpoint == true && isAlive == true)
        {
            Debug.Log("BOGUS Weakpoint Death");

            Destroy(weakpoint, 0f);
            weakGibs.Play();
            Die();
        }
        else
            TakeDamage(firearmDamage);
    }

    public void Seen()
    {
        playerSeen = true;
        animator.SetBool("playerSeen", true);
        Debug.Log("You've been seen!");
    }
}


