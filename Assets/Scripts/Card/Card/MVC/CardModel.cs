using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardModel : Model {

    public class NOTIFY {

        public class CHANGED {

            public class PARAMS {

                public const string NAME = "notify.card.changed.params";

            }

        }
    
    }




    private DBManagerCard dbManager;
    public DBManagerCard DBManager { get { return dbManager = Find( dbManager ); } }

    private CardSuitData suitData;
    public CardSuitData SuitData { get { return suitData = Find( suitData ); } }

    private CardValueData valueData;
    public CardValueData ValueData { get { return valueData = Find( valueData ); } }


    private AccountModel accountModel = null;
    public AccountInfo AccountInfo { get { return (accountModel = Find( accountModel )).AccountInfo; } }





    [SerializeField]
    private DBID<CardTemplateFaceItem> idTemplateFaceDefault = 0;
    public DBID<CardTemplateFaceItem> IDTemplateFaceDefault { get { return idTemplateFaceDefault; } }

    [SerializeField]
    private DBID<CardTemplateShirtItem> idTemplateShirtDefault = 0;
    public DBID<CardTemplateShirtItem> IDTemplateShirtDefault { get { return idTemplateShirtDefault; } }


    [SerializeField]
    private CardParams _params;
    public CardParams Params { get => _params ??= new CardParams( 0, 6 ); }






    public void SetParams( CardParams _params ) {
        this._params = _params;

        Notify( NOTIFY.CHANGED.PARAMS.NAME );
    }


}
