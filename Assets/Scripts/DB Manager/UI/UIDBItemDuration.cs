using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class UIDBItemDuration : MonoBehaviour {


    [SerializeField]
    private UITimer timer = null;


    [HideInInspector]
    public UnityEvent OnShow = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnHide = new UnityEvent();




    private void Awake() {

        timer?.OnLaunched.AddListener( OnShow.Invoke );

        timer?.OnCompleted.AddListener( OnHide.Invoke );

    }


    public void Set( DBItemDuration item, Duration elapsed ) {

        if( item == null ) {
            timer?.Stop();
            return;
        }

        Duration duration = new Duration( item.Duration.Milliseconds - elapsed.Milliseconds );

        if( duration.Seconds <= 0 ) {
            timer?.Stop();

        } else {
            timer?.Prepare( duration.Seconds );
            timer?.Launch();
        }

    }

    public void Set() {
        timer?.Stop();
    }

}