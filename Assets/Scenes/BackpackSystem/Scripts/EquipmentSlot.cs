using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot {
    public Equipment.EquipmentType equipType;
    public Weapon.WeaponType wpType;

    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        // 鼠标右键脱下
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryManager.Instance.IsPickedItem == false && transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                Item itemTemp = currentItemUI.Item;
                DestroyImmediate(currentItemUI.gameObject);
                //脱掉放到背包里面
                transform.parent.SendMessage("PutOff", itemTemp);
                InventoryManager.Instance.HideToolTip();
            }
        }

        if (eventData.button != PointerEventData.InputButton.Left) return;
        // 手上有 东西
                        //当前装备槽 有装备
                        //无装备
        // 手上没 东西
                        //当前装备槽 有装备 
                        //无装备  不做处理
        bool isUpdateProperty = false;
        if (InventoryManager.Instance.IsPickedItem == true)
        {
            //手上有东西的情况
            ItemUI pickedItem = InventoryManager.Instance.PickedItem;
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI  = transform.GetChild(0).GetComponent<ItemUI>();//当前装备槽里面的物品

                if( IsRightItem(pickedItem.Item) ){
                    InventoryManager.Instance.PickedItem.Exchange(currentItemUI);
                    isUpdateProperty = true;
                }
            }
            else
            {
                if (IsRightItem(pickedItem.Item))
                {
                    this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    InventoryManager.Instance.PutBackItem(1);
                    isUpdateProperty = true;
                }

            }
        }
        else
        {
            //手上没东西的情况
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                InventoryManager.Instance.PickupItem(currentItemUI.Item, currentItemUI.Amount);
                Destroy(currentItemUI.gameObject);
                isUpdateProperty = true;
            }
        }
        if (isUpdateProperty)
        {
            // transform.parent.SendMessage("UpdatePropertyText");
        }
    }

    //判断item是否适合放在这个位置
    public bool IsRightItem(Item item)
    {
        if ((item is Equipment && ((Equipment)(item)).EquipType == this.equipType) ||
                    (item is Weapon && ((Weapon)(item)).WpType == this.wpType))
        {
            return true;
        }
        return false;
    }
}
