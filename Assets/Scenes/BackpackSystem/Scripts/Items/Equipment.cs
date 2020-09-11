using UnityEngine;
using System.Collections;

public class Equipment : Item {

    /// <summary>
    /// 力量
    /// </summary>
    public int Strength { get; set; }
    /// <summary>
    /// 智力
    /// </summary>
    public int Intellect { get; set; }
    /// <summary>
    /// 敏捷
    /// </summary>
    public int Agility { get; set; }
    /// <summary>
    /// 体力
    /// </summary>
    public int Stamina { get; set; }
    /// <summary>
    /// 装备类型
    /// </summary>
    public EquipmentType EquipType { get; set; }

    public Equipment(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice,string sprite,
        int strength,int intellect,int agility,int stamina,EquipmentType equipType)
        : base(id, name, type, quality, des, capacity, buyPrice, sellPrice,sprite)
    {
        this.Strength = strength;
        this.Intellect = intellect;
        this.Agility = agility;
        this.Stamina = stamina;
        this.EquipType = equipType;
    }

    public enum EquipmentType
    {
        None,
        Head,
        Neck,
        Chest,
        Ring,
        Leg,
        Bracer,
        Boots,
        Shoulder,
        Belt,
        OffHand
    }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();

        string equipTypeText = "";
        switch (EquipType)
	{
		case EquipmentType.Head:
                equipTypeText="头部";
         break;
        case EquipmentType.Neck:
                equipTypeText="脖子";
         break;
        case EquipmentType.Chest:
                equipTypeText="胸部";
         break;
        case EquipmentType.Ring:
                equipTypeText="戒指";
         break;
        case EquipmentType.Leg:
                equipTypeText="腿部";
         break;
        case EquipmentType.Bracer:
                equipTypeText="护腕";
         break;
        case EquipmentType.Boots:
                equipTypeText="靴子";
         break;
        case EquipmentType.Shoulder:
                equipTypeText="护肩";
         break;
        case EquipmentType.Belt:
                equipTypeText = "腰带";
         break;
        case EquipmentType.OffHand:
                equipTypeText="副手";
         break;
	}

        string newText = string.Format("{0}\n\n<color=blue>装备类型：{1}\n力量：{2}\n智力：{3}\n敏捷：{4}\n体力：{5}</color>", text,equipTypeText,Strength,Intellect,Agility,Stamina);

        return newText;
    }
}
