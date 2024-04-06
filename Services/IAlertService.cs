using AlertifyCore.Models;

namespace AlertifyCore.Services;

public interface IAlertService
{

    void Alert(AlertType type, string message);

    void SuccessAlert(string message);

    void WarningAlert(string message);

    void ErrorAlert(string message);

}
