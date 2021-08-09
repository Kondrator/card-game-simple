using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioCardController : Controller<AudioCardView> {


    protected override void PreInitiate() { }

    protected override void Initiate() {

        Add(
            new string[] {
                CardsModel.NOTIFY.CHANGED.CARD.ADD.NAME,
                CardsModel.NOTIFY.CHANGED.CARD.REMOVE.NAME
            },
            data => {
                View.Card.GetRandom( clip => AudioManager.PlayEffect( clip ) );
            }
        );

    }


}
