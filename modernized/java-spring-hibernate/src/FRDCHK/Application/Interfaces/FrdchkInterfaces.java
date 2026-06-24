package src.FRDCHK.Application.Interfaces;

import src.FRDCHK.Domain.FrdchkDomainModels.FraudCheckRequest;
import src.FRDCHK.Domain.FrdchkDomainModels.FraudCheckDetail;
import java.util.List;

public interface FrdchkInterfaces {

    /**
     * Initialize the fraud check process.
     */
    void initializeFraudCheck();

    /**
     * Validate the input data for the fraud check process.
     *
     * @param request The fraud check request containing transaction details.
     * @return true if the input data is valid, false otherwise.
     */
    boolean validateInputData(FraudCheckRequest request);

    /**
     * Perform the fraud check logic on the provided request.
     *
     * @param request The fraud check request containing transaction details.
     * @return A list of fraud check details with the results of the fraud check.
     */
    List<FraudCheckDetail> performFraudCheck(FraudCheckRequest request);

    /**
     * Finalize the fraud check process and perform any necessary cleanup.
     */
    void finalizeFraudCheck();
}