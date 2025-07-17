using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private CoinType _coinType; //2
    [SerializeField] private int _zipCoinScore; //Para private escribir con _zipCoinScore (Guion bajo letra minuscula al inicio y segunda palabra mayuscula)
    [SerializeField] private int _zipCoinPlatinoScore;
    private int _coinAmount;

    


    public static readonly Dictionary<CoinType, int> Coins = new Dictionary<CoinType, int>() //7
    {
        {CoinType.Zipcoin, 1}, {CoinType.Zipcoin_Platino, 10}
    };


    public int CoinBalance => _coinAmount; //Getter

    //Para Crear región Ctrl + K + S
    
    #region RECOLECCIÓN DE MONEDAS
    private void OnTriggerEnter(Collider other) //5
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.CoinAmount += Colect();
        }
    }

    private int GetPointsByType() //3
    {
        switch (_coinType)
        {
            case CoinType.Zipcoin:
                return _zipCoinScore;
            case CoinType.Zipcoin_Platino:
                return _zipCoinPlatinoScore;
            default: return 10;
        }
    }

    public int Colect() //4
    {
        int score = GetCoinValue();
        _coinAmount += score; //Lambda
        gameObject.SetActive(false);
        return score;
    }

    public int GetCoinValue() //8
    {
        if (Coins.ContainsKey(_coinType))
        {
            return Coins[_coinType];
        }
        else
        {
            return 0;
        }
    }

    public int CoinsToUI() //6
    {
        return _coinAmount;
    }
}


#endregion
