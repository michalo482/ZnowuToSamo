using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox : MonoBehaviour
{
    public Material mat;

private void OnTriggerEnter(Collider collider){
	RenderSettings.skybox = mat;
}
}
