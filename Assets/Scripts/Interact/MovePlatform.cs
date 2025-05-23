using System.Collections;
using UnityEngine;

/// <summary>
/// 움직이는 플랫폼
/// </summary>
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
        if (collision.gameObject.CompareTag(TagString.Player))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagString.Player))
        {
            collision.transform.SetParent(null);
        }
    }
    /// <summary>
    /// 있는 위치에 맞춰 움직이게 하는 코루틴
    /// </summary>
    /// <returns></returns>
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
