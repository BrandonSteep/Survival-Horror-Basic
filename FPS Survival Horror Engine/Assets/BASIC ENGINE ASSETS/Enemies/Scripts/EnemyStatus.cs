using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float maxHealth = 10f;
    public float currentHealth;
    public float damageDealt = 10f;
    public EnemyMovement movementScript;
    public ParticleSystem gibs;
    public ParticleSystem weakGibs;

    public Transform fpsCam;
    public float gibTime = 5f;

    public bool isAlive;


    public float fireMultiplier;

    // reference //
    public 


    void Awake()
    {
        currentHealth = maxHealth;
        movementScript = GetComponentInParent<EnemyMovement>();
        fpsCam = Camera.main.transform;
    }

    public void TakeDamage(float firearmDamage)
    {
        movementScript.Seen();
        currentHealth -= firearmDamage;
        Debug.Log(firearmDamage);
        if (currentHealth <= 0f && isAlive == true)
        {
            isAlive = false;
            Die();
        }

    }

    public void Die()
    {
        isAlive = false;
        
        Rigidbody rb = this.gameObject.AddComponent<Rigidbody>();
        rb.AddForce(fpsCam.forward * 0.5f, ForceMode.Impulse);
    }

}
    