using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeckController : CardsController<DeckModel, DeckView> {





    protected override void PreInitiate() {
        base.PreInitiate();
    }

    protected override void Initiate() {
        base.Initiate();

        Add(
            new string[] {
                CardsModel.NOTIFY.CHANGED.CARDS.NAME,
                CardsModel.NOTIFY.CHANGED.CARD.ADD.NAME
            },
            data => {
                for( int i = 0; i < Model.List.Count; i++ ) {
                    Model.List[i].View.SetVisible( false );
                }
            },
            data => data.Initiator.transform == this.transform
        );


    }


}
