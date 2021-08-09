using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomizationModel : Model {

    public class NOTIFY {

        public class CHANGED {

            public class TABLE_BACKGROUND {

                public const string NAME = "customization.changed.table.background";

            }

            public class CARD_BACK {

                public const string NAME = "customization.changed.card.back";

            }

            public class CARD_FRONT {

                public const string NAME = "customization.changed.card.front";

            }

        }

    }





    private DBManagerCard dbManager = null;
    public DBManagerCard DBManager { get { return dbManager = Find( dbManager ); } }

    private AccountModel accountModel = null;
    public CustomizationInfo Info { get { return (accountModel = Find( accountModel )).AccountInfo.Customization; } }






    public void SetTemplate( TableTemplateBackgroundItem item ) {
        Info.IDTableBackground = item.ID;
        Notify( NOTIFY.CHANGED.TABLE_BACKGROUND.NAME );
    }

    public void SetTemplate( CardTemplateShirtItem item ) {
        Info.IDCardBack = item.ID;
        Notify( NOTIFY.CHANGED.CARD_BACK.NAME );
    }

    public void SetTemplate( CardTemplateFaceItem item ) {
        Info.IDCardFront = item.ID;
        Notify( NOTIFY.CHANGED.CARD_FRONT.NAME );
    }


}
