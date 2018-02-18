using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;
	public Color color;
	int elevation=0;
	public const float elevationStep = 2f;
	public RectTransform uiRect;

	[SerializeField]
	HexCell[] neighbors;



	//sets elevation
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

	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

	//this is used to determine neighbours

	void Awake(){
		
	}

	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
