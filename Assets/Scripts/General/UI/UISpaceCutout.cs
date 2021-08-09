using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent( typeof(LayoutElement) )]
public class UISpaceCutout : Controller {


    private ScreenHelper screen = null;
    private ScreenHelper Screen { get { return screen = Find( screen ); } }


    private LayoutElement layout = null;
    private LayoutElement Layout { get { return GetComponent<LayoutElement>(); } }






    protected override void PreInitiate() { }

    protected override void Initiate() {

        Add(
            new string[] {
                SceneController.NOTIFY_LOADED,
                ScreenHelper.NOTIFY_CHANGED,
            },
            data => {
                UpdateSpace();
            }
        );

    }




    private void UpdateSpace() {

        float size = 0;
        if( Screen.Orientation == ScreenHelper.Type.Vertical
            && UnityEngine.Screen.cutouts.Length > 0
        ) {
            size = UnityEngine.Screen.cutouts.Max( el => el.height );
        }
        
        Layout.preferredWidth = size;
        Layout.preferredHeight = size;

    }


}
