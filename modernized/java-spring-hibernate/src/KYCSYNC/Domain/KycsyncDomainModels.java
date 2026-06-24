package src.KYCSYNC.Domain;

import java.math.BigDecimal;
import java.util.List;
import jakarta.validation.constraints.Size;
import jakarta.persistence.Column;

/**
 * Domain models for KYCSYNC program, generated from COBOL data structures.
 */
public class KycsyncDomainModels {

    /**
     * Represents the main KYC record structure.
     */
    public static class KYCRecord {

        @Size(max = 20)
        @Column(length = 20)
        private String customerId; // PIC X(20)

        @Size(max = 50)
        @Column(length = 50)
        private String customerName; // PIC X(50)

        @Column(precision = 15, scale = 2)
        private BigDecimal accountBalance; // PIC S9(13)V99 COMP-3

        @Column(precision = 7, scale = 2)
        private BigDecimal riskScore; // PIC 9(5)V99 COMP-3

        private CustomerStatus customerStatus; // 88-level values

        private List<Address> addresses; // OCCURS 5 TIMES

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

        public BigDecimal getAccountBalance() {
            return accountBalance;
        }

        public void setAccountBalance(BigDecimal accountBalance) {
            this.accountBalance = accountBalance;
        }

        public BigDecimal getRiskScore() {
            return riskScore;
        }

        public void setRiskScore(BigDecimal riskScore) {
            this.riskScore = riskScore;
        }

        public CustomerStatus getCustomerStatus() {
            return customerStatus;
        }

        public void setCustomerStatus(CustomerStatus customerStatus) {
            this.customerStatus = customerStatus;
        }

        public List<Address> getAddresses() {
            return addresses;
        }

        public void setAddresses(List<Address> addresses) {
            this.addresses = addresses;
        }

        /**
         * Enum for Customer Status (88-level values).
         */
        public enum CustomerStatus {
            ACTIVE("A"),
            INACTIVE("I"),
            SUSPENDED("S");

            private final String value;

            CustomerStatus(String value) {
                this.value = value;
            }

            public String getValue() {
                return value;
            }

            public static CustomerStatus fromValue(String value) {
                for (CustomerStatus status : values()) {
                    if (status.value.equals(value)) {
                        return status;
                    }
                }
                throw new IllegalArgumentException("Unknown value: " + value);
            }
        }

        /**
         * Represents an Address structure.
         */
        public static class Address {

            @Size(max = 100)
            @Column(length = 100)
            private String street; // PIC X(100)

            @Size(max = 50)
            @Column(length = 50)
            private String city; // PIC X(50)

            @Size(max = 20)
            @Column(length = 20)
            private String postalCode; // PIC X(20)

            @Size(max = 2)
            @Column(length = 2)
            private String country; // PIC X(2)

            // Getters and Setters
            public String getStreet() {
                return street;
            }

            public void setStreet(String street) {
                this.street = street;
            }

            public String getCity() {
                return city;
            }

            public void setCity(String city) {
                this.city = city;
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
    }
}