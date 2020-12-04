using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Domain.Framework
{
    public abstract class Entity<T>
        where T : ValueObject
    {
        protected Entity(T id)
        {
            Id = id;
        }

        public T Id { get; private set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity<T> entity))
                return false;

            if (ReferenceEquals(this, entity))
                return true;

            if (GetType() != entity.GetType())
                return false;

            if (Id is null || entity.Id is null)
                return false;

            return Id == entity.Id;
        }

        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id.ToString()).GetHashCode();
        }
    }
}
