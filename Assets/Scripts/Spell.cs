using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Spells // ��� ������������ ��� ����������, ����� ������ �������� ���������� - ���������� ���
{
    none, attack, rangedAttack
}


public class SpellsFactory // ��� ������� �������� ����������, ������� ����� ����� �� �������. ������� ������ � ����� �����������
{ 
    public static int AffectedCount(Spells spell)
    {
        return GetSpell(spell).affectedCount;
    }
    public static Spell GetSpell(Spells spell) // ���������� ������ ������������ ������� ���������� �� �������� enum, ��� � �������� ����
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


public abstract class Spell // ����� ����������. ������������ �� ����, ����� ������� ����� ����������, � �������������� ����������� �����
{
    public int affectedCount = 1; // ���������� ����� ��� ����������
    public int rangeVisual = 1; // ��������� ���������� (� �������)
    public abstract bool Use(Board board, Entity caster, List<Tile> affected);
}
public class AttackSpell: Spell
{
    public AttackSpell() : base() // ������������ ������ ���������� ��� ��, �� ������� �� ������������� rangeVisual � affectedCount
    {
        rangeVisual = 1; 
        affectedCount = 1;
    }
    public override bool Use(Board board, Entity caster, List<Tile> affected) // ��� ��� ���� �������������� �����
    {
        // ��������� - ��� �� ������ �������� ��� ��� ������ ����������. ���� �� �� ������ ������, ����� �� ��������� ���������� � ������ ����������� ��� ���
        // ���� ���������� ��������� �����, �� ����������� true. ���� ��� �� ������� ��������� (��������, ���������� ���� ������� ������ �� ������), ����������� false
        // board - ��� � ����� ����, ������� ��������� � �������. c�ster - ��� �� ��������, ������� ��������� ����������.
        // affected - ������ ����� ����������, �������������� ������  affectedCount

        Entity picked = board.GetEntityOnTile(affected[0]); // �������� ��������, ������� �� ������ ��������� � �������� ���� ��������
        if (picked != null && picked.fraction != caster.fraction) // ���� �� ��������� �������� ������ ���-�� ����� � ����� ���� ����, ��� �������� ����������
        {
            bool found = false; // ��� ����, ������� ��������� � ����� DistanceBetween. ���� ���� �� ���� �� ������, �� ���� false, ����� true
            int dist = caster.GetCurrTile().DistanceBetween(board, affected[0], ref found); // �������� ���������� ������ ����� ���, ��� ����� ����������, � ����� �����
            if (found && dist == 0) // ���� ���� ������ � ���������� ����� ���, ��� ����� ����������, � ����� �����, ����� 0 ������ (�� ���� ��� ����� �������)
            {
                picked.TakeDamage(1); // ���, ��� ����� �� ������, ������� �� �������� ��� ����, �������� 1 ����
                return true; // �� ����, ���������� ����������, ���������� true
            }
        }
        return false; // ���-�� ����� �� ���, � ���������� ��������� �� ����������, ���������� false
    }
}
public class RangedAttackSpell : Spell // ��� ������ ����������, ������� ����� ��, ��� � ����������, �� ��������� �� ���������� �� 6 ������
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