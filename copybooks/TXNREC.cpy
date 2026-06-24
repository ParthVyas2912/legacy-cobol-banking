      01  TXN-RECORD.
          05  TXN-ID               PIC 9(12).
          05  TXN-ACCT-NUMBER      PIC 9(10).
          05  TXN-POST-DATE        PIC 9(8).
          05  TXN-EFFECTIVE-DATE   PIC 9(8).
          05  TXN-AMOUNT           PIC S9(11)V99 COMP-3.
          05  TXN-CODE             PIC X(03).
              88  TXN-DEPOSIT      VALUE 'DEP'.
              88  TXN-WITHDRAWAL   VALUE 'WDR'.
              88  TXN-TRANSFER     VALUE 'XFR'.
              88  TXN-FEE          VALUE 'FEE'.
              88  TXN-INTEREST     VALUE 'INT'.
              88  TXN-REVERSAL     VALUE 'REV'.
          05  TXN-CHANNEL          PIC X(04).
              88  TXN-ATM          VALUE 'ATM '.
              88  TXN-BRANCH       VALUE 'BRCH'.
              88  TXN-ONLINE       VALUE 'ONLN'.
              88  TXN-BATCH        VALUE 'BTCH'.
          05  TXN-REF-NUMBER       PIC X(20).
          05  TXN-DESCRIPTION      PIC X(40).
          05  TXN-STATUS           PIC X(01).
              88  TXN-PENDING      VALUE 'P'.
              88  TXN-POSTED       VALUE 'O'.
              88  TXN-FAILED       VALUE 'F'.
          05  TXN-BALANCE-AFTER    PIC S9(13)V99 COMP-3.
          05  TXN-USER-ID          PIC X(08).
