using WMS.Models.Common.Pagination;

namespace WMS.Models.Common;

public class ResponseModel<T>
{
    public bool Success => !Errors.Any() && StatusCode == 200;
    public int StatusCode { get; set; } = 200;
    public T Result { get; set; }
    public List<string> Messages { get; set; }
    public List<string> Errors { get; set; }
    public PaginationResultInfo ResultInfo { get; set; }

    public ResponseModel()
    {
        Errors = new List<string>();
        Messages = new List<string>();
    }

    #region Errors

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    public void AddErrors(List<string> errors)
    {
        Errors.AddRange(errors);
    }

    public void AddErrors(Dictionary<string, List<string>> errors)
    {
        foreach (var item in errors)
        {
            AddErrors(item.Value);
        }
    }

    #endregion

    public ResponseModel<T> Ok(T data)
    {
        StatusCode = 200;
        Result = data;

        return this;
    }

    public ResponseModel<T> Ok(T data, PaginationResultInfo paginationResultInfo)
    {
        Result = data;
        ResultInfo = paginationResultInfo;

        return this;
    }

    public ResponseModel<T> Created(T data)
    {
        StatusCode = 201;
        Result = data;

        return this;
    }

    public ResponseModel<T> Accepted(T data)
    {
        StatusCode = 202;
        Result = data;

        return this;
    }

    public ResponseModel<T> NotModified()
    {
        StatusCode = 304;

        return this;
    }

    public ResponseModel<T> BadRequest()
    {
        StatusCode = 400;

        return this;
    }

    public ResponseModel<T> BadRequest(string message)
    {
        StatusCode = 400;
        AddError(message);

        return this;
    }

    public ResponseModel<T> Unauthorized()
    {
        StatusCode = 401;

        return this;
    }

    public ResponseModel<T> Unauthorized(string message)
    {
        StatusCode = 401;
        AddError(message);

        return this;
    }

    public ResponseModel<T> NotFound()
    {
        StatusCode = 404;
        return this;
    }

    public ResponseModel<T> NotFound(string message)
    {
        StatusCode = 404;
        AddError(message);

        return this;
    }

    public ResponseModel<T> InternalServerError()
    {
        StatusCode = 500;

        return this;
    }

    public ResponseModel<T> InternalServerError(string message)
    {
        StatusCode = 500;
        AddError(message);

        return this;
    }

    public bool IsSuccess()
    {
        return Result != null && Success;
    }

    public bool IsSuccessWithNoContent()
    {
        return Success;
    }
}