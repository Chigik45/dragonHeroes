using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewBehaviourScript : MonoBehaviour
{
    Creature creature;
     public float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        creature = new Creature();
    }

    // Update is called once per frame
    void Update()
    {
        if (creature.tile != null)
        {
            transform.position = Vector2.Lerp(transform.position, creature.transform.position, speed * Time.deltaTime);
        }
    }
    //
}
