using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardInstancer : Element {

    [SerializeField]
    private CardController card = null;




    public CardController CreateCard( CardParams card, Transform parent = null ) {

        if( this.card == null ) {
            return null;
        }

        CardController instance = PoolGameObject.Get<CardController>( this.card.gameObject, parent );
        instance.transform.ResetTransform();
        instance.Model.SetParams( card );

        return instance;
    }


}
