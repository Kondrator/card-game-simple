using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameObjectInstantiateList : MonoBehaviour {


    [SerializeField]
    private GameObject target = null;



    public void Create( int count ){

        Clear();

        for( int i = 0; i < count; i++ ) {
            GameObject targetInstance = PoolGameObject.Get( target, this.transform );
            targetInstance.transform.ResetTransform();
        }
        
    }



    public void Clear() {
        this.transform.DeactivateAllChild();
    }


}
