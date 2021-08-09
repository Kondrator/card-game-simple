using System.Collections;
using System.Collections.Generic;
using UIWindowManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class UITimer : UIWindow, IMyManagerMonoBehaviour {



    [SerializeField]
    private Text textTime = null;
    [SerializeField]
    private Text textTimeMilliseconds = null;

    [SerializeField]
    private SliderExtended sliderProgress = null;


    [Header( "Params" )]

    [SerializeField]
    private bool isOpenCloseWhenLaunchStop = true;





    [HideInInspector]
    public UnityEvent OnLaunched = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnCompleted = new UnityEvent();


    public bool IsLaucnhing { get { return isLaucnhing; } }
    private bool isLaucnhing = false;



    private float timeTarget = 0;
    private float time = 0;






    /// <summary>
    /// Prepared timer (not start)
    /// </summary>
    /// <param name="time">In seconds</param>
    public void Prepare( float time ) {
        Stop();

        this.time = timeTarget = time;

        SetTextTime( this.time );

        if( isOpenCloseWhenLaunchStop == true ) {
            Open();
        }
    }


    /// <summary>
    /// Stop and reset timer
    /// </summary>
    public void Stop() {
        isLaucnhing = false;
        MyManagerMonoBehaviour.Remove( this );

        Reset();

        if( isOpenCloseWhenLaunchStop == true ) {
            Close();
        }
    }

    /// <summary>
    /// Clear time and progress
    /// </summary>
    public void Reset() {
        time = timeTarget = 0;

        SetTextTime( 0 );
        sliderProgress?.SetAnimation( 1f );
    }


    /// <summary>
    /// Start executing timer
    /// </summary>
    public void Launch() {
        isLaucnhing = true;
        MyManagerMonoBehaviour.Add( this );

        OnLaunched.Invoke();

        if( isOpenCloseWhenLaunchStop == true ) {
            Open();
        }
    }

    /// <summary>
    /// Prepare and Start executing timer
    /// </summary>
    public void Launch( float time ) {
        Prepare( time );

        Launch();
    }




    private void SetTextTime( float seconds ) {
        textTime.SetText( ((int)seconds).SecondsToStringTime( true, false ) );

        int milliseconds = (int)( System.Math.Round( seconds, 4 ) % 1 ) * 1000;
        textTimeMilliseconds.SetText( milliseconds.ToString() );
        textTimeMilliseconds?.transform.SetActive( milliseconds > 0 );
    }





    public void UpdateMe( float timeDelta, MyManagerMonoBehaviourType type ) {

        time -= timeDelta;

        if( time <= 0 ) {
            time = 0;
            MyManagerMonoBehaviour.Remove( this );

            if( isOpenCloseWhenLaunchStop == true ) {
                Close();
            }

            OnCompleted.Invoke();
        }

        SetTextTime( time );
        sliderProgress?.Set( time / timeTarget );

    }


}
