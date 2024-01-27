using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnScene : MonoBehaviour
{
    public string sceneName;
    public bool useExitTransition = false;
    public bool playAudioBeforeLoad = false;
    public LoadSceneMode loadMode = LoadSceneMode.Single;

    public void Spawn()
    {
        if (sceneName != "")
        {
            if (useExitTransition)
            {
                // SceneTransition.ChangeScene(sceneName);
            }
            else
            {
                if (playAudioBeforeLoad)
                {
                    StartCoroutine(PlaySoundThenSpawn());
                }
                else
                {
                    SceneManager.LoadScene(sceneName, loadMode);
                }
            }
        }
    }


    public void SpawnUnique()
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            Spawn();
        }
    }

    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    IEnumerator PlaySoundThenSpawn()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip, 0.5f);
        //Wait until clip finish playing
        yield return new WaitForSeconds(audioSource.clip.length - 2f);
        SceneManager.LoadScene(sceneName, loadMode);
    }

    public void SetSceneName(string name)
    {
        sceneName = name;
    }

}
