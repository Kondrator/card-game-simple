using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : Controller<CardModel, CardView> {

    public class NOTIFY {

        public class DOWN {

            public const string NAME = "card.controller.down";

        }

    }




    protected override void PreInitiate() {
        UpdateTemplateFace();
        UpdateTemplateShirt();

        View.ColliderEvent.OnDown.RemoveAllListeners();
        View.ColliderEvent.OnDown.AddListener( () => {
            Notify( NOTIFY.DOWN.NAME );
        } );
    }

    protected override void Initiate() {


        Add(
            CardModel.NOTIFY.CHANGED.PARAMS.NAME,
            data => {
                UpdateParams();
            },
            data => data.Initiator.transform == this.transform
        );



        Add(
            CustomizationModel.NOTIFY.CHANGED.CARD_BACK.NAME,
            data => {
                UpdateTemplateShirt();
            }
        );



        Add(
            CustomizationModel.NOTIFY.CHANGED.CARD_FRONT.NAME,
            data => {
                UpdateTemplateFace();
            }
        );


    }


    private void Update() {
        if( Input.GetKeyDown( KeyCode.Alpha1 ) ) {
            View.SetVisible( true );
        }
        if( Input.GetKeyDown( KeyCode.Alpha2 ) ) {
            View.SetVisible( false );
        }
    }




    private void UpdateParams() {

        this.name = string.Format(  "{0} {1}",
                                    Model.SuitData.GetByID( Model.Params.Suit )?.Symbol,
                                    Model.ValueData.GetByID( Model.Params.Value )?.Symbol
                                 );

        // set text
        View.TemplateFace?.SetSuit( Model.SuitData.GetByID( Model.Params.Suit )?.Icon );
        View.TemplateFace?.SetValue( Model.ValueData.GetByID( Model.Params.Value )?.Symbol );

        // set color
        View.TemplateFace?.SetColor( Model.SuitData.GetByID( Model.Params.Suit ).Color );

    }


    private void UpdateTemplateShirt() {

        // set template of card
        CardTemplateShirtItem template = Model.DBManager.Get<CardTemplateShirtItem>( Model.AccountInfo.Customization.IDCardBack );
        if( template == null ) {
            template = Model.DBManager.Get<CardTemplateShirtItem>( Model.IDTemplateShirtDefault );
        }
        View.SetTemplate( template );

    }


    private void UpdateTemplateFace() {

        // set template of card
        CardTemplateFaceItem template = Model.DBManager.Get<CardTemplateFaceItem>( Model.AccountInfo.Customization.IDCardFront );
        if( template == null ) {
            template = Model.DBManager.Get<CardTemplateFaceItem>( Model.IDTemplateFaceDefault );
        }
        View.SetTemplate( template );

        UpdateParams();

    }


}
