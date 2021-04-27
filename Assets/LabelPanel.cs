using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelPanel : MonoBehaviour
{
    private static LabelPanel instance;
    private Button okBtn;
    private Button cancelBtn;
    public TMP_InputField inputField;
    public Text text;
    public GameObject transition;
    public WorkSpaceScript workSpace;
    public string connSymbol;
    private void Awake()
    {
        instance = this;

        okBtn = transform.Find("okBtn").GetComponent<Button>();
        cancelBtn = transform.Find("cancelBtn").GetComponent<Button>();
        inputField = transform.Find("LabelField").GetComponent<TMP_InputField>();
        workSpace = transform.parent.GetComponent<WorkSpaceScript>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            okBtnClick();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cancelBtnClick();
        }
    }

    public void Show(GameObject conn)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        text = conn.GetComponentInChildren<Text>();
        transition = conn;
        okBtn.onClick.AddListener(okBtnClick);

        cancelBtn.onClick.AddListener(cancelBtnClick);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    void okBtnClick()
    {
        
        text.text = inputField.text;
        connSymbol = inputField.text;
        workSpace.addConnectionSingle(connSymbol);
        //SendMessageUpwards("addConnectionSingle", connSymbol);
        Hide();

       
    }
    void cancelBtnClick()
    {
        Hide();
    }
}
