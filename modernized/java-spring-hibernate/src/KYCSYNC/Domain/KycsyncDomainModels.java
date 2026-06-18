package src.KYCSYNC.Domain;

import java.math.BigDecimal;

/**
 * Domain model classes generated from COBOL data structures for KYCSYNC.
 */
public class KycsyncDomainModels {

    /**
     * Represents the main KYC record.
     */
    public static class KycRecord {
        // PIC X(20)
        private String customerId; // MaxLength(20)

        // PIC X(30)
        private String customerName; // MaxLength(30)

        // PIC 9(10)
        private long accountNumber;

        // PIC 9(5)V9(2) COMP-3
        private BigDecimal accountBalance; // Precision (7, 2)

        // PIC X(1)
        private String accountStatus; // MaxLength(1)

        // PIC 9(4)
        private int branchCode;

        // PIC X(10)
        private String lastUpdatedBy; // MaxLength(10)

        // PIC 9(8)
        private int lastUpdatedDate;

        // PIC 9(6)
        private int lastUpdatedTime;

        // Nested group for address details
        public static class Address {
            // PIC X(50)
            private String streetAddress; // MaxLength(50)

            // PIC X(20)
            private String city; // MaxLength(20)

            // PIC X(20)
            private String state; // MaxLength(20)

            // PIC X(10)
            private String postalCode; // MaxLength(10)

            // PIC X(2)
            private String country; // MaxLength(2)

            // Getters and Setters
            public String getStreetAddress() {
                return streetAddress;
            }

            public void setStreetAddress(String streetAddress) {
                this.streetAddress = streetAddress;
            }

            public String getCity() {
                return city;
            }

            public void setCity(String city) {
                this.city = city;
            }

            public String getState() {
                return state;
            }

            public void setState(String state) {
                this.state = state;
            }

            public String getPostalCode() {
                return postalCode;
            }

            public void setPostalCode(String postalCode) {
                this.postalCode = postalCode;
            }

            public String getCountry() {
                return country;
            }

            public void setCountry(String country) {
                this.country = country;
            }
        }

        private Address address;

        // Getters and Setters
        public String getCustomerId() {
            return customerId;
        }

        public void setCustomerId(String customerId) {
            this.customerId = customerId;
        }

        public String getCustomerName() {
            return customerName;
        }

        public void setCustomerName(String customerName) {
            this.customerName = customerName;
        }

        public long getAccountNumber() {
            return accountNumber;
        }

        public void setAccountNumber(long accountNumber) {
            this.accountNumber = accountNumber;
        }

        public BigDecimal getAccountBalance() {
            return accountBalance;
        }

        public void setAccountBalance(BigDecimal accountBalance) {
            this.accountBalance = accountBalance;
        }

        public String getAccountStatus() {
            return accountStatus;
        }

        public void setAccountStatus(String accountStatus) {
            this.accountStatus = accountStatus;
        }

        public int getBranchCode() {
            return branchCode;
        }

        public void setBranchCode(int branchCode) {
            this.branchCode = branchCode;
        }

        public String getLastUpdatedBy() {
            return lastUpdatedBy;
        }

        public void setLastUpdatedBy(String lastUpdatedBy) {
            this.lastUpdatedBy = lastUpdatedBy;
        }

        public int getLastUpdatedDate() {
            return lastUpdatedDate;
        }

        public void setLastUpdatedDate(int lastUpdatedDate) {
            this.lastUpdatedDate = lastUpdatedDate;
        }

        public int getLastUpdatedTime() {
            return lastUpdatedTime;
        }

        public void setLastUpdatedTime(int lastUpdatedTime) {
            this.lastUpdatedTime = lastUpdatedTime;
        }

        public Address getAddress() {
            return address;
        }

        public void setAddress(Address address) {
            this.address = address;
        }
    }

    /**
     * Enum for account status.
     */
    public enum AccountStatus {
        ACTIVE("A"), // Active account
        INACTIVE("I"), // Inactive account
        CLOSED("C"); // Closed account

        private final String code;

        AccountStatus(String code) {
            this.code = code;
        }

        public String getCode() {
            return code;
        }

        public static AccountStatus fromCode(String code) {
            for (AccountStatus status : AccountStatus.values()) {
                if (status.code.equals(code)) {
                    return status;
                }
            }
            throw new IllegalArgumentException("Invalid account status code: " + code);
        }
    }
}