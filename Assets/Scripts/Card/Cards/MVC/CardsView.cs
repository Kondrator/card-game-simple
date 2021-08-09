using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardsView : View {


    [SerializeField]
    private CardsPositioning positioning = null;
    public CardsPositioning Positioning { get { return positioning ??= GetComponentInChildren<CardsPositioning>(); } }


}
