using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScreenHelper : Element, IMyManagerMonoBehaviour {

    public const string NOTIFY_CHANGED = "screen.data.changed";


    public class Size {

        private float width = 0;
        public float Width { get { return width; } }

        private float height = 0;
        public float Height { get { return height; } }


        public Size( float width, float height ){
            this.width = width;
            this.height = height;
        }


        public override string ToString() {
            return string.Format( "{0} x {1}", Width, Height );
        }

    }

    public enum Type {
        Vertical,
        Horizontal,
    }




    /// <summary>
    /// Size of screen
    /// </summary>
    public Size Pixel {
        get { return pixel; }
        private set { pixel = value; }
    }
    private Size pixel = new Size( 0, 0 );

    /// <summary>
    /// Type of screen
    /// </summary>
    public Type Orientation {
        get { return orientation; }
        private set { orientation = value; }
    }
    private Type orientation = Type.Vertical;


    /// <summary>
    /// Physical size camera
    /// </summary>
    public Size Physical {
        get { return physical; }
        private set { physical = value; }
    }
    private Size physical = new Size( 0, 0 );

    private float orthographicSize;




    void Awake() {
        MyManagerMonoBehaviour.Add( this );
        UpdateSize();
    }


    public void UpdateMe( float timeDelta, MyManagerMonoBehaviourType type ) {

        if( CameraSingleton.IsExist == false ) {
            return;
        }

        // change screen size ?
        if( Pixel.Width != Screen.width
            || Pixel.Height != Screen.height
        ) {
            UpdateSize();
            Notify( NOTIFY_CHANGED );
        }

        // change orthographic size ?
        if( CameraSingleton.Camera.orthographicSize != orthographicSize ) {
            UpdateSize();
            Notify( NOTIFY_CHANGED );
        }

    }




    /// <summary>
    /// Update current sizes
    /// </summary>
    private void UpdateSize() {

        if( CameraSingleton.IsExist == false ) {
            return;
        }

        // update size screen
        Pixel = new Size( Screen.width, Screen.height );

        // update type screen
        Orientation = Pixel.Width < Pixel.Height ? Type.Vertical : Type.Horizontal;

        // update physical size camera
        orthographicSize = CameraSingleton.Camera.orthographicSize;
        float size = 2 * orthographicSize;
        Physical = new Size( size * CameraSingleton.Camera.aspect, size );

    }


}
