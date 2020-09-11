using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    #region 单例模式
    private static InventoryManager _instance;

    public static InventoryManager Instance {
        get {
            if (_instance == null) {
                //下面的代码只会执行一次
                _instance = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ();
            }
            return _instance;
        }
    }
    #endregion

    /// <summary>
    ///  物品信息的列表（集合）
    /// </summary>
    private List<Item> itemList;

    #region ToolTip
    private ToolTip toolTip;

    private bool isToolTipShow = false;

    private Vector2 toolTipPosionOffset = new Vector2 (10, -10);
    #endregion

    #region PickedItem
    private bool isPickedItem = false;

    public bool IsPickedItem {
        get {
            return isPickedItem;
        }
    }

    private ItemUI pickedItem; //鼠标选中的物体

    public ItemUI PickedItem {
        get {
            return pickedItem;
        }
    }
    #endregion

    private Canvas canvas;

    void Awake () {
        ParseItemJson ();
    }

    void Start () {
        toolTip = GameObject.FindObjectOfType<ToolTip> ();
        canvas = GameObject.Find ("Canvas").GetComponent<Canvas> ();
        pickedItem = GameObject.Find("PickedItem").GetComponent<ItemUI>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update () {
        if (isPickedItem)
        {
            //如果我们捡起了物品，我们就要让物品跟随鼠标
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            pickedItem.SetLocalPosition(position);
        }else if (isToolTipShow)
        {
            //控制提示面板跟随鼠标
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toolTip.SetLocalPotion(position+toolTipPosionOffset);
        }

        //物品丢弃的处理
        if (isPickedItem && Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)==false)
        {
            isPickedItem = false;
            PickedItem.Hide();
        }
    }

    /// <summary>
    /// 解析物品信息
    /// </summary>
    void ParseItemJson () {
        itemList = new List<Item> ();
        //文本为在Unity里面是 TextAsset类型
        TextAsset itemText = Resources.Load<TextAsset> ("Items");
        string itemsJson = itemText.text; //物品信息的Json格式
        JSONObject j = new JSONObject (itemsJson);
        foreach (JSONObject temp in j.list) {
            string typeStr = temp["type"].str;
            Item.ItemType type = (Item.ItemType) System.Enum.Parse (typeof (Item.ItemType), typeStr);

            //下面的事解析这个对象里面的公有属性
            int id = (int) (temp["id"].n);
            string name = temp["name"].str;
            Item.ItemQuality quality = (Item.ItemQuality) System.Enum.Parse (typeof (Item.ItemQuality), temp["quality"].str);
            string description = temp["description"].str;
            int capacity = (int) (temp["capacity"].n);
            int buyPrice = (int) (temp["buyPrice"].n);
            int sellPrice = (int) (temp["sellPrice"].n);
            string sprite = temp["sprite"].str;

            Item item = null;
            switch (type) {
                case Item.ItemType.Consumable:
                    int hp = (int) (temp["hp"].n);
                    int mp = (int) (temp["mp"].n);
                    item = new Consumable (id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, hp, mp);
                    break;
                case Item.ItemType.Equipment:
                    //
                    int strength = (int) temp["strength"].n;
                    int intellect = (int) temp["intellect"].n;
                    int agility = (int) temp["agility"].n;
                    int stamina = (int) temp["stamina"].n;
                    Equipment.EquipmentType equipType = (Equipment.EquipmentType) System.Enum.Parse (typeof (Equipment.EquipmentType), temp["equipType"].str);
                    item = new Equipment (id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, strength, intellect, agility, stamina, equipType);
                    break;
                case Item.ItemType.Weapon:
                    //
                    int damage = (int) temp["damage"].n;
                    Weapon.WeaponType wpType = (Weapon.WeaponType) System.Enum.Parse (typeof (Weapon.WeaponType), temp["weaponType"].str);
                    item = new Weapon (id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, damage, wpType);
                    break;
                case Item.ItemType.Material:
                    //
                    item = new Materials (id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite);
                    break;
            }
            itemList.Add (item);
            // Debug.Log (item);
        }
    }

    public Item GetItemById (int id) {
        foreach (Item item in itemList) {
            if (item.ID == id) {
                return item;
            }
        }
        return null;
    }

    public void ShowToolTip (string content) {
        if (this.isPickedItem) return;
        isToolTipShow = true;
        toolTip.Show (content);
    }

    public void HideToolTip () {
        isToolTipShow = false;
        toolTip.Hide ();
    }

    //捡起物品槽指定数量的物品
    public void PickupItem(Item item,int amount)
    {
        PickedItem.SetItem(item, amount);
        isPickedItem = true;

        PickedItem.Show();
        this.toolTip.Hide();
        //如果我们捡起了物品，我们就要让物品跟随鼠标
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
        pickedItem.SetLocalPosition(position);
    }

    /// <summary>
    /// 从手上拿掉一个物品放在物品槽里面
    /// </summary>
    public void PutBackItem(int amount=1)
    {
        PickedItem.SubAmount(amount);
        if (PickedItem.Amount <= 0)
        {
            isPickedItem = false;
            PickedItem.Hide();
        }
    }
}