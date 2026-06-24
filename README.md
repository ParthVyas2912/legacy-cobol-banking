# Legacy COBOL Banking Suite

A representative mainframe banking application: 8 COBOL programs, IMS database segments, CICS BMS maps, JCL, and copybooks.

## Programs

- ACCTMGR — account management
- ATMTXN — ATM transaction front-end (BMS)
- AUDITSRV — audit service
- BCHEOD — end-of-day batch
- FRDCHK — fraud check
- INTCALC — interest calculation
- KYCSYNC — KYC sync (IMS)
- STMTRPT — statement reporting
- TXNPOST — transaction posting

## Layout

- programs/  COBOL source
- copybooks/ shared record layouts
- bms/       CICS BMS map
- ims/       IMS DBD/PSB
- jcl/       batch JCL

This repo is the source for the SpecSuite COBOL modernization demo.
