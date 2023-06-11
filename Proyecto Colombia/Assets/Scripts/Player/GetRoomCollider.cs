using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRoomCollider : MonoBehaviour
{
    [SerializeField] LayerMask _terrainLayer;
    public PolygonCollider2D _currentRoomCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utilities.IsObjectInLayerMask(collision.gameObject, _terrainLayer))
        {
            if (collision.GetComponent<PolygonCollider2D>() != null)
            {
                _currentRoomCollider = collision.GetComponent<PolygonCollider2D>();
                Invoke("SendNewRoomCollider", 0.2f);
                Debug.Log(_currentRoomCollider);
            }
            else
            {
                Debug.Log("ERROR: Room does not have box collider");
            }
        }
    }

    void SendNewRoomCollider()
    {
        EventManager.Dispatch(ENUM_RoomsEvents.changedRoom);
    }
}

