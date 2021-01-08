using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchBellStep : MonoBehaviour
{
    IEnumerator coroutine;  
    public void FirstHit()
    {
        //coroutine = Timing();
        StartCoroutine(coroutine);
    }
    public void GoodHit()
    {

    }
    public void BadHit()
    {

    }

    //IEnumerator Timing()
    //{
    //    yield return new WaitForSeconds();
        
    //}
}
