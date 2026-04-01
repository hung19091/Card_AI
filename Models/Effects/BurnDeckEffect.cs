using Card_AI.Interfaces;

namespace Card_AI.Models.Effects
{
    public class BurnDeckEffect(int count, TargetType targetType) : ICardEffect
    {
        public void Execute(Player caster, Player target)
        {
            if (targetType == TargetType.Self)
            {
                caster.BurnFromDeck(count);
            }
            else
            {
                target.BurnFromDeck(count);
            }
        }
    }

}
