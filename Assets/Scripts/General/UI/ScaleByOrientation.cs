using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScaleByOrientation : Controller {


    [SerializeField]
    private float scaleByVertical = 1f;

    [SerializeField]
    private float scaleByHorizontal = 1f;



    private ScreenHelper screen = null;
    private ScreenHelper Screen { get { return screen = Find( screen ); } }


    private RectTransform rect = null;
    private RectTransform Rect { get { return rect ??= this.transform as RectTransform; } }





    protected override void PreInitiate() { }

    protected override void Initiate() {

        Add(
            new string[] {
                SceneController.NOTIFY_LOADED,
                ScreenHelper.NOTIFY_CHANGED,
            },
            data => {
                UpdateScale();
            }
        );

    }




    private void UpdateScale() {

        float scale = Screen.Orientation == ScreenHelper.Type.Vertical ? scaleByVertical : scaleByHorizontal;

        Rect.localScale = new Vector3( scale, scale, scale );

    }

}
