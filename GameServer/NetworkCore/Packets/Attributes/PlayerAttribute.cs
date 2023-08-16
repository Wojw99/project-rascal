using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets.Attributes
{
    public abstract class PlayerAttribute
    {
        public int PlayerId;
    }

    public class Id : PlayerAttribute
    {
        public int Value;

        public Id(int value) { Value = value; }
    }

    public class Name : PlayerAttribute
    {
        public string Value;

        public Name(string value) { Value = value; }
    }

    public class Health : PlayerAttribute
    {
        public int Value;

        public Health(int value) { Value = value; }
    }


    public class Mana : PlayerAttribute
    {
        public int Value;

        public Mana(int value) { Value = value; }
    }

    public class PositionX : PlayerAttribute
    {
        public float Value;

        public PositionX(float value) { Value = value; }
    }

    public class PositionY : PlayerAttribute
    {
        public float Value;

        public PositionY(float value) { Value = value; }
    }

    public class PositionZ : PlayerAttribute
    {
        public float Value;

        public PositionZ(float value) { Value = value; }
    }

    public class Rotation : PlayerAttribute
    {
        public float Value;

        public Rotation(float value) { Value = value; }
    }

}
