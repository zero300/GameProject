using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject player;
    public float velocity;
    Vector3 dir;

    public float waitTime;
    float count;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("test");
        dir = player.transform.position - transform.position;
        count = -waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (count < 10)
        {
            count += Time.deltaTime;
        }
        if (count > 0)
        {
            transform.Translate(dir * Time.deltaTime * velocity, Space.World);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        PlayerAttributes pa = collision.gameObject.GetComponent<PlayerAttributes>();
        if(pa is not null)
        {
            pa.GetHurt(10);
        }
        Destroy(this.gameObject);
    }


}
