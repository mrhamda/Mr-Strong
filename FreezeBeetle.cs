using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBeetle : Enemy
{

    public GameObject iceObject;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameObject _iceObject = Instantiate(iceObject);

            _iceObject.transform.SetParent(player.transform);
            _iceObject.transform.position = player.transform.position;

            player.isFreezed = true;
            Destroy(gameObject);
        }
    }

}
