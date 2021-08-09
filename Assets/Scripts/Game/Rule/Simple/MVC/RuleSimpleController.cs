using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class RuleSimpleController : Controller<RuleSimpleModel, RuleSimpleView> {

    public class NOTIFY {

        public class DECK {

            public class CREATE {

                public const string NAME = "rule.simple.deck.create";

            }


            public class DROP {

                public const string NAME = "rule.simple.deck.drop";

            }

        }


        public class GAME {

            public class SUIT {

                public class SHOW {

                    public const string NAME = "rule.simple.game.suit.show";

                }

            }


            public class TABLE {

                public class CHECK {

                    public const string NAME = "rule.simple.game.table.check";

                }

            }


            public class END {

                public class CHECK {

                    public const string NAME = "rule.simple.game.end.check";

                }

                public class WIN {

                    public const string NAME = "rule.simple.game.end.win";

                }

            }



            public class RESTART {

                public const string NAME = "rule.simple.game.restart";

            }

        }

    }




    protected override void PreInitiate() {}

    protected override void Initiate() {


        Add(
            SceneController.NOTIFY_LOADED,
            data => {
                Notify( 1f, NOTIFY.GAME.RESTART.NAME );
            }
        );


        // deck

        Add(
            NOTIFY.DECK.CREATE.NAME,
            data => {
                CardParams[] cards = DeckGenerator.Get( Find<CardSuitData>(), Find<CardValueData>() );
                cards = DeckGenerator.Mix( cards );

                System.Action completed = () => Notify( NOTIFY.DECK.DROP.NAME );
                StartCoroutine( CoroutineCreateDeck( cards.Take( Model.CountCards ).ToArray(), completed ) );
            }
        );


        Add(
            NOTIFY.DECK.DROP.NAME,
            data => {
                System.Action completed = () => Notify( NOTIFY.GAME.SUIT.SHOW.NAME );
                StartCoroutine( CoroutineDropCardsTo( View.Deck.Model.List.Count, View.Player, completed ) );
            }
        );



        // suit

        Add(
            NOTIFY.GAME.SUIT.SHOW.NAME,
            data => {
                int[] suitsAll = Model.SuitData.GetAll().Select( el => el.ID ).ToArray();
                int[] suitsHave = View.Player.Model.List.Map( el => el.Model.Params.Suit );
                int[] suitsIgnore = suitsAll.Where( el => suitsHave.Contains( el ) == false ).ToArray();

                Model.SetSuitRandom( suitsIgnore );
            }
        );


        Add(
            RuleSimpleModel.NOTIFY.SUIT.SETED.NAME,
            data => {
                CardSuitData.Item suit = data.GetParam<CardSuitData.Item>( RuleSimpleModel.NOTIFY.SUIT.PARAM_SUIT );
                View.Player.View.Positioning?.SetSelect( View.Player.Model.List .Map( el => el.Model.Params )
                                                                                .Where( el => el.Suit == suit.ID )
                                                                                .ToArray()
                                                        );
            }
        );

        Add(
            RuleSimpleModel.NOTIFY.SUIT.CLEARED.NAME,
            data => {
                View.Player.View.Positioning?.SetSelect( null );
            }
        );



        // card

        Add(
            CardController.NOTIFY.DOWN.NAME,
            data => {
                if( Model.SuitCurrent == null ) {
                    return;
                }

                CardController card = data.Initiator as CardController;
                if( card.Model.Params.Suit == Model.SuitCurrent.ID ) {
                    PlayerController player = MyOperation.GetComponentInParent<PlayerController>( card.gameObject );
                    if( player != null ) {
                        card = player.Model.List.Pop( card.Model.Params );
                        View.Table.Model.Add( card );
                    }

                    Model.ClearSuit();
                }
            }
        );



        Add(
            CardsModel.NOTIFY.CHANGED.CARD.ADD.NAME,
            data => {
                Notify( 1f, NOTIFY.GAME.TABLE.CHECK.NAME );
            },
            data => data.Initiator is TableModel
        );




        // end game

        Add(
            NOTIFY.GAME.TABLE.CHECK.NAME,
            data => {
                int count = View.Table.Model.List.Count;
                View.Table.Model.Clear();

                Model.AddPoints( count * 1 );

                Notify( 1f, NOTIFY.GAME.END.CHECK.NAME );
            }
        );


        Add(
            NOTIFY.GAME.END.CHECK.NAME,
            data => {
                if( View.Player.Model.List.Count == 0 ) {
                    Notify( NOTIFY.GAME.END.WIN.NAME );

                } else {
                    Notify( NOTIFY.GAME.SUIT.SHOW.NAME );
                }
            }
        );


        Add(
            NOTIFY.GAME.END.WIN.NAME,
            data => {
                Notify( 2f, NOTIFY.GAME.RESTART.NAME );
            }
        );


        Add(
            NOTIFY.GAME.RESTART.NAME,
            data => {
                View.Deck.Model.Clear();
                View.Table.Model.Clear();
                View.Player.Model.Clear();

                Model.ClearSuit();
                Model.ClearPoints();

                Notify( NOTIFY.DECK.CREATE.NAME );
            }
        );


    }


    private IEnumerator CoroutineCreateDeck( CardParams[] cards, System.Action completed ) {

        View.Deck.Model.Clear();

        yield return null;

        for( int i = 0; i < cards.Length; i++ ) {
            View.Deck.Model.Add( cards[i] );
            yield return new WaitForSeconds( 0.1f );
        }

        yield return new WaitForSeconds( 2f );

        completed?.Invoke();

    }



    private IEnumerator CoroutineDropCardsTo( int count, PlayerController target, System.Action completed ) {
        
        yield return null;

        for( int i = 0; i < count; i++ ) {
            CardController card = View.Deck.Model.List.Pop();
            if( card != null ) {
                target.Model.Add( card );
            }
            yield return new WaitForSeconds( 0.1f );
        }

        yield return new WaitForSeconds( 2f );

        completed?.Invoke();

    }


}
