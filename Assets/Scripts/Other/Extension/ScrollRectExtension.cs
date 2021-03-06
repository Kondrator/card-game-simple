using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public static class ScrollRectExtension{

	public static Vector2 GetSnapToPositionToBringChildIntoView( this ScrollRect instance, RectTransform child ){

		Canvas.ForceUpdateCanvases();

		Vector2 viewportLocalPosition = instance.viewport.localPosition;
		Vector2 childLocalPosition = child.localPosition;

		return new Vector2(	0 - ( viewportLocalPosition.x + childLocalPosition.x ),
							0 - ( viewportLocalPosition.y + childLocalPosition.y )
						);
	}

}
