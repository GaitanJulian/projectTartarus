using Cinemachine;
using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENUM_RoomsEvents { changedRoom }
public class CameraTargetHandler : MonoBehaviour
{
    GameObject _target;
    CinemachineVirtualCamera _virtualCamera;
    CinemachineConfiner2D _confiner;

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _confiner = GetComponent<CinemachineConfiner2D>();
        ApplyConfiner();
    }

    private void OnEnable()
    {
        EventManager.AddListener(ENUM_RoomsEvents.changedRoom, ApplyConfiner);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(ENUM_RoomsEvents.changedRoom, ApplyConfiner);
    }

    public void ApplyConfiner()
    {
        _target = FindFirstObjectByType<CharacterController>().gameObject;
        _virtualCamera.Follow = _target.transform;
        if (_target.GetComponentInChildren<GetRoomCollider>() != null) _confiner.m_BoundingShape2D = _target.GetComponentInChildren<GetRoomCollider>()._currentRoomCollider;
        else Debug.Log("you did smth wrong dude");
    }
}
