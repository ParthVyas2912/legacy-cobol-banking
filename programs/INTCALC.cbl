       IDENTIFICATION DIVISION.
       PROGRAM-ID. INTCALC.
       AUTHOR. SPECSUITE-FIXTURE.
      *> Daily interest accrual calculator. Reads ACCTFILE, applies
      *> COMP-3 interest math, writes journal records for TXNPOST.
       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT ACCT-FILE ASSIGN TO ACCTFILE
               ORGANIZATION IS INDEXED
               ACCESS MODE IS SEQUENTIAL
               RECORD KEY IS ACCT-NUMBER
               FILE STATUS IS WS-ACCT-FS.
           SELECT INT-JOURNAL ASSIGN TO INTJRNL
               ORGANIZATION IS SEQUENTIAL
               FILE STATUS IS WS-JNL-FS.
       DATA DIVISION.
       FILE SECTION.
       FD  ACCT-FILE.
           COPY ACCTREC.
       FD  INT-JOURNAL
           RECORDING MODE F.
           COPY TXNREC.
       WORKING-STORAGE SECTION.
       01  WS-ACCT-FS              PIC X(02).
       01  WS-JNL-FS               PIC X(02).
       01  WS-EOF-FLAG             PIC X(01) VALUE 'N'.
           88  WS-EOF              VALUE 'Y'.
       01  WS-DAILY-RATE           PIC 9(2)V9(8) COMP-3.
       01  WS-DAILY-INTEREST       PIC S9(13)V99 COMP-3.
       01  WS-NEW-BALANCE          PIC S9(13)V99 COMP-3.
       01  WS-NEXT-TXN-ID          PIC 9(12) COMP-3 VALUE 1.

       PROCEDURE DIVISION.
       0000-MAIN.
           OPEN INPUT  ACCT-FILE
                OUTPUT INT-JOURNAL
           PERFORM 1000-LOOP UNTIL WS-EOF
           CLOSE ACCT-FILE INT-JOURNAL
           GOBACK.

       1000-LOOP.
           READ ACCT-FILE
               AT END SET WS-EOF TO TRUE
               NOT AT END PERFORM 2000-ACCRUE
           END-READ.

       2000-ACCRUE.
           IF ACCT-ACTIVE AND ACCT-SAVINGS
               COMPUTE WS-DAILY-RATE = ACCT-INT-RATE / 36500
               COMPUTE WS-DAILY-INTEREST ROUNDED =
                   ACCT-BALANCE * WS-DAILY-RATE
               IF WS-DAILY-INTEREST > 0
                   COMPUTE WS-NEW-BALANCE =
                       ACCT-BALANCE + WS-DAILY-INTEREST
                   MOVE WS-NEXT-TXN-ID    TO TXN-ID
                   MOVE ACCT-NUMBER       TO TXN-ACCT-NUMBER
                   MOVE WS-DAILY-INTEREST TO TXN-AMOUNT
                   MOVE 'INT'             TO TXN-CODE
                   MOVE 'BTCH'            TO TXN-CHANNEL
                   MOVE 'O'               TO TXN-STATUS
                   MOVE WS-NEW-BALANCE    TO TXN-BALANCE-AFTER
                   MOVE 'DAILY INT ACCRUAL'TO TXN-DESCRIPTION
                   WRITE TXN-RECORD
                   ADD 1 TO WS-NEXT-TXN-ID
               END-IF
           END-IF.

       END PROGRAM INTCALC.
