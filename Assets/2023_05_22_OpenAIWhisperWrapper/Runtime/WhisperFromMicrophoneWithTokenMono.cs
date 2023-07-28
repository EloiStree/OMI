using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace OpenAI
{
    public class WhisperFromMicrophoneWithTokenMono : MonoBehaviour
    {
        private string [] m_tokenId;
        public string m_microphoneExactNameTarget;
        public string m_modelWhisper = "whisper-1";
        public string m_modelLanguage = "en";
        public UnityEvent m_onMessageSent;
        public UnityEvent m_onMessageFail;
        public Eloi.PrimitiveUnityEvent_String m_onTranscriptionReceived;

        private readonly string fileName = "output.wav";
        private readonly int m_maxDuration = 30;

        public  AudioClip clip;
        private OpenAIApi openai = new OpenAIApi();

        public string[] m_microhponeExactName;

        [ContextMenu("Refresh Microphone List")]
        public void RefreshMicrophoneList() {
            m_microhponeExactName = Microphone.devices;
        }

        public void SetToken(string tokenId)
        {
            m_tokenId = new string[] { tokenId }; 

        }

        public int m_microphoneIndex;
        private void Start()
        {
            var index = PlayerPrefs.GetString("user-mic-device-index");
            if(!string.IsNullOrEmpty(index.Trim()))
                index= m_microphoneExactNameTarget;
            ChangeMicrophoneIndex(index);
        }

        public  void ChangeMicrophoneIndex(string targetId)
        {
            m_microphoneExactNameTarget = targetId;
            PlayerPrefs.SetString("user-mic-device-index", m_microphoneExactNameTarget);
        }

        [ContextMenu("Start Recording")]
        public  void StartRecording()
        {
            clip = Microphone.Start(m_microphoneExactNameTarget, false, m_maxDuration, 44100);
            m_startRecording = DateTime.Now;
        }
        [ContextMenu("Stop Recording")]
        public void StopRecording() {
            AsyncEndRecording();
        }
        public DateTime m_startRecording;
        public DateTime m_stopRecording;

        public float m_audioClipLenght;
        public int m_audioClipSampleLenght;
        public double m_test;
        public int m_channelNumber;
        public AudioClip m_trimClip;
        public AudioSource m_playTrimClip;
        public bool m_dontSentToAPI;
        public int m_tokenIndex = 0;
        private async void AsyncEndRecording()
        {
            string tokenId = m_tokenId[m_tokenIndex];
            openai = new OpenAIApi(tokenId);
            m_tokenIndex++;
            if (m_tokenIndex >= m_tokenId.Length)
                m_tokenIndex = 0;

            m_stopRecording = DateTime.Now;
            float recordTime = (m_stopRecording - m_startRecording).Seconds;
            Microphone.End(m_microphoneExactNameTarget);
            m_channelNumber = clip.channels;
            float[] sample = new float[clip.samples];
            clip.GetData(sample, 0);
            int sampleNumber = (((int)recordTime) + 1) * 44100;
            m_trimClip = AudioClip.Create("TestTrim", sampleNumber, clip.channels, 44100, false);
            float[] trimClip;//= new float[sampleNumber];
            Array.Resize(ref sample, sampleNumber);
            trimClip = sample;
            //for (int i = 0; i < sample.Length && i< trimClip.Length; i++)
            //{
            //    trimClip[i] = sample[i];
            //}
            m_audioClipLenght = clip.length;
            m_audioClipSampleLenght = clip.samples;
            m_test = m_audioClipSampleLenght / 44100.0;
            m_trimClip.SetData(trimClip,0);


            m_playTrimClip.PlayOneShot(m_trimClip);

            byte[] data = SaveWav.Save(fileName, m_trimClip);
            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() { Data = data, Name = "audio.wav" },
                // File = Application.persistentDataPath + "/" + fileName,
                Model = m_modelWhisper,
                Language = m_modelLanguage
            };
            if (!m_dontSentToAPI) { 
                var res = await openai.CreateAudioTranscription(req);
                m_onTranscriptionReceived.Invoke( res.Text);
            }
        }
        private void OnDestroy()
        {
            ChangeMicrophoneIndex(m_microphoneExactNameTarget);
        }
    }
}
