using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIShowSuitController : Controller<UIShowSuitModel, UIShowSuitView> {



    protected override void PreInitiate() {}

    protected override void Initiate() {

        Add(
            RuleSimpleModel.NOTIFY.SUIT.SETED.NAME,
            data => {
                CardSuitData.Item suit = data.GetParam<CardSuitData.Item>( RuleSimpleModel.NOTIFY.SUIT.PARAM_SUIT );
                View.Image.SetSprite( suit.Icon );
                View.Image.color = suit.Color;
                View.Window.Open();
            }
        );

        Add(
            RuleSimpleModel.NOTIFY.SUIT.CLEARED.NAME,
            data => {
                View.Window.Close();
            }
        );

    }


}
