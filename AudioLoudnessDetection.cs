using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
     // states the size of the audio clip that is used to detect the loudness of a clip
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    // function to return a float representing how loud the microphone is using the GetloudnessFromAudioClip Function
    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    public void MicrophoneToAudioClip()
    {
        //get the first microphone in device list
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);


    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        //calcualtes where to start measuring the clip 
        int startPosition = clipPosition - sampleWindow;
    //goes back to start of the clip if start position is negative
    if(startPosition < 0)
        return 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);


        //compute loudness
        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }

    // Start is called before the first frame update
    void Start()
    {
        MicrophoneToAudioClip();
    }


}
