using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionField : MonoBehaviour {
	
	//public GameObject RightControllerParticleSystem;

	private ParticleSystem particles;
	private Oscillator myOsc;
	private SynthControl mySynthControl;
	private bool isPlayingParticles = false;
    private float top, bottom, front, back, left, right;

    public float noteSpacing = .2f;


	void Start () {
		myOsc = GetComponent<Oscillator>();
		mySynthControl = GetComponent<SynthControl>();
		//RightControllerParticleSystem = GameObject.Find("RightControllerParticleSystem");
		//particles = RightControllerParticleSystem.GetComponent<ParticleSystem>();
	}

    void Update() {
        top = gameObject.transform.position.y + (gameObject.transform.localScale.y / 2);
        bottom = gameObject.transform.position.y - (gameObject.transform.localScale.y / 2);
        front = gameObject.transform.position.z + (gameObject.transform.localScale.z / 2);
        back = gameObject.transform.position.z - (gameObject.transform.localScale.z / 2);
        left = gameObject.transform.position.x - (gameObject.transform.localScale.x / 2);
        right = gameObject.transform.position.x + (gameObject.transform.localScale.x / 2);
    }

	void OnTriggerStay(Collider col){
        DetermineNote(col);
        SetVolume(col);
        SetCutoffFreq(col);

        //Vector3 colPos = col.gameObject.transform.position;
        //PlayParticles ();
    }

    void DetermineNote(Collider col) {
        Vector3 colPos = col.gameObject.transform.position;
       
        float distFromBottom = colPos.y - bottom;
        int localOctave = (int)Mathf.Round(distFromBottom / noteSpacing) / (myOsc.frequencies.Length);
      
        int note = (int)(Mathf.Round(distFromBottom / noteSpacing) % (myOsc.frequencies.Length));
        print("octave: " + localOctave + " note: " + note);
       
        if (note >= 0) myOsc.PlaySound(note, localOctave);
    }

    void SetVolume(Collider col) {
        Vector3 colPos = col.gameObject.transform.position;
        //determine where collider is within the box from
        float distFromBack = colPos.z - back;
        float localPosZ = distFromBack / gameObject.transform.localScale.z;
        float clampedLocalPosZ = Mathf.Clamp(localPosZ, 0, 1f);
     

        mySynthControl.SetVolume(Mathf.Lerp(-80f, 0f, clampedLocalPosZ));
    }

    void SetCutoffFreq(Collider col) {
        Vector3 colPos = col.gameObject.transform.position;
        //determine where collider is within the box from
        float distFromLeft = colPos.x - left;
        float localPosX = distFromLeft / gameObject.transform.localScale.x;
        float clampedLocalPosX = Mathf.Clamp(localPosX, 0, 1f);
        //print(clampedLocalPosX);


        mySynthControl.SetCutoff(Mathf.Lerp(500f, 15000f, clampedLocalPosX));
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
