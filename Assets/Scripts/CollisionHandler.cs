using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
   [SerializeField] float levelLoadDelay = 1;
   [SerializeField] AudioClip crashSound;
   [SerializeField] AudioClip successSound;

   [SerializeField] ParticleSystem successParticles;
   [SerializeField] ParticleSystem crashParticles;


    AudioSource myAudioSource;

    bool isTransitioning = false;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.Stop();

    }


    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning == true)
        {
            return;
        }
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartNextLevelSequence();
                break;
            default:
                StartCrashSequence();
                break;
            
        }
    }

    void StartCrashSequence()
    {
        // to do particle effect upon crash
        isTransitioning = true;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(crashSound);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel",levelLoadDelay);
    }

    void StartNextLevelSequence()
    {
        //to do position of the player freezes

        isTransitioning = true;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(successSound);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel",levelLoadDelay);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void ReloadLevel()
    {
       int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    
}
