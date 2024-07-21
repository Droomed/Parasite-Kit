using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public string lastScene;

    private int coins = 0;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);    
        
        DontDestroyOnLoad(gameObject);
    }

    public void AwardCoin()
    {
        coins++;
    }

    public int GetCoins()
    {
        return coins;
    }
}
