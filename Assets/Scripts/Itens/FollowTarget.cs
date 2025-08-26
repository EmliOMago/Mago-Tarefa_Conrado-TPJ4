using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target = null)
        {
            Debug.Log("objeto sem alvo");
            Destroy(this);

        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
