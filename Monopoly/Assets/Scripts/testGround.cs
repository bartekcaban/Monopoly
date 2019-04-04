using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGround : MonoBehaviour
{
	public GameObject board;
    // Start is called before the first frame update
    void Start()
    {
        setTestGroundActive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTestGroundActive() {
    	board.SetActive(true);
    	Time.timeScale = 1f;
    }
}
