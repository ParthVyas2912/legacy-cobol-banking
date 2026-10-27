       IDENTIFICATION DIVISION.
       PROGRAM-ID. STMTRPT.
       AUTHOR. SPECSUITE-FIXTURE.
      *> Monthly statement report. Joins DB2 transaction history with
      *> the IMS customer database; emits formatted statement records.
       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT STMT-FILE ASSIGN TO STMTOUT
               ORGANIZATION IS SEQUENTIAL
               FILE STATUS IS WS-STMT-FS.
       DATA DIVISION.
       FILE SECTION.
       FD  STMT-FILE
           RECORDING MODE F.
       01  STMT-LINE.
           05  STMT-ACCT-NUM       PIC 9(10).
           05  FILLER              PIC X(02) VALUE SPACES.
           05  STMT-DESCRIPTION    PIC X(40).
           05  FILLER              PIC X(02) VALUE SPACES.
           05  STMT-AMOUNT         PIC -Z(11)9.99.
           05  STMT-BALANCE        PIC -Z(13)9.99.
           05  FILLER              PIC X(20) VALUE SPACES.
       WORKING-STORAGE SECTION.
       01  WS-STMT-FS              PIC X(02).
       01  WS-CURRENT-ACCT         PIC 9(10) VALUE 0.
       01  WS-SQLCODE-MIRROR       PIC S9(9) COMP.
       01  WS-IMS-FUNC             PIC X(04).
           COPY CUSTREC REPLACING ==CUST-RECORD==
                        BY ==WS-CUST-IO-AREA==.
       01  WS-CUST-PCB-MASK.
           05  CUST-DBD-NAME       PIC X(08).
           05  CUST-LEVEL          PIC X(02).
           05  CUST-STATUS         PIC X(02).
           05  CUST-PROCOPT        PIC X(04).
           05  CUST-RSVD           PIC S9(05) COMP.
           05  CUST-SEG-NAME       PIC X(08).
           05  CUST-KFB-LEN        PIC S9(05) COMP.
           05  CUST-SENSEG-COUNT   PIC S9(05) COMP.
           05  CUST-KEY-FB         PIC X(80).
       01  WS-CUST-SSA.
           05  CUST-SSA-NAME       PIC X(08) VALUE 'CUSTOMER'.
           05  FILLER              PIC X(01) VALUE '('.
           05  CUST-SSA-FLD        PIC X(08) VALUE 'CUSTID  '.
           05  FILLER              PIC X(02) VALUE ' ='.
           05  CUST-SSA-VAL        PIC 9(08).
           05  FILLER              PIC X(01) VALUE ')'.
           EXEC SQL INCLUDE SQLCA END-EXEC.
           EXEC SQL DECLARE STMT-CSR CURSOR WITH HOLD FOR
               SELECT ACCT, AMOUNT, BALANCE_AFTER, DESCRIPTION
                 FROM TXN_LEDGER
                WHERE ACCT = :WS-CURRENT-ACCT
                  AND POSTED_AT >= CURRENT_DATE - 30 DAYS
                ORDER BY POSTED_AT
           END-EXEC.

       PROCEDURE DIVISION.
       0000-MAIN.
           OPEN OUTPUT STMT-FILE
           PERFORM 1000-FETCH-CUSTOMERS
           CLOSE STMT-FILE
           GOBACK.

       1000-FETCH-CUSTOMERS.
           MOVE 'GN  ' TO WS-IMS-FUNC
           PERFORM 1100-IMS-GN-LOOP UNTIL CUST-STATUS = 'GB'.

       1100-IMS-GN-LOOP.
           CALL 'CBLTDLI' USING WS-IMS-FUNC
                                WS-CUST-PCB-MASK
                                WS-CUST-IO-AREA
           IF CUST-STATUS = '  '
               MOVE CUST-ID TO CUST-SSA-VAL
               PERFORM 2000-OPEN-CURSOR
               PERFORM 3000-FETCH-LOOP UNTIL SQLCODE NOT = 0
               PERFORM 4000-CLOSE-CURSOR
           END-IF.

       2000-OPEN-CURSOR.
           MOVE 0 TO SQLCODE
           PERFORM 2050-COMPUTE-RANGE
           EXEC SQL OPEN STMT-CSR END-EXEC.

       2050-COMPUTE-RANGE.
           IF CUST-ACCT-COUNT > 0
               MOVE CUST-ACCT-NUM(1) TO WS-CURRENT-ACCT
           ELSE
               MOVE 0 TO WS-CURRENT-ACCT
           END-IF.

       3000-FETCH-LOOP.
           EXEC SQL FETCH STMT-CSR
                    INTO :STMT-ACCT-NUM,
                         :STMT-AMOUNT,
                         :STMT-BALANCE,
                         :STMT-DESCRIPTION
           END-EXEC
           IF SQLCODE = 0
               WRITE STMT-LINE
           END-IF.

       4000-CLOSE-CURSOR.
           EXEC SQL CLOSE STMT-CSR END-EXEC.

       END PROGRAM STMTRPT.
