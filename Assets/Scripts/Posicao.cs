﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Posicao : MonoBehaviour {
	public GameObject peca;
	// Use this for initialization
	void Start () {
		if(peca){
			peca.transform.position = gameObject.transform.position;
		}
	}
}