using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeSkybox : MonoBehaviour
{
    [SerializeField]
    private List<Material> skyboxes = new List<Material>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Skybox1()
    {
        RenderSettings.skybox = skyboxes[0];
    }

    public void Skybox2()
    {
        RenderSettings.skybox = skyboxes[1];
    }

    public void Skybox3()
    {
        RenderSettings.skybox = skyboxes[2];
    }
}
