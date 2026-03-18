using Card_AI.Interfaces;

namespace Card_AI.Models.Effects
{
    public class BurnDeckEffect(int count) : ICardEffect
    {
        public void Execute(Player caster, Player target) => caster.BurnFromDeck(count);
    }

}
