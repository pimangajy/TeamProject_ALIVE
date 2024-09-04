using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public AudioSource Source2D;    // 2d ����
    public AudioSource Source3D;    // 3d ����

    [Header("BGM")]
    public AudioClip BGMClips;

    [Header("��ưŬ��")]
    public AudioClip btnClick;

    [Header("�÷��̾� ����")]
    public List<AudioClip> playerSoundList;
    private Dictionary<string, AudioClip> playerSoundDict;

    [Header("���� ����")]
    public List<AudioClip> zombieSoundList;
    private Dictionary<string, AudioClip> zombieSoundDict;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;    // OnSceneLoaded �Լ� �߰�
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
        // 2D ���� �ҽ� ����
        Source2D.spatialBlend = 0f;  // 2D ����� ����
        Source2D.loop = false;
        Source2D.playOnAwake = false; 

        // 3D ���� �ҽ� ����
        Source3D.spatialBlend = 1.0f;  // 3D ����� ����
        Source3D.loop = false;
        Source3D.rolloffMode = AudioRolloffMode.Logarithmic;
        Source3D.playOnAwake = false; 
    }

    private Dictionary<string, AudioClip> InitSoundDict(List<AudioClip> soundList)     // ��ųʸ� ����
    {
        Dictionary<string, AudioClip> soundDict = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in soundList)
        {
            soundDict.Add(clip.name, clip);
        }
        return soundDict;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� �ε�ɶ� ����
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

    private void PlayBGM(int sceneIndex)         // bgm�� 0�� 1�� �������� ����ɼ��ְ�
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

    public void BtnClick()             // ��ưŬ���� ���� �Լ�
    {
        Source2D.PlayOneShot(btnClick);
    }

    public void SetVolume(float vol)   // ���� ����
    {
        Source2D.volume = vol;
        Source3D.volume = vol;
    }

    public void PlayerSound(string soundName, Vector3 pos) // �÷��̾� ���� ���
    {
        if (playerSoundDict.ContainsKey(soundName))
        {
            AudioSource.PlayClipAtPoint(playerSoundDict[soundName], pos, Source3D.volume);
        }
    }

    public void ZombieSound(string soundName, Vector3 pos) // ���� ���� ���
    {
        if (zombieSoundDict.ContainsKey(soundName))
        {
            AudioSource.PlayClipAtPoint(zombieSoundDict[soundName], pos, Source3D.volume);
        }
    }
}
