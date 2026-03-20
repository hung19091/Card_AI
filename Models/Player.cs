using Card_AI.Models;
using Card_AI.Systems;

public class Player
{
    public string Name { get; set; }

    //封裝 (Encapsulation)：Player.HP 設為 private set，外部不能隨意改血量，必須透過 TakeDamage 方法。
    public int HP { get; private set; } = 40;
    public int MaxHP { get; private set; }
    public int Mana { get; set; } = 3;
    public bool IsHuman { get; set; } = false;
    public Deck Deck { get; set; }
    public List<Card> Hand { get; private set; } = [];
    public List<Card> Graveyard { get; private set; } = [];

    public Player(string name, int hp, int mana, Deck deck, bool ishuman = false)
    {
        Name = name;
        HP = hp;
        MaxHP = hp;
        Mana = mana;
        Deck = deck;
        IsHuman = ishuman;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        if (HP < 0) HP = 0;
    }

    public void Heal(int amount)
    {
        HP += amount;
        if (HP > MaxHP) HP = MaxHP;
    }

    public void ShowStatus()
    {
        Console.WriteLine($"[{Name} - HP: {HP}/{MaxHP} | Mana: {Mana} | 手牌: {Hand.Count} 張]");
    }

    public void ShowHand()
    {
        Console.WriteLine($"\n--- {Name} 的手牌清單 ---");
        if (Hand.Count == 0)
        {
            Console.WriteLine("(目前沒有手牌)");
            return;
        }

        for (int i = 0; i < Hand.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Hand[i].Name} (消耗: {Hand[i].ManaCost})");
        }
        Console.WriteLine("-----------------------");
    }

    public void Draw(int count = 1)
    {
        bool isVisible = IsHuman; // 玩家自己看到具體卡名，敵人（或隱藏狀態）只顯示張數
        int drawnCount = 0;
        List<string> drawnNames = new List<string>();

        for (int i = 0; i < count; i++)
        {
            if (Deck.Draw() is Card card)
            {
                Hand.Add(card);
                drawnCount++;
                drawnNames.Add(card.Name);
            }
            else
            {
                Console.WriteLine($"[提示] {Name} 的牌組已空！");
                break;
            }
        }

        if (drawnCount > 0)
        {
            if (isVisible)
            {
                // 玩家自己看到具體卡名
                foreach (var name in drawnNames)
                {
                    Console.WriteLine($"{Name} 抽到了「{name}」。");
                }
            }
            else
            {
                // 敵人（或隱藏狀態）只顯示張數
                Console.WriteLine($"{Name} 抽了 {drawnCount} 張牌。");
            }
        }
    }

    // 封裝：統一的出牌/丟牌入口 (從手牌進墳場)
    public void Discard(Card card)
    {
        if (Hand.Contains(card))
        {
            Hand.Remove(card);
            Graveyard.Add(card);
            Console.WriteLine($"{Name} 的 {card.Name} 進入了廢牌堆。");
        }
    }

    // 封裝：統一的燒牌入口 (從牌組直接進墳場)
    public void BurnFromDeck(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (Deck.Draw() is Card card)
            {
                Graveyard.Add(card);
                Console.WriteLine($"{Name} 的 {card.Name} 進入了廢牌堆。");
            }
        }
    }

    public Card? DecideCardToPlay()
    {
        // 簡單的 AI：從手牌找第一張付得起的牌
        // 進階一點可以找傷害最高的，或是快沒血時優先找補血牌
        foreach (var card in Hand)
        {
            if (this.Mana >= card.ManaCost)
            {
                return card;
            }
        }
        return null; // 沒錢出牌或沒牌了
    }

    public bool PlayCard(Card card, Player target)
    {
        // 1. 檢查法力是否足夠
        if (this.Mana < card.ManaCost)
        {
            return false;
        }

        // 2. 執行扣費與邏輯
        this.Mana -= card.ManaCost;

        // 3. 呼叫卡片本身的 Play (傳入 2 個參數)
        card.Play(this, target);

        // 重點：呼叫封裝好的方法，從手牌移除 & 自動進墳場
        this.Discard(card);

        return true;
    }
}
