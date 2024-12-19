using System.Collections;
using UnityEngine;

namespace Player
{
    public class AudioSystem : MonoBehaviour
    {

        //========================================================
        //                       Properties
        //========================================================
        // Components
        Rigidbody2D rb;
        AudioSource audioSource;

        // Script Components
        EventController eventController;
        Stats playerStats;

        // Audio Clips
        public AudioClip idleSound;
        public AudioClip drivingSound;
        public AudioClip brakingSound;
        public AudioClip modelCollisionSound;

        // Helper states
        public float maxAudioVolume = 0.5f;
        public float durationToStop = 3f;
        public float bufferTime = 5f; // Buffer time before stopping
        bool isDriving = false;
        bool isBraking = false;
        bool stopRequested = false;
        string lastAction = "idle"; // driving, braking, idle


        //========================================================
        //                     Mono Methods
        //========================================================

        void Awake()
        {
            // Components
            rb = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();

            // Script Components
            eventController = GetComponent<EventController>();
            playerStats = GetComponent<Stats>();
        }

        void Start()
        {
            eventController.move += Driving;
            eventController.movementNotOccuring += StopDriving;
            eventController.brake += Brake;
            eventController.modelCollided += ModelCollided;
            eventController.lose += StopDriving;
            eventController.finish += StopDriving;
        }

        void Update()
        {
        }


        //========================================================
        //                       Methods
        //========================================================

        void Driving(bool isGrounded, bool isOnRamp, bool isOnDune, bool isPowerUp, bool canRotate)
        {
            // Reset
            StopAllCoroutines();
            audioSource.volume = maxAudioVolume;

            if (isDriving)
            {
                stopRequested = false; // Cancel any stop request
                return;
            }
            isDriving = true;
            isBraking = false;
            audioSource.clip = drivingSound;
            lastAction = "driving";
            audioSource.Play();
        }

        void Brake(bool isGrounded, bool isOnRamp, bool isOnDune, bool isPowerUp, bool canRotate)
        {
            if(lastAction != "braking" )
            {
                // Ignoring the if below
            }
            else if (!isDriving || isBraking )
            {
                return;
            }
            // stopRequested = true;
            // StartCoroutine(BufferBeforeStop());
            // if (!isDriving || isBraking)
            // {
            //     return;
            // }
            isBraking = true;
            audioSource.clip = brakingSound;
            lastAction = "braking";
            audioSource.Play();
        }

        void StopDriving()
        {
            if (!isDriving)
            {
                return;
            }
            stopRequested = true;
            StartCoroutine(BufferBeforeStop());
        }

        void ModelCollided()
        {
            // if (isDriving)
            // {
            //     audioSource.Stop();
            //     isDriving = false;
            // }
            // audioSource.PlayOneShot(modelCollisionSound);

        }

        IEnumerator BufferBeforeStop()
        {
            yield return new WaitForSeconds(bufferTime);

            if (stopRequested)
            {
                isDriving = false;
                isBraking = false;
                // Lower volume incrementally then stop
                StartCoroutine(LowerAudioThenStop());
            }
        }

        IEnumerator LowerAudioThenStop()
        {
            float startVolume = audioSource.volume;
            float step = startVolume / (durationToStop / 0.1f);

            while (audioSource.volume > 0)
            {
                audioSource.volume -= step;
                yield return new WaitForSeconds(0.1f);
            }
            audioSource.Stop();
            // Go back to full volume
            audioSource.volume = maxAudioVolume;
        }

    }
}