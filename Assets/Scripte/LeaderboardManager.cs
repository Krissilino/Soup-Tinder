using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{

    public LeaderboardEntry[] players = new LeaderboardEntry[10];

    public Text playerName;
    public Text playerHighscore;
    public Text playerRank;
    public Image playerAvatar;
    public Image playerHut;

    public AvatarsAndHats avatarHats;

    private List<List<string>> leaders = new List<List<string>>();

    // Start is called before the first frame update
    void Start()
    {

        //Fill in Non-Server Information
        (Sprite, Sprite) sprites = avatarHats.ReturnAvatarAndHatSprites(GameManager.avatarNum, GameManager.bestHatNum);

        playerAvatar.sprite = sprites.Item1;
        playerHut.sprite = sprites.Item2;

        playerName.text = "Deine Platzierung " + GameManager.name;
        playerHighscore.text = GameManager.highscore.ToString();
        StartCoroutine(GetScoreRank());

        //Connect with server to get top 10 players and display them
        StartCoroutine(GetLeaderboard());

        //Ask for connections and receive information

        //parse information for players
        

        //Setup player pictures
    }

    IEnumerator GetScoreRank()
    {
        string getRankURL = "https://krizzdesign.de/getScoreRank.php";
        string rank = "";

        using (UnityWebRequest rank_get = UnityWebRequest.Get(getRankURL))
        {
            yield return rank_get.SendWebRequest();

            if (rank_get.isNetworkError || rank_get.isHttpError)
            {
                print("There was an error getting the rank: " + rank_get.error);
            }
            else
            {
                string table = rank_get.downloadHandler.text;
                List<string> tab = new List<string>(table.Split('#'));

                for (int r = 1; r <= tab.Count; r++)
                {
                    if (tab[r-1] == GameManager.name)
                    {
                        rank = r.ToString();
                        break;
                    }
                }
            }
        }

        if (rank == "")
            playerRank.text = "Keine Platzierung";
        else
        {
            playerRank.text = rank + ". Platz";
        }

    }

    IEnumerator GetLeaderboard()
    {
        string getLeaderURL = "https://krizzdesign.de/getLeaderboard.php";

        using (UnityWebRequest leader_get = UnityWebRequest.Get(getLeaderURL))
        {
            yield return leader_get.SendWebRequest();

            if (leader_get.isNetworkError || leader_get.isHttpError)
            {
                print("There was an error getting the high score: " + leader_get.error);
            }
            else
            {
                string table = leader_get.downloadHandler.text;
                List<string> tab = new List<string>(table.Split('#'));
                foreach (string t in tab)
                {
                    List<string> parsedT = new List<string>(t.Split('~'));
                    leaders.Add(parsedT);
                }

                leaders.RemoveAt(leaders.Count - 1);
            }
        }

        SetupPlayers();
    }

    private void SetupPlayers()
    {
        for(int p = 0; p < 10; p++)
        {
            print(leaders.Count);
            if (p >= leaders.Count)
                break;

            List<string> player = leaders[p];

            string pName = player[0];
            int pScore = int.Parse(player[1]);
            int pAvatar = int.Parse(player[2]);
            int pHut = int.Parse(player[3]);

            players[leaders.Count-(p+1)].UpdateEntry(pAvatar, pHut, pScore, pName);

        }
    }

    public void BackToMenu()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        GameManager.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }
}
