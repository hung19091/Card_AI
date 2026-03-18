using Card_AI.Interfaces;

namespace Card_AI.Models.Effects
{
    public class StealEffect : ICardEffect
    {
        public void Execute(Player caster, Player target)
        {
            if (target.Deck.Draw() is Card stolenCard)
            {
                caster.Hand.Add(stolenCard); // 從對手牌組抽一張放進我手牌
                Console.WriteLine($"{caster.Name} 偷走了 {target.Name} 的一張牌！");
            }
        }
    }
}
