using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objectsToActivate = new();
    [SerializeField]
    private LayerMask playerLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos si la capa del objeto está en el LayerMask
        if (((1 << other.gameObject.layer) & playerLayerMask) != 0)
        {
            print("Triggered");
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayerMask) != 0)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(false);
            }
        }
    }
}
