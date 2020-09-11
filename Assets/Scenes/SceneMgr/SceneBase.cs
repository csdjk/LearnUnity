using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneBase : IState {
    public abstract uint StateId { get; set; }

    public void OnEnter (IState prevState, object param1 = null, object param2 = null) {
        this.onSceneEnter (prevState, param1);
    }

    public void OnFixedUpdate () {

    }

    public void OnLateUpdate () { }

    public void OnLeave (IState nextState, object param1 = null, object param2 = null) {
        this.onLeaveScene (nextState, param1);
    }

    public void OnRelease () {

    }

    public void OnUpdate () {

    }

    public abstract void onSceneEnter (IState prevState, object param);

    protected abstract void onLeaveScene (IState nextState, object param);
}

public class Scene1 : SceneBase {

    public override uint StateId { get => 1; set =>
            throw new System.NotImplementedException (); }

    public override void onSceneEnter (IState prevState, object param) {
        Debug.Log ("enter scene1, prevState:" + prevState + " parame:" + param);

    }
    protected override void onLeaveScene (IState nextState, object param) {
        Debug.Log ("Leave scene1, nextState " + nextState + " parame:" + param);
    }
}

public class Scene2 : SceneBase {

    public override uint StateId { get => 2; set =>
            throw new System.NotImplementedException (); }

    public override void onSceneEnter (IState prevState, object param) {
        Debug.Log ("enter scene2, prevState " + prevState + " parame:" + param);

    }
    protected override void onLeaveScene (IState nextState, object param) {
        Debug.Log ("Leave scene2, nextState " + nextState + " parame:" + param);
    }
}