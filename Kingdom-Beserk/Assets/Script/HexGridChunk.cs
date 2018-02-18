using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridChunk : MonoBehaviour {


	HexCell[] cells;

	//HexMesh hexMesh;
	public HexMesh terrain;
	Canvas gridCanvas;


	public void AddCell (int index, HexCell cell) {
		cells[index] = cell;
		cell.chunk = this;
		cell.transform.SetParent(transform, false);
		cell.uiRect.SetParent(gridCanvas.transform, false);
	}


	public void Triangulate( ){
		terrain.Clear();
		for (int i=0; i < cells.Length; i++) {

			Triangulate (cells [i]);
		}

		terrain.Apply();
	}



	void Triangulate (HexCell cell) {
		for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
			Triangulate(d, cell);
		}
	}

	void Triangulate (HexDirection direction, HexCell cell) {
		Vector3 center = cell.transform.localPosition;
		Vector3 vert1 = center + Hexmetrics.GetFirstSolidCorner (direction);
		Vector3 vert2 = center + Hexmetrics.GetSecondSolidCorner (direction);
		terrain.AddTriangle (center, vert1, vert2);

		terrain.AddTriangleColor (cell.color);

		if (direction <= HexDirection.SE) {
			TriangulateConnection (direction, cell, vert1, vert2);
		}


	}
	void TriangulateConnection (
		HexDirection direction, HexCell cell, Vector3 vert1, Vector3 vert2
	) {
		HexCell neighbor = cell.GetNeighbor(direction);
		if (neighbor == null) {
			return;
		}
		Vector3 bridge = Hexmetrics.GetBridge(direction);
		Vector3 vert3 = vert1 + bridge;
		Vector3 vert4 = vert2 + bridge;
		vert3.y = vert4.y = neighbor.Elevation * Hexmetrics.elevationChange;

		terrain.AddQuad(vert1, vert2, vert3, vert4);
		terrain.AddQuadColor(cell.color, neighbor.color);

		HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
		if (direction <= HexDirection.E && nextNeighbor != null) {

			Vector3 vert5 = vert2 + Hexmetrics.GetBridge(direction.Next());
			vert5.y = nextNeighbor.Elevation * Hexmetrics.elevationChange;
			terrain.AddTriangle(vert2, vert4, vert5);
			terrain.AddTriangleColor(cell.color, neighbor.color, nextNeighbor.color);
		}
	}


	public void ShowUI (bool visible) {
		gridCanvas.gameObject.SetActive(visible);
	}

	void Awake () {
		gridCanvas = GetComponentInChildren<Canvas>();
		//hexMesh = GetComponentInChildren<HexMesh>();

		cells = new HexCell[Hexmetrics.chunkSizeX * Hexmetrics.chunkSizeZ];
		ShowUI(false);
	}

	void Start () {
		
	}
		
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Refresh () {
		enabled = true;
	}

	void LateUpdate() {

		Triangulate ();
		enabled = false;
	}
}
