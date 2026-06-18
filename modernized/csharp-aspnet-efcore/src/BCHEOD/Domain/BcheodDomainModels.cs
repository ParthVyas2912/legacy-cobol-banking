using System;
using System.ComponentModel.DataAnnotations;

namespace BCHEOD.Domain
{
    public class BcheodDomainModels
    {
        public class ExampleRecord
        {
            [Precision(15, 2)]
            public decimal ExampleDecimalField { get; set; } // PIC S9(13)V99 COMP-3

            [MaxLength(20)]
            public string ExampleStringField { get; set; } // PIC X(20)

            public int ExampleIntegerField { get; set; } // PIC 9(5)

            public long ExampleLongField { get; set; } // PIC 9(10)

            public ExampleEnum ExampleEnumField { get; set; } // 88-level

            public ExampleGroup ExampleGroupField { get; set; } // GROUP item

            public ExampleNestedRecord[] ExampleArrayField { get; set; } // OCCURS 10 TIMES

            public ExampleRedefine ExampleRedefineField { get; set; } // REDEFINES
        }

        public enum ExampleEnum
        {
            Value1, // 88 VALUE '1'
            Value2, // 88 VALUE '2'
            Value3  // 88 VALUE '3'
        }

        public class ExampleGroup
        {
            [MaxLength(10)]
            public string GroupStringField { get; set; } // PIC X(10)

            [Precision(7, 2)]
            public decimal GroupDecimalField { get; set; } // PIC S9(5)V99 COMP-3
        }

        public class ExampleNestedRecord
        {
            [MaxLength(15)]
            public string NestedStringField { get; set; } // PIC X(15)

            public int NestedIntegerField { get; set; } // PIC 9(4)
        }

        public class ExampleRedefine
        {
            [MaxLength(5)]
            public string RedefineStringField { get; set; } // PIC X(5)

            [Precision(5, 2)]
            public decimal RedefineDecimalField { get; set; } // PIC S9(3)V99 COMP-3
        }
    }
}