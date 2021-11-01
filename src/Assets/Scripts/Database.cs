
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class Database : UdonSharpBehaviour
{
    [UdonSynced]
    public string[] names;
    [UdonSynced]
    public string[] ids;

    public InputField nameField;
    public InputField idField;
    Animator nameAnimator;
    Animator idAnimator;

    const int size = 1000;
    int i = 0;

    public void Start()
    {
        names = new string[1000];
        ids = new string[1000];

        nameAnimator = nameField.gameObject.GetComponent<Animator>();
        idAnimator = idField.gameObject.GetComponent<Animator>();
    }

    public void onSubmit()
    {
        string name = nameField.text;
        string id = idField.text;

        long idNum = 0;
        if (!long.TryParse(id, out idNum) || id.Length > 10)
        {
            idAnimator.SetTrigger("incorrect");
            idField.text = "Wrong ID";
        }
        else if(i < size)
        {
            idAnimator.SetTrigger("correct");
            nameAnimator.SetTrigger("correct");
            names[i] = name;
            ids[i] = id;
            idField.text = "";
            nameField.text = "";
        }
        else
        {
            idAnimator.SetTrigger("incorrect");
            nameAnimator.SetTrigger("incorrect");
            idField.text = "No More Space";
        }
    }
}
