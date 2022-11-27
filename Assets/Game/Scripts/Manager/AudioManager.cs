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
        //TODO ����I������
    }
    /// <summary>
    /// ���񭵼�
    /// </summary>
    /// <param name="soundName">���֦W��</param>
    /// <param name="loop">�O�_�`���A�D�I�����֪�clip �A�Y���`�� �nStop</param>
    /// <param name="volumn"></param>
    /// <param name="isBGsound"></param>
    public void PlaySound(string soundName , bool loop = false ,float volumn = 0.5f , bool isBGsound = false)
    {
        if (isBGsound)  PlayBG(LoadAudioClip(soundName) , volumn , true);
        else PlayGO(LoadAudioClip(soundName) , volumn , loop);
        
    }
    /// <summary>
    /// ����I������
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
    /// �����n��
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
    /// �Ȱ�����A�D�I������
    /// </summary>
    public void StopGOSound()
    {
        GO_audioSource.Stop();
    }
    /// <summary>
    /// �Ȱ��I������
    /// </summary>
    public void PauseBGSound()
    {
        BG_audioSource.Pause();
    }
    /// <summary>
    /// �ھڦW����oaudioclip
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private AudioClip LoadAudioClip(string name) => Resources.Load<AudioClip>(prefix + name);

    // �ثe�Τ���
    public override void UpdateManager()
    {

    }
    public override void DestroyManager()
    {
        GameObject.Destroy(BG_audioSource.gameObject);
        GameObject.Destroy(GO_audioSource.gameObject);
    }
}
