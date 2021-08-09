using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardView : View {

    [Header( "Containers" )]

    [SerializeField]
    private Transform container = null;

    [SerializeField]
    private Transform containerFace = null;

    [SerializeField]
    private Transform containerShirt = null;

    [SerializeField]
    private Transform containerDark = null;


    [Header( "Colliders" )]

    [SerializeField]
    private ColliderEvent colliderEvent = null;
    public ColliderEvent ColliderEvent { get { return colliderEvent; } }




    private CardTemplateFaceItem templateFace;
    public CardTemplateFaceItem TemplateFace { get { return templateFace; } set { templateFace = value; } }

    private CardTemplateShirtItem templateShirt;
    public CardTemplateShirtItem TemplateShirt { get { return templateShirt; } set { templateShirt = value; } }





    protected override void OnEnable() {
        base.OnEnable();

        SetVisible( false );
        SetDark( false );
    }



    /// <summary>
    /// Instantiate face
    /// </summary>
    public void SetTemplate( CardTemplateFaceItem template ) {

        if( this.templateFace != null ) {
            this.templateFace.Deactivate();
            this.templateFace = null;
        }

        if( template != null ) {
            this.templateFace = PoolGameObject.Get<CardTemplateFaceItem>( template.gameObject, containerFace ?? this.transform );
            this.templateFace.transform.ResetTransform();
        }

    }



    /// <summary>
    /// Instantiate shirt
    /// </summary>
    public void SetTemplate( CardTemplateShirtItem template ) {

        if( this.templateShirt != null ) {
            this.templateShirt.Deactivate();
            this.templateShirt = null;
        }

        if( template != null ) {
            this.templateShirt = PoolGameObject.Get<CardTemplateShirtItem>( template.gameObject, containerShirt ?? this.transform );
            this.templateShirt.transform.ResetTransform();
        }

    }




    /// <summary>
    /// Set visible face
    /// </summary>
    public void SetVisible( bool isVisible ) {
        container.transform.localEulerAngles = new Vector3( isVisible ? 0 : 180, 0, 0 );
    }

    /// <summary>
    /// Set dark card
    /// </summary>
    public void SetDark( bool isDark ) {
        containerDark?.SetActive( isDark );
    }

}
