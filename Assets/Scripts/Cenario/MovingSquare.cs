using UnityEngine;

public class MovingSquare : MonoBehaviour
{
    public float amplitude = 2.0f;
    public float speed = 1.0f;
    public bool vertical = true;

    private Vector3 initialPosition;
    private float movementOffset;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        movementOffset = Mathf.Sin(Time.time * speed) * amplitude;

        if (vertical)
        {
            transform.position = initialPosition + Vector3.up * movementOffset;
        }
        else
        {
            transform.position = initialPosition + Vector3.right * movementOffset;
        }
    }
}