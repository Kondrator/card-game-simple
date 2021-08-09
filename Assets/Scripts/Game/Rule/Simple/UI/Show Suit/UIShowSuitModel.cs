using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIShowSuitModel : Model {


    private CardSuitData data = null;
    public CardSuitData Data { get { return data = Find( data ); } }


}
