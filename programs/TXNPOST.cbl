       IDENTIFICATION DIVISION.
       PROGRAM-ID. TXNPOST.
       AUTHOR. SPECSUITE-FIXTURE.
      *> Transaction-posting service. Reads TXNIN batch, applies to
      *> accounts via ACCTMGR linkage, writes audit log and DB2 row.
       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT TXN-IN ASSIGN TO TXNIN
               ORGANIZATION IS SEQUENTIAL
               ACCESS MODE IS SEQUENTIAL
               FILE STATUS IS WS-TXN-FS.
           SELECT AUDIT-OUT ASSIGN TO AUDITOUT
               ORGANIZATION IS SEQUENTIAL
               ACCESS MODE IS SEQUENTIAL
               FILE STATUS IS WS-AUD-FS.
       DATA DIVISION.
       FILE SECTION.
       FD  TXN-IN
           RECORDING MODE F.
           COPY TXNREC.
       FD  AUDIT-OUT.
       01  AUDIT-RECORD             PIC X(200).
       WORKING-STORAGE SECTION.
       01  WS-TXN-FS                PIC X(02).
       01  WS-AUD-FS                PIC X(02).
       01  WS-EOF-FLAG              PIC X(01) VALUE 'N'.
           88  WS-EOF               VALUE 'Y'.
       01  WS-CALL-AREA.
           05  CALL-FUNCTION        PIC X(04).
           05  CALL-ACCT-NUMBER     PIC 9(10).
           05  CALL-RESULT-CODE     PIC 9(02).
       01  WS-RECORD-COUNT          PIC 9(09) COMP-3 VALUE 0.
       01  WS-FAIL-COUNT            PIC 9(09) COMP-3 VALUE 0.
           EXEC SQL INCLUDE SQLCA END-EXEC.

       PROCEDURE DIVISION.
       0000-MAIN.
           PERFORM 1000-OPEN-FILES
           PERFORM 2000-PROCESS-TXNS UNTIL WS-EOF
           PERFORM 3000-CLOSE-FILES
           DISPLAY 'TXNPOST: PROCESSED ' WS-RECORD-COUNT
                                  ' FAILED ' WS-FAIL-COUNT
           GOBACK.

       1000-OPEN-FILES.
           OPEN INPUT  TXN-IN
                OUTPUT AUDIT-OUT.

       2000-PROCESS-TXNS.
           READ TXN-IN
               AT END SET WS-EOF TO TRUE
               NOT AT END
                   ADD 1 TO WS-RECORD-COUNT
                   PERFORM 2100-APPLY-TXN
           END-READ.

       2100-APPLY-TXN.
           MOVE 'UPDT'           TO CALL-FUNCTION
           MOVE TXN-ACCT-NUMBER  TO CALL-ACCT-NUMBER
           CALL 'ACCTMGR' USING WS-CALL-AREA
           IF CALL-RESULT-CODE = 0
               EXEC SQL INSERT INTO TXN_LEDGER
                          (TXN_ID, ACCT, AMOUNT, CODE, POSTED_AT)
                       VALUES (:TXN-ID, :TXN-ACCT-NUMBER,
                               :TXN-AMOUNT, :TXN-CODE,
                               CURRENT_TIMESTAMP)
               END-EXEC
               MOVE 'POSTED' TO AUDIT-RECORD
           ELSE
               ADD 1 TO WS-FAIL-COUNT
               MOVE 'FAILED' TO AUDIT-RECORD
           END-IF
           WRITE AUDIT-RECORD.

       3000-CLOSE-FILES.
           CLOSE TXN-IN AUDIT-OUT.

       END PROGRAM TXNPOST.
