using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    [SerializeField] private Transform transform;
    [SerializeField] private LayerMask layerMask;

    private float distance = 5f;
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * distance);
        if(Physics.Raycast(transform.position,transform.forward, out var hit, distance, layerMask))
        {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(-hit.transform.forward * 5f, ForceMode.Impulse);
        }
    }
}
