using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private Vector3 min;
    [SerializeField] private Vector3 max;
    private float speed = 2f;
    private float waitTime = 1f;

    private bool isMoving = true;
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        target = max;
        StartCoroutine(Move());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
    IEnumerator Move()
    {
        while (true)
        {
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }

            transform.position = target;

            yield return new WaitForSeconds(waitTime);

            target = target == max ? min : max;
        }
    }
}
