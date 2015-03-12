using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextList : MonoBehaviour {

	public List <string> texts = new List<string>();

	void Start () { } 

	public virtual void addText() {}
}
