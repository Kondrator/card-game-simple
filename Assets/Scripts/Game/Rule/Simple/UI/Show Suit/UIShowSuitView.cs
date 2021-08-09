using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UIWindowManager;
using UnityEngine;
using UnityEngine.UI;


public class UIShowSuitView : View {


    [SerializeField]
    private UIWindow window = null;
    public UIWindow Window { get { return window; } }


    [SerializeField]
    private Image image = null;
    public Image Image { get { return image; } }


}
