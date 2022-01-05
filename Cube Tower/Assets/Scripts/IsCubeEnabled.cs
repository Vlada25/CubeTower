using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCubeEnabled : MonoBehaviour
{
    public int needToUnlock;
    public Material closedMatetial;
    private void Start()
    {
        if (PlayerPrefs.GetInt("score") < needToUnlock)
        {
            GetComponent<MeshRenderer>().material = closedMatetial;
        }
    }
}
