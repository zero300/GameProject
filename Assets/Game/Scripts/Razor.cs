using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razor : MonoBehaviour 
{
    public LayerMask characterLayer;
    void Start()
    {
        
    }
    private void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10.0f, characterLayer))
        {
            
            if(hit.collider.CompareTag("Player") )
            {
                // TODO : �C����F or ����
                Debug.Log("�C����F");
            }
            else
            {
                //TODO : �Ǫ����`�A���� �� ?
                Destroy(hit.transform.gameObject);
            }
            //TODO : �z��?
            Destroy(gameObject);
        }
    }
}
