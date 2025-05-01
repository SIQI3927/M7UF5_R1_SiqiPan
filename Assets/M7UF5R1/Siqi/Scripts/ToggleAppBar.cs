using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAppBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleAppBar()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
