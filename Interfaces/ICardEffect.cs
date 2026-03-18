namespace Card_AI.Interfaces
{
    public interface ICardEffect
    {
        // 統一規格：誰放的、對誰放
        void Execute(Player caster, Player target);
    }
}
