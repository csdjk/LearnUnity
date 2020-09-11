using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GCTest : MonoBehaviour {


    //------------------------List test  PASS---------------------------------------
    private List<int> _ListTest;
    void List_Test_Update (int length) {
        for (int i = 0; i < length; i++) {
            // List<int> localList = new List<int> ();
            // _ListTest.Clear();
            _ListTest = new List<int> ();
        }
    }
    //------------------------List test end---------------------------------------

    //------------------------String test  PASS-------------------------------
    //----未优化----
    public Text timerText;
    private float timer;
    void String_Test_Update () {
        timer += Time.deltaTime;
        timerText.text = "Time:" + timer.ToString ();
    }
    //----优化版：避免频繁拼接字符串----
    public Text timerHeaderText;
    public Text timerValueText;
    private float _timer;
    void String_Start () {
        timerHeaderText.text = "TIME:";
    }
    void String_Test_optimize_Update () {
        timerValueText.text = timer.ToString ();
    }
    //------------------------String test  end---------------------------------------

    // 为优化版
    void StringPerformance(){
        String str = "";
        for (int i = 0; i < 1000; i++)
        {
            str += "sss";
        }
    }
    // 优化版 ：使用StringBuilder拼接字符串
    void StringPerformance_O(){
        StringBuilder str = new StringBuilder(1000);
        for (int i = 0; i < 1000; i++)
        {
            str.Append("sss");
        }
    }

    // 
    void Start () {
        String_Start ();
    }
    void Update () {
        // List_Test_Update (10000);

        // String_Test_Update ();
        // String_Test_optimize_Update ();

        // StringPerformance();
        // StringPerformance_O();
    }
}