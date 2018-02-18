using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexFeatureManager : MonoBehaviour {

	public Transform featurePrefab;
	public Transform container;

	public void Clear (){
		if (container) {
			Destroy (container.gameObject);
		}
		container = new GameObject("Features Container").transform;
		container.SetParent(transform, false); 

	}

	public void Apply () {}

	public void AddFeature (Vector3 position) {


		Transform instance = Instantiate(featurePrefab);
		position.y += instance.localScale.y * 0.5f;
		instance.position = position;
	

		instance.SetParent(container, false);
	}
}