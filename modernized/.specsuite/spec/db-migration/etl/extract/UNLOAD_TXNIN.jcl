//TXNIN   JOB (ACCT,MIGR),'DATA EXTRACT',CLASS=A,MSGCLASS=X,
//             NOTIFY=&SYSUID
//*
//* MainframeForge — Extract TXNIN (sequential flat file) to staging
//*
//STEP010  EXEC PGM=IEBGENER
//SYSPRINT DD  SYSOUT=*
//SYSUT1   DD  DSN=MIGR.UNLOAD.TXNIN,DISP=SHR
//SYSUT2   DD  DSN=MIGR.UNLOAD.TXNIN.UNLOAD,
//             DISP=(NEW,CATLG,DELETE),
//             UNIT=SYSDA,
//             SPACE=(CYL,(50,25)),
//             DCB=(RECFM=FB,LRECL=129,BLKSIZE=12900)
//SYSIN    DD  DUMMY
//