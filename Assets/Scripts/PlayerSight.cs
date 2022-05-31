using System.Collections;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    public bool alive= true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Eyes")
        {
            other.transform.parent.GetComponent<Monster>().CheckSight();
        }
    }
   
}
