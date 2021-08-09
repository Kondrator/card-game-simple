using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[DBManager.EditorInfo( "Card template face", "Шаблон лицевой стороны карты", "card.template.face" )]
public class CardTemplateFaceItem : DBItem<DBManagerCard> {


    [SerializeField]
    private Image[] imageSuit;
    public Image[] ImageSuit { get { return imageSuit ??= new Image[0]; } }

    [SerializeField]
    private Text[] textValue;
    public Text[] TextValue { get { return textValue ??= new Text[0]; } }




    public void SetSuit( Sprite sprite ) {
        ImageSuit.SetSprite( sprite );
    }

    public void SetValue( string text ) {
        TextValue.SetText( text );
    }


    public void SetColor( Color color ) {
        SetColor( ImageSuit, color );
        SetColor( TextValue, color );
    }



    private void SetColor( Image[] images, Color color ) {
        for( int i = 0; i < images.Length; i++ ) {
            images[i].color = color;
        }
    }

    private void SetColor( Text[] texts, Color color ) {
        for( int i = 0; i < texts.Length; i++ ) {
            texts[i].color = color;
        }
    }

}
