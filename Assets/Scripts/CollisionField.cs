using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionField : MonoBehaviour {

	public Oscillator leftOsc;
    public Oscillator rightOsc;
    public SynthControl leftSynthControl;
    public SynthControl rightSynthControl;
    private bool playingSoundLeft = false;
    private bool playingSoundRight = false;
    private float bottom, back, left;
    public float noteSpacing = .2f;

    void Update() {
        bottom = gameObject.transform.position.y - (gameObject.transform.localScale.y / 2);
        back = gameObject.transform.position.z - (gameObject.transform.localScale.z / 2);
        left = gameObject.transform.position.x - (gameObject.transform.localScale.x / 2);
    }

	void OnTriggerStay(Collider col) {
        print(col.gameObject.name);
        if(Input.GetAxis("TriggerLeft") > 0 && col.gameObject.name == "LeftController") {
            playingSoundLeft = true;
            PlaySoundLeft(col);
        }

        if(Input.GetAxis("TriggerRight") > 0 && col.gameObject.name == "RightController") {
            playingSoundRight = true;
            PlaySoundRight(col);
        }

        if(Input.GetAxis("TriggerLeft") == 0 && playingSoundLeft) {
            leftOsc.StopSound();
            playingSoundLeft = false;
        }

        if(Input.GetAxis("TriggerRight") == 0 && playingSoundRight) {
            rightOsc.StopSound();
            playingSoundRight = false;
        }
    }

    void PlaySoundLeft(Collider col) {
        DetermineNote(col, leftOsc);
        SetVolume(col, leftSynthControl);
        SetCutoffFreq(col, leftSynthControl);
    }

    void PlaySoundRight(Collider col) {
        DetermineNote(col, rightOsc);
        SetVolume(col, rightSynthControl);
        SetCutoffFreq(col, rightSynthControl);
    }

    void DetermineNote(Collider col, Oscillator osc) {
        Vector3 colPos = col.gameObject.transform.position;
        float distFromBottom = colPos.y - bottom;
        int localOctave = (int)Mathf.Round(distFromBottom / noteSpacing) / (osc.frequencies.Length);
        int note = (int)(Mathf.Round(distFromBottom / noteSpacing) % (osc.frequencies.Length));
        print("octave: " + localOctave + " note: " + note);

        if(note >= 0) {
            osc.PlaySound(note, localOctave);
        }
    }

    void SetVolume(Collider col, SynthControl sc) {
        Vector3 colPos = col.gameObject.transform.position;
        //determine where collider is within the box from
        float distFromBack = colPos.z - back;
        float localPosZ = distFromBack / gameObject.transform.localScale.z;
        float clampedLocalPosZ = Mathf.Clamp(localPosZ, 0, 1f);

        sc.SetVolume(Mathf.Lerp(-80f, 0f, clampedLocalPosZ));
    }

    void SetCutoffFreq(Collider col, SynthControl sc) {
        Vector3 colPos = col.gameObject.transform.position;
        //determine where collider is within the box from
        float distFromLeft = colPos.x - left;
        float localPosX = distFromLeft / gameObject.transform.localScale.x;
        float clampedLocalPosX = Mathf.Clamp(localPosX, 0, 1f);
        //print(clampedLocalPosX);
        sc.SetCutoff(Mathf.Lerp(500f, 15000f, clampedLocalPosX));
    }

    void OnTriggerExit(Collider col){
        if (col.gameObject.name == "LeftController") {
            leftOsc.StopSound();
        }
        if(col.gameObject.name == "RightController") {
            rightOsc.StopSound();
        }

        //StopParticles ();
        Debug.Log("triggerexit");
	}
}
