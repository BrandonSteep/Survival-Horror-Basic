using UnityEngine;

public class ItemSway : MonoBehaviour
{
    public float amount;
    public float smoothAmount;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);

        transform.localPosition = Vector3.Slerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
    }
}