using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ManagerBase
{
    public AudioManager(GameFacade gameFacade)
    {
        this.facade = gameFacade;
    }



    private AudioSource BG_audioSource;
    private AudioSource GO_audioSource;

    private const string prefix = "Sounds/";
    
    public  override void InitManager()
    {
        GameObject obj1 = new GameObject("BG_AudioSource");
        GameObject obj2 = new GameObject("GO_AudioSource");
        BG_audioSource = obj1.AddComponent<AudioSource>();
        GO_audioSource = obj2.AddComponent<AudioSource>();

        obj1.transform.SetParent(facade.transform);
        obj2.transform.SetParent(facade.transform);
        //TODO 播放背景音樂
    }
    /// <summary>
    /// 播放音樂
    /// </summary>
    /// <param name="soundName">音樂名稱</param>
    /// <param name="loop">是否循環，非背景音樂的clip ，若為循環 要Stop</param>
    /// <param name="volumn"></param>
    /// <param name="isBGsound"></param>
    public void PlaySound(string soundName , bool loop = false ,float volumn = 0.5f , bool isBGsound = false)
    {
        if (isBGsound)  PlayBG(LoadAudioClip(soundName) , volumn , true);
        else PlayGO(LoadAudioClip(soundName) , volumn , loop);
        
    }
    /// <summary>
    /// 播放背景音樂
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volumn"></param>
    /// <param name="loop"></param>
    private void PlayBG(AudioClip clip , float volumn , bool loop)
    {
        BG_audioSource.Stop();
        BG_audioSource.clip = clip;
        BG_audioSource.loop = loop;
        BG_audioSource.volume = volumn;
        BG_audioSource.Play();
    }
    /// <summary>
    /// 播放聲音
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volumn"></param>
    /// <param name="loop"></param>
    private void PlayGO(AudioClip clip , float volumn , bool loop)
    {
        if (loop)
        {
            GO_audioSource.volume = volumn;
            GO_audioSource.loop = loop;
            GO_audioSource.clip = clip;
            GO_audioSource.Play();
        }
        else
        {
            GO_audioSource.PlayOneShot(clip, 0.5f);
        }
    }
    /// <summary>
    /// 暫停播放，非背景音樂
    /// </summary>
    public void StopGOSound()
    {
        GO_audioSource.Stop();
    }
    /// <summary>
    /// 暫停背景音樂
    /// </summary>
    public void PauseBGSound()
    {
        BG_audioSource.Pause();
    }
    /// <summary>
    /// 根據名稱獲得audioclip
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private AudioClip LoadAudioClip(string name) => Resources.Load<AudioClip>(prefix + name);

    // 目前用不到
    public override void UpdateManager()
    {

    }
    public override void DestroyManager()
    {
        GameObject.Destroy(BG_audioSource.gameObject);
        GameObject.Destroy(GO_audioSource.gameObject);
    }
}
