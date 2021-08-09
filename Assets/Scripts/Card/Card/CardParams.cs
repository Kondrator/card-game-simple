using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CardParams {


    [SerializeField]
    [CardSuitData.Popup]
    private int suit = 0;
    public int Suit { get => suit; }


    [SerializeField]
    [CardValueData.Popup]
    private int value = 0;
    public int Value { get => value; }



    public CardParams( int suit, int value ) {
        this.suit = suit;
        this.value = value;
    }




    public override bool Equals( object obj ) {

        if( obj is CardParams == false ) {
            return false;
        }

        CardParams target = (CardParams)obj;

        if( target.Suit != Suit ) {
            return false;
        }

        if( target.Value != Value ) {
            return false;
        }

        return true;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

}
