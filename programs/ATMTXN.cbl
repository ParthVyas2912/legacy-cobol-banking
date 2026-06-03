       IDENTIFICATION DIVISION.
       PROGRAM-ID. ATMTXN.
       AUTHOR. SPECSUITE-FIXTURE.
      *> ATM transaction screen handler. Receives BMS map, validates
      *> via FRDCHK + ACCTMGR, returns next screen.
       ENVIRONMENT DIVISION.
       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01  WS-RESPONSE-CODE        PIC S9(04) COMP.
       01  WS-CALL-AREA.
           05  CALL-FUNCTION       PIC X(04).
           05  CALL-ACCT-NUMBER    PIC 9(10).
           05  CALL-RESULT-CODE    PIC 9(02).
       01  WS-MAP-DATA.
           COPY ATMMAP.
       01  WS-TXN-DATA.
           COPY TXNREC.

       PROCEDURE DIVISION.
       0000-START.
           EXEC CICS HANDLE CONDITION
                          MAPFAIL(9000-MAP-ERROR)
                          ERROR(9100-FATAL)
           END-EXEC
           EXEC CICS RECEIVE MAP('ATMMAIN')
                             MAPSET('ATMMSET')
                             INTO(WS-MAP-DATA)
                             RESP(WS-RESPONSE-CODE)
           END-EXEC
           PERFORM 1000-VALIDATE
           PERFORM 2000-POST-TXN
           PERFORM 3000-SEND-NEXT
           EXEC CICS RETURN END-EXEC.

       1000-VALIDATE.
           MOVE 'READ'                TO CALL-FUNCTION
           MOVE MAP-ACCT-INPUT        TO CALL-ACCT-NUMBER
           EXEC CICS LINK PROGRAM('ACCTMGR')
                          COMMAREA(WS-CALL-AREA)
                          LENGTH(LENGTH OF WS-CALL-AREA)
                          RESP(WS-RESPONSE-CODE)
           END-EXEC.

       2000-POST-TXN.
           MOVE MAP-ACCT-INPUT TO TXN-ACCT-NUMBER
           MOVE MAP-AMT-INPUT  TO TXN-AMOUNT
           MOVE 'WDR'          TO TXN-CODE
           MOVE 'ATM '         TO TXN-CHANNEL
           EXEC CICS LINK PROGRAM('FRDCHK')
                          COMMAREA(WS-TXN-DATA)
                          LENGTH(LENGTH OF WS-TXN-DATA)
                          RESP(WS-RESPONSE-CODE)
           END-EXEC.

       3000-SEND-NEXT.
           MOVE 'TXN ACCEPTED'    TO MAP-MSG-OUT
           EXEC CICS SEND MAP('ATMNEXT')
                          MAPSET('ATMMSET')
                          FROM(WS-MAP-DATA)
                          ERASE
                          RESP(WS-RESPONSE-CODE)
           END-EXEC.

       9000-MAP-ERROR.
           MOVE 'INVALID MAP INPUT' TO MAP-MSG-OUT.

       9100-FATAL.
           EXEC CICS ABEND ABCODE('FATL') END-EXEC.

       END PROGRAM ATMTXN.
