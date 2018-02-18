using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {

	Mesh hexMesh ;
	List <Vector3> vertices;
	List <int> triangles;
	List<Color> colors;
	MeshCollider meshCollider;


	void Awake() {
		GetComponent<MeshFilter> ().mesh = hexMesh = new Mesh ();
		meshCollider = gameObject.AddComponent<MeshCollider>();
		hexMesh.name = "Hex Mesh";
		vertices = new List<Vector3> ();
		colors = new List<Color> ();
		triangles = new List <int> ();
	
	
	
	}


	public void Triangulate( HexCell[] cells){
		hexMesh.Clear ();
		vertices.Clear ();
		triangles.Clear ();
		colors.Clear ();
		for (int i=0; i < cells.Length; i++) {

			Triangulate (cells [i]);
		}

		hexMesh.vertices = vertices.ToArray();
		hexMesh.triangles = triangles.ToArray();
		hexMesh.colors = colors.ToArray ();
		hexMesh.RecalculateNormals();
		meshCollider.sharedMesh = hexMesh;
	}


//	void Triangulate (HexCell cell){
//	
//		Vector3 center = cell.transform.localPosition;
//		for (int i = 0; i < 6; i++) {
//			AddTriangle (
//				center,
//				center + Hexmetrics.corners [i],
//				center + Hexmetrics.corners [i+1]
//			);
//			AddTriangleColor (cell.color);
//		}
//	}
	void Triangulate (HexCell cell) {
	for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
		Triangulate(d, cell);
	}
}

void Triangulate (HexDirection direction, HexCell cell) {
		Vector3 center = cell.transform.localPosition;
		Vector3 vert1 = center + Hexmetrics.GetFirstSolidCorner (direction);
		Vector3 vert2 = center + Hexmetrics.GetSecondSolidCorner (direction);
		AddTriangle (center, vert1, vert2);

		AddTriangleColor (cell.color);

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

		AddQuad(vert1, vert2, vert3, vert4);
		AddQuadColor(cell.color, neighbor.color);

		HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
		if (nextNeighbor != null) {
			AddTriangle(vert2, vert4, vert2+ Hexmetrics.GetBridge(direction.Next()));
			AddTriangleColor(cell.color, neighbor.color, nextNeighbor.color);
		}
	}


	void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3){
	
		int vertexIndex = vertices.Count;
		vertices.Add (v1);
		vertices.Add (v2);
		vertices.Add (v3);

		triangles.Add (vertexIndex);
		triangles.Add (vertexIndex + 1);
		triangles.Add (vertexIndex + 2);
	
	
	}



	void AddTriangleColor(Color color){
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

	//supporting multiple colors for each traingle based off vertices;
	void AddTriangleColor (Color c1, Color c2, Color c3) {
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c3);
	}



	void TriangulateConnections(){
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//fills in space 
	void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		vertices.Add(v4);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
	}

	//this is for the spaces between the solid color cores between hexes. 
	void AddQuadColor (Color c1, Color c2, Color c3, Color c4) {
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c3);
		colors.Add(c4);
	}

	//variant of addquadcolor that only needs 2 colors
	void AddQuadColor (Color c1, Color c2) {
		colors.Add(c1);
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c2);
	}
}
