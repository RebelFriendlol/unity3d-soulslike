using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EquipmentTogglePanel : MonoBehaviour
{
    public Vendor vendor;
    public GameObject itemEntryPrefab;         // Prefab zawieraj¹cy: ItemName (Text), ItemToggle (Toggle)
    public Transform itemListParent;           // Obiekt z VerticalLayoutGroup
    public Image equippedItemIcon;
    public Text equippedItemNameText;

    private List<Toggle> toggles = new List<Toggle>();
    private int equippedIndex = -1;

    void OnEnable()
    {
        RefreshItemList();
    }

    void RefreshItemList()
    {
        // Wyczyœæ poprzedni¹ listê
        foreach (Transform child in itemListParent)
        {
            Destroy(child.gameObject);
        }

        toggles.Clear();
        GenerateItemList();
    }

    void GenerateItemList()
    {
        SimpleItem[] items = vendor.GetItems();
        bool[] purchased = vendor.GetPurchasedItems();

        for (int i = 0; i < items.Length && i < 10; i++)
        {
            if (!purchased[i])
                continue;

            GameObject entry = Instantiate(itemEntryPrefab, itemListParent);
            Text nameText = entry.transform.Find("ItemName").GetComponent<Text>();
            Toggle toggle = entry.transform.Find("ItemToggle").GetComponent<Toggle>();

            int index = i;
            nameText.text = items[i].itemName;
            toggle.isOn = false;

            toggle.onValueChanged.AddListener((bool isOn) =>
            {
                if (isOn)
                    OnItemSelected(index);
            });

            toggles.Add(toggle);
        }
    }

    void OnItemSelected(int selectedIndex)
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            if (i != selectedIndex)
                toggles[i].isOn = false;
        }

        SimpleItem selectedItem = vendor.GetItemByIndex(selectedIndex);
        if (selectedItem != null)
        {
            equippedItemIcon.sprite = selectedItem.icon;
            equippedItemIcon.enabled = true;

            equippedItemNameText.text = "Equipped: " + selectedItem.itemName;
            Debug.Log("Equipped item: " + selectedItem.itemName);

            equippedIndex = selectedIndex;
        }
    }
}
