using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour {
    public int band;
    public float startScale, maxScale;
    public bool useBuffer;
    public Color myColor;

    Material material;

    private void Start() {
       material = GetComponent<MeshRenderer>().materials[0]; 
       //print(material);
    }
	
	void Update () {
        if(useBuffer) {
            transform.localScale = new Vector3(
                transform.localScale.x,
                (AudioVisualizer.audioBandBuffer[band] * maxScale) + startScale,
                transform.localScale.z
            );
            material.SetColor("Color", myColor);
            Color color = new Color(AudioVisualizer.audioBandBuffer[band], AudioVisualizer.audioBandBuffer[band],AudioVisualizer.audioBandBuffer[band]);
            // print(color);
            material.SetColor("EmissionColor", color);
        }
        if(!useBuffer){
            transform.localScale = new Vector3(
                transform.localScale.x,
                (AudioVisualizer.audioBand[band] *maxScale) + startScale,
                transform.localScale.z
            );
            material.SetColor("Color", myColor);
            Color color = new Color(AudioVisualizer.audioBand[band], AudioVisualizer.audioBand[band],AudioVisualizer.audioBand[band]);
            material.SetColor("EmissionColor", color); 
        }
        
	}
}
