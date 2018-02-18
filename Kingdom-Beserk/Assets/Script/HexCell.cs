using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;
	public Color color;
	int elevation=0;
	public const float elevationStep = 2f;
	public RectTransform uiRect;

	public int Elevation {
		get {
			return elevation;
		}
		set {
			elevation = value;
			Vector3 position = transform.localPosition;
			position.y = value * Hexmetrics.elevationChange;
			transform.localPosition = position;

			//move label up as well to keep it on the tile
			Vector3 uiPosition = uiRect.localPosition;
			uiPosition.z = elevation * -Hexmetrics.elevationChange;
			uiRect.localPosition = uiPosition;
		}
	}

	void Awake(){
		
	}

	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
