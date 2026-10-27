       IDENTIFICATION DIVISION.
       PROGRAM-ID. FRDCHK.
       AUTHOR. SPECSUITE-FIXTURE.
      *> Fraud-screening service. CICS pseudo-conversational program.
      *> Reads transaction, scores against velocity rules, may freeze
      *> the account by calling ACCTMGR.
       ENVIRONMENT DIVISION.
       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-VELOCITY-COUNT       PIC 9(05) COMP-3 VALUE 0.
       01  WS-VELOCITY-AMT         PIC S9(11)V99 COMP-3 VALUE 0.
       01  WS-RISK-SCORE           PIC 9(03) COMP-3.
       01  WS-RESPONSE-CODE        PIC S9(04) COMP.
       01  WS-CALL-AREA.
           05  CALL-FUNCTION       PIC X(04).
           05  CALL-ACCT-NUMBER    PIC 9(10).
           05  CALL-RESULT-CODE    PIC 9(02).
           EXEC SQL INCLUDE SQLCA END-EXEC.

       LINKAGE SECTION.
           COPY TXNREC REPLACING ==TXN-RECORD== BY ==DFHCOMMAREA==.

       PROCEDURE DIVISION.
       0000-START.
           PERFORM 1000-COMPUTE-VELOCITY
           PERFORM 2000-SCORE-RISK
           IF WS-RISK-SCORE > 800
               PERFORM 3000-FREEZE-ACCOUNT
           END-IF
           EXEC CICS RETURN END-EXEC.

       1000-COMPUTE-VELOCITY.
           EXEC SQL
               SELECT COUNT(*), COALESCE(SUM(AMOUNT),0)
                 INTO :WS-VELOCITY-COUNT, :WS-VELOCITY-AMT
                 FROM TXN_LEDGER
                WHERE ACCT = :TXN-ACCT-NUMBER
                  AND POSTED_AT >= CURRENT_TIMESTAMP - 1 HOUR
           END-EXEC.

       2000-SCORE-RISK.
           MOVE 0 TO WS-RISK-SCORE
           IF WS-VELOCITY-COUNT > 10
               ADD 300 TO WS-RISK-SCORE
           END-IF
           IF WS-VELOCITY-AMT > 10000
               ADD 400 TO WS-RISK-SCORE
           END-IF
           IF TXN-AMOUNT > 50000
               ADD 250 TO WS-RISK-SCORE
           END-IF.

       3000-FREEZE-ACCOUNT.
           MOVE 'UPDT'           TO CALL-FUNCTION
           MOVE TXN-ACCT-NUMBER  TO CALL-ACCT-NUMBER
           EXEC CICS LINK PROGRAM('ACCTMGR')
                          COMMAREA(WS-CALL-AREA)
                          LENGTH(LENGTH OF WS-CALL-AREA)
                          RESP(WS-RESPONSE-CODE)
           END-EXEC
           EXEC SQL INSERT INTO FRAUD_FLAGS
                       (ACCT, SCORE, REASON, TS)
                    VALUES (:TXN-ACCT-NUMBER,
                            :WS-RISK-SCORE,
                            'AUTO-FREEZE',
                            CURRENT_TIMESTAMP)
           END-EXEC.

       END PROGRAM FRDCHK.
