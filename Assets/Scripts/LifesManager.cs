using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
public class LifesManager : MonoBehaviour
{
    public int currentLifes;
    [SerializeField] private int maxLifes;
    Vector3 spawnLocation;

    public UnityEvent ZeroLifeRemaining;
    public UnityEvent OneLifeRemaining;
    public UnityEvent TwoLifeRemaining;
    public UnityEvent ThreeLifeRemaining;
    private void Awake()
    {
        maxLifes = 3;
        currentLifes = maxLifes;
    }

    private void Start()
    {
        spawnLocation = transform.position;
    }

    private void Update()
    {
        switch(currentLifes)
        {
            case 0:
                ZeroLifeRemaining.Invoke();
                break;
            case 1:
                OneLifeRemaining.Invoke();
                break;
            case 2:
                TwoLifeRemaining.Invoke();
                break;
            case 3:
                ThreeLifeRemaining.Invoke();
                break;
        }
        if (currentLifes <= 0)
        {
            transform.position = spawnLocation;
            currentLifes = maxLifes;
        }

    }
    public void getHurt(int damage)
    {
        currentLifes -= damage;
        Debug.Log("player got hurt");
    }

}
