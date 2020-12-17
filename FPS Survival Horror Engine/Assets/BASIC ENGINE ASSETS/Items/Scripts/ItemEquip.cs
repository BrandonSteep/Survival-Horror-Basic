using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquip : MonoBehaviour
{
    public ItemBehaviour itemBehaviour;

    public Rigidbody rb;
    public BoxCollider coll;
    private Animator animator;
    public Transform player, itemSlot, fpsCam;
    public GameObject inventory;

    public float dropForwardForce = 3f, dropUpwardForce = 1f;

    public bool equipped;
    public static bool slotFull;

    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        coll = this.gameObject.GetComponent<BoxCollider>();
        animator = this.gameObject.GetComponent<Animator>();
        player = player.transform;
        fpsCam = Camera.main.transform;

        if (!equipped)
        {
            rb.isKinematic = false;
            itemBehaviour.enabled = false;
            animator.enabled = false;
        }
        if (equipped)
        {
            rb.isKinematic = true;
            itemBehaviour.enabled = true;
        }
    }

    private void Update()
    {
        itemSlot = inventory.GetComponent<ItemSwitching>().currentSlot;
        if (itemSlot.childCount > 0 && Input.GetKeyDown(KeyCode.Q)) Drop();
    }

    public void PickUp()
    {
        if (itemSlot.childCount == 0)
        {
            // change equipment status//
            equipped = true;
            slotFull = true;

            //add as child and set rotation of item slot//
            transform.SetParent(itemSlot);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;

            gameObject.layer = LayerMask.NameToLayer("Equipped");

            //adjust rb and coll parameters//
            rb.isKinematic = true;
            // coll.isTrigger = true;

            itemBehaviour.enabled = true;
            animator.enabled = true;
            if (itemBehaviour.isKey == true || itemBehaviour.isHeal == true)
            {
                animator.SetBool("Dropped", false);
            }
        }
        else
        {
            Debug.Log("You just can't do it...");
        }
    }

    public void Drop()
    {
        // change equipment status//


        //set parent to null//
        transform.SetParent(null);

        //adjust rb and coll parameters//
        rb.isKinematic = false;
        //coll.isTrigger = false;

        //item carries player's momentum//
        rb.velocity = player.GetComponent<CharacterController>().velocity;

        gameObject.layer = LayerMask.NameToLayer("Default");

        if (equipped == true)
        {
            //add force//
            rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
            rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

            //add random rotation//
            float random = Random.Range(-1f, 1f);
            rb.AddTorque(new Vector3(random, random, random) * 10);
        }
        
        itemBehaviour.enabled = false;
        equipped = false;
        slotFull = false;

        if (itemBehaviour.isKey == true)
        {
            animator.SetBool("Dropped", true);
        }

        Invoke ("animatorOff", .25f);
    }

    public void animatorOff()
    {
        animator.enabled = false;
    }
}
