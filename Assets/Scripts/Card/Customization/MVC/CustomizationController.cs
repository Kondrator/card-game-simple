using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomizationController : Controller<CustomizationModel, CustomizationView> {

    public class NOTIFY {

        public class SHOW {

            public class TABLE_BACKGROUND {

                public const string NAME = "customization.show.table.background";

            }

            public class CARD_BACK {

                public const string NAME = "customization.show.card.back";

            }

            public class CARD_FRONT {

                public const string NAME = "customization.show.card.front";

            }

        }

    }




    protected override void PreInitiate() {}

    protected override void Initiate() {


        Add(
            NOTIFY.SHOW.TABLE_BACKGROUND.NAME,
            data => {
                View.Window.Show(
                    Model.DBManager.GetList<TableTemplateBackgroundItem>().ToArray(),
                    item => Model.SetTemplate( item as TableTemplateBackgroundItem ),
                    Model.Info.IDTableBackground
                );
            }
        );


        Add(
            NOTIFY.SHOW.CARD_BACK.NAME,
            data => {
                View.Window.Show(
                    Model.DBManager.GetList<CardTemplateShirtItem>().ToArray(),
                    item => Model.SetTemplate( item as CardTemplateShirtItem ),
                    Model.Info.IDCardBack
                );
            }
        );


        Add(
            NOTIFY.SHOW.CARD_FRONT.NAME,
            data => {
                View.Window.Show(
                    Model.DBManager.GetList<CardTemplateFaceItem>().ToArray(),
                    item => Model.SetTemplate( item as CardTemplateFaceItem ),
                    Model.Info.IDCardFront
                );
            }
        );





        Add(
            CustomizationModel.NOTIFY.CHANGED.TABLE_BACKGROUND.NAME,
            data => {
                Notify( AccountController.NOTIFY_ACCOUNT_SAVE_DATA );

                View.Window.SetSelect( Model.Info.IDTableBackground );
            }
        );

        Add(
            CustomizationModel.NOTIFY.CHANGED.CARD_BACK.NAME,
            data => {
                Notify( AccountController.NOTIFY_ACCOUNT_SAVE_DATA );

                View.Window.SetSelect( Model.Info.IDCardBack );
            }
        );

        Add(
            CustomizationModel.NOTIFY.CHANGED.CARD_FRONT.NAME,
            data => {
                Notify( AccountController.NOTIFY_ACCOUNT_SAVE_DATA );

                View.Window.SetSelect( Model.Info.IDCardFront );
            }
        );

    }


}
