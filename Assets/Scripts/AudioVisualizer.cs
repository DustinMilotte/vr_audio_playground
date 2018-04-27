using UnityEngine;

public class AudioVisualizer : MonoBehaviour {
    #region Variables
    public AudioSource audioSource;
    private float[] samplesLeft = new float[512];
    private float[] samplesRight = new float[512];

    private float[] freqBand = new float[8];
    private float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];
    private float[] freqBandsHighest = new float[8];

    // audio band 64 variables
    private float[] freqBand64 = new float[64];
    private float[] bandBuffer64 = new float[64];
    private float[] bufferDecrease64 = new float[64];
    private float[] freqBandsHighest64 = new float[64];

    [HideInInspector]
    public static float[] audioBand, audioBandBuffer;
    [HideInInspector]
    public static float[] audioBand64, audioBandBuffer64;

    [HideInInspector]
    public float amplitude, amplitudeBuffer; //todo remove static
    public float audioProfile;
    private float amplitudeHighest;

    public enum Channel { stereo, left, right };
    public Channel channel = new Channel();
    #endregion

    void Start() {
        audioBand = new float[8];
        audioBandBuffer = new float[8];
        audioBand64 = new float[64];
        audioBandBuffer64 = new float[64];

        //_audioSource = GetComponent<AudioSource>();
        AudioProfile(audioProfile);
    }

    void Update() {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        //MakeFrequencyBands64();
        BandBuffer();
        //BandBuffer64();
        CreateAudioBands();
        //CreateAudioBands64();
        GetAmplitude();
    }

    void GetSpectrumAudioSource() {
        audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
    }

    void MakeFrequencyBands() {
        int count = 0;

        for(int i = 0; i < 8; i++) {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(sampleCount == 7) {
                sampleCount += 2;
            }
            for(int j = 0; j < sampleCount; j++) {
                if(channel == Channel.stereo) {
                    average += samplesLeft[count] + samplesRight[count] * (count * 1);
                    count++;
                }
                if(channel == Channel.left) {
                    average += samplesLeft[count] * (count * 1);
                    count++;
                }
                if(channel == Channel.right) {
                    average += samplesRight[count] * (count * 1);
                    count++;
                }

            }
            average /= count;
            freqBand[i] = average * 10;
        }
    }

    void MakeFrequencyBands64() {
        int count = 0;
        int sampleCount = 1;
        int power = 0;


        for(int i = 0; i < 64; i++) {
            float average = 0;
            if(i == 16 || i == 32 || i == 40 || i == 48 || i == 56) {
                power++;
                sampleCount = (int)Mathf.Pow(2, i) * 2;
                if(power == 3) {
                    sampleCount -= 2;
                }
            }

            //if(sampleCount == 7) {
            //    sampleCount += 2;
            //}

            for(int j = 0; j < sampleCount; j++) {
                if(channel == Channel.stereo) {
                    average += samplesLeft[count] + samplesRight[count] * (count * 1);
                    count++;
                }
                if(channel == Channel.left) {
                    average += samplesLeft[count] * (count * 1);
                    count++;
                }
                if(channel == Channel.right) {
                    average += samplesRight[count] * (count * 1);
                    count++;
                }

            }
            average /= count;
            freqBand64[i] = average * 80;
        }
    }

    void BandBuffer() {
        for(int g = 0; g < 8; g++) {
            if(freqBand[g] > bandBuffer[g]) {
                bandBuffer[g] = freqBand[g];
                bufferDecrease[g] = 0.005f;
            }
            if(freqBand[g] < bandBuffer[g]) {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void BandBuffer64() {
        for(int g = 0; g < 64; g++) {
            if(freqBand64[g] > bandBuffer64[g]) {
                bandBuffer64[g] = freqBand64[g];
                bufferDecrease64[g] = 0.005f;
            }
            if(freqBand64[g] < bandBuffer64[g]) {
                bandBuffer64[g] -= bufferDecrease64[g];
                bufferDecrease64[g] *= 1.2f;
            }
        }
    }

    void CreateAudioBands() {
        for(int i = 0; i < 8; i++) {
            if(freqBand[i] > freqBandsHighest[i]) {
                freqBandsHighest[i] = freqBand[i];
            }
            audioBand[i] = (freqBand[i] / freqBandsHighest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandsHighest[i]);

        }
    }

    void CreateAudioBands64() {
        for(int i = 0; i < 64; i++) {
            if(freqBand64[i] > freqBandsHighest64[i]) {
                freqBandsHighest64[i] = freqBand64[i];
            }
            audioBand64[i] = (freqBand64[i] / freqBandsHighest64[i]);
            audioBandBuffer64[i] = (bandBuffer64[i] / freqBandsHighest64[i]);

        }
    }

    void AudioProfile(float audioProfile) {
        for(int i = 0; i < 8; i++) {
            freqBandsHighest[i] = audioProfile;
        }
    }

    void GetAmplitude() {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;
        for(int i = 0; i < 8; i++) {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }
        if(currentAmplitude > amplitudeHighest) {
            amplitudeHighest = currentAmplitude;
        }
        amplitude = currentAmplitude / amplitudeHighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }
}
