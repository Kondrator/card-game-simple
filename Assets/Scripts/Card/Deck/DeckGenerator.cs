using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DeckGenerator {


    public static CardParams[] Get( CardSuitData suitData, CardValueData valueData ) {

        CardSuitData.Item[] suits = suitData.GetAll();
        CardValueData.Item[] values = valueData.GetAll();
        CardParams[] cards = new CardParams[suits.Length * values.Length];

        int i = 0;
        for( int s = 0; s < suits.Length; s++ ) {
            for( int v = 0; v < values.Length; v++ ) {
                cards[i++] = new CardParams( suits[s].ID, values[v].ID );
            }
        }

        return cards;
    }


    public static CardParams[] Mix( CardParams[] cards, int power = 100 ) {

        for( int i = 0; i < power; i++ ) {
            int iSwap0 = Random.Range( 0, cards.Length );
            int iSwap1 = Random.Range( 0, cards.Length );

            CardParams temp = cards[iSwap0];
            cards[iSwap0] = cards[iSwap1];
            cards[iSwap1] = temp;
        }

        return cards;
    }


}
