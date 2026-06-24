       IDENTIFICATION DIVISION.
       PROGRAM-ID. KYCSYNC.
       AUTHOR. SPECSUITE-FIXTURE.
      *> Synchronises customer KYC state from IMS to DB2 audit table.
      *> Pure IMS DL/I + DB2 — no CICS, no flat files.
       ENVIRONMENT DIVISION.
       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-IMS-FUNC             PIC X(04).
       01  WS-CUST-IO-AREA.
           COPY CUSTREC.
       01  WS-PCB-MASK.
           05  PCB-DBD-NAME        PIC X(08).
           05  PCB-LEVEL           PIC X(02).
           05  PCB-STATUS-CODE     PIC X(02).
               88 PCB-OK           VALUE '  '.
               88 PCB-END-OF-DB    VALUE 'GB'.
           05  PCB-PROCOPT         PIC X(04).
           05  PCB-RSVD            PIC S9(05) COMP.
           05  PCB-SEG-NAME        PIC X(08).
           05  PCB-KFB-LEN         PIC S9(05) COMP.
           05  PCB-SENSEG-COUNT    PIC S9(05) COMP.
           05  PCB-KEY-FB          PIC X(80).
       01  WS-NEXT-COUNT           PIC 9(09) COMP-3 VALUE 0.
           EXEC SQL INCLUDE SQLCA END-EXEC.

       PROCEDURE DIVISION.
       0000-MAIN.
           MOVE 'GN  ' TO WS-IMS-FUNC
           PERFORM 1000-FETCH-LOOP UNTIL PCB-END-OF-DB
           DISPLAY 'KYCSYNC PROCESSED ' WS-NEXT-COUNT
           GOBACK.

       1000-FETCH-LOOP.
           CALL 'CBLTDLI' USING WS-IMS-FUNC
                                WS-PCB-MASK
                                WS-CUST-IO-AREA
           IF PCB-OK
               ADD 1 TO WS-NEXT-COUNT
               PERFORM 2000-UPSERT-KYC
           END-IF.

       2000-UPSERT-KYC.
           EXEC SQL MERGE INTO KYC_AUDIT AS T
                       USING (VALUES (:CUST-ID,
                                      :CUST-KYC-STATUS,
                                      :CUST-RISK-SCORE,
                                      CURRENT_TIMESTAMP))
                              AS S(CUST_ID, STATUS, SCORE, TS)
                          ON T.CUST_ID = S.CUST_ID
                        WHEN MATCHED THEN
                          UPDATE SET STATUS = S.STATUS,
                                     SCORE  = S.SCORE,
                                     TS     = S.TS
                        WHEN NOT MATCHED THEN
                          INSERT (CUST_ID, STATUS, SCORE, TS)
                          VALUES (S.CUST_ID, S.STATUS,
                                  S.SCORE, S.TS)
           END-EXEC.

       END PROGRAM KYCSYNC.
