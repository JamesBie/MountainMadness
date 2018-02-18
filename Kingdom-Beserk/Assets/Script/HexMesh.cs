using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {

	Mesh hexMesh ;
	static List <Vector3> vertices= new List<Vector3>();
	static List <int> triangles = new List<int>();
	static List<Color> colors = new List <Color>();
	MeshCollider meshCollider;


	void Awake() {
		GetComponent<MeshFilter> ().mesh = hexMesh = new Mesh ();
		meshCollider = gameObject.AddComponent<MeshCollider>();
		hexMesh.name = "Hex Mesh";
	}




	public void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3){
	
		int vertexIndex = vertices.Count;
		vertices.Add (v1);
		vertices.Add (v2);
		vertices.Add (v3);

		triangles.Add (vertexIndex);
		triangles.Add (vertexIndex + 1);
		triangles.Add (vertexIndex + 2);
	
	
	}



	public void AddTriangleColor(Color color){
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

	//supporting multiple colors for each traingle based off vertices;
	public void AddTriangleColor (Color c1, Color c2, Color c3) {
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
	public void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
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
	public void AddQuadColor (Color c1, Color c2, Color c3, Color c4) {
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c3);
		colors.Add(c4);
	}

	//variant of addquadcolor that only needs 2 colors
	public void AddQuadColor (Color c1, Color c2) {
		colors.Add(c1);
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c2);
	}

	public void Clear () {
		hexMesh.Clear();
		vertices.Clear();
		colors.Clear();
		triangles.Clear();
	}

	public void Apply () {
		hexMesh.SetVertices(vertices);
		hexMesh.SetColors(colors);
		hexMesh.SetTriangles(triangles, 0);
		hexMesh.RecalculateNormals();
		meshCollider.sharedMesh = hexMesh;
	}
}
