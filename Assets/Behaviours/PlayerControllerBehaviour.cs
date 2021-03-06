﻿using Assets;
using Assets.Behaviours;
using Assets.Extensions;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

class PlayerControllerBehaviour : MonoBehaviour
{
    public float jumpVelocity = 6f;
    public float runSpeed = 0.1f;
    public float walkSpeed = 0.1f;
    public float jumpGracePeriod = 0.1f;
    public GameObject DeadPlayer = null;
    public AudioClip JumpAudio = null;
    public AudioClip LandAudio = null;
    public AudioClip FootstepsAudio = null;
    public AudioClip DeathAudio = null;

    const float _minSeparationDistance = 0.1f;
    private readonly Lazy<Rigidbody2D> _rigidbody;
    private readonly Lazy<Collider2D> _collider;
    private readonly Lazy<PhysicsObject> _physicsObject;
    private readonly Lazy<GameObject> _prompt;
    private readonly Lazy<Tilemap[]> _tileMaps;
    private readonly Lazy<LevelControllerBehaviour> _levelController;
    private readonly Lazy<List<AudioSource>> _audioSources;

    private bool _jumpPending = false;
    private float _lastGroundedTime = -999;
    private bool _jumpedSinceLastGrounded = false;
    private bool _quitting = false;
    private bool _wasGrounded = true;

    // static
    static List<PlayerControllerBehaviour> _allPlayers = new List<PlayerControllerBehaviour>();

    public static PlayerControllerBehaviour FirstPlayer() => _allPlayers.FirstOrDefault();

    public static IEnumerable<PlayerControllerBehaviour> AllPlayers() => _allPlayers;

    public PlayerControllerBehaviour()
    {
        _rigidbody = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
        _collider = new Lazy<Collider2D>(GetComponent<Collider2D>);
        _physicsObject = new Lazy<PhysicsObject>(GetComponent<PhysicsObject>);
        _prompt = new Lazy<GameObject>(() => transform.Find("Prompt").gameObject);
        _tileMaps = new Lazy<Tilemap[]>(FindObjectsOfType<Tilemap>);
        _levelController = new Lazy<LevelControllerBehaviour>(() => GameObject.FindObjectOfType<LevelControllerBehaviour>());
        _audioSources = new Lazy<List<AudioSource>>(() => GetComponents<AudioSource>().ToList());
    }

    // end-static

    private void Awake()
    {
        _allPlayers.Add(this);
    }

    private void Start()
    {
        _physicsObject.Value.PositionOnGround();
    }

    private void OnApplicationQuit()
    {
        _quitting = true;        
    }

    private void OnDestroy()
    {
        _allPlayers.Remove(this);
    }

    private void PlaySound(AudioClip clip, int channel)
    {
        AudioSource audioSource = _audioSources.Value[channel];
        if (!audioSource.isPlaying || audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void StopSound(AudioClip clip, int channel)
    {
        AudioSource audioSource = _audioSources.Value[channel];
        if (audioSource.isPlaying && audioSource.clip == clip)
        {
            audioSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var interactable = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().FirstOrDefault(x => x.CanInteractWith(this));
        _prompt.Value.SetActive(interactable != null);
        if (interactable != null
            && Input.GetKeyDown(KeyCode.E)
            && !_levelController.Value.IsTimeStopped)
        {
            _prompt.Value.SetActive(false);
            interactable.InteractWith(this);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            && (_physicsObject.Value.Grounded || Time.fixedTime - _lastGroundedTime < jumpGracePeriod)
            && !_jumpedSinceLastGrounded)
        {
            _jumpPending = true;
        }

        if (_tileMaps.Value.All(x => x.localBounds.SqrDistance(transform.position) > 100))
        {
            KillPlayer();
        }

        if (!_wasGrounded && _physicsObject.Value.Grounded)
        {
            PlaySound(LandAudio, 1);
        }

        _wasGrounded = _physicsObject.Value.Grounded;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _physicsObject.Value.WalkIntent = -runSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) )
        {
            _physicsObject.Value.WalkIntent = runSpeed;
        }
        else
        {
            _physicsObject.Value.WalkIntent = 0;
        }

        if (!_physicsObject.Value.Grounded || Mathf.Abs(_physicsObject.Value.MovementLastFrame.x) < 0.01)
        {
            StopFootsteps();
        }
        else
        {
            PlaySound(FootstepsAudio, 0);
        }

        if (_jumpPending && jumpVelocity > 0)
        {
            _physicsObject.Value.YVelocity = jumpVelocity;
            _jumpPending = false;
            _jumpedSinceLastGrounded = true;
            PlaySound(JumpAudio, 1);
        }
        else if(_physicsObject.Value.Grounded)
        {
            _lastGroundedTime = Time.fixedTime;
            _jumpedSinceLastGrounded = false;
        }
    }

    public void StopFootsteps()
    {
        StopSound(FootstepsAudio, 0);
    }

    private void OnDisable()
    {
        _physicsObject.Value.WalkIntent = 0;
    }

    public void KillPlayer()
    {
        if (!_quitting && DeadPlayer != null)
        {
            var corpse = Instantiate(DeadPlayer, transform.position, transform.rotation);

            corpse.GetComponent<CorpseController>().PlaySound(DeathAudio);
        }

        GameObject.Destroy(gameObject);
    }
}
