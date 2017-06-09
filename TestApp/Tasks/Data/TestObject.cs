using Mirage.Generators;
using Mirage.Generators.Default;
using System;

namespace TestApp.Tasks.Data
{
    public class TestObject
    {
        [BoolGenerator]
        public virtual bool BoolReference { get; set; }

        public virtual byte[] ByteArrayReference { get; set; }

        [ByteGenerator]
        public virtual byte ByteReference { get; set; }

        [CharGenerator]
        public virtual char CharReference { get; set; }

        [DecimalGenerator]
        public virtual decimal DecimalReference { get; set; }

        [DoubleGenerator]
        public virtual double DoubleReference { get; set; }

        [FloatGenerator]
        public virtual float FloatReference { get; set; }

        [GuidGenerator]
        public virtual Guid GuidReference { get; set; }

        [IntGenerator]
        public virtual int ID { get; set; }

        [IntGenerator]
        public virtual int IntReference { get; set; }

        [LongGenerator]
        public virtual long LongReference { get; set; }

        [StringGenerator]
        public virtual string NullStringReference { get; set; }

        [ShortGenerator]
        public virtual short ShortReference { get; set; }

        [StringGenerator]
        public virtual string StringReference { get; set; }
    }
}