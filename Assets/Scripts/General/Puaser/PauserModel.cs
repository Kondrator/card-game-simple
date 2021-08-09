using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UIWindowManager;
using UnityEngine;



public class PauserModel : Model {




    private List<MonoBehaviour> initors = null;
    private List<MonoBehaviour> Initors { get { return initors = initors ?? new List<MonoBehaviour>(); } }



    public bool IsPause {
        get {
            if( UIWindow.HasOpened == true ) {
                return true;
            }

            Fix();
            return Initors.Count > 0;
        }
    }




    /// <summary>
    /// Remove trash initors
    /// </summary>
    private void Fix() {
        for( int i = 0; i < Initors.Count; i++ ) {

            if( Initors[i] == null ) {
                Initors.RemoveAt( i-- );
                continue;
            }

            if( Initors[i].isActiveAndEnabled == false ) {
                Initors.RemoveAt( i-- );
                continue;
            }

        }
    }


    public void Add( MonoBehaviour initor ) {
        Initors.Add( initor );
    }


    public void Remove( MonoBehaviour initor ) {
        Initors.RemoveAll( el => el == initor );
    }


    public void Clear() {
        Initors.Clear();
    }

}
