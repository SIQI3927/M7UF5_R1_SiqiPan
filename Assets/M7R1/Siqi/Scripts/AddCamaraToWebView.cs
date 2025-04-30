using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;

public class AddCamaraToWebView : MonoBehaviour
{
    public Canvas canvas_Webview;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            //if (cam.GetComponentInParent<NetworkObject>().IsOwner) // Solo la cámara del jugador local
            //{
            //    canvas_Webview.worldCamera = cam;
            //    break;
            //}
            canvas_Webview.worldCamera = cam;
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
