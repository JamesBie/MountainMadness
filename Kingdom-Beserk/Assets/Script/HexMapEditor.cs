
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {
	public Color[] colors;
	public HexGrid hexGrid;
	public Color activeColor;
	bool isDrag;
	HexDirection dragDirection;
	HexCell previousCell;

	void Awake () {
		SelectColor(0);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
			if (Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1)) {
				Debug.Log ("left/right button clicked");
				HandleInput();
			}
		else {
			previousCell = null;
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			hexGrid.ColorCell(hit.point, activeColor);
			HexCell currentCell = hexGrid.GetCell(hit.point);

			if (previousCell && previousCell != currentCell) {
				ValidateDrag(currentCell);
			}
			else {
				isDrag = false;
			}
			previousCell = currentCell; // sets the previous cell as what we just clicked for future use
		}else {
			previousCell = null;
		}
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
	}

	void ValidateDrag (HexCell currentCell) {
		for (
			dragDirection = HexDirection.NE;
			dragDirection <= HexDirection.NW;
			dragDirection++
		) {
			if (previousCell.GetNeighbor(dragDirection) == currentCell) {
				isDrag = true;
				return;
			}
		}
		isDrag = false;
	}


}
