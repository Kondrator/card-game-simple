using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioRuleSimpleController : Controller<AudioRuleSimpleView> {


    protected override void PreInitiate() { }

    protected override void Initiate() {

        Add(
            RuleSimpleModel.NOTIFY.POINTS.ADD.NAME,
            data => {
                View.AddPoints.GetRandom( clip => AudioManager.PlayEffect( clip ) );
            }
        );

        Add(
            RuleSimpleController.NOTIFY.GAME.END.WIN.NAME,
            data => {
                View.Win.GetRandom( clip => AudioManager.PlayEffect( clip ) );
            }
        );

    }


}
