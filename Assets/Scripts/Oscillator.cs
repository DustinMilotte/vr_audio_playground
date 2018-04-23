using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour {

	public double frequency = 440.0;
	public float gain;
	public float volume;
	public float[] frequencies;
	public int thisFreq;
	public enum Waveform {sine, square};
	public Waveform myWave;

	private double increment;
	private double phase;
	private double samplingFrequency = 48000.0;



	void Start()
	{
		frequencies = new float[8];
		frequencies [0] = 146.83f;
		frequencies [1] = 174.61f;
		frequencies [2] = 195.99f;
		frequencies [3] = 220f;
		frequencies [4] = 261.62f;
		frequencies [5] = 740;
		frequencies [6] = 831;
		frequencies [7] = 880;
	}

	public void PlaySound (int note)
	{
		gain = volume;
		frequency = frequencies [note];
	}


	public void StopSound(){
		gain = 0f;
	}

	void OnAudioFilterRead(float [] data, int channels)
	{
		increment = frequency * 2.0 * Mathf.PI / samplingFrequency;

		for (int i = 0; i < data.Length; i += channels) {
			phase += increment;
			if (myWave == Waveform.sine) { 
				data [i] = (float)(gain * Mathf.Sin ((float)phase));
			} else if (myWave == Waveform.square) {
				if (gain * Mathf.Sin ((float)phase) >= 0 * gain) {
					data [i] = (float)gain * 0.6f;
				} else {
					data [i] = (-(float)gain) * 0.6f;
				}
			}
			if (channels == 2) {
				data [i + 1] = data [i];
			}

			if (phase > (Mathf.PI * 2)) {
				phase = 0.0;
			}
		}
	}

}
