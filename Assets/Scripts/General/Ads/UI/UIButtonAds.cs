using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[RequireComponent( typeof( Button ) )]
public class UIButtonAds : Element {

    [Header( "UI" )]

    [SerializeField]
    private Text text = null;



    [Header( "Events" )]

    [SerializeField]
    public UnityEvent OnSuccess = new UnityEvent();

    [SerializeField]
    public UnityEvent OnFail = new UnityEvent();

    [SerializeField]
    public UnityEvent OnCompleted = new UnityEvent();



    private Button button = null;



    private void Awake() {
        button = GetComponent<Button>();
        button.AddListenerOnClick( CallbackClick );
    }


    private void CallbackClick() {

        System.Action success = () => OnSuccess.Invoke();
        System.Action fail = () => OnFail.Invoke();
        System.Action completed = () => OnCompleted.Invoke();

        Notify(
            AdsController.NOTIFY.SHOW_VIDEO.NAME,
            new NotifyData.Param( AdsController.NOTIFY.PARAM_SUCCESS, success ),
            new NotifyData.Param( AdsController.NOTIFY.PARAM_FAIL, fail ),
            new NotifyData.Param( AdsController.NOTIFY.PARAM_COMPLETED, completed )
        );

    }



    public void SetText( string text ) {
        this.text.SetText( text );
    }


}
