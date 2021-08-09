using System.Collections;
using System.Collections.Generic;
using UIWindowManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIButtonApply : MonoBehaviour {

    private enum State {
        Default = 0,
        Apply = 1,
    }




    [SerializeField]
    private UIButtonAnimation buttonDefault = null;

    [SerializeField]
    private UIButtonAnimation buttonApply = null;




    public UnityEvent OnApply = new UnityEvent();



    private State state = State.Default;



    private void Start() {

        buttonDefault.OnClick.AddListener( () => {
            Set( State.Apply );
        } );

        buttonApply.OnClick.AddListener( () => {
            Set( State.Default );
            OnApply.Invoke();
        } );

        Set( State.Default );
    }


    private void OnEnable() {
        Set( State.Default );
    }



    private void Set( State state ) {
        this.state = state;

        buttonDefault.Visible( state == State.Default );
        buttonApply.Visible( state == State.Apply );

    }

}
