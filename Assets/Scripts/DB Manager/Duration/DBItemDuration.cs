using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[DisallowMultipleComponent]
[RequireComponent( typeof( DBItem ) )]
public class DBItemDuration : MonoBehaviour {


    [SerializeField]
    private Duration duration = 0;
    public Duration Duration { get { return duration; } }




    public static DBItemDuration GetComponent( DBItem target ) {
        return target.GetComponent<DBItemDuration>();
    }

    public static bool IsComplete( DBItem target, Duration elapsed ) {

        DBItemDuration component = GetComponent( target );

        if( component == null ) {
            return true;
        }

        return component.Duration.Milliseconds <= elapsed.Milliseconds;
    }

}
