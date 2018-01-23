using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class OnlineScore {
    //static private string secretKey = "GKd5kxT1RO+vx&aFaan.B|GH`wY#+x";
    static private string highscoreURL = "http://hygon.fr/getHigh.php?";

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

}