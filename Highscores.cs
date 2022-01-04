using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscores : MonoBehaviour
{

    const string privateCode = "r3OnTs_lfkmP0nzKerbFGA9NkTn8YdMUOxrbSHqczHUw";
    const string publicCode = "5e1a20dffe224b0478da0692";
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;
    public Text loadingIndicator;
    public Text errorIndicator;
    static Highscores instance;
    DisplayHighscores highscoresDisplay;

    private void Start()
    {
        instance = this;
        highscoresDisplay = GetComponent<DisplayHighscores>();
    }
    private void Awake()
    {
        instance = this;
        highscoresDisplay = GetComponent<DisplayHighscores>();
    }

    public static void AddNewHighscore(float skill, string playerName, string ship)
    {
        instance.StartCoroutine(instance.UploadNewHighscore(skill, playerName, ship));
        //instance.StartCoroutine(instance.UploadNewHighscore(skill, playerName, ship));
    }

    IEnumerator UploadNewHighscore(float skill, string playerName, string ship)
    {
        if (ship == null)
        {
            ship = "";
        }
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(playerName) + "/" + WWW.EscapeURL(((int)(skill * 1000)).ToString()) + "/" + (this.stringToInt(playerName) + this.encryption(skill, playerName)).ToString() + "/" + WWW.EscapeURL(ship));
        //(playerName + this.encryption(skill))
        //this.md5(playerName + ship + skill + 2.718.ToString())
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            print("Upload Successful");
            loadingIndicator.gameObject.SetActive(false);
            errorIndicator.gameObject.SetActive(false);
            DownloadHighscores();
        }
        else
        {
            print("Error uploading: " + www.error);
            loadingIndicator.gameObject.SetActive(false);
            errorIndicator.gameObject.SetActive(true);
        }
    }

    public void DownloadHighscores()
    {
        StartCoroutine("DownloadHighscoresFromDatabase");
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighscores(www.text);
            highscoresDisplay.OnHighscoresDownloaded(highscoresList);
            //print(www.text);
        }
        else
        {
            print("Error downloading: " + www.error);
        }
    }

    void FormatHighscores(string textStream)
    {
        Debug.Log(textStream);
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string playerName = entryInfo[0];
            float skill = int.Parse(entryInfo[1]);
            string check = entryInfo[2];
            string ship = entryInfo[3];

            highscoresList[i] = new Highscore(playerName, skill, check, ship);
        }
    }

    public int encryption(float num, string name)
    {
        return (int)(Mathf.Pow((float)(num * 2718 / 3141), 1618f));
    }

    public int stringToInt(string str)
    {
        int total = 0;
        string map = "abcdefghijklmnopqrstuvwxyz";
        str = str.ToLower();
        char[] strArray = str.ToCharArray();
        char[] mapArray = map.ToCharArray();
        for (int i = 0; i < str.Length; i++)
        {
            for (int k = 0; k < map.Length; k++)
            {
                if (strArray[i] == mapArray[k])
                {
                    total += k;
                    break;
                }
                else
                {
                    total += i*k;
                }
            }
        }
        return total;
    }

    public string md5(string str)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        byte[] bytes = encoding.GetBytes(str);
        var sha = new System.Security.Cryptography.MD5CryptoServiceProvider();
        return System.BitConverter.ToString(sha.ComputeHash(bytes));
    }

    public string Sha1Sum2(string str)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        byte[] bytes = encoding.GetBytes(str);
        var sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        return System.BitConverter.ToString(sha.ComputeHash(bytes));
    }
}

public struct Highscore
{
    public string playerName;
    public float skill;
    public string check;
    public string ship;

    public Highscore(string _playerName, float _skill, string _check, string _ship)
    {
        playerName = _playerName;
        skill = _skill;
        check = _check;
        ship = _ship;
    }
}
