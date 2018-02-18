using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridChunk : MonoBehaviour {


	HexCell[] cells;

	HexMesh hexMesh;
	Canvas gridCanvas;


	public void AddCell (int index, HexCell cell) {
		cells[index] = cell;
		cell.chunk = this;
		cell.transform.SetParent(transform, false);
		cell.uiRect.SetParent(gridCanvas.transform, false);
	}

	public void ShowUI (bool visible) {
		gridCanvas.gameObject.SetActive(visible);
	}

	void Awake () {
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

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

		hexMesh.Triangulate (cells);
		enabled = false;
	}
}
