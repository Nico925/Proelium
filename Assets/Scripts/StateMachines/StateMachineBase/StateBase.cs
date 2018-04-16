using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase {

    public string StateName;

    protected StateMachineBase stateMachine;

    public StateBase() {
        StateName = GetType().FullName;
    }

    public void OnPreStart(StateMachineBase _stateMachine) {
        stateMachine = _stateMachine;
        OnStart();
    }

    public virtual void OnStart() { }

    public virtual void OnUpdate() { }

    public virtual void OnEnd() { }

    #region Events
    public delegate void StateEvent();

    public StateEvent OnStateEnd;
    #endregion

}
