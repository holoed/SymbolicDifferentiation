using System;

namespace SymbolicDifferentiation.Core.Computation
{
    public class Atom
    {
        private readonly double _doubleValue;
        private readonly bool _boolValue;
        private enum UnionType
        {
            Double, Bool
        } UnionType _type;

        public Atom()
        {}

        public Atom(double value)
        {
            _type = UnionType.Double;
            _doubleValue = value;
        }

        public Atom(bool value)
        {
            _type = UnionType.Bool;
            _boolValue = value;
        }

        public static Atom operator+ (Atom left, Atom right)
        {
            EnsureTypeConsistency(left, right);
            return new Atom(left._doubleValue + right._doubleValue);
        }

        public static Atom operator -(Atom left, Atom right)
        {
            EnsureTypeConsistency(left, right);
            return new Atom(left._doubleValue - right._doubleValue);
        }

        public static Atom operator *(Atom left, Atom right)
        {
            EnsureTypeConsistency(left, right);
            return new Atom(left._doubleValue * right._doubleValue);
        }

        public static Atom operator /(Atom left, Atom right)
        {
            EnsureTypeConsistency(left, right);
            return new Atom(left._doubleValue / right._doubleValue);
        }

        public static Atom operator >(Atom left, Atom right)
        {
            EnsureTypeConsistency(left, right);
            return new Atom(left._doubleValue > right._doubleValue);
        }

        public static Atom operator <(Atom left, Atom right)
        {
            EnsureTypeConsistency(left, right);
            return new Atom(left._doubleValue < right._doubleValue);
        }

        public static Atom operator ^(Atom left, Atom right)
        {
            EnsureTypeConsistency(left, right);
            return new Atom(Math.Pow(left._doubleValue, right._doubleValue));
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
            return new Atom(value);
        }

        public static implicit operator Atom(bool value)
        {
            return new Atom(value);
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
            return _doubleValue.GetHashCode() ^ _boolValue.GetHashCode();
        }

        private static void EnsureTypeConsistency(Atom left, Atom right)
        {
            if (left._type != right._type)
                throw new InvalidCastException(String.Format("{0} and {1} types are not compatible", left._type, right._type));
        }
    }
}