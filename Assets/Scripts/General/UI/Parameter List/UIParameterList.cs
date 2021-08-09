using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIParameterList : MonoBehaviour {

    [SerializeField]
    private UIParameterItem item = null;



    public int Count { get { return this.transform.childCount; } }




    void Start() {
        item?.Deactivate();
    }




    public void Clear() {
        this.transform.DeactivateAllChild();
    }

    public void Set( string[] data ) {

        Clear();

        for( int i = 0; i < data.Length; i++ ) {
            Add( data[i] );
        }

    }




    public void Add( string data ) {
        UIParameterItem itemInstance = PoolGameObject.Get<UIParameterItem>( item.gameObject, this.transform );
        itemInstance.transform.ResetTransform();
        itemInstance.Set( data );
    }

    public void Add( string[] data ) {
        for( int i = 0; i < data.Length; i++ ) {
            Add( data[i] );
        }
    }






}