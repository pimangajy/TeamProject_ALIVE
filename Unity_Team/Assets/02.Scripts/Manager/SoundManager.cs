using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public AudioSource Source2D;    // 2d 사운드
    public AudioSource Source3D;    // 3d 사운드

    [Header("BGM")]
    public AudioClip BGMClips;

    [Header("버튼클릭")]
    public AudioClip btnClick;

    [Header("플레이어 사운드")]
    public List<AudioClip> playerSoundList;
    private Dictionary<string, AudioClip> playerSoundDict;

    [Header("좀비 사운드")]
    public List<AudioClip> zombieSoundList;
    private Dictionary<string, AudioClip> zombieSoundDict;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;    // OnSceneLoaded 함수 추가
            DontDestroyOnLoad(this.gameObject);
            playerSoundDict = InitSoundDict(playerSoundList);
            zombieSoundDict = InitSoundDict(zombieSoundList);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitAudioSources();
        PlayBGM(SceneManager.GetActiveScene().buildIndex);
    }

    private void InitAudioSources()
    {
        // 2D 사운드 소스 설정
        Source2D.spatialBlend = 0f;  // 2D 사운드로 설정
        Source2D.loop = false;
        Source2D.playOnAwake = false; 

        // 3D 사운드 소스 설정
        Source3D.spatialBlend = 1.0f;  // 3D 사운드로 설정
        Source3D.loop = false;
        Source3D.rolloffMode = AudioRolloffMode.Logarithmic;
        Source3D.playOnAwake = false; 
    }

    private Dictionary<string, AudioClip> InitSoundDict(List<AudioClip> soundList)     // 딕셔너리 설정
    {
        Dictionary<string, AudioClip> soundDict = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in soundList)
        {
            soundDict.Add(clip.name, clip);
        }
        return soundDict;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 로드될때 실행
    {
        if (scene.buildIndex == 0 || scene.buildIndex == 1)
        {

            if (!Source2D.isPlaying)
            {
                Source2D.clip = BGMClips;
                Source2D.loop = true;
                Source2D.Play();
            }
        }
        else
        {
            Source2D.Stop();
            Source2D.clip = null;
            Source2D.loop = false;
        }
    }

    private void PlayBGM(int sceneIndex)         // bgm이 0과 1번 씬에서만 실행될수있게
    {
        if (sceneIndex == 0 || sceneIndex == 1)
        {
            
            if (!Source2D.isPlaying)
            {
                Source2D.clip = BGMClips;
                Source2D.loop = true;
                Source2D.Play();
            }
        }
        else
        {
            Source2D.Stop();
            Source2D.clip = null;
            Source2D.loop = false;
        }

    }

    public void BtnClick()             // 버튼클릭시 사운드 함수
    {
        Source2D.PlayOneShot(btnClick);
    }

    public void SetVolume(float vol)   // 볼륨 조절
    {
        Source2D.volume = vol;
        Source3D.volume = vol;
    }

    public void PlayerSound(string soundName, Vector3 pos) // 플레이어 사운드 재생
    {
        if (playerSoundDict.ContainsKey(soundName))
        {
            AudioSource.PlayClipAtPoint(playerSoundDict[soundName], pos, Source3D.volume);
        }
    }

    public void ZombieSound(string soundName, Vector3 pos) // 좀비 사운드 재생
    {
        if (zombieSoundDict.ContainsKey(soundName))
        {
            AudioSource.PlayClipAtPoint(zombieSoundDict[soundName], pos, Source3D.volume);
        }
    }
}
