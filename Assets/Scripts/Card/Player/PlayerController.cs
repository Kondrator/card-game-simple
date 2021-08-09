using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : CardsController<PlayerModel, PlayerView> {


    protected override void PreInitiate() { }

    protected override void Initiate() {

        Add(
            new string[] {
                CardsModel.NOTIFY.CHANGED.CARDS.NAME,
                CardsModel.NOTIFY.CHANGED.CARD.ADD.NAME
            },
            data => {
                for( int i = 0; i < Model.List.Count; i++ ) {
                    Model.List[i].View.SetVisible( true );
                }
                Model.List.Sort();
            },
            data => data.Initiator.transform == this.transform
        );

    }


}
