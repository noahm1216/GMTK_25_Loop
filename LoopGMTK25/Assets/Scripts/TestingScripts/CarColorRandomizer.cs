using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColorRandomizer : MonoBehaviour
{
    public List<Material> carColorMats = new List<Material>();
    void Awake()
    {
        int randomColor = Random.Range(0, carColorMats.Count-1);
        this.GetComponent<MeshRenderer>().material = carColorMats[randomColor];
    }
}
