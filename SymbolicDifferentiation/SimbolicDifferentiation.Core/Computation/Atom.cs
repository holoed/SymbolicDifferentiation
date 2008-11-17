#region License

/* ****************************************************************************
 * Copyright (c) Edmondo Pentangelo. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public License. 
 * A copy of the license can be found in the License.html file at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 * ***************************************************************************/

#endregion

using System;

namespace SymbolicDifferentiation.Core.Computation
{
    public class Atom
    {
        private readonly double _doubleValue;
        private readonly bool _boolValue;
        private enum UnionType { Double, Bool } readonly UnionType _type;

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
            if (obj is bool) return _type == UnionType.Bool && obj.Equals(_boolValue);
            if (obj is double) return _type == UnionType.Double && obj.Equals(_doubleValue);

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

        public override string ToString()
        {
            switch (_type)
            {
                case UnionType.Double:
                    return _doubleValue.ToString();
                case UnionType.Bool:
                    return _boolValue.ToString();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}