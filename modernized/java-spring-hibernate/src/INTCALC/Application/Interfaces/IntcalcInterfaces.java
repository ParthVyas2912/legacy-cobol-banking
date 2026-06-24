package src.INTCALC.Application.Interfaces;

import src.INTCALC.Domain.IntcalcDomainModels.Account;
import src.INTCALC.Domain.IntcalcDomainModels.InterestCalculation;
import src.INTCALC.Domain.IntcalcDomainModels.Transaction;
import src.INTCALC.Domain.IntcalcDomainModels.Customer;

import java.math.BigDecimal;
import java.util.List;

public interface IntcalcInterfaces {

    /**
     * Initialize the program and set up necessary configurations.
     */
    void initializeProgram();

    /**
     * Read account data from the repository.
     * 
     * @param accountNumber The account number to retrieve.
     * @return The account details.
     */
    Account readAccountData(String accountNumber);

    /**
     * Calculate interest for a given account.
     * 
     * @param account The account for which interest is to be calculated.
     * @param interestRate The interest rate to be applied.
     * @return The calculated interest details.
     */
    InterestCalculation calculateInterest(Account account, BigDecimal interestRate);

    /**
     * Update account balance after interest calculation.
     * 
     * @param account The account to be updated.
     * @param calculatedInterest The calculated interest to be added to the account balance.
     */
    void updateAccountBalance(Account account, BigDecimal calculatedInterest);

    /**
     * Process a transaction for an account.
     * 
     * @param transaction The transaction details.
     * @return The updated account details after processing the transaction.
     */
    Account processTransaction(Transaction transaction);

    /**
     * Generate a report for a customer, including all associated accounts and balances.
     * 
     * @param customerId The customer ID for which the report is to be generated.
     * @return The customer details including accounts and balances.
     */
    Customer generateCustomerReport(String customerId);

    /**
     * Finalize the program and perform cleanup operations.
     */
    void finalizeProgram();
}