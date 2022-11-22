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
                // TODO : 遊戲輸了 or 扣血
                Debug.Log("遊戲輸了");
            }
            else
            {
                //TODO : 怪物死亡，昏厥 或 ?
                Destroy(hit.transform.gameObject);
            }
            //TODO : 爆炸?
            Destroy(gameObject);
        }
    }
}
