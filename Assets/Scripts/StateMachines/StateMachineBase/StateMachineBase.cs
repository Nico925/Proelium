using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineBase : MonoBehaviour {

    private StateBase _currentState;

    public StateBase CurrentState
    {
        get { return _currentState; }
        set 
        {
            if (_currentState != value) 
            {
                if (CheckRules(value, _currentState) == true) {
                    OnStateChange(value, _currentState);
                    _currentState = value;
                } else {

                    Debug.LogWarningFormat("Non è possibile passare dallo stato {0} allo stato {1}", 
                        _currentState != null ? _currentState.StateName : "Null", 
                        value.StateName);
                    PassThroughOrder.Clear();
                }
            }
        }
    }

    List<StateBase> PassThroughOrder = new List<StateBase>();


    protected bool PassThrough {
        get 
        {
            if (PassThroughOrder.Count > 0)
                return true;
            else
                return false;
        }
    }


    void OnStateChange(StateBase _newState, StateBase _oldState)
    {

        if (_oldState != null) { 
            _oldState.OnEnd();
            _oldState.OnStateEnd -= OnCurrentStateEnded;
        }
        _newState.OnStateEnd += OnCurrentStateEnded;
        _newState.OnPreStart(this);
    }

    public void SetPassThroughOrder(List<StateBase> _passThroughOrder) 
    {
        if(PassThroughOrder.Count > 0)
        {
            foreach (StateBase item in _passThroughOrder)
            {
                PassThroughOrder.Add(item);
            }
        }
        else
        {
            PassThroughOrder = _passThroughOrder;
        }
            
        CurrentState = PassThroughOrder[0];
        PassThroughOrder.Remove(CurrentState);
    }

    /// <summary>
    /// Controlla se la transizione fra il vecchio strato ed il nuovo stato è consentita
    /// overraidare per aggiungere regole
    /// </summary>
    /// <param name="_newState"></param>
    /// <param name="_oldState"></param>
    /// <returns></returns>
    protected virtual bool CheckRules(StateBase _newState, StateBase _oldState) 
    {
        return true;
    }

    private void Update()
    {
        if (CurrentState != null)
            CurrentState.OnUpdate();
        if (PassThrough == true) 
        {
            CurrentState = PassThroughOrder[0];
            PassThroughOrder.Remove(CurrentState);
        }
    }

    #region Events
    public delegate void MachineEvent(string _machineName);

    public static MachineEvent OnMachineEnd;

    #region Handlers
    /// <summary>
    /// Chiamata in automatico quando lo stato corrente si auto dichiara terminato.
    /// </summary>
    protected virtual void OnCurrentStateEnded() { }
    #endregion

    #endregion
}
