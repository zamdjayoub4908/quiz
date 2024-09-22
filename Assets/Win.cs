using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class Win : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void move(int index)
    {
        SceneManager.LoadScene(index);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
