using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scene1 scene1 = new Scene1();
        Scene2 scene2 = new Scene2();
        SceneManagr.addScene(scene1);
        SceneManagr.addScene(scene2);


        SceneManagr.go(scene1.StateId,"zzzz");
        SceneManagr.go(scene2.StateId,"xxxx");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
