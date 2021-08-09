using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardsList : Element {


    //[SerializeField]
    private Transform container = null;
    private Transform Container { get { return container ??= this.transform; } }




    private CardInstancer instancer = null;
    private CardInstancer Instancer { get { return instancer = Find( instancer ); } }

    private List<CardController> instances = null;
    private List<CardController> Instances { get { return instances ??= new List<CardController>(); } }




    public int Count { get { return Instances.Count; } }

    public CardController this[int index] { get { return Instances[index]; } }




    public void Set( CardParams[] cards ) {

        Clear();

        for( int i = 0; i < cards.Length; i++ ) {
            Instantiate( cards[i] );
        }

    }

    public void Clear() {
        for( int i = 0; i < Instances.Count; i++ ) {
            Instances[i].Deactivate();
        }
        Instances.Clear();
    }



    public void Add( CardParams card ) {
        Instantiate( card );
    }

    public void Remove( CardParams card ) {
        for( int i = 0; i < Instances.Count; i++ ) {
            if( Instances[i].Model.Params.Equals( card ) ) {
                Instances[i].Deactivate();
                Instances.RemoveAt( i-- );
            }
        }
    }



    public void Add( CardController card ) {
        Instances.Add( card );
        card.transform.SetParent( Container );
    }

    public CardController Pop( CardParams card ) {
        for( int i = 0; i < Instances.Count; i++ ) {
            if( Instances[i].Model.Params.Equals( card ) ) {
                CardController result = Instances[i];
                result.View.SetDark( false );
                Instances.RemoveAt( i );
                return result;
            }
        }
        return null;
    }

    public CardController Pop() {
        if( Count == 0 ) {
            return null;
        }

        return Pop( this[Count - 1].Model.Params );
    }





    public T[] Map<T>( System.Func<CardController, T> selector ) {

        T[] result = new T[Count];

        for( int i = 0; i < Count; i++ ) {
            result[i] = selector( this[i] );
        }

        return result;
    }




    public void Sort() {

        Instances.Sort( new ComparerBySuit() );

    }





    private void Instantiate( CardParams card ) {

        CardController instance = Instancer.CreateCard( card, Container );

        if( instance != null ) {
            Instances.Add( instance );
        }

    }







    public class ComparerBySuit : IComparer<CardController> {

        public int Compare( CardController x, CardController y ) {

            if( x.Model.Params.Suit != y.Model.Params.Suit ) {
                return x.Model.Params.Suit.CompareTo( y.Model.Params.Suit );
            }

            return y.Model.Params.Value.CompareTo( x.Model.Params.Value );
        }

    }

}
