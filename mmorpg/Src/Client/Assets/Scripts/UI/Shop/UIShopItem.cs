using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour, ISelectHandler 
{

    public Image icon;
    public Text title;
    public Text price;
    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    private bool selected;

    public bool Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;
            this.background.overrideSprite = selected ? selectedBg : normalBg;
        }
    }

    public int ShopItemID { get; set; }

    private UIShop shop;
    private ItemDefine item;
    private ShopItemDefine ShopItem {  get; set; }

    public void SetShopItem(int id, ShopItemDefine shopItem, UIShop owner)
    {
        this.shop = owner;
        this.ShopItemID = id;
        this.ShopItem = shopItem;
        this.item = DataManager.Instance.Items[this.ShopItem.ItemID];
    }

    public void OnSelect(BaseEventData eventData)
    {
        this.Selected = true;
        this.shop.SelectShopItem(this);
    }
}
