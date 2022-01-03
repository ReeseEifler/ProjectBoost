using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelay = 1f;
    [SerializeField] AudioClip crashSound, successSound;
    [SerializeField] ParticleSystem crashParticles, successParticles;
    AudioSource audioSource;
    Movement movement;

    bool isTransitioning = false, isLanded = false;
    Vector3 lastLandingPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        //if (isTransitioning && !isLanded) TestLanding();   
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning) return;
        switch (other.gameObject.tag)
        {
            case "Respawn":
                break;
            case "Finish":
                StartLandingTests();
                break;
            case "Fuel":
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        crashParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        Invoke("ReloadLevel", loadDelay);
    }

    void StartLevelEndSequence()
    {
        isLanded = true;
        successParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        Invoke("LoadNextLevel", loadDelay);
    }

    void StartLandingTests()
    {
        isTransitioning = true;
        movement.enabled = false;
        Invoke("TestLanding", 0.2f);
    }

    void TestLanding()
    {
        if (lastLandingPosition == null)
            lastLandingPosition = transform.position;
        else
        {
            if (transform.position == lastLandingPosition)
            {
                if (Mathf.Abs(transform.rotation.eulerAngles.x) < 45) 
                    StartLevelEndSequence();
                else 
                    StartCrashSequence();
            }
            else
            {
                lastLandingPosition = transform.position;
                Invoke("TestLanding", 0.2f);
            }
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex,
            nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) nextSceneIndex = 0;
        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
