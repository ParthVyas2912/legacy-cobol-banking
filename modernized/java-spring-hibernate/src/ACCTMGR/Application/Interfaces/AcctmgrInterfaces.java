package src.ACCTMGR.Application.Interfaces;

import src.ACCTMGR.Domain.AcctmgrDomainModels.Account;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Transaction;
import src.ACCTMGR.Domain.AcctmgrDomainModels.Customer;

import java.math.BigDecimal;
import java.util.List;

/**
 * Interfaces for the ACCTMGR application, generated from the COBOL program.
 * Each paragraph in the COBOL program is represented as a method.
 * File I/O operations are represented as repository interfaces.
 * CICS operations are represented as service interfaces.
 */
public interface AcctmgrInterfaces {

    /**
     * Interface for account-related operations.
     */
    interface AccountRepository {
        /**
         * Reads an account by its ID.
         *
         * @param accountId The ID of the account to read.
         * @return The account details.
         */
        Account readAccountById(String accountId);

        /**
         * Writes account details to the repository.
         *
         * @param account The account details to write.
         */
        void writeAccount(Account account);

        /**
         * Deletes an account by its ID.
         *
         * @param accountId The ID of the account to delete.
         */
        void deleteAccount(String accountId);
    }

    /**
     * Interface for transaction-related operations.
     */
    interface TransactionRepository {
        /**
         * Reads transactions for a specific account.
         *
         * @param accountId The ID of the account.
         * @return A list of transactions for the account.
         */
        List<Transaction> readTransactionsByAccountId(String accountId);

        /**
         * Writes a transaction to the repository.
         *
         * @param transaction The transaction details to write.
         */
        void writeTransaction(Transaction transaction);
    }

    /**
     * Interface for customer-related operations.
     */
    interface CustomerRepository {
        /**
         * Reads a customer by their ID.
         *
         * @param customerId The ID of the customer to read.
         * @return The customer details.
         */
        Customer readCustomerById(String customerId);

        /**
         * Writes customer details to the repository.
         *
         * @param customer The customer details to write.
         */
        void writeCustomer(Customer customer);
    }

    /**
     * Interface for account management services.
     */
    interface AccountManagementService {
        /**
         * Opens a new account.
         *
         * @param account The account details to open.
         */
        void openAccount(Account account);

        /**
         * Closes an account.
         *
         * @param accountId The ID of the account to close.
         */
        void closeAccount(String accountId);

        /**
         * Updates the balance of an account.
         *
         * @param accountId The ID of the account.
         * @param amount    The amount to update the balance by.
         */
        void updateAccountBalance(String accountId, BigDecimal amount);
    }

    /**
     * Interface for transaction management services.
     */
    interface TransactionManagementService {
        /**
         * Processes a deposit transaction.
         *
         * @param accountId The ID of the account.
         * @param amount    The amount to deposit.
         */
        void processDeposit(String accountId, BigDecimal amount);

        /**
         * Processes a withdrawal transaction.
         *
         * @param accountId The ID of the account.
         * @param amount    The amount to withdraw.
         */
        void processWithdrawal(String accountId, BigDecimal amount);

        /**
         * Processes a transfer transaction.
         *
         * @param fromAccountId The ID of the source account.
         * @param toAccountId   The ID of the destination account.
         * @param amount        The amount to transfer.
         */
        void processTransfer(String fromAccountId, String toAccountId, BigDecimal amount);
    }

    /**
     * Interface for customer management services.
     */
    interface CustomerManagementService {
        /**
         * Registers a new customer.
         *
         * @param customer The customer details to register.
         */
        void registerCustomer(Customer customer);

        /**
         * Updates customer details.
         *
         * @param customer The updated customer details.
         */
        void updateCustomer(Customer customer);

        /**
         * Deletes a customer by their ID.
         *
         * @param customerId The ID of the customer to delete.
         */
        void deleteCustomer(String customerId);
    }

    /**
     * Main application interface for executing the ACCTMGR program.
     */
    interface AcctmgrApplication {
        /**
         * Executes the main ACCTMGR program logic.
         */
        void execute();
    }
}