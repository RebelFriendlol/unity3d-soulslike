using UnityEngine;
using UnityEngine.UI;

public class DashPickupZone : MonoBehaviour
{
    public GameObject vfxEffect;
    public DashManager dashManager;
    public Text pickupText;

    private bool playerInside = false;

    void Start()
    {
        if (vfxEffect != null)
        {
            Renderer renderer = vfxEffect.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (!renderer.material.name.EndsWith("(Instance)"))
                    renderer.material = new Material(renderer.sharedMaterial);

                renderer.material.SetColor("_SpecColor", Color.white);
            }

            vfxEffect.SetActive(true);
        }

        if (pickupText != null)
            pickupText.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            pickupText.enabled = true;
            GiveOneDash(); // automatycznie po wejœciu
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (pickupText != null)
                pickupText.enabled = false;
        }
    }

    void GiveOneDash()
    {
        if (dashManager == null)
        {
            Debug.LogWarning("Brak przypisanego DashManagera!");
            return;
        }

        if (dashManager.TryAddExtraDash())
        {
            Debug.Log("Gracz otrzyma³ jeden dash!");
        }

        if (pickupText != null)
            pickupText.enabled = false;

        gameObject.SetActive(false);
    }
}
