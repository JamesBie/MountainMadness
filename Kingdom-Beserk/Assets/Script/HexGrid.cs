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
	Canvas gridCanvas;
	HexMesh hexMesh;

	HexCell [] cells;

	void Awake () {
		gridCanvas = GetComponentInChildren<Canvas> ();
		hexMesh = GetComponentInChildren<HexMesh> ();
	
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
	


		//Text label coodrinates to display where each cell is. can remove later
		Text label = Instantiate<Text> (cellLabelPrefab);
		label.rectTransform.SetParent (gridCanvas.transform, false);
		label.rectTransform.anchoredPosition = new Vector2 (position.x, position.z);
		label.text = x.ToString() + "\n" + z.ToString();
		label.text = cell.coordinates.ToStringOnSeparateLines();


	}


	// Use this for initialization
	void Start () {
		hexMesh.Triangulate (cells);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
