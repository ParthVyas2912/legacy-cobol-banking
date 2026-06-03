      01  ACCT-RECORD.
          05  ACCT-NUMBER          PIC 9(10).
          05  ACCT-CUSTOMER-ID     PIC 9(8).
          05  ACCT-OPENED-DATE     PIC 9(8).
          05  ACCT-TYPE            PIC X(02).
              88  ACCT-CHECKING    VALUE 'CK'.
              88  ACCT-SAVINGS     VALUE 'SV'.
              88  ACCT-LOAN        VALUE 'LN'.
          05  ACCT-STATUS          PIC X(01).
              88  ACCT-ACTIVE      VALUE 'A'.
              88  ACCT-FROZEN      VALUE 'F'.
              88  ACCT-CLOSED      VALUE 'C'.
          05  ACCT-BALANCE         PIC S9(13)V99 COMP-3.
          05  ACCT-AVAIL-BAL       PIC S9(13)V99 COMP-3.
          05  ACCT-INT-RATE        PIC 9(2)V9(4) COMP-3.
          05  ACCT-LAST-TXN-DT     PIC 9(8).
          05  ACCT-CURRENCY        PIC X(03).
          05  ACCT-BRANCH-CODE     PIC 9(4).
          05  ACCT-FLAGS           PIC X(04).
              88  ACCT-OVERDRAFT-OK  VALUE 'YYYY' 'YYNN' 'YYYN'.
          05  ACCT-FILLER          PIC X(50).
