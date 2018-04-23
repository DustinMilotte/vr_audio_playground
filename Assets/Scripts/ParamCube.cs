﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour {
    public int _band;
    public float _startScale, _maxScale;
    public bool _useBuffer;
    public Color _myColor;

    Material _material;

    private void Start() {
       _material = GetComponent<MeshRenderer>().materials[0]; 
       //print(_material);
    }
	
	void Update () {
        if(_useBuffer) {
            transform.localScale = new Vector3(
                transform.localScale.x,
                (AudioPeer._audioBandBuffer[_band] * _maxScale) + _startScale,
                transform.localScale.z
            );
            _material.SetColor("_Color", _myColor);
            Color _color = new Color(AudioPeer._audioBandBuffer[_band], AudioPeer._audioBandBuffer[_band],AudioPeer._audioBandBuffer[_band]);
            // print(_color);
            _material.SetColor("_EmissionColor", _color);
        }
        if(!_useBuffer){
            transform.localScale = new Vector3(
                transform.localScale.x,
                (AudioPeer._audioBand[_band] *_maxScale) + _startScale,
                transform.localScale.z
            );
            _material.SetColor("_Color", _myColor);
            Color _color = new Color(AudioPeer._audioBand[_band], AudioPeer._audioBand[_band],AudioPeer._audioBand[_band]);
            _material.SetColor("_EmissionColor", _color); 
        }
        
	}
}
