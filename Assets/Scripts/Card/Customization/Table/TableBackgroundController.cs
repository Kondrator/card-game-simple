using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TableBackgroundController : Controller<TableBackgroundModel, TableBackgroundView> {



    protected override void PreInitiate() {}

    protected override void Initiate() {

        Add(
            SceneController.NOTIFY_LOADED,
            data => {
                UpdateTemplate();
            }
        );

        Add(
            CustomizationModel.NOTIFY.CHANGED.TABLE_BACKGROUND.NAME,
            data => {
                UpdateTemplate();
            }
        );

    }



    private void UpdateTemplate() {

        TableTemplateBackgroundItem template = Model.DBManager.Get<TableTemplateBackgroundItem>( Model.Info.IDTableBackground );
        if( template == null ) {
            template = Model.DBManager.Get<TableTemplateBackgroundItem>( 0 );
        }

        View.Set( template );

    }

}
