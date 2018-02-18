using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

	public int width=6;
	public int height=6;
	public HexCell cellPrefab;
	public Text cellLabelPrefab;
	public Material[] HexMaterials;
	MeshRenderer mr;
	Canvas gridCanvas;
	HexMesh hexMesh;


	public Color defaultColor = Color.white;
	public Color touchedColor = Color.magenta;

	HexCell [] cells;

	void Awake () {
		gridCanvas = GetComponentInChildren<Canvas> ();
		hexMesh = GetComponentInChildren<HexMesh> ();
		mr = GetComponentInChildren<MeshRenderer> ();
	
		cells = new HexCell[height * width];

		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	
	}
		


	void CreateCell (int x, int z, int i){
		Vector3 position;
		position.x = (x + z*0.5f-z/2) * (Hexmetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (Hexmetrics.outerRadius*1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates (x,z);
		cell.color = HexMaterials [Random.Range(0,HexMaterials.Length)].color;

		//setting the neighbour cells
		if (x > 0) {  //sets east/west
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}

		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - width]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
				}
			}
			else {
				cell.SetNeighbor(HexDirection.SW, cells[i - width]);
				if (x < width - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
				}
			}
		}
	


		//Text label coodrinates to display where each cell is. can remove later
		Text label = Instantiate<Text> (cellLabelPrefab);
		label.rectTransform.SetParent (gridCanvas.transform, false);
		label.rectTransform.anchoredPosition = new Vector2 (position.x, position.z);
		label.text = x.ToString() + "\n" + z.ToString();
		label.text = cell.coordinates.ToStringOnSeparateLines();

		cell.uiRect = label.rectTransform;
	}


	// Use this for initialization
	void Start () {
		hexMesh.Triangulate (cells);
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1)) {
			Debug.Log ("left/right button clicked");
			HandleInput();
		}
		
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.Log ("Raycast looking for collider");
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			TouchCell(hit.point);

			Refresh ();
		}
	}

	// changes the color of cell when its clicked
	void TouchCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		Debug.Log("touched at " + coordinates.ToString());
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		HexCell cell = cells[index];
		cell.color = touchedColor;
		ElevateCell (cell);

	}

	void ElevateCell(HexCell cell){
		if(Input.GetButton("Jump")) {
			Debug.Log ("Space bar held while clicking");
			if (Input.GetMouseButtonDown (0)) {
				cell.Elevation += (int)Hexmetrics.elevationChange;
			} else if (Input.GetMouseButtonDown (1)) {
			
				cell.Elevation -=(int) Hexmetrics.elevationChange;
			}

			}
	
	}


	public HexCell GetCell (Vector3 position) {
				position = transform.InverseTransformPoint(position);
				HexCoordinates coordinates = HexCoordinates.FromPosition(position);
				Debug.Log("touched at " + coordinates.ToString());
				int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
				return cells[index];
	
			}

	public void Refresh() {
	
		hexMesh.Triangulate(cells);
	}

	//used with hex map color editor which is not in use
//	public void ColorCell (Vector3 position, Color color) {
//		position = transform.InverseTransformPoint(position);
//		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
//		Debug.Log("touched at " + coordinates.ToString());
//		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
//		HexCell cell = cells[index];
//		cell.color = color;
//		hexMesh.Triangulate(cells);
//	}
}
