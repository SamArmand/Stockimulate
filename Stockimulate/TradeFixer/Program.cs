using System;

namespace TradeFixer
{
	public class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			TradeManager tradeManager = new TradeManager();

			DataAccess dataAccess = DataAccess.Instance;

			dataAccess.Reset();

			var trades = dataAccess.GetTrades("","","","","","");

			foreach (var trade in trades) 
			{

				tradeManager.CreateTrade(trade.Buyer.Id, trade.Seller.Id, trade.Instrument.Symbol, trade.Quantity, trade.Price, trade.BrokerId);

			}


		}
	}
}
