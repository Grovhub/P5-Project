using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrate : MonoBehaviour
{
    public float height;
    public GameObject camera;
    public GameObject body;
    public GameObject legs;

        
    
    // Start is called before the first frame update
    void Start()
    {
     /*   height = camera.transform.position.y;
        Vector3 _legs = new Vector3(0,height/2f,0);
        legs.transform.position.y = height-_legs.transform.position.y;
        Vector3 _body = new Vector3(0,height/3f,0);
        body.transform.position.y = height-_body.transform.position.y;
*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
