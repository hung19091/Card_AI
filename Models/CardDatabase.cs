using Card_AI.Interfaces;
using Card_AI.Models.Effects;
using System.Text.Json;

namespace Card_AI.Models
{
    public static class CardDatabase
    {
        // 使用 Dictionary 方便透過名稱快速查找
        private static Dictionary<string, CardData> _allCardData = new();

        public static void Initialize(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var rawData = JsonSerializer.Deserialize<List<CardData>>(jsonString, options);

            if (rawData != null)
            {
                _allCardData = rawData.ToDictionary(d => d.Name);
            }
            Console.WriteLine($"[系統] 圖鑑初始化完成，共載入 {_allCardData.Count} 種卡片。");
        }

        // 核心功能：每次呼叫都回傳一張「全新的」卡片實例
        public static Card? GetCard(string name)
        {
            if (!_allCardData.TryGetValue(name, out var data)) return null;

            // 這裡搬運你之前的工廠邏輯 (將 CardData 轉為 ComplexCard)
            var card = Card.Create(data.Name, data.Cost).WithDescription(data.Description);
            foreach (var eff in data.Effects)
            {
                // 解析目標：如果 JSON 沒寫，保持null
                TargetType? targetType = eff.Target?.ToLower() switch
                {
                    "self" => TargetType.Self,
                    "opponent" => TargetType.Opponent,
                    _ => null
                };

                ICardEffect effect = eff.Type switch
                {
                    // 如果 targetType 有值就傳進去，沒值 (null) 就會觸發建構子的預設值
                    "Damage" => targetType.HasValue ? new DamageEffect(eff.Value, targetType.Value) : new DamageEffect(eff.Value),
                    "Heal" => targetType.HasValue ? new HealEffect(eff.Value, targetType.Value) : new HealEffect(eff.Value),
                    "Draw" => new DrawEffect(eff.Value),
                    _ => throw new Exception($"未知效果: {eff.Type}")
                };
                card.With(effect);
            }
            return card;
        }
    }

}
