using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridChunk : MonoBehaviour {


	HexCell[] cells;

	//HexMesh hexMesh;
	public HexMesh terrain;
	public HexFeatureManager features;
	Canvas gridCanvas;


	public void AddCell (int index, HexCell cell) {
		cells[index] = cell;
		cell.chunk = this;
		cell.transform.SetParent(transform, false);
		cell.uiRect.SetParent(gridCanvas.transform, false);
	}


	public void Triangulate( ){
		terrain.Clear();
		features.Clear ();
		for (int i=0; i < cells.Length; i++) {

			Triangulate (cells [i]);
		}

		terrain.Apply();
		features.Apply ();
	}



	void Triangulate (HexCell cell) {
		for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
			Triangulate(d, cell);
		}

		features.AddFeature(cell.transform.position);
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

		if (cell.GetEdgeType (direction) == HexEdgeType.Slope) {
			TriangulateEdgeTerraces (vert1, vert2, cell, vert3, vert4, neighbor);
		}else {
		terrain.AddQuad(vert1, vert2, vert3, vert4);
		terrain.AddQuadColor(cell.color, neighbor.color);
		}
//		for (int i = 2; i < Hexmetrics.terraceSteps; i++) {
//			Vector3 v1 = vert3;
//			Vector3 v2 = vert4;
//			Color c1 = c2;
//			vert3 = Hexmetrics.TerraceLerp(beginLeft, endLeft, i);
//			vert4 = Hexmetrics.TerraceLerp(beginRight, endRight, i);
//			c2 = Hexmetrics.TerraceLerp(beginCell.color, endCell.color, i);
//			terrain.AddQuad(v1, v2, v3, v4);
//			terrain.AddQuadColor(c1, c2);
//		}
//
//		terrain.AddQuad(vert3, vert4, endLeft, endRight);
//		terrain.AddQuadColor(c2, endCell.color);


		HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
		if (direction <= HexDirection.E && nextNeighbor != null) {

			Vector3 vert5 = vert2 + Hexmetrics.GetBridge(direction.Next());
			vert5.y = nextNeighbor.Elevation * Hexmetrics.elevationChange;


			if (cell.Elevation <= neighbor.Elevation) {
				if (cell.Elevation <= nextNeighbor.Elevation) {
					TriangulateCorner(vert2, cell, vert4, neighbor, vert5, nextNeighbor);
				}else {
					TriangulateCorner(vert5, nextNeighbor, vert2, cell, vert4, neighbor);
				}
			}else if (neighbor.Elevation <= nextNeighbor.Elevation) {
				TriangulateCorner(vert4, neighbor, vert5, nextNeighbor, vert2, cell);
			}
			else {
				TriangulateCorner(vert5, nextNeighbor, vert2, cell, vert4, neighbor);
			}

			terrain.AddTriangle(vert2, vert4, vert5);
			terrain.AddTriangleColor(cell.color, neighbor.color, nextNeighbor.color);

		}
	}

	void TriangulateEdgeTerraces (
		Vector3 beginLeft, Vector3 beginRight, HexCell beginCell,
		Vector3 endLeft, Vector3 endRight, HexCell endCell
	) {

		Vector3 vert3 = Hexmetrics.TerraceLerp(beginLeft, endLeft, 1);
		Vector3 vert4 = Hexmetrics.TerraceLerp(beginRight, endRight, 1);
		Color c2 = Hexmetrics.TerraceLerp(beginCell.color, endCell.color, 1);

		terrain.AddQuad(beginLeft, beginRight, vert3, vert4);
		terrain.AddQuadColor(beginCell.color, c2);

		for (int i = 2; i < Hexmetrics.terraceSteps; i++) {
			Vector3 vert1 = vert3;
			Vector3 vert2 = vert4;
			Color c1 = c2;
			vert3 = Hexmetrics.TerraceLerp(beginLeft, endLeft, i);
			vert4 = Hexmetrics.TerraceLerp(beginRight, endRight, i);
			c2 = Hexmetrics.TerraceLerp(beginCell.color, endCell.color, i);
			terrain.AddQuad(vert1, vert2, vert3, vert4);
			terrain.AddQuadColor(c1, c2);
		}

		terrain.AddQuad(vert3, vert4, endLeft, endRight);
		terrain.AddQuadColor(c2, endCell.color);
	}


	void TriangulateCorner (
		Vector3 bottom, HexCell bottomCell,
		Vector3 left, HexCell leftCell,
		Vector3 right, HexCell rightCell
	) {
		HexEdgeType leftEdgeType = bottomCell.GetEdgeType(leftCell);
		HexEdgeType rightEdgeType = bottomCell.GetEdgeType(rightCell);

		if (leftEdgeType == HexEdgeType.Slope) {
			if (rightEdgeType == HexEdgeType.Slope) {
				TriangulateCornerTerraces(
					bottom, bottomCell, left, leftCell, right, rightCell
				);
				return;
			}
		}

		terrain.AddTriangle(bottom, left, right);
		terrain.AddTriangleColor(bottomCell.color, leftCell.color, rightCell.color);
	}

	void TriangulateCornerTerraces (
		Vector3 begin, HexCell beginCell,
		Vector3 left, HexCell leftCell,
		Vector3 right, HexCell rightCell
	) {

		Vector3 vert3 = Hexmetrics.TerraceLerp(begin, left, 1);
		Vector3 vert4 = Hexmetrics.TerraceLerp(begin, right, 1);
		Color c3 = Hexmetrics.TerraceLerp(beginCell.color, leftCell.color, 1);
		Color c4 = Hexmetrics.TerraceLerp(beginCell.color, rightCell.color, 1);

		terrain.AddTriangle(begin, vert3, vert4);
		terrain.AddTriangleColor(beginCell.color, c3, c4);

		for (int i = 2; i < Hexmetrics.terraceSteps; i++) {
			Vector3 vert1 = vert3;
			Vector3 vert2 = vert4;
			Color c1 = c3;
			Color c2 = c4;
			vert3 = Hexmetrics.TerraceLerp(begin, left, i);
			vert4 = Hexmetrics.TerraceLerp(begin, right, i);
			c3 = Hexmetrics.TerraceLerp(beginCell.color, leftCell.color, i);
			c4 = Hexmetrics.TerraceLerp(beginCell.color, rightCell.color, i);
			terrain.AddQuad(vert1, vert2, vert3, vert4);
			terrain.AddQuadColor(c1, c2, c3, c4);
		}

		terrain.AddQuad(vert3, vert4, left, right);
		terrain.AddQuadColor(c3, c4, leftCell.color, rightCell.color);

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
