using APIProject.Models;
using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Business
{
    public class ResponseBusiness
    {
        public JsonResultModel response(int status, int code, string message, object data)
        {
            JsonResultModel result = new JsonResultModel();
            result.Status = status;
            result.Code = code;
            result.Message = message;
            result.Data = data;
            return result;
        }

       
        public JsonResultModel SuccessResult(string mess, object data)
        {
            JsonResultModel result = new JsonResultModel();
            result.Message = mess;
            result.Status = SystemParam.SUCCESS;
            result.Code = SystemParam.SUCCESS_CODE;
            result.Data = data;
            return result;
        }
        public JsonResultModel ErrorResult(string mess, int code)
        {
            JsonResultModel result = new JsonResultModel();
            result.Message = mess;
            result.Status = SystemParam.ERROR;
            result.Code = code;
            result.Data = null;
            return result;
        }
        
    }
}
