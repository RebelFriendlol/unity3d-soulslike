using UnityEngine;
using UnityEngine.UI;

public class Vendor : MonoBehaviour
{
    public SimpleItem[] items;
    public GameObject[] itemSlots;

    public Image itemPreviewImage;
    public Text itemNameText;
    public Text itemDescriptionText;
    public Text itemPriceText;

    public Button buyButton;
    public PlayerStats playerStats;

    private int selectedItemIndex = -1;
    private bool[] purchasedItems;

    void Start()
    {
        purchasedItems = new bool[items.Length];
        SetupItems();

        if (buyButton != null)
            buyButton.onClick.AddListener(BuyItem);
    }

    void SetupItems()
    {
        for (int i = 0; i < items.Length && i < itemSlots.Length; i++)
        {
            int index = i;
            itemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].icon;
            itemSlots[i].GetComponent<Button>().interactable = true;
            itemSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
            itemSlots[i].GetComponent<Button>().onClick.AddListener(() => ShowPreview(index));
        }
    }

    void ShowPreview(int index)
    {
        selectedItemIndex = index;
        itemPreviewImage.sprite = items[index].icon;
        itemPreviewImage.color = new Color(1f, 1f, 1f, 1f);

        itemNameText.text = items[index].itemName;
        itemDescriptionText.text = items[index].itemDescription;
        itemPriceText.text = "Price: " + items[index].itemPrice + " Souls";
    }

    void HidePreview()
    {
        selectedItemIndex = -1;
        itemPreviewImage.sprite = null;
        itemPreviewImage.color = new Color(1f, 1f, 1f, 0f);

        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemPriceText.text = "";
    }

    void BuyItem()
    {
        if (selectedItemIndex < 0 || selectedItemIndex >= items.Length)
            return;

        if (purchasedItems[selectedItemIndex])
            return;

        int price = items[selectedItemIndex].itemPrice;

        if (playerStats.souls >= price)
        {
            playerStats.souls -= price;
            purchasedItems[selectedItemIndex] = true;

            Debug.Log("Bought item: " + items[selectedItemIndex].itemName);

            Button slotButton = itemSlots[selectedItemIndex].GetComponent<Button>();
            slotButton.interactable = false;

            Image iconImage = itemSlots[selectedItemIndex].transform.GetChild(0).GetComponent<Image>();
            iconImage.sprite = null;
            iconImage.enabled = false;

            playerStats.NotifyStatsChanged();
            HidePreview();
        }
        else
        {
            Debug.Log("Not enough souls!");
        }
    }

    public bool[] GetPurchasedItems()
    {
        return purchasedItems;
    }

    public SimpleItem[] GetItems()
    {
        return items;
    }

    public SimpleItem GetItemByIndex(int index)
    {
        if (index >= 0 && index < items.Length)
            return items[index];
        return null;
    }
}
