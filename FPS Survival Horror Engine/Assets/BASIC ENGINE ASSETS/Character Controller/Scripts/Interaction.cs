using UnityEngine;

public class Interaction : MonoBehaviour
{

    public Camera fpsCam;

    public LayerMask ignoreLayers;

    public float checkDistance = 200f;

    private ItemEquip itemEquip;

    private DoorController doorController;
    private Animator doorAnim;

    private GameObject player;
    private AmmoInventory ammoInventory;
    private Ammunition ammo;

    private GameObject inventory;
    private ItemSwitching switchScript;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        fpsCam = Camera.main;
        player = this.gameObject;
        ammoInventory = player.GetComponentInChildren<AmmoInventory>();

        inventory = GameObject.Find("Inventory");
        switchScript = inventory.GetComponent<ItemSwitching>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        RaycastHit hit;
        Ray checkItem = new Ray(fpsCam.transform.position, fpsCam.transform.forward);
        if (Physics.Raycast(checkItem, out hit, checkDistance, ~ignoreLayers.value))
        {
            Debug.Log(hit.transform.name);
            GameObject itemSelected = hit.transform.gameObject;
            if (itemSelected.GetComponent<ItemEquip>())
            {
                itemEquip = itemSelected.GetComponent<ItemEquip>();
                itemEquip.PickUp();
                switchScript.Text.text = hit.collider.name;
                gameManager.SlotTextFade();
            }

            if (hit.collider.CompareTag("Door"))
            {
                doorController = hit.collider.gameObject.GetComponent<DoorController>();

                if (doorController.doorUnlocked == true)
                {
                    Debug.Log("Door Opened");
                    doorController.PlayAnimation();
                }
                else
                {
                    doorAnim = doorController.gameObject.GetComponentInParent<Animator>();
                    doorAnim.SetTrigger("Locked");
                    Debug.Log("The Door is Locked...");
                }
            }

            if (hit.collider.CompareTag("Ammo"))
            {
                ammo = hit.collider.gameObject.GetComponent<Ammunition>();
                ammoInventory.Collect(ammo.ammoType, ammo.amount);
                Destroy(ammo.gameObject);
                switchScript.Text.text = hit.collider.name;
                gameManager.SlotTextFade();

            }
        }
    }
}