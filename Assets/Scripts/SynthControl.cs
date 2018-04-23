using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SynthControl : MonoBehaviour {

	public AudioMixer synthMixer;

	public void SetVolume(float vol){
		synthMixer.SetFloat ("synthVol", vol);
	}

	public void SetCutoff(float cutoff){
		synthMixer.SetFloat ("synthLPCutoff", cutoff);
	}
}
