using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Row{

	public List<GroundPiece> groundPieces;
	public int indexInRowsList;

	public Row(){
		groundPieces = new List<GroundPiece> ();
	}

}
