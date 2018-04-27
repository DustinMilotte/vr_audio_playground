using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnamplitude : MonoBehaviour {

	public float startScale, maxScale;
	public bool useBuffer;
	Material material;
	public float red, green, blue;
    public AudioVisualizer audioVisualizer;


	// Use this for initialization
	void Start () {
		material = GetComponent<MeshRenderer>().materials[0];
	}
	
	// Update is called once per frame
	void Update () {
        if(useBuffer){
            transform.localScale = new Vector3(
                (audioVisualizer.amplitudeBuffer * maxScale) + startScale,
                (audioVisualizer.amplitudeBuffer * maxScale) + startScale,
                (audioVisualizer.amplitudeBuffer * maxScale) + startScale
            );
            // material.SetColor("Color", myColor);
            Color color = new Color(red * audioVisualizer.amplitudeBuffer, green * audioVisualizer.amplitude, blue * audioVisualizer.amplitude);
            material.SetColor("EmissionColor", color); 
        }
		
		if(!useBuffer) {
            transform.localScale = new Vector3(
                (audioVisualizer.amplitude * maxScale) + startScale,
                (audioVisualizer.amplitude * maxScale) + startScale,
                (audioVisualizer.amplitude * maxScale) + startScale
            );
            // material.SetColor("Color", myColor);
            Color color = new Color(red * audioVisualizer.amplitude, green * audioVisualizer.amplitude, blue * audioVisualizer.amplitude);
            // print(color);
            material.SetColor("EmissionColor", color);
        }	
	}
}
