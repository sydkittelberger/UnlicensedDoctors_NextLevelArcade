using UnityEngine;

public class CreateHighScoreList : MonoBehaviour
{
    //Private Variables
    
    //Public Variables
    public GameObject listItem;

    void Start()
    {
        foreach( ScoreEntry s in GameManager.manager.scoreData.scoreList)
        {
            GameObject li = Instantiate(listItem);
            li.transform.SetParent(this.gameObject.transform);
            li.GetComponent<SetScoreText>().SetText(s.initials, s.score.ToString());
        }
    }
}
