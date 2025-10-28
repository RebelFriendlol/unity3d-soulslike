using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SimpleItem
{
    public Sprite icon;
    public string itemName;

    [TextArea]
    public string itemDescription;

    public int itemPrice; // Price in Souls
}
