using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            GameManager.Instance.EnteredDock();
        }
        
    }
}
