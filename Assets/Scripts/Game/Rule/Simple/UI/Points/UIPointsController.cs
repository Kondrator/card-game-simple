using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPointsController : Controller<UIPointsView> {



    protected override void PreInitiate() {
        View.Text.SetText( "0" );
    }


    protected override void Initiate() {

        Add(
            new string[] {
                RuleSimpleModel.NOTIFY.POINTS.ADD.NAME,
                RuleSimpleModel.NOTIFY.POINTS.CLEAR.NAME
            },
            data => {
                int points = data.GetParam<int>( RuleSimpleModel.NOTIFY.POINTS.PARAM_POINTS );
                View.Text.SetText( points.ToString() );
            }
        );

    }


}
