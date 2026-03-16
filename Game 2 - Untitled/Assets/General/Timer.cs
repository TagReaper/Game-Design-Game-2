using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool playOnStart;
    public float waitTime;
    public bool looping;
    public bool paused;
    public bool isEnded = true;
    public float timeRemaining = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playOnStart)
        {
            Play();
        } else
        {
            timeRemaining = waitTime;
            paused = true;
        }
    }

    public void Play()
    {
        timeRemaining = waitTime;
        paused = false;
        isEnded = false;
    }

    public void TogglePause()
    {
        if (!paused){paused = true;} else {paused = false;}
    }

    public void ToggleLoop()
    {
        if (!looping){looping = true;} else {looping = false;}
    }

    // Update is called once per frame
    void Update()
    {
        if(timeRemaining <= 0)
        {
            OnEnd();
        } else if (!paused)
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    private void OnEnd()
    {
        isEnded = true;
        timeRemaining = 0;
        if (looping)
        {
            Play();
        }
    }
}
