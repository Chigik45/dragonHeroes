using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Entity // это пример простого препятствия. оно просто есть, ничего не делает
{
    public new Fractions fraction = Fractions.none; // фракция, нейтральная ко всем

    public Obstacle(Tile tile) : base(tile)
    {
        placeholderSprite = "wall"; // это имя изображения-заглушки. можете не перезаписывать, у нас всё равно нет нормальных изображений пока
        turnMax = 0; // сколько перемещений в ход существо модет сделать
        needToBeSimulated = false; // установите это в false если делаете что-то, на чём камера не должна фокусироваться во время игры
        healthPoints = maxHealthPoints = 999; // ! копируйте именно такую конструкцию ! это то, сколько у сущности здоровья
    }
    public override void CalculateTurn(Board board)
    {
        base.CalculateTurn(board);
    }

    public override void DoSpells(Board board)
    { 
    }
}
public class Attacker: Entity // пример преследующего противника, копировать код по необходимости
{
    public new Fractions fraction = Fractions.enemy; // фракция противника. редактор её перезаписывает, так что эта строчка только для лютого дебага, не заморачивайтесь с ней

    public Attacker(Tile tile): base(tile) // обязательно отнаследовать конструктор
    {
        placeholderSprite = "attacker"; // спрайт-заглушка, можно не перезаписывать
        turnMax = 3; // сколько перемещений в ход существо может сделать
        healthPoints = maxHealthPoints = 3; // ! копируйте именно такую конструкцию ! это то, сколько у сущности здоровья
    }
    public override void CalculateTurn(Board board)
    {
        base.CalculateTurn(board); // обязательно в начале метода прописывать это!
        List<Tile> pathToEnemy = board.GetPathToEnemy(this); // получаем массив клеточек, по которым надо пройти чтобы дойти от этого существа до ближайшего врага
        foreach (var el in pathToEnemy) // этот цикл переносит путь, найденный в строке до этого, в маршрут существа. можно копировать, если хотите так же
        {
            tilesWalk.Enqueue(el);
        }
        tilesWalk.Enqueue(null); // существо стоит один ход. в это время оно будет использовать заклинание из очереди заклинаний
        spells.Enqueue(Spells.attack); // а вот и заклинание, которое мы будем добавлять в строку заклинания
    }

    public override void DoSpells(Board board) // это тоже нужно перезаписывать, чтобы существо понимало, как ему использовать заклинания из своей очереди заклинаний
    {
        if (spells.Count == 0) // это копируем, чтобы конфузов не было
            return; // это копируем, чтобы конфузов не было
        if (spells.Peek() == Spells.attack) // для заклинания типа attack
        {
            SpellsFactory.GetSpell(Spells.attack).Use(board, this, new List<Tile>() { board.GetNearestEnemy(this)?.GetCurrTile() }); 
            // используем заклинание, передаём board, устанавливаем эту сущность как того, кто использовал заклинание, а единственной целью устанавливаем ближайшего врага
        }
    }
}
public class Ranged : Entity // это класс дальнобойного противника, тут тоже полезная инфа
{
    public new Fractions fraction = Fractions.enemy;
    int reload = 0; // отдельная переменная под перезарядку дальнобойной атаки, можете при желании такие вводить

    public Ranged(Tile tile) : base(tile)
    {
        placeholderSprite = "ranged";
        turnMax = 3;
        healthPoints = maxHealthPoints = 2;
    }
    public override void CalculateTurn(Board board)
    {
        // это существо бежит от игрока в самую дальнюю точку если тот близко, и ползёт к нему если оно далеко
        base.CalculateTurn(board);
        int pathLength = -1;
        if (reload > 0)
            --reload;
        List<Tile> path = new List<Tile>();
        if (board.GetPathToEnemy(this).Count >= 5) // если оно далеко, то оно находит путь до игрока и идёт к нему
        {
            path = board.GetPathToEnemy(this);
        }
        else // если оно близко, то оно находит путь до дальнейшей от игрока точки и бежит туда, убегая от игрока
        {
            for (int i = 0; i < 20; ++i)
            {
                for (int j = 0; j < 20; ++j) // перебираем все тайлы
                {
                    bool _ = true;
                    bool found = false;
                    List<Tile> pathToEnemy = board.GetTile(i, j).FindWayTo(board, board.GetNearestEnemy(this).GetCurrTile(), ref found);
                    // получаем путь от перебираемой точки до ближайшего к этому тайлу противника
                    if (found && pathToEnemy.Count > pathLength) // сравниваем длину полученного пути и того пути, который мы нашли до этого
                    {
                        path = current.FindWayTo(board, board.GetTile(i, j), ref _);
                        pathLength = pathToEnemy.Count;
                    }
                }
            }
        }

        foreach (var el in path)
        {
            tilesWalk.Enqueue(el);
        }
        tilesWalk.Enqueue(null);
        spells.Enqueue(Spells.rangedAttack);
    }

    public override void DoSpells(Board board)
    {
        if (spells.Count == 0)
            return;
        if (spells.Peek() == Spells.rangedAttack)
        {
            if (reload > 0)
                return;
            bool used = SpellsFactory.GetSpell(Spells.rangedAttack).Use(board, this, new List<Tile>() { board.GetNearestEnemy(this)?.GetCurrTile() });
            if (used)
            {
                reload = 3; // устанавливаем перезарядку в 3
            }
        }
    }
}
public class Feth : Attacker // если надо скопировать поведение, можно наследоваться не от Entity, а от уже готового шаблона существа
{
    public Feth(Tile tile) : base(tile)
    {
        placeholderSprite = "feth";
        turnMax = 3;
        healthPoints = maxHealthPoints = 10;
    }
}