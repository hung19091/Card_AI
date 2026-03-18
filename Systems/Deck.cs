using Card_AI.Models;
using System.Security.Cryptography;

namespace Card_AI.Systems
{
    public class Deck
    {
        private List<Card> cards = new List<Card>();
        public int Count => cards.Count;

        public void AddCard(Card card) => cards.Add(card);

        public void Shuffle()
        {
            // 經典的 Fisher-Yates 洗牌演算法
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = RandomNumberGenerator.GetInt32(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        public Card? Draw()
        {
            if (cards.Count == 0) return null;
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }
}
