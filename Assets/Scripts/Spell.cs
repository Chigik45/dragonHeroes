using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Spells // это перечисление имён заклинаний, когда хотите добавить заклинание - добавляйте тут
{
    none, attack, rangedAttack
}


public class SpellsFactory // это фабрика создания заклинаний, которая выдаёт карты по запросу. трогать только в месте комментария
{ 
    public static int AffectedCount(Spells spell)
    {
        return GetSpell(spell).affectedCount;
    }
    public static Spell GetSpell(Spells spell) // добавляйте выдачу необходимого объекта заклинания по значению enum, как в примерах ниже
    {
        switch (spell)
        {
            case Spells.attack:
                return new AttackSpell();
            case Spells.rangedAttack:
                return new RangedAttackSpell();
            default:
                return new AttackSpell();
        }
    }
}


public abstract class Spell // класс заклинания. наследуйтесь от него, чтобы создать новое заклинание, и переопределите абстрактный метод
{
    public int affectedCount = 1; // количество целей для заклинания
    public int rangeVisual = 1; // дальность заклинания (в клетках)
    public abstract bool Use(Board board, Entity caster, List<Tile> affected);
}
public class AttackSpell: Spell
{
    public AttackSpell() : base() // конструкторы можете копировать так же, но меняйте по необходимости rangeVisual и affectedCount
    {
        rangeVisual = 1; 
        affectedCount = 1;
    }
    public override bool Use(Board board, Entity caster, List<Tile> affected) // вот так надо переопределять метод
    {
        // пояснение - тут вы должны добавить код для работы заклинания. сами же вы должны понять, можно ли применить заклинание с такими параметрами или нет
        // если заклинание применить можно, вы возвращаете true. если его не удалось применить (например, переданный тайл слишком далеко от игрока), возвращайте false
        // board - это в целом поле, которое передаётся в функцию. cаster - это та сущность, которая применила заклинание.
        // affected - массив целей заклинания, гарантированно больше  affectedCount

        Entity picked = board.GetEntityOnTile(affected[0]); // получаем сущность, стоящую на первой выбранной в качестве цели клеточке
        if (picked != null && picked.fraction != caster.fraction) // если на выбранной клеточке правда кто-то стоит и стоит враг того, кто применил заклинание
        {
            bool found = false; // это флаг, который передаётся в метод DistanceBetween. если путь до цели не найден, то флаг false, иначе true
            int dist = caster.GetCurrTile().DistanceBetween(board, affected[0], ref found); // получаем количество тайлов между тем, кто юзнул заклинание, и нашей целью
            if (found && dist == 0) // если путь найден и расстояние между тем, кто юзнул заклинание, и нашей целью, равно 0 тайлам (то есть они стоят впритык)
            {
                picked.TakeDamage(1); // тот, кто стоит на клетке, которую мы отметили как цель, получает 1 урон
                return true; // всё клёво, заклинание отработало, возвращаем true
            }
        }
        return false; // что-то пошло не так, и заклинание применить не получилось, возвращаем false
    }
}
public class RangedAttackSpell : Spell // это другое заклинание, которое такое же, как и предыдущее, но действует на расстоянии до 6 клеток
{
    public RangedAttackSpell() : base()
    {
        affectedCount = 1;
        rangeVisual = 6;
    }
    public override bool Use(Board board, Entity caster, List<Tile> affected)
    {
        Entity picked = board.GetEntityOnTile(affected[0]);
        if (picked != null && picked.fraction != caster.fraction)
        {
            bool found = false;
            int dist = caster.GetCurrTile().DistanceBetween(board, affected[0], ref found);
            if (found && dist < 6)
            {
                picked.TakeDamage(1);
                return true;
            }
        }
        return false;
    }
}