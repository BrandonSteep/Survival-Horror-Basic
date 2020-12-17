using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakpoint : EnemyStatus
{
    public float damageMult;
    public float weakpointHealth;

    public bool vitalWeakpoint;

    public void DamageMultiplier(float firearmDamage)
    {
        Debug.Log("Weakpoint Hit");
        firearmDamage *= damageMult;
        weakpointHealth -= firearmDamage;

        if (weakpointHealth <= 0f && vitalWeakpoint == true)
        {
            Destroy(this.gameObject, 0.1f);
            Invoke("Die", 5f);
        }
        if (weakpointHealth <= 0)
        {
            Invoke("Die", 5f);
        }
        else
        TakeDamage(firearmDamage);
    }
}
