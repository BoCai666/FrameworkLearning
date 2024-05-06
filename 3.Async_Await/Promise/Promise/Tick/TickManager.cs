using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promise
{
    public class TickManager
    {
        private List<ITicker> tickers = new List<ITicker>();

        private static TickManager inst;
        public static TickManager Inst => inst ?? (inst = new TickManager());


        public void Update(float dt)
        {
            for (int i = 0; i < tickers.Count; i++)
            {
                var ticker = tickers[i];
                if (ticker.Tick(dt))
                {
                    tickers.RemoveAt(i);
                    i--;
                }
            }
        }

        public void AddTicker(ITicker ticker)
        {
            tickers.Add(ticker);
        }
    }
}
