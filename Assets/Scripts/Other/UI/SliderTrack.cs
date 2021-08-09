using System.Collections;
using System.Collections.Generic;
using UIWindowManager;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


[DisallowMultipleComponent]
[RequireComponent( typeof( Slider ) )]
public class SliderTrack : MonoBehaviour {


    [SerializeField]
    private Slider slider = null;


    [Header( "Animation" )]

    [SerializeField]
    private UIAnimation uiAnimation = null;

    [SerializeField]
    private float duration = 1f;




    private void Awake() {
        Hide();
    }



    /// <summary>
    /// Show track (animation)
    /// </summary>
    public void Show ( float progress ) {
        if( slider == null ||
            uiAnimation == null
        ) {
            return;
        }

        uiAnimation.AnimationSetHide.Duration = duration;

        if( uiAnimation.IsVisible == true ) {
            uiAnimation.Hide();

        } else {
            slider.value = progress;
            uiAnimation.Hide();
        }
    }


    /// <summary>
    /// Hide track (fast)
    /// </summary>
    public void Hide() {
        if( slider != null ) {
            slider.value = 0;
        }

        if( uiAnimation != null ) {
            uiAnimation.Hide( false, true );
        }
    }





#if UNITY_EDITOR
    [CustomEditor( typeof( SliderTrack ) )]
    private class SliderTrackEditor : Editor<SliderTrack> {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            component.slider = component.GetComponent<Slider>();

            if( GUI.changed ) {
                EditorUtility.SetDirty( component );
            }

        }

    }
#endif


}
