       IDENTIFICATION DIVISION.
       PROGRAM-ID. BCHEOD.
       AUTHOR. SPECSUITE-FIXTURE.
      *> End-of-day batch driver. Sequences INTCALC then TXNPOST and
      *> writes a status row to DB2. Deliberately uses ALTER (a Phase
      *> 17 dead-modernization marker).
       ENVIRONMENT DIVISION.
       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-STATUS-CODE          PIC X(02) VALUE 'OK'.
       01  WS-CALL-AREA.
           05  CALL-FUNCTION       PIC X(04).
           05  CALL-ACCT-NUMBER    PIC 9(10).
           05  CALL-RESULT-CODE    PIC 9(02).
           EXEC SQL INCLUDE SQLCA END-EXEC.

       PROCEDURE DIVISION.
       0000-DRIVE.
           ALTER 1000-NEXT-STEP TO PROCEED TO 2000-INT-STEP.
           PERFORM 9000-MARK-START
           GO TO 1000-NEXT-STEP.

       1000-NEXT-STEP.
           GO TO 2000-INT-STEP.

       2000-INT-STEP.
           CALL 'INTCALC'
           ALTER 1000-NEXT-STEP TO PROCEED TO 3000-POST-STEP
           GO TO 1000-NEXT-STEP.

       3000-POST-STEP.
           CALL 'TXNPOST'
           ALTER 1000-NEXT-STEP TO PROCEED TO 4000-DONE
           GO TO 1000-NEXT-STEP.

       4000-DONE.
           PERFORM 9100-MARK-END
           GOBACK.

       9000-MARK-START.
           EXEC SQL INSERT INTO EOD_RUNS (STARTED_AT, STATUS)
                    VALUES (CURRENT_TIMESTAMP, 'RUN')
           END-EXEC.

       9100-MARK-END.
           EXEC SQL UPDATE EOD_RUNS
                       SET ENDED_AT = CURRENT_TIMESTAMP,
                           STATUS   = :WS-STATUS-CODE
                     WHERE STARTED_AT = (SELECT MAX(STARTED_AT)
                                           FROM EOD_RUNS)
           END-EXEC.

       END PROGRAM BCHEOD.
