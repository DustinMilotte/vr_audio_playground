using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionField : MonoBehaviour {
	
	//public GameObject RightControllerParticleSystem;

	private ParticleSystem particles;
	private Oscillator myOsc;
	private SynthControl mySynthControl;
	private bool isPlayingParticles = false;
    private float bottom;
    private float top;
    public float noteSpacing = .2f;


	void Start () {
		myOsc = GetComponent<Oscillator>();
        print("freq [] length " + myOsc.frequencies.Length);
		mySynthControl = GetComponent<SynthControl>();
		//RightControllerParticleSystem = GameObject.Find("RightControllerParticleSystem");
		//particles = RightControllerParticleSystem.GetComponent<ParticleSystem>();
	}

    void Update() {
        // find the bottom of the box
        bottom = gameObject.transform.position.y - (gameObject.transform.localScale.y / 2);
        //print("bottom " + bottom);
        //print("local y scale " + this.transform.localScale.y);
        top = gameObject.transform.position.y + (gameObject.transform.localScale.y / 2);
    }

	void OnTriggerStay(Collider col){
        PlaySound(col);
		Vector3 colPos = col.gameObject.transform.position;
		//Debug.Log("Tiggered by " + col.gameObject.name + "at " +col.gameObject.transform.position);
		//if (colPos.y < 1.2) {
		//		myOsc.PlaySound (0);
		//} else if (colPos.y < 1.4) {
		//		myOsc.PlaySound (1);
		//} else if (colPos.y < 1.6) {
		//		myOsc.PlaySound (2);
		//} else if (colPos.y < 1.8) {
		//		myOsc.PlaySound (3);
		//} else if (colPos.y < 2) {
		//	myOsc.PlaySound (4);
		//} 

		mySynthControl.SetVolume(Mathf.Lerp(-80f, 0f, colPos.z));
		mySynthControl.SetCutoff(Mathf.Lerp(100f, 15000f, colPos.x));

		//PlayParticles ();
	}

    void PlaySound(Collider col) {
        Vector3 colPos = col.gameObject.transform.position;
       
        float distFromBottom = colPos.y - bottom;
        int localOctave = (int)Mathf.Round(distFromBottom / noteSpacing) / (myOsc.frequencies.Length) + 1;
      
        int note = (int)(Mathf.Round(distFromBottom / noteSpacing) % (myOsc.frequencies.Length));
        print("octave" + localOctave + "note" + note);
        //use bottom of gameObject to play sound
        if (note >= 0) myOsc.PlaySound(note, localOctave);
    }

	void OnTriggerExit(){
		myOsc.StopSound();
		//StopParticles ();
		Debug.Log("triggerexit");
	}

    void PlayParticles() {
        if(!isPlayingParticles) {
            particles.Play();
            isPlayingParticles = true;
        }
    }

    void StopParticles() {
        particles.Stop();
        isPlayingParticles = false;
    }
}
