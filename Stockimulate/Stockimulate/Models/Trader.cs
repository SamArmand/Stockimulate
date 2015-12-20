using System.Collections.Generic;
using System.Linq;

namespace Stockimulate.Models
{
    internal class Trader
    {
        internal int Id { get; }

        internal string Name { get; }

        //Lazy
        internal Team Team { get; }

        internal Dictionary<string, Account> Accounts { get; }

        internal int Funds { get; set; }

        internal Trader(int id, string name, Team team, Dictionary<string, Account> accounts, int funds)
        {
            Id = id;
            Name = name;
            Team = team;
            Accounts = accounts;
            Funds = funds;
        }

        internal int TotalValue(Dictionary<string, int> prices) => Funds + PositionValues(prices).Values.Sum();

        internal Dictionary<string, int> PositionValues(Dictionary<string, int> prices) => prices.ToDictionary(price => price.Key, price => Accounts[price.Key].Position);

        internal int PnL(Dictionary<string, int> prices) => TotalValue(prices) - 1000000;
    }
}