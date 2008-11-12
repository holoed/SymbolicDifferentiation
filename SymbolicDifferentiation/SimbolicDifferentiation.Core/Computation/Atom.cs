using System;

namespace SymbolicDifferentiation.Core.Computation
{
    public class Atom
    {
        private double _doubleValue;
        private bool _boolValue;

        public Atom()
        {}

        public Atom(double value)
        {
            _doubleValue = value;
        }

        public static Atom operator+ (Atom left, Atom right)
        {
            return new Atom {_doubleValue = left._doubleValue + right._doubleValue};
        }

        public static Atom operator -(Atom left, Atom right)
        {
            return new Atom { _doubleValue = left._doubleValue - right._doubleValue };
        }

        public static Atom operator *(Atom left, Atom right)
        {
            return new Atom { _doubleValue = left._doubleValue * right._doubleValue };
        }

        public static Atom operator /(Atom left, Atom right)
        {
            return new Atom { _doubleValue = left._doubleValue / right._doubleValue };
        }

        public static Atom operator >(Atom left, Atom right)
        {
            return new Atom { _boolValue = left._doubleValue > right._doubleValue };
        }

        public static Atom operator <(Atom left, Atom right)
        {
            return new Atom { _boolValue = left._doubleValue < right._doubleValue };
        }

        public static Atom operator ^(Atom left, Atom right)
        {
            return new Atom {_doubleValue = Math.Pow(left._doubleValue, right._doubleValue) };
        }

        public static implicit operator bool(Atom value)
        {
            return value._boolValue;
        }

        public static implicit operator Double(Atom value)
        {
            return value._doubleValue;
        }

        public static implicit operator Atom(double value)
        {
            return new Atom {_doubleValue = value};
        }

        public static implicit operator Atom(bool value)
        {
            return new Atom { _boolValue = value };
        }

        public override bool Equals(object obj)
        {
            var other = obj as Atom;
            return Equals(other);
        }

        public bool Equals(Atom obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj._doubleValue == _doubleValue;
        }

        public override int GetHashCode()
        {
            return _doubleValue.GetHashCode();
        }
    }
}