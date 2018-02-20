using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;
	public Color color;
	public int elevation=0;
	public const float elevationStep = 2f;
	public RectTransform uiRect;
	public HexGridChunk chunk;
	bool hasIncomingRiver, hasOutgoingRiver;
	HexDirection incomingRiver, outgoingRiver;

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

			//Checks for invalid river creation
			if (
				hasOutgoingRiver &&
				elevation < GetNeighbor(outgoingRiver).elevation
			) {
				RemoveOutgoingRiver();
			}
			if (
				hasIncomingRiver &&
				elevation > GetNeighbor(incomingRiver).elevation
			) {
				RemoveIncomingRiver();
			}
			Refresh ();
		
		}
	}

	public Color Color {
		get {
			return color;
		}
		set {
			if (color == value) {
				return;
			}
			color = value;
			Refresh();
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

	void Refresh () {
		chunk.Refresh();
		for (int i = 0; i < neighbors.Length; i++) {
			HexCell neighbor = neighbors[i];
			if (neighbor != null && neighbor.chunk != chunk) {
				neighbor.chunk.Refresh();
			}
		}
	}



	// This is river related 
	public bool HasIncomingRiver {
		get {
			return hasIncomingRiver;
		}
	}

	public bool HasOutgoingRiver {
		get {
			return hasOutgoingRiver;
		}
	}

	public HexDirection IncomingRiver {
		get {
			return incomingRiver;
		}
	}

	public HexDirection OutgoingRiver {
		get {
			return outgoingRiver;
		}
	}

	public bool HasRiver {
		get {
			return hasIncomingRiver || hasOutgoingRiver;
		}
	}

	public bool HasRiverBeginOrEnd {
		get {
			return hasIncomingRiver != hasOutgoingRiver;
		}
	}

	public bool HasRiverThroughEdge (HexDirection direction) {
		return
			hasIncomingRiver && incomingRiver == direction ||
			hasOutgoingRiver && outgoingRiver == direction;
	}

	public void RemoveOutgoingRiver () {
		if (!hasOutgoingRiver) {
			return;
		}
		hasOutgoingRiver = false;
		RefreshSelfOnly();
		HexCell neighbor = GetNeighbor(outgoingRiver);
		neighbor.hasIncomingRiver = false;
		neighbor.RefreshSelfOnly();
	}
	void RefreshSelfOnly () {
		chunk.Refresh();
	}
	public void RemoveIncomingRiver () {
		if (!hasIncomingRiver) {
			return;
		}
		hasIncomingRiver = false;
		RefreshSelfOnly();

		HexCell neighbor = GetNeighbor(incomingRiver);
		neighbor.hasOutgoingRiver = false;
		neighbor.RefreshSelfOnly();
	}

	public void RemoveRiver () {
		RemoveOutgoingRiver();
		RemoveIncomingRiver();
	}

	public void SetOutgoingRiver (HexDirection direction) {
		if (hasOutgoingRiver && outgoingRiver == direction) {
			return;
		}
		HexCell neighbor = GetNeighbor(direction);
		if (!neighbor || elevation < neighbor.elevation) {
			return;
		}
		RemoveOutgoingRiver();
		if (hasIncomingRiver && incomingRiver == direction) {
			RemoveIncomingRiver();
		}

		hasOutgoingRiver = true;
		outgoingRiver = direction;
		RefreshSelfOnly();

		neighbor.RemoveIncomingRiver();
		neighbor.hasIncomingRiver = true;
		neighbor.incomingRiver = direction.Opposite();
		neighbor.RefreshSelfOnly();
	}

	//gets cells edge type for cliffs in a certain direction
	public HexEdgeType GetEdgeType (HexDirection direction) {
		return Hexmetrics.GetEdgeType(
			elevation, neighbors[(int)direction].elevation
		);
	}

	//determines edge type between two cells for triangulating corners
	public HexEdgeType GetEdgeType (HexCell otherCell) {
		return Hexmetrics.GetEdgeType(
			elevation, otherCell.elevation
		);
	}




}
