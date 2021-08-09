using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TableBackgroundModel : Model {

    private DBManagerCard dbManager = null;
    public DBManagerCard DBManager { get { return dbManager = Find( dbManager ); } }

    private AccountModel accountModel = null;
    public CustomizationInfo Info { get { return (accountModel = Find( accountModel )).AccountInfo.Customization; } }

}
