      01  CUST-RECORD.
          05  CUST-ID              PIC 9(8).
          05  CUST-NAME.
              10  CUST-LAST-NAME   PIC X(30).
              10  CUST-FIRST-NAME  PIC X(20).
          05  CUST-TAX-ID          PIC 9(9).
          05  CUST-DOB             PIC 9(8).
          05  CUST-ADDRESS.
              10  CUST-STREET      PIC X(40).
              10  CUST-CITY        PIC X(25).
              10  CUST-STATE       PIC X(02).
              10  CUST-ZIP         PIC 9(05).
          05  CUST-PHONE           PIC X(15).
          05  CUST-RISK-SCORE      PIC 9(03) COMP-3.
          05  CUST-KYC-STATUS      PIC X(01).
              88  CUST-KYC-OK      VALUE 'Y'.
              88  CUST-KYC-PEND    VALUE 'P'.
              88  CUST-KYC-FAIL    VALUE 'N'.
          05  CUST-ACCT-COUNT      PIC 9(03) COMP-3.
          05  CUST-ACCT-LIST OCCURS 10 TIMES
                          INDEXED BY CUST-ACCT-IDX.
              10  CUST-ACCT-NUM    PIC 9(10).
              10  CUST-ACCT-ROLE   PIC X(02).
