using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent( typeof(CardsList) )]
public class CardsPositioning : MonoBehaviour, IMyManagerMonoBehaviour {



    [SerializeField]
    [Range( 0, 10f )]
    private float width = 1;

    [SerializeField]
    [Range( 0, 2f )]
    private float maxSizeCard = 1f;


    [SerializeField]
    [Range( 0, 50f )]
    private float speed = 1f;

    [SerializeField]
    [Range( 1f, 3f )]
    private float coefficientSelect = 2f;

    [SerializeField]
    [Range( 0.1f, 1f )]
    private float zIndex = 0.15f;



    private CardsList list = null;
    private CardsList List { get { return list ??= GetComponent<CardsList>(); } }





    private CardParams[] select = null;



    public void SetSelect( CardParams[] select ) {
        this.select = select;
    }






    private void OnEnable() {
        MyManagerMonoBehaviour.Add( this );
    }

    private void OnDisable() {
        MyManagerMonoBehaviour.Remove( this );
    }



    public void UpdateMe( float timeDelta, MyManagerMonoBehaviourType type ) {

        float width = this.width;
        if( width > List.Count * (maxSizeCard / 2f) ) {
            width = List.Count * (maxSizeCard / 2f);
        }
        

        float size = width / List.Count;

        float sizeSelect = size;
        float sizeUnselect = size;
        if( select != null ) {
            sizeSelect = size * coefficientSelect;
            if( List.Count == select.Length ) {
                sizeSelect = maxSizeCard;
                width = maxSizeCard * select.Length;
            }

            if( sizeSelect * select.Length > width * 0.8f ) {
                sizeSelect = width * 0.8f / select.Length;
            }
            sizeSelect = Mathf.Clamp( sizeSelect, 0, maxSizeCard );

            sizeUnselect = (width - sizeSelect * select.Length) / (List.Count - select.Length);
            if( List.Count == select.Length ) {
                sizeUnselect = 0;
            }

            width = sizeSelect * select.Length + sizeUnselect * (List.Count - select.Length);
        }

        float offset = (width / 2f - size / 2f);


        Vector3 position = new Vector3();

        for( int i = 0; i < List.Count; i++ ) {
            bool isSelect = select == null ? false : select.Any( el => el.Equals( List[i].Model.Params ) );
            bool isSelectPrev = select == null ? false : i > 0 && select.Any( el => el.Equals( List[i - 1].Model.Params ) );

            float sizeCurrent = size;
            if( select != null ) {
                sizeCurrent = isSelectPrev ? sizeSelect : sizeUnselect;
            }

            if( i == 0 ) {
                position.x = size * i - offset;
                position.z = 0;

            } else {
                position.x += sizeCurrent;
                position.z -= zIndex;
            }

            List[i].transform.localPosition = Vector3.Lerp( List[i].transform.localPosition, position, timeDelta * speed );
            List[i].View.SetDark( select != null && isSelect == false );
        }

    }

}
