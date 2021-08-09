using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RuleSimpleModel : Model {

    public class NOTIFY {

        public class SUIT {

            public class SETED {

                public const string NAME = "rule.simple.suit.seted";

            }

            public class CLEARED {

                public const string NAME = "rule.simple.suit.cleared";

            }

            /// <summary>
            /// Type = CardSuitData.Item
            /// </summary>
            public const string PARAM_SUIT = "suit";
        
        }



        public class POINTS {

            public class ADD {

                public const string NAME = "rule.simple.points.add";

            }

            public class CLEAR {

                public const string NAME = "rule.simple.points.clear";

            }


            /// <summary>
            /// Type = int
            /// </summary>
            public const string PARAM_POINTS = "points";

        }

    }



    [SerializeField]
    [Range( 1, 36 )]
    private int countCards = 13;
    public int CountCards { get { return countCards; } }




    private CardSuitData suitData = null;
    public CardSuitData SuitData { get { return suitData = Find( suitData ); } }


    private CardSuitData.Item suitCurrent = null;
    public CardSuitData.Item SuitCurrent { get { return suitCurrent; } }




    private int points = 0;
    public int Points { get { return points; } }





    public void SetSuitRandom( params int[] suitIgnore ) {

        CardSuitData.Item[] suits = SuitData.GetAll();
        CardSuitData.Item suit = null;

        do {
            suit = suits.RandomItem();

        } while(    suits.Length > suitIgnore.Length
                    && System.Array.Exists( suitIgnore, el => el == suit.ID ) == true
        );

        SetSuit( suit );
    }

    public void SetSuit( CardSuitData.Item suit ) {
        suitCurrent = suit;
        Notify(
            NOTIFY.SUIT.SETED.NAME,
            new NotifyData.Param( NOTIFY.SUIT.PARAM_SUIT, suit )
        );
    }


    public void ClearSuit() {
        suitCurrent = null;
        Notify( NOTIFY.SUIT.CLEARED.NAME );
    }





    public void AddPoints( int points ) {
        this.points += points;
        Notify(
            NOTIFY.POINTS.ADD.NAME,
            new NotifyData.Param( NOTIFY.POINTS.PARAM_POINTS, this.points )
        );
    }


    public void ClearPoints() {
        points = 0;
        Notify(
            NOTIFY.POINTS.CLEAR.NAME,
            new NotifyData.Param( NOTIFY.POINTS.PARAM_POINTS, points )
        );
    }


}
