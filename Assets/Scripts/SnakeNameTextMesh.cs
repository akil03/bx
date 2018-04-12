using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNameTextMesh : MonoBehaviour {

	MeshRenderer meshRenderer;
	TextMesh text;
	public TextMesh shadow;


	public string sortingLayerName = "Default";
	public int sortingOrder = 0;



	void Awake(){
		meshRenderer = GetComponent<MeshRenderer> ();
		text = GetComponent<TextMesh> ();

		meshRenderer.sortingLayerName = sortingLayerName;
		meshRenderer.sortingOrder = sortingOrder;
			}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetColor(Color color){
		text.color = color;
	}

	public void SetText(string value){
		//value = "";

		text.text = value;
		shadow.text = value;
	}
}
