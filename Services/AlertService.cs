using AlertifyCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace AlertifyCore.Services;

public class AlertService : IAlertService
{
    #region Fields
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
    #endregion

    #region Ctor
    public AlertService(IHttpContextAccessor httpContextAccessor,
                       ITempDataDictionaryFactory tempDataDictionaryFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _tempDataDictionaryFactory = tempDataDictionaryFactory;
    }
    #endregion

    #region  Methods
    public void Alert(AlertType type, string message)
    {
        PrepareTempData(type, message);
    }

    public void ErrorAlert(string message)
    {
        PrepareTempData(AlertType.Error, message);
    }

    public void SuccessAlert(string message)
    {
        PrepareTempData(AlertType.Success, message);
    }

    public void WarningAlert(string message)
    {
        PrepareTempData(AlertType.Warning, message);
    }
    #endregion

    private  void PrepareTempData(AlertType type, string message, bool encode = true)
    {
        var context = _httpContextAccessor.HttpContext;
        var tempData = _tempDataDictionaryFactory.GetTempData(context);

        //Messages have stored in a serialized list
        var messages = tempData.ContainsKey(AlertifyCoreDefaults.AlertListKey)
            ? JsonConvert.DeserializeObject<IList<AlertData>>(tempData[AlertifyCoreDefaults.AlertListKey].ToString())
            : new List<AlertData>();

        messages.Add(new AlertData
        {
            Message = message,
            Type = type,        
        });

        tempData[AlertifyCoreDefaults.AlertListKey] = JsonConvert.SerializeObject(messages);
    }
}
