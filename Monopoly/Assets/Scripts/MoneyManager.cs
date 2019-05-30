using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private List<Player> players;

    public MoneyManager(List<Player> players)
    {
        this.players = players;
    }

    public int GetAccountState(Player player)
    {
        return player.GetCash();
    }

    public int GetAccountState(string playerName)
    {
        return this.players.Find(x => x.playerName == playerName).GetCash();
    }

    public void SetAccountState(Player player, int amountOfMoney)
    {
        player.SetCash(amountOfMoney);
    }

    public void SetAccountState(string playerName, int amountOfMoney)
    {
        this.players.Find(x => x.playerName == playerName).SetCash(amountOfMoney);
    }

    public void DepositOnAccount(Player player, int amountOfMoney)
    {
        player.SetCash(player.GetCash() + amountOfMoney);
    }

    public void DepositOnAccount(string playerName, int amountOfMoney)
    {
        Player player = this.players.Find(x => x.playerName == playerName);
        player.SetCash(player.GetCash() + amountOfMoney);
    }

    public void WithdrawFromAccount(Player player, int amountOfMoney)
    {
        player.SetCash(player.GetCash() - amountOfMoney);
    }

    public void WithdrawFromAccount(string playerName, int amountOfMoney)
    {
        Player player = this.players.Find(x => x.playerName == playerName);
        player.SetCash(player.GetCash() - amountOfMoney);
    }

    public bool DoesPlayerHasAnyMoneyLeft(Player player)
    {
        return (player.GetCash() > 0) ? true : false;
    }

    public bool DoesPlayerHasAnyMoneyLeft(string playerName)
    {
        return (this.players.Find(x => x.playerName == playerName).GetCash() > 0) ? true : false;
    }

    public void PlayerBancrupcy(Player player)
    {
        players.Remove(player);
    }


}
