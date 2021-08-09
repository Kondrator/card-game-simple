using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public static class MyOperationAnimation{


	public static AnimationCurve GetCurveSimple(){
		return AnimationCurveExtensions.GetCurveSimple();
	}
	public static AnimationCurve CheckCurve( AnimationCurve curve ){
		if( curve == null ){
			return AnimationCurveExtensions.GetCurveSimple();
		}
		return curve;
	}


	
	private static WaitForSecondsRealtime WaitSeconds{
		get{
			return new WaitForSecondsRealtime( 0.01f );
		}
	}

	private static int CalculateCountStep( float speedAnim ){
		return (int)(1000 / speedAnim);
	}





	public static void StartAnimationMoveFromTo(	Transform moveFrom, Transform moveTo, 
													Transform targetAnim, Sprite targetSprite, Transform parent,
													AnimationCurve curveAnimScale, float duration = 1f, System.Action callbackCompleted = null
	){
		
		if( moveFrom == null
			|| moveTo == null
			|| targetAnim == null
			|| targetSprite == null
			|| parent == null
		){
			return;
		}

		Image animItemNew = PoolGameObject.Get<Image>( targetAnim.gameObject, parent );
		animItemNew.sprite = targetSprite;
		animItemNew.transform.localScale = Vector3.one;
	
		RectTransform rectItemNew = animItemNew.transform as RectTransform;
		RectTransform rectItemUIFrom = moveFrom as RectTransform;
		RectTransform rectTargetTo = moveTo as RectTransform;
		
		Vector3 positionFrom = rectItemUIFrom.position;
		Vector3 positionTo = rectTargetTo.position;
		rectItemNew.position = positionFrom;
		
		TimerExecutor.Add( duration, 0.01f, ( float progress ) =>{
			rectItemNew.position = Vector3.Lerp( positionFrom, positionTo, progress );
			if( curveAnimScale != null ){
				float scale = curveAnimScale.Evaluate( progress );
				animItemNew.transform.localScale = new Vector3( scale, scale, 1f );
			}

		}, () =>{
			animItemNew.Deactivate();
			if( callbackCompleted != null ){
				callbackCompleted();
			}
		} );

	}





	
	/// <summary>
	/// Play animation clip with name in Animation target.
	/// </summary>
	/// <param name="target">Target Animation.</param>
	/// <param name="name">Name of animation clip.</param>
	/// <param name="callbackCompleted">Callback for end of animation.</param>
	public static IEnumerator CoroutineCustom( Animation target, string name, float speed = 1, System.Action callbackCompleted = null ){
		
		// check erros
		if( target == null
			|| target.GetClip( name ) == null
		){
			if( callbackCompleted != null ){
				callbackCompleted();
			}
			yield return new WaitForEndOfFrame();
		}

		// start play
		target[name].speed = speed;
		target.Play( name, PlayMode.StopAll );

		// wait end
		while( target.isPlaying ){
			yield return new WaitForEndOfFrame();
		}

		// observer end
		if( callbackCompleted != null ){
			callbackCompleted();
		}

	}




	/// <summary>
	/// Animation move object from point1 ti point2.
	/// </summary>
	/// <param name="rectOpacity">Target.</param>
	/// <param name="animSpeed">Speed animation.</param>
	/// <param name="curve">Curve animation. Если == NULL - simple curve [0; 1].</param>
	/// <param name="posFrom">Move from.</param>
	/// <param name="posTo">Move to.</param>
	public static IEnumerator CoroutineMove(	RectTransform rect, 
												int animSpeed, AnimationCurve curve, 
												Vector2 posFrom, Vector2 posTo,
												System.Action onCompleted = null ){

		curve = CheckCurve( curve );

		int count = CalculateCountStep( animSpeed );
		
		for( int i = 0; i < count; i++ ){

			float time = (float)i / (float)count;
			
			float x = curve.Evaluate( time, posFrom.x, posTo.x );
			float y = curve.Evaluate( time, posFrom.y, posTo.y );
			rect.anchoredPosition = new Vector2( x, y );

			yield return WaitSeconds;

		}

		rect.anchoredPosition = posTo;

		// observer
		if( onCompleted != null ){
			onCompleted();
		}

	}

	
	/// <summary>
	/// Animation scale and opacity.
	/// </summary>
	/// <param name="rectOpacity">Target opacity.</param>
	/// <param name="rectPOOF">Target scale.</param>
	/// <param name="animSpeed">Speed animation.</param>
	/// <param name="curve">Curve animation. Если == NULL - simple curve [0; 1].</param>
	/// <param name="alphaFrom">Opacity from.</param>
	/// <param name="alphaTo">Opacity to.</param>
	/// <param name="scaleFrom">Scale from.</param>
	/// <param name="scaleTo">Scale to.</param>
	public static IEnumerator CoroutinePoof(	RectTransform rectOpacity, RectTransform rectPOOF, 
												int animSpeed, AnimationCurve curve, 
												float alphaFrom, float alphaTo, float scaleFrom, float scaleTo,
												System.Action onCompleted = null ){

		curve = CheckCurve( curve );

		// setting begin animation
		CanvasGroup canvas = MyOperation.AddComponent<CanvasGroup>( rectOpacity.gameObject );
		canvas.alpha = alphaFrom;
		canvas.blocksRaycasts = alphaTo > 0 ? true : false;
		rectPOOF.localScale = new Vector3( scaleFrom, scaleFrom, 1 );


		int count = CalculateCountStep( animSpeed );
		for( int i = 0; i < count; i++ ){

			float time = (float)i / (float)count;

			// opacity - line
			canvas.alpha = alphaFrom + (alphaTo - alphaFrom) * time;
			// opacity - curve
			float scale = curve.Evaluate( time, scaleFrom, scaleTo );
			rectPOOF.localScale = new Vector3( scale, scale, 1 );
			
			yield return WaitSeconds;

		}

		yield return new WaitForEndOfFrame();
		
		canvas.alpha = alphaTo;
		canvas.blocksRaycasts = canvas.alpha == 0 ? false : true;
		rectPOOF.localScale = new Vector3( scaleTo, scaleTo, 1 );

		// observer
		if( onCompleted != null ){
			onCompleted();
		}

	}



	/// <summary>
	/// Animation opacity.
	/// </summary>
	/// <param name="rectOpacity">Target.</param>
	/// <param name="animSpeed">Speed animation.</param>
	/// <param name="curve">Curve animation. Если == NULL - simple curve [0; 1].</param>
	/// <param name="alphaFrom">Opacity from.</param>
	/// <param name="alphaTo">Opacity to.</param>
	public static IEnumerator CoroutineOpacity(	RectTransform rectOpacity, 
												int animSpeed, AnimationCurve curve, 
												float alphaFrom, float alphaTo,
												System.Action onCompleted = null ){

		curve = CheckCurve( curve );

		// setting begin animation
		CanvasGroup canvas = MyOperation.AddComponent<CanvasGroup>( rectOpacity.gameObject );
		canvas.alpha = alphaFrom;
		canvas.blocksRaycasts = true;

		int count = CalculateCountStep( animSpeed );
		for( int i = 0; i < count; i++ ){

			float time = (float)i / (float)count;

			// opacity - curve
			canvas.alpha = curve.Evaluate( time, alphaFrom, alphaFrom + (alphaTo - alphaFrom) * time );
			
			yield return WaitSeconds;

		}

		yield return new WaitForEndOfFrame();
		
		canvas.alpha = alphaTo;
		canvas.blocksRaycasts = canvas.alpha == 0 ? false : true;

		// observer
		if( onCompleted != null ){
			onCompleted();
		}

	}
	/// <summary>
	/// Animation opacity.
	/// </summary>
	/// <param name="rectOpacity">Target.</param>
	/// <param name="animSpeed">Speed animation.</param>
	/// <param name="curve">Curve animation. Если == NULL - simple curve [0; 1].</param>
	/// <param name="isOn">True - show. False - hide.</param>
	public static IEnumerator CoroutineOpacity(	RectTransform rectOpacity, 
												int animSpeed, AnimationCurve curve, 
												bool isOn,
												System.Action onCompleted = null
	){
		CanvasGroup canvasGroup = rectOpacity.AddComponent<CanvasGroup>();
		
		float from = canvasGroup.alpha;
		float to = isOn == true ? 1f : 0;

		return CoroutineOpacity( rectOpacity, animSpeed, curve, from, to, onCompleted );

	}

	
	
	
	// for animation "TV".
	private static Dictionary<RectTransform, Vector2> sizes;
	private static Dictionary<RectTransform, Vector2> Sizes{
		get{
			if( sizes == null ){
				sizes = new Dictionary<RectTransform, Vector2>();
			}
			return sizes;
		}
		set{
			sizes = value;
		}
	}
	/// <summary>
	/// Animation TV.
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="animSpeed">Speed animation.</param>
	/// <param name="isOn">True - ON. False - OFF.</param>
	public static IEnumerator CoroutineTV( RectTransform target, int animSpeed, bool isOn, System.Action onCompleted = null ){
		
		// setting size
		Vector2 size = Vector2.zero;
		if( Sizes.ContainsKey( target ) == false ){
			Sizes[target] = target.GetSize();
			// fix
			target.SetPivotAndAnchors( new Vector2( 0.5f, 1f ) );
		}
		size = sizes[target];
		
		// setting opacity
		CanvasGroup canvas = MyOperation.AddComponent<CanvasGroup>( target.gameObject );
		canvas.alpha = 1;

		
		float percentWidthAtTime = 0.5f;
		float percentHeightAtTime = 1f - percentWidthAtTime;
		float firstHeight = 25;


		int count = CalculateCountStep( animSpeed );
		for( int i = 0; i < count; i++ ){

			int iFix = i;
			if( isOn == false ){
				iFix = count - iFix;
			}

			float time = (float)iFix / (float)count;


			// SIZE
			Vector2 sizeNew = new Vector2();
			// x
			sizeNew.x = size.x * (time / percentWidthAtTime);
			if( sizeNew.x > size.x ){
				sizeNew.x = size.x;
			}
			// y
			if( sizeNew.x == size.x ){
				sizeNew.y = firstHeight + (size.y - firstHeight) * ((time - percentWidthAtTime) / percentHeightAtTime);

			}else{
				sizeNew.y = firstHeight;
			}




			// set size
			target.sizeDelta = sizeNew;
			
			yield return WaitSeconds;

		}

		yield return new WaitForEndOfFrame();
		
		canvas.alpha = 1;
		target.sizeDelta = size;

		// observer
		if( onCompleted != null ){
			onCompleted();
		}

		yield return new WaitForEndOfFrame();
		
	}








	// for animation "Offset with opacity".
	private static Dictionary<RectTransform, Vector2> positions;
	private static Dictionary<RectTransform, Vector2> Positions{
		get{
			if( positions == null ){
				positions = new Dictionary<RectTransform, Vector2>();
			}
			return positions;
		}
		set{
			positions = value;
		}
	}
	/// <summary>
	/// Animation offset and opacity.
	/// </summary>
	/// <param name="initiate">Initiator.</param>
	/// <param name="target">Target.</param>
	/// <param name="animSpeed">Speed animation.</param>
	/// <param name="curve">Curve animation. Если == NULL - simple curve [0; 1].</param>
	/// <param name="isOn">True - show. False - hide.</param>
	public static IEnumerator CoroutineOffsetOpacity(	MonoBehaviour initiate, RectTransform target, 
														int animSpeed, AnimationCurve curve,
														bool isOn, Vector2 moveShowFrom, Vector2 moveHideTo, bool isOpacity,
														System.Action onCompleted = null ){

		curve = CheckCurve( curve );

		// for opacity
		CanvasGroup canvasGroup = MyOperation.AddComponent<CanvasGroup>( target.gameObject );


		// SETTING position
		Vector2 moveFrom = target.anchoredPosition;
		if( Positions.ContainsKey( target ) == false ){
			Positions[target] = target.anchoredPosition;
			if( isOpacity == true
				&& isOn == true
			){
				canvasGroup.alpha = 0;
			}
		}
		Vector2 position = Positions[target];


		// offset setting params
		Vector2 moveTo = target.anchoredPosition;//new Vector2( position.x, position.y );
		// ON
		if( isOn == true ){
			//if( canvasGroup.alpha == 0 || isOpacity == false ){
				// use only need axis
				if( moveShowFrom.x != 0 || moveHideTo.x != 0 ){
					moveFrom.x = position.x + moveShowFrom.x;
					moveTo.x = position.x;
				}
				// use only need axis
				if( moveShowFrom.y != 0 || moveHideTo.y != 0 ){
					moveFrom.y = position.y + moveShowFrom.y;
					moveTo.y = position.y;
				}
			//}

		// OFF
		}else{
			//if( canvasGroup.alpha != 1 ){
				// use only need axis
				if( moveHideTo.x != 0 ){
					moveTo.x = position.x;
				}
				// use only need axis
				if( moveHideTo.y != 0 ){
					moveTo.y = position.y;
				}
			//}
			moveTo += moveHideTo;
		}




		
		// SETTING opacity params
		float alphaFrom = canvasGroup.alpha;
		float alphaTo = isOn == true ? 1f : 0;
		canvasGroup.blocksRaycasts = alphaTo > 0 ? true : false;


		// animation OPACITY and MOVE
		int count = CalculateCountStep( animSpeed );
		for( int i = 0; i < count; i++ ){

			float time = (float)i / (float)count;

			if( isOpacity == true ){
				// opacity - curve
				//SetAlhpa( canvasGroup, curve.Evaluate( time, alphaFrom, alphaFrom + (alphaTo - alphaFrom) * time ) );
				canvasGroup.alpha = curve.Evaluate( time, alphaFrom, alphaFrom + (alphaTo - alphaFrom) * time );
			}

			// move - curve
			float x = curve.Evaluate( time, moveFrom.x, moveTo.x );
			float y = curve.Evaluate( time, moveFrom.y, moveTo.y );
			target.anchoredPosition = new Vector2( x, y );
			
			yield return WaitSeconds;

		}

		yield return new WaitForEndOfFrame();
		
		// apply finish
		if( isOpacity == true ){
			canvasGroup.alpha = alphaTo;
		}
		canvasGroup.blocksRaycasts = canvasGroup.alpha == 0 ? false : true;
		target.anchoredPosition = moveTo;




		// observer
		if( onCompleted != null ){
			onCompleted();
		}

		yield return new WaitForEndOfFrame();

	}
	private static void SetAlhpa( CanvasGroup canvasGroup, float alpha ){
		if( canvasGroup != null ){
			canvasGroup.alpha = alpha;
		}
	}

}
