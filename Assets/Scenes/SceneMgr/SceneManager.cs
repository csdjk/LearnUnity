using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagr : MonoBehaviour {
    private static StateMachine mSceneStateMachine = new StateMachine ();

    public static SceneBase current {
        get {
            return mSceneStateMachine.CurState == null ? null : mSceneStateMachine.CurState as SceneBase;
        }
    }

    public static void go (uint sceneId, object param) {
        mSceneStateMachine.SwitchState (sceneId, param);
    }

    public static void addScene (SceneBase scene) {
        mSceneStateMachine.RegisterState (scene);
    }

    public static void removeScene (uint sceneId) {
        mSceneStateMachine.RemoveState (sceneId);
    }

    void Update () {
        mSceneStateMachine.Update ();
    }
}