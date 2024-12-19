using UnityEngine;
using Player;
using System;
using Unity.Cinemachine;

public class Zoom : MonoBehaviour
{

    //=========================================================================
    //                          Properties
    //=========================================================================

    [SerializeField] CinemachineCamera _cinemachineCamera;
    // LensSettings lens;
    CinemachineThirdPersonFollow thirdPersonFollow;

    EventController playerEvents;

    // orthographic size
    [Header("Zoom Settings")]
    [SerializeField] float _zoomSpeed = 0.1f;
    [SerializeField] float zoomedInValue = 10;
    [SerializeField] float zoomedOutValue = 20;
    private float _zoomGoal = 0;

    // y rig shoulder offset
    [Header("Shoulder Offset Settings")]
    [SerializeField] float zoomedInShoulderOffset = 1.37f;
    [SerializeField] float zoomedOutShoulderOffset = -10;
    [SerializeField] float zoomedShoulderOffsetSpeed = 0.1f;
    float _shoulderOffsetGoal = 0;

    //=========================================================================
    //                          Mono Methods
    //=========================================================================

    void Awake()
    {
        playerEvents = FindFirstObjectByType<EventController>();
        thirdPersonFollow = _cinemachineCamera.GetComponent<CinemachineThirdPersonFollow>();
    }

    void Start()
    {
        // Get Cinemachine camera
        if (_cinemachineCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera component not found in the scene.");
        }

        playerEvents.zoom += OnZoom;
    }

    void OnDestroy()
    {
        playerEvents.zoom -= OnZoom;
    }

    //=========================================================================
    //                          Zoom Methods
    //=========================================================================

    void OnZoom(string value, bool moveYOffset)
    {

        if (value == "in")
        {
            _zoomGoal = zoomedInValue;
            if (moveYOffset)
                _shoulderOffsetGoal = zoomedOutShoulderOffset;
            else
                _shoulderOffsetGoal = zoomedInShoulderOffset;
        }
        else if (value == "out")
        {
            _zoomGoal = zoomedOutValue;
            if (moveYOffset)
                _shoulderOffsetGoal = zoomedOutShoulderOffset;
            else
                _shoulderOffsetGoal = zoomedInShoulderOffset;
        }

        //
        if (_cinemachineCamera.Lens.OrthographicSize != _zoomGoal)
            _cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(_cinemachineCamera.Lens.OrthographicSize, _zoomGoal, _zoomSpeed);
        if (thirdPersonFollow.ShoulderOffset.y != _shoulderOffsetGoal)
            thirdPersonFollow.ShoulderOffset.y = Mathf.Lerp(thirdPersonFollow.ShoulderOffset.y, _shoulderOffsetGoal, zoomedShoulderOffsetSpeed);
    }
}