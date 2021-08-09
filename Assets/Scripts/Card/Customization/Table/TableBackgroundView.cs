using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TableBackgroundView : View {


    [SerializeField]
    private Transform container;


    private TableTemplateBackgroundItem instance = null;


    public void Set( TableTemplateBackgroundItem instance ) {

        if( this.instance != null ) {
            this.instance.Deactivate();
        }

        if( instance != null ) {
            this.instance = PoolGameObject.Get<TableTemplateBackgroundItem>( instance.gameObject, container );
            this.instance.transform.ResetTransform();
        }
    
    }

}
