using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TransformScaler : MonoBehaviour, IMyManagerMonoBehaviour {


    [Header( "Bounds" )]

    [SerializeField]
    private float min = 0.1f;

    [SerializeField]
    private float max = 2f;


    [Header( "Power" )]

    [SerializeField]
    private float power = 1f;




    private void OnEnable() {
        MyManagerMonoBehaviour.Add( this );
    }

    private void OnDisable() {
        MyManagerMonoBehaviour.Remove( this );
    }



    public void UpdateMe( float timeDelta, MyManagerMonoBehaviourType type ) {

#if UNITY_ANDROID || UNITY_IOS
        float scroll = Input.GetAxis( "Mouse ScrollWheel" ) * power;
        float result = Mathf.Clamp( transform.localScale.x + scroll, min, max );
        transform.localScale = new Vector3( result, result, result );

#else
        
#endif

    }


}
