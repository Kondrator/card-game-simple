using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIParameterItem : MonoBehaviour {

    [SerializeField]
    private Text textDescription = null;


    public void Set( string data ) {
        textDescription.SetText( data );
    }

}