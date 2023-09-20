using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    [SerializeField] private float disappearTime;
    
    public void DisappearObject(GameObject go)
    {
        StartCoroutine(CountDownToAppear(go));
    }

    private IEnumerator CountDownToAppear(GameObject go)
    {
        go.SetActive(false);
        yield return new WaitForSeconds(disappearTime);
        go.SetActive(true);
    }
}
