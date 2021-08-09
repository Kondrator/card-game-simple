using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent( typeof( Canvas ) )]
[RequireComponent( typeof( GraphicRaycaster ) )]
public class UICanvasMain : Element {


    public static UICanvasMain Instance { get { return instance; } }
    private static UICanvasMain instance = null;


    public Canvas Canvas { get { return canvas = canvas ?? GetComponent<Canvas>(); } }
    private Canvas canvas = null;

    public GraphicRaycaster Raycaster { get { return raycaster = raycaster ?? GetComponent<GraphicRaycaster>(); } }
    private GraphicRaycaster raycaster = null;



    private void Awake() {
        instance = this;
    }


}
