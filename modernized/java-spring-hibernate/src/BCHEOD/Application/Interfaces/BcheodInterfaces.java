package src.BCHEOD.Application.Interfaces;

import src.BCHEOD.Domain.BcheodDomainModels.Account;
import src.BCHEOD.Domain.BcheodDomainModels.Transaction;
import src.BCHEOD.Domain.BcheodDomainModels.Customer;

import java.math.BigDecimal;
import java.util.List;

public interface BcheodInterfaces {

    /**
     * Initialize the application and perform any required setup.
     */
    void initializeApplication();

    /**
     * Process a batch of transactions for a given account.
     *
     * @param account The account for which transactions are to be processed.
     * @param transactions The list of transactions to process.
     */
    void processTransactions(Account account, List<Transaction> transactions);

    /**
     * Calculate the total balance for a given customer across all accounts.
     *
     * @param customer The customer whose total balance is to be calculated.
     * @return The total balance across all accounts.
     */
    BigDecimal calculateTotalBalance(Customer customer);

    /**
     * Generate a report for a specific account.
     *
     * @param account The account for which the report is to be generated.
     * @return The generated report as a string.
     */
    String generateAccountReport(Account account);

    /**
     * Validate a transaction before processing.
     *
     * @param transaction The transaction to validate.
     * @return True if the transaction is valid, false otherwise.
     */
    boolean validateTransaction(Transaction transaction);

    /**
     * Update the status of an account.
     *
     * @param account The account whose status is to be updated.
     * @param status The new status to set.
     */
    void updateAccountStatus(Account account, Account.AccountStatus status);

    /**
     * Retrieve a customer by their unique identifier.
     *
     * @param customerId The unique identifier of the customer.
     * @return The customer object, or null if not found.
     */
    Customer getCustomerById(String customerId);

    /**
     * Retrieve an account by its account number.
     *
     * @param accountNumber The account number.
     * @return The account object, or null if not found.
     */
    Account getAccountByNumber(String accountNumber);

    /**
     * Retrieve a transaction by its transaction ID.
     *
     * @param transactionId The transaction ID.
     * @return The transaction object, or null if not found.
     */
    Transaction getTransactionById(String transactionId);

    /**
     * Save or update a customer record.
     *
     * @param customer The customer object to save or update.
     */
    void saveOrUpdateCustomer(Customer customer);

    /**
     * Save or update an account record.
     *
     * @param account The account object to save or update.
     */
    void saveOrUpdateAccount(Account account);

    /**
     * Save or update a transaction record.
     *
     * @param transaction The transaction object to save or update.
     */
    void saveOrUpdateTransaction(Transaction transaction);

    /**
     * Delete a customer record by their unique identifier.
     *
     * @param customerId The unique identifier of the customer to delete.
     */
    void deleteCustomerById(String customerId);

    /**
     * Delete an account record by its account number.
     *
     * @param accountNumber The account number of the account to delete.
     */
    void deleteAccountByNumber(String accountNumber);

    /**
     * Delete a transaction record by its transaction ID.
     *
     * @param transactionId The transaction ID of the transaction to delete.
     */
    void deleteTransactionById(String transactionId);
}