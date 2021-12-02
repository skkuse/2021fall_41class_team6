
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

    public UserSubmit userSubmit;

    public Text[] texts;
    public Text[] pageTexts;

    const int _size = 1000;
    int _idx = -1;
    int _page = 1;

    public int idx
    {
        get => _idx;
    }
    public int size
    {
        get => _size;
    }

    public void Start()
    {
        names = new string[size];
        ids = new string[size];
        emails = new string[size];
    }

    public void onSubmit()
    {
        _idx++;
        names[idx] = userSubmit.username;
        ids[idx] = userSubmit.id;
        emails[idx] = userSubmit.email;

        onChangePage();
    }

    /*
     Pagination 관련 코드 모음
     */
    private void onChangePage()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            int textIdx = (_page-1) * texts.Length + i;
            Text text = texts[i];
            if (_idx < textIdx)
                text.text = "";
            else
                text.text = string.Format("{0} / {1} / {2}", ids[textIdx], names[textIdx], emails[textIdx]);
        }
    }
    private void setPageTexts()
    {
        if (_page <= 4)
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
                pageTexts[i].text = (_page - 3 + i).ToString();
            }
        }
    }

    public void onClick1()
    {
        _page = int.Parse(pageTexts[0].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick2()
    {
        _page = int.Parse(pageTexts[1].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick3()
    {
        _page = int.Parse(pageTexts[2].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick4()
    {
        _page = int.Parse(pageTexts[3].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick5()
    {
        _page = int.Parse(pageTexts[4].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick6()
    {
        _page = int.Parse(pageTexts[5].text);
        setPageTexts();
        onChangePage();
    }
    public void onClick7()
    {
        _page = int.Parse(pageTexts[6].text);
        setPageTexts();
        onChangePage();
    }
}
