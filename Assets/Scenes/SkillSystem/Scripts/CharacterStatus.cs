using System;
using UnityEngine;

public class CharacterStatus : MonoBehaviour {
    
    public float HP {
        set { }
        get {
            return HP;
        }
    }

    public float MaxHP = 100;

    public float SP = 100;
    public float MaxSP = 100;

    public float baseATK = 100;
    public float Defence = 0;
    public float Interval = 0;
    public float Distance = 0;

    /// <summary>
    /// 减少HP
    /// </summary>
    /// <param name="atk">攻击力</param>
    internal void Damage(float atk)
    {
       
    }
}