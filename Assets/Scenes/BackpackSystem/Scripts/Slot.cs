using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 物品槽
/// </summary>
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

    public GameObject itemPrefab;
    /// <summary>
    /// 把item放在自身下面
    /// 如果自身下面已经有item了，amount++
    /// 如果没有 根据itemPrefab去实例化一个item，放在下面
    /// </summary>
    /// <param name="item"></param>
    public void StoreItem (Item item) {
        if (transform.childCount == 0) {
            GameObject itemGameObject = Instantiate (itemPrefab) as GameObject;
            itemGameObject.transform.SetParent (this.transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI> ().SetItem (item);
        } else {
            transform.GetChild (0).GetComponent<ItemUI> ().AddAmount ();
        }
    }

    /// <summary>
    /// 得到当前物品槽存储的物品类型
    /// </summary>
    /// <returns></returns>
    public Item.ItemType GetItemType () {
        return transform.GetChild (0).GetComponent<ItemUI> ().Item.Type;
    }

    /// <summary>
    /// 得到物品的id
    /// </summary>
    /// <returns></returns>
    public int GetItemId () {
        return transform.GetChild (0).GetComponent<ItemUI> ().Item.ID;
    }

    public bool IsFilled () {
        ItemUI itemUI = transform.GetChild (0).GetComponent<ItemUI> ();
        return itemUI.Amount >= itemUI.Item.Capacity; //当前的数量大于等于容量
    }

    public void OnPointerEnter (PointerEventData eventData) {
        if (transform.childCount > 0) {
            string toolTipText = transform.GetChild (0).GetComponent<ItemUI> ().Item.GetToolTipText ();
            InventoryManager.Instance.ShowToolTip (toolTipText);
        }
    }

    public void OnPointerExit (PointerEventData eventData) {
        if (transform.childCount > 0)
            InventoryManager.Instance.HideToolTip ();
    }

    public virtual void OnPointerDown (PointerEventData eventData) {
        // 鼠标右键穿上装备
        if (eventData.button == PointerEventData.InputButton.Right) {
            if (InventoryManager.Instance.IsPickedItem == false && transform.childCount > 0) {
                ItemUI currentItemUI = transform.GetChild (0).GetComponent<ItemUI> ();
                if (currentItemUI.Item is Equipment || currentItemUI.Item is Weapon) {
                    currentItemUI.SubAmount (1);
                    Item currentItem = currentItemUI.Item;
                    if (currentItemUI.Amount <= 0) {
                        DestroyImmediate (currentItemUI.gameObject);
                        InventoryManager.Instance.HideToolTip ();
                    }
                    CharacterPanel.Instance.PutOn (currentItem);
                }
            }
        }
        if (eventData.button != PointerEventData.InputButton.Left) return;

        // 当前格子有物品
        if (transform.childCount > 0) {
            ItemUI currentItem = transform.GetChild (0).GetComponent<ItemUI> ();
            //当前没有选中任何物品( 当前手上没有任何物品)当前鼠标上没有任何物品
            if (InventoryManager.Instance.IsPickedItem == false) {
                // 按住 control 拾取一半
                if (Input.GetKey (KeyCode.LeftControl)) {
                    int amountPicked = (currentItem.Amount + 1) / 2;
                    InventoryManager.Instance.PickupItem (currentItem.Item, amountPicked);
                    int amountRemained = currentItem.Amount - amountPicked;
                    if (amountRemained <= 0) {
                        Destroy (currentItem.gameObject); //销毁当前物品
                    } else {
                        currentItem.SetAmount (amountRemained);
                    }
                } //拾取全部 
                else {
                    InventoryManager.Instance.PickupItem (currentItem.Item, currentItem.Amount);
                    Destroy (currentItem.gameObject); //销毁当前物品
                }
            } // 当前手上已有物品
            else {
                //如果是同一个物品       
                if (currentItem.Item.ID == InventoryManager.Instance.PickedItem.Item.ID) {
                    // 一个一个的添加
                    if (Input.GetKey (KeyCode.LeftControl)) {
                        if (currentItem.Item.Capacity > currentItem.Amount) //当前物品槽还有容量
                        {
                            currentItem.AddAmount ();
                            InventoryManager.Instance.PutBackItem ();
                        } else {
                            return;
                        }
                    } // 添加全部
                    else {
                        if (currentItem.Item.Capacity > currentItem.Amount) {
                            int amountRemain = currentItem.Item.Capacity - currentItem.Amount; //当前物品槽剩余的空间
                            if (amountRemain >= InventoryManager.Instance.PickedItem.Amount) {
                                currentItem.SetAmount (currentItem.Amount + InventoryManager.Instance.PickedItem.Amount);
                                InventoryManager.Instance.PutBackItem (InventoryManager.Instance.PickedItem.Amount);
                            } else {
                                currentItem.SetAmount (currentItem.Amount + amountRemain);
                                InventoryManager.Instance.PutBackItem (amountRemain);
                            }
                        } else {
                            return;
                        }
                    }
                } //不是同一个物品,则交换选中的物品和格子中的物品
                else {
                    Item item = currentItem.Item;
                    int amount = currentItem.Amount;
                    currentItem.SetItem (InventoryManager.Instance.PickedItem.Item, InventoryManager.Instance.PickedItem.Amount);
                    InventoryManager.Instance.PickedItem.SetItem (item, amount);
                }
            }
        } // 当前格子为空 
        else {
            if (InventoryManager.Instance.IsPickedItem == true) {
                if (Input.GetKey (KeyCode.LeftControl)) {
                    this.StoreItem (InventoryManager.Instance.PickedItem.Item);
                    InventoryManager.Instance.PutBackItem ();
                } else {
                    for (int i = 0; i < InventoryManager.Instance.PickedItem.Amount; i++) {
                        this.StoreItem (InventoryManager.Instance.PickedItem.Item);
                    }
                    InventoryManager.Instance.PutBackItem (InventoryManager.Instance.PickedItem.Amount);
                }
            } else {
                return;
            }

        }
    }
}