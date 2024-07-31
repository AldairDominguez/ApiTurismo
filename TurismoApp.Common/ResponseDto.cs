namespace TurismoApp.Common;

public class ResponseDto
{
    public string Message { get; set; }
    private bool _isValid { get; set; }

    public bool IsValid => _isValid;
    public object Data { get; set; }

    public static ResponseDto Ok(object data = null ,string message = null)
    {
        ResponseDto response = new ResponseDto();
        response._isValid = true;
        response.Message = message ?? "Se ejecuto con exito";
        response.Data = data;
        return response;
    }

    public static ResponseDto Error(string message)
    {
        ResponseDto response = new ResponseDto();
        response._isValid = false;
        response.Message = message;
        return response;
    }
}