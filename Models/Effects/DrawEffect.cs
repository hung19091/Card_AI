using Card_AI.Interfaces;

namespace Card_AI.Models.Effects
{
    public class DrawEffect(int count) : ICardEffect
    {
        public void Execute(Player caster, Player target) => caster.Draw(count);
    }
}
