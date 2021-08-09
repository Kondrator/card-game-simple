using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RendererMaterialColorAnimation : MonoBehaviour, IMyManagerMonoBehaviour {

	[SerializeField]
	private Gradient gradient = new Gradient();
	[SerializeField, Range( 0.5f, 10f )]
	private float duration = 2f;
	[SerializeField, Range( 0, 1f )]
	private float progressStart = 0;


	private new Renderer renderer = null;
	private new Material material = null;

	private float progress = 0;
	private bool progressTo = true;


	void Awake(){
		renderer = GetComponent<Renderer>();
		if( renderer != null ){
			material = renderer.material;
		}
	}



	void OnEnable(){
		progress = progressStart;
		MyManagerMonoBehaviour.Add( this );
	}

	void OnDisable(){
		MyManagerMonoBehaviour.Remove( this );
	}



	public void UpdateMe( float timeDelta, MyManagerMonoBehaviourType type ){
		if( material != null ){
			float value = (1f / duration) * timeDelta;
			progress += progressTo ? value : -value;

			if( progress > 1
				|| progress < 0
			){
				progressTo = !progressTo;
				progress = Mathf.Clamp01( progress );
			}

			material.SetColor( "_Emission", gradient.Evaluate( progress ) );
		}
	}

}
