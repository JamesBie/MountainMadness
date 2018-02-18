using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapCamera : MonoBehaviour {

	Transform swivel, stick ;

	float zoom = 1f;
	public float stickMinZoom, stickMaxZoom;
	public float swivelMinZoom, swivelMaxZoom;
	public float moveSpeedMinZoom, moveSpeedMaxZoom;
	public HexGrid grid;
	public float rotationSpeed;

	void Awake () {
		swivel = transform.GetChild (0);
		stick = swivel.GetChild (0);

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float zoomDelta = Input.GetAxis ("Mouse ScrollWheel");
		if (zoomDelta != 0f) {
			AdjustZoom (zoomDelta);
		}

		float xDelta = Input.GetAxis ("Horizontal");
		float zDelta = Input.GetAxis ("Vertical");

		float rotationDelta = Input.GetAxis ("Rotation");
		if (rotationDelta != 0f) {

			AdjustRotation (rotationDelta);
		}

		if (xDelta != 0f || zDelta != 0f) {
			AdjustPosition (xDelta, zDelta);
		}
		
	}

	// allows rotation of the camera
	void AdjustRotation (float delta) {
	}


	// allows you to move the camera
	void AdjustPosition (float xDelta, float zDelta) {
		Vector3 direction = new Vector3 (xDelta, 0f, zDelta).normalized;// allows diagonal movement to be the same as side
		float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
		float distance = Mathf.Lerp (moveSpeedMinZoom,moveSpeedMaxZoom,zoom) * damping *  Time.deltaTime; // this allows faster scrolling when zoomed out
		Vector3 position = transform.localPosition;
		position += direction*distance;
		transform.localPosition = ClampPosition(position);
	}

	Vector3 ClampPosition( Vector3 position) {
		float xMax =(grid.chunkCountX * Hexmetrics.chunkSizeX -0.5f)*(2f * Hexmetrics.innerRadius); //doesn't allow your camera to move off the map

		position.x = Mathf.Clamp(position.x, 0f, xMax);

		float zMax = (grid.chunkCountZ * Hexmetrics.chunkSizeZ-1f) * (1.5f * Hexmetrics.outerRadius);

		position.z = Mathf.Clamp(position.z, 0f, zMax);

		return position;

	}

	void AdjustZoom (float delta){
		zoom = Mathf.Clamp01 (zoom + delta);

		float distance = Mathf.Lerp (stickMinZoom, stickMaxZoom, zoom);
		stick.localPosition = new Vector3 (0f, 0f, distance);

		float angle = Mathf.Lerp (swivelMinZoom, swivelMaxZoom, zoom);
		swivel.localRotation = Quaternion.Euler (angle, 0f, 0f);
	}


}
