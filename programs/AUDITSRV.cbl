       IDENTIFICATION DIVISION.
       PROGRAM-ID. AUDITSRV.
       AUTHOR. SPECSUITE-FIXTURE.
      *> Centralised audit-log service. CICS-callable. Persists to a
      *> VSAM file (a Phase 17 vsam-to-relational candidate).
       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT AUDIT-FILE ASSIGN TO AUDITVSAM
               ORGANIZATION IS INDEXED
               ACCESS MODE IS DYNAMIC
               RECORD KEY IS AUDIT-KEY
               ALTERNATE RECORD KEY IS AUDIT-ACCT-NUMBER
                   WITH DUPLICATES
               FILE STATUS IS WS-FS.
       DATA DIVISION.
       FILE SECTION.
       FD  AUDIT-FILE.
       01  AUDIT-RECORD.
           05  AUDIT-KEY           PIC X(20).
           05  AUDIT-ACCT-NUMBER   PIC 9(10).
           05  AUDIT-ACTION        PIC X(16).
           05  AUDIT-USER          PIC X(08).
           05  AUDIT-TS            PIC X(26).
           05  AUDIT-DETAIL        PIC X(120).
       WORKING-STORAGE SECTION.
       01  WS-FS                   PIC X(02).
       01  WS-RESPONSE-CODE        PIC S9(04) COMP.

       LINKAGE SECTION.
       01  DFHCOMMAREA.
           05  L-AUDIT-KEY         PIC X(20).
           05  L-AUDIT-ACCT-NUMBER PIC 9(10).
           05  L-AUDIT-ACTION      PIC X(16).
           05  L-AUDIT-USER        PIC X(08).
           05  L-AUDIT-TS          PIC X(26).
           05  L-AUDIT-DETAIL      PIC X(120).
           05  L-RESULT-CODE       PIC 9(02).

       PROCEDURE DIVISION.
       0000-START.
           MOVE L-AUDIT-KEY         TO AUDIT-KEY
           MOVE L-AUDIT-ACCT-NUMBER TO AUDIT-ACCT-NUMBER
           MOVE L-AUDIT-ACTION      TO AUDIT-ACTION
           MOVE L-AUDIT-USER        TO AUDIT-USER
           MOVE L-AUDIT-TS          TO AUDIT-TS
           MOVE L-AUDIT-DETAIL      TO AUDIT-DETAIL
           EXEC CICS WRITE FILE('AUDITVSAM')
                          FROM(AUDIT-RECORD)
                          RIDFLD(AUDIT-KEY)
                          RESP(WS-RESPONSE-CODE)
           END-EXEC
           IF WS-RESPONSE-CODE = 0
               MOVE 0 TO L-RESULT-CODE
           ELSE
               MOVE 9 TO L-RESULT-CODE
           END-IF
           EXEC CICS RETURN END-EXEC.

       END PROGRAM AUDITSRV.
