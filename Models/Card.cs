using Card_AI.Interfaces;

namespace Card_AI.Models
{
    public abstract class Card(string name, int cost)
    {
        public string Name { get; set; } = name;
        public int ManaCost { get; set; } = cost;

        // 靜態進入點：讓你可以用 Card.Create(...) 開始
        public static ComplexCard Create(string name, int cost)
        {
            return new ComplexCard(name, cost);
        }

        // 統一為 3 個參數
        public abstract void Play(Player caster, Player target);
    }

    /*  
        組合模式 (Composition Pattern)
        在哪裡： ComplexCard 類別中持有 List<ICardEffect>。
        用途： 這是本專案最核心的改進。我們不再用繼承（如 HealAndDamageCard）來處理多重效果，而是透過「組合」多個效果組件來打造一張卡片。
        筆記關鍵字： 組合優於繼承(Composition over Inheritance)。
    */

    /*
        策略模式 (Strategy Pattern)
        在哪裡： ICardEffect 介面與其多個實作（DamageEffect, HealEffect）。
        用途： ComplexCard 不需要知道具體效果怎麼運作，它只管呼叫 Execute()。具體的「策略」（傷害或治療）在執行時才決定。
        筆記關鍵字： 定義演算法家族、動態切換行為。
     */

    /*
        流暢介面 / 建造者模式簡化版 (Fluent Interface / Builder)
        在哪裡： Card.Create("...").With(effect).WithDescription("...")
        用途： 透過回傳 this (自己)，讓物件的初始化過程像寫自然語言一樣連貫，提高程式碼的可讀性。
        筆記關鍵字： 鏈式調用 (Chaining)、語意化代碼。
     */

    // 繼承 (Inheritance)：ComplexCard 繼承自 Card，共享名字與消耗的屬性。

    // 複合效果卡：現在它變得很強大，可以裝載多個效果
    public class ComplexCard(string name, int cost) : Card(name, cost)
    {
        private List<ICardEffect> _effects = [];
        private string _description = "";

        // 使用 With 代替 AddEffect，並回傳 ComplexCard 本身
        public ComplexCard With(ICardEffect effect)
        {
            _effects.Add(effect);
            return this; // 這是串接的關鍵
        }

        public ComplexCard WithDescription(string desc)
        {
            _description = desc;
            return this;
        }

        // 多型 (Polymorphism)：主程式呼叫 card.Play() 時，不需要知道它是哪種卡，程式會自動執行對應效果。
        public override void Play(Player caster, Player target)
        {
            Console.WriteLine($"{caster.Name} 發動了 {Name}！");
            //  這裡可以先印出卡牌描述，讓玩家知道它的效果
            if (!string.IsNullOrEmpty(_description))
            {
                Console.WriteLine($"效果：{_description}");
            }
            foreach (var effect in _effects)
            {
                effect.Execute(caster, target);
            }
        }
    }
}
