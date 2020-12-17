using UnityEngine;
using System.Collections;
using TMPro;

public class ItemSwitching : MonoBehaviour
{

    public int selectedSlot = 0;
    public Transform currentSlot;

    public TMP_Text Text;

    public GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        SelectItemSlot();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedSlot = selectedSlot;


        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedSlot >= transform.childCount - 1)
                selectedSlot = 0;
            else
            selectedSlot++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedSlot <= 0)
                selectedSlot = transform.childCount - 1;
            else
                selectedSlot--;
        }


        // Inventory Keycodes //
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedSlot = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedSlot = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedSlot = 2;
        }



        if (previousSelectedSlot != selectedSlot)
        {
            
            SelectItemSlot();
        }
    }

    void SelectItemSlot()
    {
        int i = 0;
        foreach (Transform itemSlot in transform)
        {
            if(i == selectedSlot)
            {
                itemSlot.gameObject.SetActive(true);
                currentSlot = itemSlot.gameObject.transform;
                currentSlot.gameObject.tag = "CurrentlyEquipped";
                SetText();
                //Text.text = currentSlot.gameObject.child.name;


            }
            else
            {
                itemSlot.gameObject.SetActive(false);
                gameObject.tag = "Untagged";
            }

            i++;
        }
    }

    public void SetText()
    {
        foreach (Transform eachChild in currentSlot)
        {
            Text.text = eachChild.gameObject.name;
        }
    }
}
