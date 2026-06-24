       IDENTIFICATION DIVISION.
       PROGRAM-ID. ACCTMGR.
       AUTHOR. SPECSUITE-FIXTURE.
       INSTALLATION. SPECSUITE-BANKING.
       DATE-WRITTEN. 2024-01-01.
      *> Account master CRUD service. CICS-based; updates ACCTFILE.
       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT ACCT-FILE ASSIGN TO ACCTFILE
               ORGANIZATION IS INDEXED
               ACCESS MODE IS RANDOM
               RECORD KEY IS ACCT-NUMBER
               FILE STATUS IS WS-ACCT-FS.
       DATA DIVISION.
       FILE SECTION.
       FD  ACCT-FILE.
           COPY ACCTREC.
       WORKING-STORAGE SECTION.
       01  WS-ACCT-FS              PIC X(02).
       01  WS-RESPONSE-CODE        PIC S9(04) COMP.
       01  WS-MAP-AREA.
           COPY ACCTREC REPLACING ==ACCT-== BY ==MAP-ACCT-==.
       01  WS-CUST-LOOKUP-AREA.
           COPY CUSTREC.
       01  WS-SQL-STATE            PIC X(05).
           EXEC SQL INCLUDE SQLCA END-EXEC.

       LINKAGE SECTION.
       01  DFHCOMMAREA.
           05  COMM-FUNCTION       PIC X(04).
               88  COMM-CREATE     VALUE 'CRTE'.
               88  COMM-READ       VALUE 'READ'.
               88  COMM-UPDATE     VALUE 'UPDT'.
               88  COMM-CLOSE      VALUE 'CLSE'.
           05  COMM-ACCT-NUMBER    PIC 9(10).
           05  COMM-RESULT-CODE    PIC 9(02).

       PROCEDURE DIVISION.
       MAIN-ROUTINE SECTION.
       0000-START.
           EVALUATE TRUE
               WHEN COMM-CREATE
                   PERFORM 1000-CREATE-ACCOUNT
               WHEN COMM-READ
                   PERFORM 2000-READ-ACCOUNT
               WHEN COMM-UPDATE
                   PERFORM 3000-UPDATE-ACCOUNT
               WHEN COMM-CLOSE
                   PERFORM 4000-CLOSE-ACCOUNT
               WHEN OTHER
                   MOVE 99 TO COMM-RESULT-CODE
           END-EVALUATE
           EXEC CICS RETURN END-EXEC.

       1000-CREATE-ACCOUNT.
           MOVE COMM-ACCT-NUMBER TO ACCT-NUMBER
           MOVE ZEROES           TO ACCT-BALANCE
           MOVE 'A'              TO ACCT-STATUS
           EXEC CICS WRITE FILE('ACCTFILE')
                          FROM(ACCT-RECORD)
                          RIDFLD(ACCT-NUMBER)
                          RESP(WS-RESPONSE-CODE)
           END-EXEC
           IF WS-RESPONSE-CODE = 0
               EXEC SQL INSERT INTO ACCT_AUDIT
                          (ACCT_NUM, ACTION, ACTOR, TS)
                       VALUES (:ACCT-NUMBER, 'CREATE',
                               :COMM-FUNCTION, CURRENT_TIMESTAMP)
               END-EXEC
               MOVE 0 TO COMM-RESULT-CODE
           ELSE
               MOVE 1 TO COMM-RESULT-CODE
           END-IF.

       2000-READ-ACCOUNT.
           MOVE COMM-ACCT-NUMBER TO ACCT-NUMBER
           EXEC CICS READ FILE('ACCTFILE')
                          INTO(ACCT-RECORD)
                          RIDFLD(ACCT-NUMBER)
                          RESP(WS-RESPONSE-CODE)
           END-EXEC
           IF WS-RESPONSE-CODE = 0
               MOVE 0 TO COMM-RESULT-CODE
           ELSE
               MOVE 2 TO COMM-RESULT-CODE
           END-IF.

       3000-UPDATE-ACCOUNT.
           MOVE COMM-ACCT-NUMBER TO ACCT-NUMBER
           EXEC CICS READ FILE('ACCTFILE')
                          INTO(ACCT-RECORD)
                          RIDFLD(ACCT-NUMBER)
                          UPDATE
                          RESP(WS-RESPONSE-CODE)
           END-EXEC
           IF WS-RESPONSE-CODE = 0
               PERFORM 3100-APPLY-UPDATES
               EXEC CICS REWRITE FILE('ACCTFILE')
                                 FROM(ACCT-RECORD)
                                 RESP(WS-RESPONSE-CODE)
               END-EXEC
           END-IF
           IF WS-RESPONSE-CODE = 0
               MOVE 0 TO COMM-RESULT-CODE
           ELSE
               MOVE 3 TO COMM-RESULT-CODE
           END-IF.

       3100-APPLY-UPDATES.
           MOVE MAP-ACCT-BALANCE   TO ACCT-BALANCE
           MOVE MAP-ACCT-AVAIL-BAL TO ACCT-AVAIL-BAL.

       4000-CLOSE-ACCOUNT.
           MOVE COMM-ACCT-NUMBER TO ACCT-NUMBER
           EXEC CICS READ FILE('ACCTFILE')
                          INTO(ACCT-RECORD)
                          RIDFLD(ACCT-NUMBER)
                          UPDATE
                          RESP(WS-RESPONSE-CODE)
           END-EXEC
           IF WS-RESPONSE-CODE = 0
               MOVE 'C' TO ACCT-STATUS
               EXEC CICS REWRITE FILE('ACCTFILE')
                                 FROM(ACCT-RECORD)
                                 RESP(WS-RESPONSE-CODE)
               END-EXEC
               EXEC SQL UPDATE CUSTOMER
                          SET ACTIVE_ACCT_COUNT = ACTIVE_ACCT_COUNT - 1
                        WHERE CUST_ID = :ACCT-CUSTOMER-ID
               END-EXEC
               MOVE 0 TO COMM-RESULT-CODE
           ELSE
               MOVE 4 TO COMM-RESULT-CODE
           END-IF.

       END PROGRAM ACCTMGR.
