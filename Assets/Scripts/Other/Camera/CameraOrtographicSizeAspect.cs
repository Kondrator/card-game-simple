using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent( typeof(Camera) )]
public class CameraOrtographicSizeAspect : Controller {



    [SerializeField]
    [Range( 0f, 10f )]
    private float coefficient = 5f;



    private ScreenHelper screen = null;
    private ScreenHelper Screen { get { return screen = Find( screen ); } }


    private new Camera camera = null;
    private Camera Camera { get { return camera ??= GetComponent<Camera>(); } }





    protected override void PreInitiate() { }

    protected override void Initiate() {

        Add(
            new string[] {
                SceneController.NOTIFY_LOADED,
                ScreenHelper.NOTIFY_CHANGED,
            },
            data => {
                UpdateSize();
            }
        );

    }




    private void UpdateSize() {

        float max = Mathf.Max( Screen.Physical.Width, Screen.Physical.Height );
        float min = Mathf.Min( Screen.Physical.Width, Screen.Physical.Height );

        Camera.orthographicSize = coefficient * Mathf.Clamp( max / min, 1.6f, 10f );

    }


}
