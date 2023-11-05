using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{

    public delegate void DiamondTaken();
    public static event DiamondTaken OnDiamondTaken;

 
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            OnDiamondTaken?.Invoke();
   
        }
    }

   
}
