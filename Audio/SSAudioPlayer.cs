using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SSAudioPlayer : MonoBehaviour
{
    /// <summary>
    /// 声音播放模式
    /// </summary>
    public enum Mode
    {
        Once = 0,
        Loop = 1,
    }

    [System.Serializable]
    public class AudioData
    {
        internal AudioSource audioSource;
        /// <summary>
        /// 优先播放clip数组中的声音,如果clip数组长度为0,则默认播放AudioSource的clip声音
        /// </summary>
        public AudioClip[] clipArray;
        /// <summary>
        /// AudioClip数组的索引
        /// </summary>
        int IndexClip = 0;

        internal void Init(AudioSource audioSource)
        {
            if (audioSource != null)
            {
                if (clipArray.Length > 1)
                {
                    //clip数组长度大于1时关闭audioSource的循环播放开关
                    audioSource.loop = false;
                }
                audioSource.playOnAwake = false;
                audioSource.Stop();
                this.audioSource = audioSource;
            }
        }

        void Reset()
        {
            IndexClip = 0;
            IsPlayNextAudioClip = false;
        }

        AudioClip GetAudioClip()
        {
            AudioClip clip = null;
            if (clipArray.Length > 0)
            {
                clip = clipArray[IndexClip];
                IndexClip++;
                if (IndexClip >= clipArray.Length)
                {
                    IndexClip = 0;
                }
            }
            else
            {
                if (audioSource != null)
                {
                    clip = audioSource.clip;
                }
            }
            return clip;
        }
        
        internal void Play(Mode mode)
        {
            if (audioSource != null)
            {
                AudioClip clip = GetAudioClip();
                if (clip != null)
                {
                    audioSource.loop = mode == Mode.Once ? false : true;
                    audioSource.clip = clip;
                    if (audioSource.isPlaying == true)
                    {
                        audioSource.Stop();
                    }
                    audioSource.Play();
                }
            }
        }

        internal void Stop()
        {
            if (audioSource != null && audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
            Reset();
        }

        /// <summary>
        /// 播放下一首音乐
        /// </summary>
        void PlayNextAudio()
        {
            Play(Mode.Once);
        }

        bool IsPlayNextAudioClip = false;
        internal IEnumerator CheckPlayNextAudioClip()
        {
            if (audioSource == null)
            {
                yield break;
            }
            IsPlayNextAudioClip = true;

            do
            {
                //SSDebug.Log("time ====== " + Time.time);
                if (audioSource.isPlaying == false)
                {
                    //播放下一个音乐
                    PlayNextAudio();
                }
                yield return new WaitForSeconds(0.5f);
            } while (IsPlayNextAudioClip == true);
        }

        internal bool GetIsStartCheckPlayNextAudioClip()
        {
            return clipArray.Length > 1 ? true : false;
        }

        /// <summary>
        /// 获取是否允许播放声音
        /// </summary>
        internal bool GetIsCanPlay()
        {
            if (audioSource == null)
            {
                return false;
            }

            bool isPlay = false;
            if (clipArray.Length > 0)
            {
                isPlay = true;
            }
            else
            {
                if (audioSource.clip != null)
                {
                    isPlay = true;
                }
            }
            return isPlay;
        }
    }
    public AudioData m_AudioData;
    bool IsPlay = false;

    void Awake()
    {
        if (m_AudioData != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            m_AudioData.Init(audioSource);
        }
    }

    internal void Play(Mode mode)
    {
        if (m_AudioData == null)
        {
            return;
        }

        if (m_AudioData.GetIsCanPlay() == false)
        {
            return;
        }

        if (m_AudioData.GetIsStartCheckPlayNextAudioClip() == true)
        {
            if (IsPlay == false)
            {
                IsPlay = true;
                //数组长度大于1时循环播放clip数组的声音
                StartCoroutine(m_AudioData.CheckPlayNextAudioClip());
            }
        }
        else
        {
            m_AudioData.Play(mode);
        }
    }

    internal void Stop()
    {
        if (m_AudioData == null)
        {
            return;
        }

        if (m_AudioData.GetIsCanPlay() == false)
        {
            return;
        }
        IsPlay = false;
        m_AudioData.Stop();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.P))
    //    {
    //        Play(Mode.Once);
    //    }

    //    if (Input.GetKeyUp(KeyCode.L))
    //    {
    //        Stop();
    //    }
    //}
}
