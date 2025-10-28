using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI; // Referencja do panelu UI sklepu

    void Start()
    {
        if (shopUI != null)
            shopUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shopUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shopUI.SetActive(false);
        }
    }
}
