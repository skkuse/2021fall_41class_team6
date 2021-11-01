
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class Database : UdonSharpBehaviour
{
    public string[] names;
    public string[] ids;
    public string[] emails;

    public Text[] texts;
    public Text[] pageTexts;

    public InputField nameField;
    public InputField idField;
    public InputField emailField;
    Animator nameAnimator;
    Animator idAnimator;
    Animator emailAnimator;

    const int size = 1000;
    int idx = 0;
    int page = 0;

    public void Start()
    {
        names = new string[1000];
        ids = new string[1000];
        emails = new string[1000];

        nameAnimator = nameField.gameObject.GetComponent<Animator>();
        idAnimator = idField.gameObject.GetComponent<Animator>();
        emailAnimator = emailField.gameObject.GetComponent<Animator>();
    }

    public void onSubmit()
    {
        string name = nameField.text;
        string id = idField.text;
        string email = emailField.text;

        int atPos = email.IndexOf('@');
        bool atCond = atPos != -1 && atPos == email.LastIndexOf('@');
        bool dotCond = !email.EndsWith(".") && email.IndexOf('.') != -1;

        long idNum = 0;
        if (!long.TryParse(id, out idNum) || id.Length > 10)
        {
            idAnimator.SetTrigger("incorrect");
            idField.text = "Wrong ID";
        }
        else if (!atCond || !dotCond)
        {
            emailAnimator.SetTrigger("incorrect");
            emailField.text = "Wrong Email";
        }
        else if(idx < size) //성공
        {
            idAnimator.SetTrigger("correct");
            nameAnimator.SetTrigger("correct");
            emailAnimator.SetTrigger("correct");

            names[idx] = name;
            ids[idx] = id;
            emails[idx] = email;

            idField.text = "";
            nameField.text = "";
            emailField.text = "";

            idx++;
            setTexts();
        }
        else
        {
            idAnimator.SetTrigger("incorrect");
            nameAnimator.SetTrigger("incorrect");
            emailAnimator.SetTrigger("incorrect");

            idField.text = "No More Space";
        }
    }


    /*
     Pagination 관련 코드 모음
     */
    private void onChangePage()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            int textIdx = (page-1) * texts.Length + i;
            if (idx < textIdx)
                continue;
            Text text = texts[i];
            text.text = string.Format("{0} / {1} / {2}", ids[textIdx], names[textIdx], emails[textIdx]);
        }
    }
    private void setTexts()
    {
        if (page <= 4)
        {
            for (int i = 0; i < pageTexts.Length; i++)
            {
                pageTexts[i].text = (i + 1).ToString();
            }
        }
        else
        {
            for (int i = 0; i < texts.Length; i++)
            {
                pageTexts[i].text = (page - 3 + i).ToString();
            }
        }
    }

    public void onClick1()
    {
        page = int.Parse(pageTexts[0].text);
        setTexts();
        onChangePage();
    }
    public void onClick2()
    {
        page = int.Parse(pageTexts[1].text);
        setTexts();
        onChangePage();
    }
    public void onClick3()
    {
        page = int.Parse(pageTexts[2].text);
        setTexts();
        onChangePage();
    }
    public void onClick4()
    {
        page = int.Parse(pageTexts[3].text);
        setTexts();
        onChangePage();
    }
    public void onClick5()
    {
        page = int.Parse(pageTexts[4].text);
        setTexts();
        onChangePage();
    }
    public void onClick6()
    {
        page = int.Parse(pageTexts[5].text);
        setTexts();
        onChangePage();
    }
    public void onClick7()
    {
        page = int.Parse(pageTexts[6].text);
        setTexts();
        onChangePage();
    }
}
