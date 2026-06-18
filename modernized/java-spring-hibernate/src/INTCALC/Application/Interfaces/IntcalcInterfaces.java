package src.INTCALC.Application.Interfaces;

import src.INTCALC.Domain.IntcalcDomainModels.MainRecord;
import src.INTCALC.Domain.IntcalcDomainModels.MonthlyTransaction;
import java.math.BigDecimal;

/**
 * Application interfaces for the INTCALC program.
 */
public interface IntcalcInterfaces {

    /**
     * Initializes the program and sets up necessary configurations.
     */
    void initializeProgram();

    /**
     * Reads input records from the input file.
     * 
     * @return The main record read from the input file.
     */
    MainRecord readInputRecord();

    /**
     * Calculates the interest for the given account.
     * 
     * @param accountBalance The account balance.
     * @param interestRate The interest rate.
     * @return The calculated interest amount.
     */
    BigDecimal calculateInterest(BigDecimal accountBalance, BigDecimal interestRate);

    /**
     * Updates the account balance with the calculated interest.
     * 
     * @param mainRecord The main record to update.
     * @param interestAmount The calculated interest amount.
     */
    void updateAccountBalance(MainRecord mainRecord, BigDecimal interestAmount);

    /**
     * Writes the updated record to the output file.
     * 
     * @param mainRecord The main record to write.
     */
    void writeOutputRecord(MainRecord mainRecord);

    /**
     * Processes monthly transactions for the account.
     * 
     * @param monthlyTransactions Array of monthly transactions.
     * @return The total transaction amount for the year.
     */
    BigDecimal processMonthlyTransactions(MonthlyTransaction[] monthlyTransactions);

    /**
     * Finalizes the program and performs cleanup operations.
     */
    void finalizeProgram();
}