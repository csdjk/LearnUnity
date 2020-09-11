using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    #region basic property
    private int basicStrength = 10;
    private int basicIntellect = 10;
    private int basicAgility = 10;
    private int basicStamina = 10;
    private int basicDamage = 10;

    public int BasicStrength
    {
        get
        {
            return basicStrength;
        }
    }
    public int BasicIntellect
    {
        get
        {
            return basicIntellect;
        }
    }
    public int BasicAgility
    {
        get
        {
            return basicAgility;
        }
    }
    public int BasicStamina
    {
        get
        {
            return basicStamina;
        }
    }
    public int BasicDamage
    {
        get
        {
            return basicDamage;
        }
    }
    #endregion

    private int coinAmount = 100;

    private Text coinText;

    public int CoinAmount
    {
        get
        {
            return coinAmount;
        }
        set
        {
            coinAmount = value;
            coinText.text = coinAmount.ToString();
        }
    }

    void Start()
    {
        // coinText = GameObject.Find("Coin").GetComponentInChildren<Text>();
        // coinText.text = coinAmount.ToString();
    }

    // Update is called once per frame
	void Update () {
        //G 随机得到一个物品放到背包里面
        if (Input.GetKeyDown(KeyCode.G))
        {
            int id = Random.Range(1, 18);
            Knapsack.Instance.StoreItem(id);
        }

        //T 控制背包的显示和隐藏
        if (Input.GetKeyDown(KeyCode.T))
        {
            Knapsack.Instance.DisplaySwitch();
        }
        //Y 控制箱子的显示和隐藏
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Chest.Instance.DisplaySwitch();
        }
        // //U 控制角色面板的 显示和隐藏
        // if (Input.GetKeyDown(KeyCode.U))
        // {
        //     CharacterPanel.Instance.DisplaySwitch();
        // }
        // //I 控制商店显示和隐藏 
        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     Vendor.Instance.DisplaySwitch();
        // }
        // //O 控制锻造界面显示和隐藏 
        // if (Input.GetKeyDown(KeyCode.O))
        // {
        //     Forge.Instance.DisplaySwitch();
        // }
	}

    /// <summary>
    /// 消费
    /// </summary>
    public bool ConsumeCoin(int amount)
    {
        if (coinAmount >= amount)
        {
            coinAmount -= amount;
            coinText.text = coinAmount.ToString();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 赚取金币
    /// </summary>
    /// <param name="amount"></param>
    public void EarnCoin(int amount)
    {
        this.coinAmount += amount;
        coinText.text = coinAmount.ToString();
    }
}
