using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SynthControl : MonoBehaviour {

	public AudioMixer synthMixer;
    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetVolume(float vol){
        print("vol " + vol);
		synthMixer.SetFloat ("synthVol", vol);
	}

	public void SetCutoff(float cutoff){
		synthMixer.SetFloat ("synthLPCutoff", cutoff);
	}

    public void SetPan(float pan) {
        audioSource.panStereo = pan;
    }
}
