using UnityEngine;

/// <summary>
/// 점프 발판
/// </summary>
public class JumpPad : MonoBehaviour
{
    private float jumpForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out Rigidbody rb)) return;

        Vector3 contactDir = (rb.transform.position - transform.position).normalized;

        if (Vector3.Dot(contactDir, Vector3.up) > 0.5f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            SoundManager.Instance.PlaySound(SoundType.SFX, "JumpPad");
        }
    }
}
