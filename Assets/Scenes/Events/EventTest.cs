using UnityEngine;
public class EventTest : MonoBehaviour {
    // Start is called before the first frame update
    void Start () {
        //注册事件
       EventCenter.AddListener<string>(MyEventType.ShowText,ShowText);
        //广播
       EventCenter.Broadcast(MyEventType.ShowText,"123456");
    }

   
    void OnDestroy()
    {
        //注销事件
       EventCenter.RemoveListener<string>(MyEventType.ShowText,ShowText);
        
    }

    void ShowText(string s){
        Debug.Log(s);
    }

}