using System.Collections;
using UnityEngine;

public class ShootPlatform : MonoBehaviour
{
    private float waitTime = 1f;
    private Coroutine coroutine;
    private float shootPower = 10f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagString.Player))
        {
            coroutine = StartCoroutine(Shoot(collision.gameObject.GetComponent<Rigidbody>()));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagString.Player))
        {
            StopCoroutine(coroutine);
        }
    }
    IEnumerator Shoot(Rigidbody rb)
    {
        yield return new WaitForSeconds(waitTime);

        rb.AddForce(Vector3.up * shootPower, ForceMode.Impulse);
    }
}
