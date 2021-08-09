using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class RectTransformExtension{
	
	public static Rect ToScreenSpace( this RectTransform target, float scaleOffset = 0 ){
		Vector2 size = Vector2.Scale( target.rect.size, target.lossyScale );
		return new Rect( (Vector2)target.position - ( size * scaleOffset ), size );
	}

	public static Rect ToScreenSpaceWithCheckLayout( this RectTransform target, float scaleOffset = 0 ){
		if( target.GetComponent<LayoutGroup>() != null
			|| target.GetComponent<ContentSizeFitter>() != null
		){
			bool isActive = target.gameObject.activeSelf;
			target.gameObject.SetActive( true );

			//target.GetComponent<ContentSizeFitter>().SetLayoutVertical();
			LayoutRebuilder.ForceRebuildLayoutImmediate( target );

			target.gameObject.SetActive( isActive );
		}

		return target.ToScreenSpace( scaleOffset );
	}


    public static void SetDefaultScale( this RectTransform trans ){
        trans.localScale = new Vector3( 1, 1, 1 );
    }

    public static void SetPivotAndAnchors( this RectTransform target, Vector2 aVec ){

		Vector2 position = target.position;

		Vector2 size = target.GetSize();
		target.anchorMin = aVec;
		target.anchorMax = aVec;

		Vector2 pivotNew = aVec;
		Vector2 pivotDiff = pivotNew - target.pivot;
		target.pivot = pivotNew;
			
		position.x += size.x * pivotDiff.x;
		position.y += size.y * pivotDiff.y;

		target.position = position;
		target.sizeDelta = size;

    }

    public static Vector2 GetSize( this RectTransform trans ){
        return trans.rect.size;
    }
    public static float GetWidth( this RectTransform trans ){
        return trans.rect.width;
    }
    public static float GetHeight( this RectTransform trans ){
        return trans.rect.height;
    }

    public static void SetPositionOfPivot( this RectTransform trans, Vector2 newPos ){
        trans.localPosition = new Vector3( newPos.x, newPos.y, trans.localPosition.z );
    }

    public static void SetLeftBottomPosition( this RectTransform trans, Vector2 newPos ){
        trans.localPosition = new Vector3( newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z );
    }
    public static void SetLeftTopPosition( this RectTransform trans, Vector2 newPos ){
        trans.localPosition = new Vector3( newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z );
    }
    public static void SetRightBottomPosition( this RectTransform trans, Vector2 newPos ){
        trans.localPosition = new Vector3( newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z );
    }
    public static void SetRightTopPosition( this RectTransform trans, Vector2 newPos ){
        trans.localPosition = new Vector3( newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z );
    }

    public static void SetSize( this RectTransform trans, Vector2 newSize ){
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2( deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y );
        trans.offsetMax = trans.offsetMax + new Vector2( deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y) );
    }
    public static void SetWidth( this RectTransform trans, float newSize ){
        SetSize( trans, new Vector2( newSize, trans.rect.size.y ) );
    }
    public static void SetHeight( this RectTransform trans, float newSize ){
        SetSize( trans, new Vector2( trans.rect.size.x, newSize ) );
    }

	/// <summary>
	/// Get Pivot in point.
	/// </summary>
	/// <param name="transform">Target.</param>
	public static Vector2 GetPivotPoint( this RectTransform transform ){

		if( transform == null ) return Vector2.zero;

		
		Vector2 posPivotFix = new Vector2(	transform.rect.width - transform.rect.width * transform.pivot.x,
											transform.rect.height * transform.pivot.y );

		return posPivotFix;

	}

}