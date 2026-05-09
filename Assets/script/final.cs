using UnityEngine;

public class final : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject playerControl;
    [SerializeField] AudioSource levelJingle;
    [SerializeField] GameObject levelBGM;
    [SerializeField] GameObject fadeOut;

    void OnTriggerEnter(Collider other)
    {
        playerControl.GetComponent<PlayerControls>().enabled = false;
        playerControl.GetComponent<Animator>().Play("Idle");
        levelBGM.SetActive(false);
        levelJingle.Play();
        fadeOut.SetActive(true);
    }
}
