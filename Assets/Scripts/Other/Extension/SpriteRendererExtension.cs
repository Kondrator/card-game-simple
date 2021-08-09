using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class SpriteRendererExtension {


    public static void SetSprite( this SpriteRenderer target, Sprite sprite ) {
        if( target != null ) {
            target.sprite = sprite;
        }
    }

    public static void SetSprite( this SpriteRenderer target, ResourceSprite sprite ) {
        if( target != null ) {
            sprite.Get( target.SetSprite );
        }
    }


}
