using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GradientExtension{

	public static Gradient GetSimple(){
		Gradient gradient = new Gradient();
		
		gradient.alphaKeys = new GradientAlphaKey[]{	new GradientAlphaKey( 1, 0 ),
														new GradientAlphaKey( 1, 1 )
													};
		gradient.colorKeys = new GradientColorKey[]{	new GradientColorKey( Color.white, 0 ), 
														new GradientColorKey( Color.white, 1 ) 
													};

		return gradient;
	}

	public static Gradient GetSimple( Color color ){
		Gradient gradient = new Gradient();
		
		gradient.alphaKeys = new GradientAlphaKey[]{	new GradientAlphaKey( 1, 0 ),
														new GradientAlphaKey( 1, 1 )
													};
		gradient.colorKeys = new GradientColorKey[]{	new GradientColorKey( color, 0 ), 
														new GradientColorKey( color, 1 ) 
													};

		return gradient;
	}

	public static Gradient GetSimple( Color color1, Color color2 ){
		Gradient gradient = new Gradient();
		
		gradient.alphaKeys = new GradientAlphaKey[]{	new GradientAlphaKey( 1, 0 ),
														new GradientAlphaKey( 1, 1 )
													};
		gradient.colorKeys = new GradientColorKey[]{	new GradientColorKey( color1, 0 ), 
														new GradientColorKey( color2, 1 ) 
													};

		return gradient;
	}
}
