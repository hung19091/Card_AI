namespace Card_AI.Models
{
    public class CardData
    {
        public required string Name { get; set; }
        public int Cost { get; set; }
        public required string Description { get; set; }
        public required List<EffectData> Effects { get; set; }
    }

    public class EffectData
    {
        public required string Type { get; set; } // 例如 "Damage", "Heal", "Draw"
        public int Value { get; set; }
        public string? Target { get; set; } // 對應 JSON 裡的 "Target"
    }
}
