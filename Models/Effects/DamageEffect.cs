using Card_AI.Interfaces;

namespace Card_AI.Models.Effects
{
    public class DamageEffect(int amount, TargetType targetType) : ICardEffect
    {
        public void Execute(Player caster, Player target)
        {
            // 根據標籤決定誰受傷
            Player finalTarget = targetType == TargetType.Self ? caster : target;
            finalTarget.TakeDamage(amount);

            string msg = targetType == TargetType.Self ? "對自己造成" : "對敵人造成";
            Console.WriteLine($"{msg} {amount} 點傷害。");
        }
    }
}
