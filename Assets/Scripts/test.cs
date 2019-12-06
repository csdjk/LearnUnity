using UnityEngine;
public class test : MonoBehaviour {
    // Start is called before the first frame update
    void Start () {
        //注册事件
       EventCenter.AddListener<string>(EventType.ShowText,ShowText);
        //广播
       EventCenter.Broadcast(EventType.ShowText,"123456");
    }

   
    void OnDestroy()
    {
        //注销事件
       EventCenter.RemoveListener<string>(EventType.ShowText,ShowText);
        
    }

    void ShowText(string s){
        Debug.Log(s);
    }


}