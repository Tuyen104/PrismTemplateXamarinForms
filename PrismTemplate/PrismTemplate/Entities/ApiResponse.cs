using System;
using PrismTemplate.Services;

namespace PrismTemplate.Entities
{
    public class ApiResponse<T>
    {
        readonly T _result;

        readonly int _statusCode;

        readonly string _errorMessage;

        public ApiResponse(T result = default(T), int statusCode = 0, string errorMessage = null)
        {
            _result = result;
            _statusCode = statusCode;
            _errorMessage = errorMessage;
        }

        public ApiResponse<bool> CreateApiResponseWithBoolResult()
        {
            return new ApiResponse<bool>(!HasError(), _statusCode, _errorMessage);
        }

        public bool HasError()
        {
            return ApiService.HasError(_statusCode);
        }

        public T GetResult()
        {
            return _result;
        }

        public int GetStatusCode()
        {
            return _statusCode;
        }

        public string GetErrorMessage()
        {
            return _errorMessage;
        }

        public void Check(Action<T> successAction, Action<int> errorAction = null)
        {
            if (ApiService.HasError(_statusCode))
            {
                errorAction?.Invoke(_statusCode);
            }
            else
            {
                successAction?.Invoke(_result);
            }
        }
    }
}
