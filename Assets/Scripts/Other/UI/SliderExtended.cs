using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


[DisallowMultipleComponent]
[RequireComponent( typeof( Slider ) )]
public class SliderExtended : MonoBehaviour {

    [SerializeField]
    private Slider slider = null;


    [Header( "Animation" )]

    [SerializeField, Tooltip( "Duration of animation set new value (in seconds)" )]
    private float duration = 1f;

    [SerializeField, Tooltip( "Curve of animation set new value" )]
    private AnimationCurve curve = AnimationCurveExtensions.GetCurveSimple();




    // current progerss
    private float progress = -1;
    public float Progress { get { return progress; } }

    // for animation progress
    private TimerExecutor.Item timerProgress = null;




    /// <summary>
    /// Set progress bar (fast).
    /// </summary>
    /// <param name="progress">Progress [0; 1f]</param>
    public void Set( float progress ) {

        this.progress = progress;

        if( slider != null ) {
            slider.value = (slider.maxValue - slider.minValue) * progress + slider.minValue;
        }

    }


    /// <summary>
    /// Set progress bar (animation)
    /// </summary>
    /// <param name="progress">Progress [0; 1f]</param>
    public void SetAnimation( float progress ) {

        if( timerProgress != null ) {
            timerProgress.ForceComplete( false );
            timerProgress = null;
        }

        timerProgress = TimerExecutor.Add(
            duration,
            ( float time ) => {
                Set( curve.Evaluate( time, this.progress, progress ) );
            },
            () => {
                timerProgress = null;
            }
        );

    }


    /// <summary>
    /// Set progress bar (animation)
    /// </summary>
    /// <param name="progress">Progress [0; 1f]</param>
    /// <param name="duration">Custom duration animation (in seconds)</param>
    public void SetAnimation( float progress, float duration ) {

        if( timerProgress != null ) {
            timerProgress.ForceComplete( false );
            timerProgress = null;
        }

        timerProgress = TimerExecutor.Add(
            duration,
            ( float time ) => {
                Set( curve.Evaluate( time, this.progress, progress ) );
            },
            () => {
                timerProgress = null;
            }
        );

    }






#if UNITY_EDITOR
    [CustomEditor( typeof( SliderExtended ) )]
    private class SliderExtendedEditor : Editor<SliderExtended> {

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
