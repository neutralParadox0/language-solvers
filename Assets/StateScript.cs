using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScript : MonoBehaviour
{
    public bool isAccept;
    public int id;
    public List<List<Object>> connections;
    public GameObject workSpace;
    private bool isDragging;
    
    void OnMouseDrag()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.Translate(mousePosition);
    }

    private void OnMouseUp()
    {

        SendMessageUpwards("switchState", transform);
    }
    public void SetID(int newId)
    {
        id = newId;
    }    
    
    public void SetAccept(bool acceptState)
    {
        isAccept = acceptState;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //System.Console.WriteLine("I Exist");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool getAccept()
    {
        return isAccept;
    }
    
}
