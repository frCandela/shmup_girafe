using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class OnlineScore {
    static private string secretKey = "GKd5kxT1RO+vx&aFaan.B|GH`wY#+x";
    static private string highscoreURL = "http://hygon.fr/getHigh.php?";

    public static IEnumerator GetScores(Text t, int check) {
        t.text = "Loading Scores";

        WWW hs_get = new WWW(highscoreURL + "&check=" + check);
        yield return hs_get;

        if (hs_get.error != null) {
            Debug.Log("There was an error getting the high score: " + hs_get.error);
        } else {
            t.text = hs_get.text;
        }
    }

}