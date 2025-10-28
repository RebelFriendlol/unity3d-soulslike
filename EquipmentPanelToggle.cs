using UnityEngine;

public class EquipmentPanelToggle : MonoBehaviour
{
    public GameObject equipmentPanel;

    public void TogglePanel()
    {
        equipmentPanel.SetActive(!equipmentPanel.activeSelf);
    }

    public void ClosePanel()
    {
        equipmentPanel.SetActive(false);
    }

    void Start()
    {
        if (equipmentPanel != null)
            equipmentPanel.SetActive(false); // Ukryj na starcie
    }
}
