using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtension {


    public static void SetTriggerCheck( this Animator animator, string name ) {
        if( animator != null ) {
            animator.SetTrigger( name );
        }
    }


    public static void SetBoolCheck( this Animator animator, string name, bool value ) {
        if( animator != null ) {
            animator.SetBool( name, value );
        }
    }


    public static void SetFloatCheck( this Animator animator, string name, float value ) {
        if( animator != null ) {
            animator.SetFloat( name, value );
        }
    }


}
