using Card_AI.Interfaces;

namespace Card_AI.Models.Effects
{
    public class HealEffect(int amount, TargetType targetType) : ICardEffect
    {
        public void Execute(Player caster, Player target)
        {
            Player finalTarget = targetType == TargetType.Self ? caster : target;
            finalTarget.Heal(amount);

            string msg = targetType == TargetType.Self ? "對自己回復" : "對敵人回復";
            Console.WriteLine($"{msg} {amount} 點生命。");
        }
    }
}
