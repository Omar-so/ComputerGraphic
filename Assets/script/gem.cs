using UnityEngine;

public class gem : MonoBehaviour
{
    [SerializeField] int rotateSpeed = 2;
    [SerializeField] AudioClip gemCollectSound;

    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.gameObject.name);

        // play sound only if assigned
        if (gemCollectSound != null)
        {
            AudioSource.PlayClipAtPoint(gemCollectSound, transform.position);
        }

        Destroy(gameObject); // 👈 moved here — always runs no matter what
    }
}