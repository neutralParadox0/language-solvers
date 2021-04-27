using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public struct tableEntry
{
    public GameObject from { get; set; }
    public GameObject to { get; set; }
    public GameObject connection { get; set; }
    public string symbol { get; set; }
    public tableEntry (GameObject FROM, GameObject TO, GameObject conn, string SYMBOL)
    {
        this.from = FROM;
        this.to = TO;
        this.symbol = SYMBOL;
        this.connection = conn;
    }
}
public class WorkSpaceScript : MonoBehaviour
{
    public GameObject blank;
    public GameObject state;
    public GameObject acceptState;
    public GameObject stateTransition;
    public GameObject CircularTransition;
    public int currentId;
    public bool drawState;
    public bool isDrawing;
    public Vector3[] drawPosition;
    public bool flip;
    public string test;
    public GameObject[] DrawObjects;
    public string symbol;
    public List<tableEntry> connectionTable;
    [SerializeField] private LabelPanel input;
    public Canvas canvas;
    public Text text;
    public bool isActive;
    public GameObject initial;
    public tableEntry conn;
    public TMP_Text langText;
    public GameObject langPanel;
    public void addState(bool accept)
    {
        if (accept == true)
        {
            blank = Instantiate(acceptState,new Vector2(0, 0), Quaternion.identity);
            blank.SendMessage("SetAccept", true);
        }
        else
        {
            blank = Instantiate(state, new Vector2(0, 0), Quaternion.identity);
            blank.SendMessage("SetAccept", false);
        }
        blank.transform.parent = transform;
        blank.transform.SetAsFirstSibling();

        blank.name = "q" + currentId.ToString();
        if(currentId == 0)
        {
            initial = blank;
        }
        blank.SendMessage("SetID", currentId);
        currentId++;
        
    }

    public void addConnection(GameObject from, GameObject to, string symbol)
    {
        tableEntry newConn = new tableEntry(from, to, blank, symbol);
        connectionTable.Add(newConn);
    }
    public void addConnectionSingle(string sym)
    {
        test = sym;
        conn = new tableEntry(DrawObjects[0], DrawObjects[1], blank, sym);
        connectionTable.Add(conn);
    }

    public void setDrawState(bool state) => drawState = state;

    public void setIsDrawing(bool newVal) => isDrawing = newVal;

    public void switchState(Transform pos)
    {
        if(isDrawing)
        {
            if (drawState == true)
            {
                drawState = false;
                isDrawing = false;
                drawPosition[1] = pos.position;
                DrawObjects[1] = pos.gameObject;
                drawTransition();
            }
            else
            {
                drawState = true;
                drawPosition[0] = pos.position;
                DrawObjects[0] = pos.gameObject;
            }

        }
    }
    void drawTransition()
    {

        Vector3 point1 = transform.InverseTransformPoint(drawPosition[0]);
        Vector3 point2 = transform.InverseTransformPoint(drawPosition[1]);
           
        float distance = Vector3.Distance(drawPosition[0], drawPosition[1]);
        if(DrawObjects[0] == DrawObjects[1])
        {
            blank = Instantiate(CircularTransition, drawPosition[0] + new Vector3(0, -1.0f, 0), Quaternion.identity);
        }
        else
        {
            blank = Instantiate(stateTransition, drawPosition[0] + ((drawPosition[1] - drawPosition[0]) / 2), Quaternion.identity);
            blank.transform.localScale = new Vector3(distance, distance, 1);
            blank.transform.localRotation *= Quaternion.FromToRotation(Vector3.right, point2);
            //Vector3 v3Dir = blank.transform.position - drawPosition[1];
            //float angle = Mathf.Atan2(v3Dir.y, v3Dir.x) * Mathf.Rad2Deg;
            //blank.transform.eulerAngles = new Vector3(0, 0, angle);
        }
        
        blank.transform.parent = transform;
        text = blank.GetComponentInChildren<Text>();
        text.transform.rotation = Quaternion.Euler(blank.transform.rotation.x * -1.0f, blank.transform.rotation.y * -1.0f, 0.0f);
        input.transform.SetAsLastSibling();
        input.Show(blank);
        isActive = input.isActiveAndEnabled;

    }

    // Start is called before the first frame update
    void Start()
    {
        currentId = 0;
        drawState = false;
        drawPosition = new Vector3[2];
        flip = false;
        DrawObjects = new GameObject[2];
        symbol = "";
        connectionTable = new List<tableEntry>();
    }


    // Update is called once per frame
    void Update()
    {
        //System.Console.WriteLine(transform.childCount);
    }
    public void ExitGame ()
    {
        Application.Quit();
    }
    public void submitAnswer()
    {
        if(testMachine(initial, langText.text))
        {
            langPanel.GetComponent<Image>().color = Color.green;
        }
        else
        {
            langPanel.GetComponent<Image>().color = Color.red;
        }
    }
    public bool testMachine( GameObject from, string lang)
    {
        if(lang.Equals("\n"))
        {
            if (from.GetComponent<StateScript>().isAccept)
                return true;
            else
                return false;
        }
        else
        {
            char testChar = lang[0];
            string nextString = lang.Remove(0, 1);
            List<tableEntry> connections = connectionTable.FindAll(
                delegate (tableEntry ent)
                {
                    return ent.from == from;
                });
            GameObject newFrom = connections.Find(
                delegate (tableEntry ent)
                {
                    return ent.symbol[0] == testChar;
                }).to;
            if(connections.Count == 0 || newFrom == null)
            {
                return false;
            }
            return testMachine(newFrom, nextString);
        } 
    }
    
}

