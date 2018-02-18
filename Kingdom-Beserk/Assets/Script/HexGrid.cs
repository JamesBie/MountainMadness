using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {


	public int chunkCountX = 4; // how many chunks in the map along x
	public int chunkCountZ = 3; //how many chunks in the map along z
	public bool labels;

	int cellCountX; //how many cells in total along x
	int cellCountZ;// how many cells in total along z

	public HexCell cellPrefab;
	public HexGridChunk chunkPrefab;
	public Text cellLabelPrefab;
	public Material[] HexMaterials;
	private Color activeColor;
	private HexMapEditor editor ;

	//MeshRenderer mr;

	HexGridChunk[] chunks;


	public Color defaultColor = Color.white;
	public Color touchedColor = Color.magenta;

	HexCell [] cells;

	void Awake () {

	//	mr = GetComponentInChildren<MeshRenderer> ();
	

		cellCountX = chunkCountX * Hexmetrics.chunkSizeX;
		cellCountZ = chunkCountZ * Hexmetrics.chunkSizeZ;

		CreateChunks ();
		CreateCells ();
	}
	void CreateCells(){
		cells = new HexCell[cellCountZ * cellCountX];

		for (int z = 0, i = 0; z < cellCountZ; z++) {
			for (int x = 0; x < cellCountX; x++) {
				CreateCell(x, z, i++);
			}
		}
	
	}
		
	void CreateChunks () {
		chunks = new HexGridChunk[chunkCountX * chunkCountZ];

		for (int z = 0, i = 0; z < chunkCountZ; z++) {
			for (int x = 0; x < chunkCountX; x++) {
				HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
				chunk.transform.SetParent(transform);
			}
		}
	}

	void CreateCell (int x, int z, int i){
		Vector3 position;
		position.x = (x + z*0.5f-z/2) * (Hexmetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (Hexmetrics.outerRadius*1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		//cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates (x,z);
		cell.color = HexMaterials [Random.Range(0,HexMaterials.Length)].color;

		//setting the neighbour cells
		if (x > 0) {  //sets east/west
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}

		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
				}
			}
			else {
				cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
				if (x < cellCountX - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
				}
			}
		}
	


		//Text label coodrinates to display where each cell is. can remove later
		Text label = Instantiate<Text> (cellLabelPrefab);
		//label.rectTransform.SetParent (gridCanvas.transform, false);
		label.rectTransform.anchoredPosition = new Vector2 (position.x, position.z);
		label.text = x.ToString() + "\n" + z.ToString();
		label.text = cell.coordinates.ToStringOnSeparateLines();

		cell.uiRect = label.rectTransform;

		AddCellToChunk(x, z, cell);
	}


	void AddCellToChunk (int x, int z, HexCell cell) {

		int chunkX = x / Hexmetrics.chunkSizeX;
		int chunkZ = z / Hexmetrics.chunkSizeZ;
		HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

		int localX = x - chunkX * Hexmetrics.chunkSizeX;
		int localZ = z - chunkZ * Hexmetrics.chunkSizeZ;
		chunk.AddCell(localX + localZ * Hexmetrics.chunkSizeX, cell);
	}

	// Use this for initialization
	void Start () {
		ShowUI (labels);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1)) {
			Debug.Log ("left/right button clicked");
			//HandleInput();
		}
		
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.Log ("Raycast looking for collider");
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			ColorCell(hit.point, editor.activeColor);


		}
	}

	// changes the color of cell when its clicked
//	void TouchCell (Vector3 position) {
//		position = transform.InverseTransformPoint(position);
//		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
//		Debug.Log("touched at " + coordinates.ToString());
//		int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
//		HexCell cell = cells[index];
//		ElevateCell (cell);
//		cell.Color = touchedColor;
//
//
//	}

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
		int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
				return cells[index];
	
			}

	public void ShowUI (bool visible) {
		for (int i = 0; i < chunks.Length; i++) {
			chunks[i].ShowUI(visible);
		}
	}



	public void ColorCell (Vector3 position, Color color) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		Debug.Log("touched at " + coordinates.ToString());
		int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
		HexCell cell = cells[index];
		ElevateCell (cell);
		cell.Color = color;
		//hexMesh.Triangulate(cells);
	}
}
