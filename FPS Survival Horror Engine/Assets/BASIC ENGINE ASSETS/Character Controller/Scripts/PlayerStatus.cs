using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerStatus : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public int damage = 20;

    public bool takingDamage;
    public bool isDead;


    // References //
    public PlayerMovement movement;
    public GameObject currentlyEquipped;
    private ItemEquip equipScript;
    


    private void Start()
    {
        takingDamage = false;
        isDead = false;
        movement.enabled = true;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        /*                               *\
         *   FOR TESTING PURPOSES ONLY   * 
         *                               *
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(20);
        }
        */


        if (currentHealth <= 0)
        {
            if (isDead == false)
            {
                isDead = true;
                movement.enabled = false;
                equipScript = gameObject.GetComponentInChildren<ItemEquip>();
                equipScript.Drop();
                movement.animator.enabled = true;
                movement.animator.SetTrigger("Die");
                FindObjectOfType<GameManager>().EndGame();
            }
        }
        else
        {
            return;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

    }

    public void Damage(int amount)
    {
        takingDamage = true;
        currentHealth -= amount;
        CameraShaker.Instance.ShakeOnce((amount / 5), (amount / 5), .01f, 0.5f);
        Invoke("DamageDone", 0.5f);
    }

    void DamageDone()
    {
        //so I guess I'll be leavin//
        takingDamage = false;
    }

    void OnTriggerStay(Collider coll)
    {
        
        if(coll.gameObject.tag == "Enemy" && takingDamage == false)
        {
            coll.gameObject.GetComponentInParent<EnemyMovement>().playerSeen = true;
            DamageRange ouchies = coll.gameObject.GetComponent<DamageRange>();
            Damage(ouchies.amount);
        }
    }


}
