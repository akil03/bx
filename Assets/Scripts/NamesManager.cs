using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamesManager : MonoBehaviour {

	public static NamesManager instance;

	public TextAsset namesTextFile;
    

	[HideInInspector]
	public string[] names;

	void Awake(){
		instance = this;
		names = namesTextFile.text.Split ('\n');
		enabled = false;

	}


	public string  GetRandomName(){

		int random = Random.Range (0, names.Length);
		return "Trainer "+names [random];

	}

	public string GetFakeName(){
		int random = Random.Range (0, names.Length);
		return (names [random]+Random.Range(10,999).ToString());
	}





}
