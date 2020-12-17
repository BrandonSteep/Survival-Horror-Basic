using UnityEngine;
using EZCameraShake;


public class ItemBehaviour : MonoBehaviour
{
    public ItemEquip itemEquip;
    public bool isKey, isFirearm, isHeal;


    [Header("K e y   S t a t s")]
    [SerializeField] private KeyType keyType;
    public enum KeyType
    {
        NotKey,
        Emerald,
        Ruby,
        Saphire
    }
    public int doorCode;
    public float keyRange = 2f;
    public bool isCrest;



    [Header("H e a l   S t a t s")]
    public bool isEdible;

    public int healAmount;



    [Header("F i r e a r m   S t a t s")]

    public AmmoType ammoType;
    [SerializeField]
    private int ammoStock;

    public float firearmDamage = 2f;
    public float knockback = 2f;
    public float fireRate, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, shotsPerTap;
    public bool allowButtonHold;
    public int bulletsLeft, bulletsShot;
    public LayerMask ignoreLayers;
    public float recoilAmount, recoilFalloff;
    // bools //
    bool shooting, readyToShoot, reloading;

    

    [Header("r e f e r e n c e")]
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem barrelSmoke;
    public RaycastHit rayHit;
    public LayerMask enemyType;
    private Animator animator;
    public GameObject player;

    public AmmoInventory ammo;

    private DoorController doorController;

    public PlayerStatus status;


    [SerializeField]
    private GameObject bulletHoleDecalPrefab;
    [SerializeField]
    private GameObject enemyHitParticle;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fpsCam = Camera.main;
        bulletsLeft = magazineSize;
        readyToShoot = true;
        animator = GetComponent<Animator>();
        status = player.GetComponent<PlayerStatus>();
        ammo = player.GetComponentInChildren<AmmoInventory>();

        ammoStock = ammo.GetStock(ammoType);
    }

    private void Update()
    {
        if (status.isDead == true)
        {
            return;
        }

        if (isKey == true)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                KeyInput();
            }
        }

        if (isFirearm == true)
        { 
            FirearmInput();
        }

        if (isHeal == true)
        {
            HealInput();
        }

        if (isFirearm == true)
        {
            if (bulletsLeft == 0)
            {
                animator.SetBool("Empty", true);
            }
        }
    }

    // K E Y //
    private void KeyInput()
    {

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, keyRange, ~ignoreLayers.value) && rayHit.collider.CompareTag("Door"))
        {
            Debug.Log("This is a Door");
            doorController = rayHit.collider.gameObject.GetComponent<DoorController>();

            if (doorController.doorUnlocked == false)
            {
                if (doorController.GetKeyType() == keyType)
                {
                    animator.SetTrigger("Unlock");
                    doorController.unlockDoor();
                }
            }
            else
            {
                animator.SetTrigger("Inspect");
            }
        }
        else
        {
            animator.SetTrigger("Inspect");
        }
    }




    // H E A L //
    private void HealInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && status.currentHealth < status.maxHealth)
        {
            itemEquip.enabled = false;
            if (isEdible == true)
            {
                animator.SetTrigger("Eat");
            }
            Destroy(gameObject, 1.2f);
            status.currentHealth = status.currentHealth += healAmount;
            return;
        }
    }




    // F I R E A R M //
    private void FirearmInput()
    {
        ammoStock = ammo.GetStock(ammoType);
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // reload //
        if (Input.GetKeyDown(KeyCode.R) && readyToShoot && bulletsLeft < magazineSize && !reloading && ammoStock > 0) Reload();

        // SHOOT //
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {

            bulletsShot = shotsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //SHOOTING FX//
        animator.SetTrigger("Shoot");
        muzzleFlash.Play();
        barrelSmoke.Play();


        //SPREAD//
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, ~ignoreLayers.value))
        {
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
            {
                GameObject enemyHitGO = Instantiate(enemyHitParticle, rayHit.point, Quaternion.LookRotation(-rayHit.normal));
                Destroy(enemyHitGO, 2f);

                if (rayHit.collider.name == "Weakpoint")
                {
                    rayHit.collider.GetComponentInParent<EnemyMovement>().DamageMultiplier(firearmDamage);
                }
                else
                {
                    rayHit.collider.GetComponentInParent<EnemyMovement>().TakeDamage(firearmDamage);
                }

                if (rayHit.collider.GetComponent<Rigidbody>() != null)
                {
                    Rigidbody rigidBody = rayHit.collider.GetComponent<Rigidbody>();
                    rigidBody.AddForce(fpsCam.transform.forward * knockback, ForceMode.Impulse);
                }
            }
            if (rayHit.collider.CompareTag("Item"))
            {
                Rigidbody rigidBody = rayHit.collider.GetComponent<Rigidbody>();
                rigidBody.AddForce(fpsCam.transform.forward * knockback, ForceMode.Impulse);
            }

            if (rayHit.collider.GetComponent<Rigidbody>() != null)
            {
                Rigidbody rigidBody = rayHit.collider.GetComponent<Rigidbody>();
                rigidBody.AddForce(fpsCam.transform.forward * knockback, ForceMode.Impulse);
            }

            if (rayHit.collider.CompareTag("Environment"))
            {
                var decal = Instantiate(bulletHoleDecalPrefab);
                decal.transform.position = rayHit.point;
                decal.transform.forward = rayHit.normal * -1f;
                Destroy(decal, 8f);
            }
        }

        CameraShaker.Instance.ShakeOnce(recoilAmount, recoilAmount, .01f, recoilFalloff);



        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", fireRate);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        animator.SetTrigger("Reload");
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        int newMag = ammo.Collect(ammoType, (-magazineSize + bulletsLeft));
        bulletsLeft = bulletsLeft + -newMag;
        reloading = false;
        animator.SetBool("Empty", false);
    }
}
