using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioFX : MonoBehaviour {

    [SerializeField]
    private AudioClip clip = null;





    private void OnEnable() {
        AudioManager.PlayEffect( clip );
    }


}
