using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnim;

    private bool doorOpen = false;
    public bool doorUnlocked;

    [SerializeField] private ItemBehaviour.KeyType keyType;
    public ItemBehaviour.KeyType GetKeyType()
    {
        return keyType;
    }

    private void Awake()
    {
        doorAnim = gameObject.GetComponentInParent<Animator>();
    }

    public void PlayAnimation()
    {
        if (!doorOpen && doorUnlocked == true)
        {
            doorAnim.SetTrigger("OpenClose");
            doorOpen = true;
        }
        if (doorOpen)
        {
            doorAnim.SetTrigger("OpenClose");
            doorOpen = false;
        }
    }

    public void unlockDoor()
    {
        doorUnlocked = true;
        Debug.Log("Door Unlocked!");
    }
}
