using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class OnlineScore {
    static private string secretKey = "GKd5kxT1RO+vx&aFaan.B|GH`wY#+x";
    static private string highscoreURL = "http://hygon.fr/getHigh.php?";
    static private string addScoreURL = "http://hygon.fr/setHigh.php?";

    public static IEnumerator GetScores(List<Record> list, int check) {
        list.Clear();

        WWW hs_get = new WWW(highscoreURL + "&check=" + check);
        yield return hs_get;

        if (hs_get.error != null) {
            Debug.Log("There was an error getting the high score: " + hs_get.error);
        } else {
            string[] res = hs_get.text.Split();
            for (int i = 0; i < res.Length; i += 2) {
                Record rec = new Record();
                rec.name = res[i];
                Int32.TryParse(res[i + 1], out rec.score);
                list.Add(rec);
            }
        }
    }

    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    public static IEnumerator SetScores(int[] scores, string playerName)
    {
        string scoresHash = "";
        for (int i = 0; i < 12; i++)
            scoresHash += scores[i];

        string hash = Md5Sum(playerName + scoresHash + secretKey);

        string scoresParam = "";
        for (int i = 0; i < 12; i++)
            scoresParam += "&score" + i + "=" + scores[i];

        string post_url = addScoreURL + "pseudo=" + WWW.EscapeURL(playerName) + scoresParam + "&hash=" + hash;
        Debug.Log(post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;

        if (hs_post.error != null)
            Debug.LogWarning("There was an error posting the high score: " + hs_post.error);
    }

}