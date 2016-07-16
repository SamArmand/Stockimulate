using System.Collections.Generic;
using System.Linq;
using Stockimulate.Architecture;

namespace Stockimulate.Models
{
    internal class Trader
    {
        private readonly int _teamId;

        internal int Id { get; }

        internal string Name { get; }

        //Lazy load
        private Team _team;
        internal Team Team => _team ?? (_team = DataAccess.SessionInstance.GetTeam(_teamId));

        //Lazy load
        private Dictionary<string, Account> _accounts;
        internal Dictionary<string, Account> Accounts => _accounts ?? (_accounts = DataAccess.SessionInstance.GetAccounts(Id));

        internal int Funds { get; set; }

        internal Trader(int id, string name, int teamId, int funds)
        {
            Id = id;
            Name = name;
            _teamId = teamId;
            Funds = funds;
        }

        internal int TotalValue(Dictionary<string, int> prices) => Funds + PositionValues(prices).Values.Sum();

        internal Dictionary<string, int> PositionValues(Dictionary<string, int> prices) => prices.Where(price => Accounts.ContainsKey(price.Key)).ToDictionary(price => price.Key, price => _accounts[price.Key].Position*price.Value);

        internal int PnL(Dictionary<string, int> prices) => TotalValue(prices) - 1000000;
    }
}