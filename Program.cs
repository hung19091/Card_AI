using Card_AI.Models;
using Card_AI.Systems;

// 1. 初始化
Deck myDeck = new();
Deck enemyDeck = new();
Player hero = new("勇者", 100, 3, myDeck, true);
Player enemy = new("惡魔", 40, 5, enemyDeck);

// 2. 準備牌組 (通常會用工廠模式或 JSON 讀取)
/*
myDeck.AddCard(Card.Create("火球術", 2).With(new DamageEffect(30))
        .WithDescription("發射一顆熾熱火球，造成 30 點傷害"));
myDeck.AddCard(Card.Create("吸血術", 1).With(new DamageEffect(15)).With(new HealEffect(15))
        .WithDescription("吸取敵人的生命，造成 15 點傷害並回復 15 點生命"));
myDeck.AddCard(Card.Create("傷藥", 1).With(new HealEffect(20))
        .WithDescription("使用傷藥，回復 20 點生命"));
myDeck.AddCard(Card.Create("貪婪而貪婪之壺", 2).With(new DamageEffect(30, TargetType.Self)).With(new DrawEffect(2))
        .WithDescription("從壺中釋放出強大的能量，犧牲自己 30 點生命並抽 2 張牌"));
*/

// 啟動遊戲時載入一次圖鑑
CardDatabase.Initialize("Cards.json");

for (int i = 0; i < 2; i++) myDeck.AddCard(CardDatabase.GetCard("貪婪而貪婪之壺")!);
myDeck.AddCard(CardDatabase.GetCard("火球術")!);
myDeck.AddCard(CardDatabase.GetCard("吸血術")!);
myDeck.AddCard(CardDatabase.GetCard("傷藥")!);
myDeck.Shuffle();

enemyDeck.AddCard(CardDatabase.GetCard("火球術")!);
enemyDeck.AddCard(CardDatabase.GetCard("普通攻擊")!);
enemyDeck.AddCard(CardDatabase.GetCard("爛傷藥")!);
enemyDeck.Shuffle();

// 3. 抽牌
Console.WriteLine("----------起始手牌----------");
hero.Draw();
enemy.Draw();
Console.WriteLine("----------------------------\n");

// 4. 簡單的遊戲迴圈
bool isBattleOver = false;

while (!isBattleOver)
{
    // --- 玩家回合 ---
    bool isPlayerTurn = true; // 控制玩家是否還在操作階段
    hero.Mana += 1;
    hero.Draw();

    while (isPlayerTurn)
    {
        enemy.ShowStatus();
        Console.WriteLine("----------------------------");
        hero.ShowStatus();
        hero.ShowHand();

        Console.Write("請選擇出牌編號 (輸入 0 結束回合): ");
        string? input = Console.ReadLine();

        if (input == "0")
        {
            isPlayerTurn = false; // 玩家主動結束回合
            Console.WriteLine("結束回合...");
        }
        else if (int.TryParse(input, out int choice) && choice > 0 && choice <= hero.Hand.Count)
        {
            Card selectedCard = hero.Hand[choice - 1];

            // PlayCard 會回傳 bool (法力檢查)
            if (hero.PlayCard(selectedCard, enemy))
            {
                isPlayerTurn = false;
                Console.WriteLine("\n按任意鍵繼續操作...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("法力不足，請選擇其他卡片！");
                Console.WriteLine("\n按任意鍵繼續操作...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        else
        {
            Console.WriteLine("無效的輸入，請重新選擇！");
            Console.WriteLine("\n按任意鍵繼續操作...");
            Console.ReadKey();
            Console.Clear();
        }

        // 檢查敵方是否已死亡，若是則立即結束戰鬥
        if (enemy.HP <= 0)
        {
            isPlayerTurn = false;
        }
    }

    if (enemy.HP <= 0) { Console.WriteLine("你贏了！"); break; }

    // --- 敵人回合 ---
    Console.WriteLine($"\n>>> {enemy.Name} 的回合 <<<");
    enemy.Mana += 1;
    enemy.Draw();

    Card? aiCard = enemy.DecideCardToPlay();
    if (aiCard != null)
    {
        enemy.PlayCard(aiCard, hero);
    }
    else
    {
        Console.WriteLine($"{enemy.Name} 放棄出牌。");
    }

    if (hero.HP <= 0) { Console.WriteLine("你被擊敗了..."); break; }

    Console.WriteLine("\n按任意鍵進入下一回合...");
    Console.ReadKey();
    Console.Clear();
}