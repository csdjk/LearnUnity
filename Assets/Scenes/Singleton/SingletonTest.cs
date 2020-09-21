using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonTest : MonoSingleton<SingletonTest>
{
    public override void Init(){
        Debug.Log("初始化");
    }

    public void Test(){

    }
}
