using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Column{

	public List<GroundPiece> groundPieces;
	public int indexInColumnsList;

	public Column(){
		groundPieces = new List<GroundPiece> ();

	}

}
