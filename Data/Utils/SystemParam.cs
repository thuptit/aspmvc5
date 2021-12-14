using Data.Business;
using Data.DB;
using Data.Model.APIWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.Utils
{
    //demo enum
    //public enum DEMO
    //{
    //    VINH, // default = 0
    //    HUNG, // = 1
    //    SY, // = 2
    //    TRUONG = 5,
    //    TU, // = 6
    //    TUNG, // = 7
    //}


    //error code api
    public enum ERROR_CODE
    {
        NOT_EXIST_PRODUCT = -11,
        PRODUCT_LOCKED = -12,
        PRODUCT_ACTIVED = -13,
        OTP_EXPIRE = -14,
        OTP_FAIL = -15,
        OTP_FAIL_COUNT = -16,
        OTP_SEND_COUNT = -17,
        PRODUCT_NOT_OUT_WAREHOUSE = -18
    }

    //status of product
    public enum PRODUCT
    {
        LOCK,
        ACTIVE,
        IN_WAREHOUSE,
        OUT_WAREHOUSE
    }

    public enum IS_CUSTOMER
    {
        NO,
        YES
    }

    public class SystemParam : GenericBusiness
    {


        public const int ERROR = 0;
        public const int SUCCESS = 1;
        public const int SUCCESS_CODE = 200;
        public const int PROCESS_ERROR = 500;

        public const string CONVERT_DATETIME = "dd/MM/yyyy";
        public const string CONVERT_DATETIME_HAVE_HOUR = "dd/MM/yyyy HH:mm";
        public const int MAX_ROW_IN_LIST = 20;
        public const int ACTIVE = 1;
        public const int NO_ACTIVE = 0;
        public const string DSN_SENTRY = "http://5a4f4a1077d446069f63608b26a1937d@sentry.winds.vn/30";


        public const string TOKEN_INVALID = "Phiên đăng nhập hết hạn";
        public const string TOKEN_NOT_FOUND = "Token not found";
        public const string SERVER_ERROR = "Hệ thống đang bảo trì!";
        public const string SUCCESS_MESSAGE = "Thành công";
        public const string ERROR_MESSAGE = "Thất bại";


        public const string MESSAGE_ERROR_IS_CUSTOMER_NO = "Tài khoản của bạn chưa phải khách hàng";
        public const string MESSAGE_REQUIRED = "Vui lòng nhập đầy đủ thông tin";
        public const string MESSAGE_ERROR_EXIST_EMAIL = "Email đã tồn tại";
        public const string MESSAGE_DATA_NOT_FOUND = "Kiểm tra dữ liệu đầu vào";
        public const string MESSAGE_OTP_NOT_FOUND = "Vui lòng nhập OTP";
        public const string MESSAGE_INVALID_PHONE = "Vui lòng nhập đúng định dạng số điện thoại";
        public const string MESSAGE_INVALID_IMEI = "IMEI không đúng định dạng";
        public const string MESSAGE_ERROR_NOT_EXIST_NEWS = "Bài viết không tồn tại";
        public const string MESSAGE_ERROR_NOT_EXIST_PRODUCT = "Thiết bị không phải sản phẩm của gotech chính hãng";
        public const string MESSAGE_ERROR_PRODUCT_LOCKED = "Thiết bị hiện tại đang bị khóa";
        public const string MESSAGE_ERROR_PRODUCT_ACTIVED = "Thiết bị này đã được kích hoạt từ trước";
        public const string MESSAGE_ERROR_PRODUCT_NOT_OUT_WAREHOUSE = "Thiết bị chưa được xuất kho";
        public const string MESSAGE_ERROR_OTP_EXPIRE = "OTP hết hiệu lực";
        public const string MESSAGE_ERROR_OTP_FAIL = "OTP không hợp lệ";
        public const string MESSAGE_ERROR_OTP_FAIL_COUNT = "Bạn nhập sai OTP quá nhiều. Vui lòng thử lại vào ngày hôm sau.";
        public const string MESSAGE_ERROR_OTP_SEND_COUNT = "Mỗi ngày mã xác thực chỉ được gửi 3 lần. Vui lòng thử lại vào ngày hôm sau.";

        public const string MESSAGE_ERROR_SMS_SEND_FAIL = "Không thử gửi OTP tới điện thoại";
        public const string MESSAGE_ERROR_SMS_TOKEN_INVALID = "Token SMS không hợp lệ";
        public const string MESSAGE_ERROR_SMS_USER_LOCKED = "Tài khoản SMS đã bị khóa";
        public const string MESSAGE_ERROR_SMS_USER_INVALID = "Tài khoản SMS không chính xác";
        public const string MESSAGE_ERROR_SMS_USER_NOT_ROLE = "Tài khoản SMS không có quyền";
        public const string MESSAGE_ERROR_SMS_RECEIVER_INVALID = "Số điện thoại không hợp lệ";


        public const int ERROR_PASSWORD = 2;
        public const int ERROR_EXIST_EMAIL = 10;
        public const int EXIST = 2;
        public const int EXISTCODE = 3;
        public const int EXISTTIME = 4;
        public const int EXISTDISPLAYORDER = 5;
        public const int EXISTARMY = 6;
        public const int ERROR_PARENT = 1000;
        public const int ERROR_CHILD = 1001;


        public const int NOT_EXIST = -1;
        public const int USER_EXIST = 2;
        public const bool BOOLEAN_TRUE = true;
        public const bool BOOLEAN_FALSE = false;
        public const int DUPLICATE_NAME = 2;


        //login
        public const int FAIL_LOGIN = 2;
        public const int FAIL = 501;
        public const int COMPLETE = 200;

        //role
        public const int ROLE_USER_ADMIN = 2;
        public const string ROLE_ADMIN_STRING = "Admin";
        public const int ROLE_USER_SUPER_ADMIN = 1;
        public const string ROLE_ADMIN_SUPER_STRING = "Super Admin";
        // role Tổ quốc nhé ae
        public const int ROLE_ADMIN = 1;
        public const string ROLE_USER_ADMIN_STRING = "Admin";
        public const int ROLE_USER_POLICY_1 = 2;
        public const string ROLE_USER_POLICY_1_STRING = "Phòng chính sách 1";
        public const int ROLE_USER_DEPARTMENT = 3;
        public const string ROLE_ADMIN_DEPARTMENT_STRING = "Sở NCC";
        public const int CHECK_COINCIDENT = 99;

        public const int ROLE_NOT_USER_ADMIN = 4;

        //result when not role super admin
        public const int NO_ROLE_SUPER_ADMIN = -1;
        //Default Passwword
        public const string DEFAULT_PASSWORD = "123456";
        //trạng thái hồ sơ
        public const int STATUS_PENDING_RECORD = 0; //chờ xác nhận
        public const int STATUS_ACCEPTED_RECORD = 1; //xác nhận
        public const int STATUS_REJECT_RECORD = 2; //từ chối
        public const int STATUS_WAIT_PRESIDENT = 3; //từ chối
        //trạng thái nhóm hồ sơ
        public const int STATUS_GROUP_WAIT = 5; //chờ xác nhận
        public const int STATUS_GROUP_REGISTRATION = 6; //xác nhận
        public const int STATUS_GROUP_REJECT = 7; //xác nhận
        //Loại đối tượng 
        public const int ORTHER = 1;//Đối tương khác
        public const int ISGUERRILLA = 2;//Du kích quân đân
        public const int ISARMY = 3;//Quân đội

        //Trạng thái bản ghi hồ sơ
        public const int PENDING = 0;//Chờ
        public const int ACCEPT = 1;//Chấp nhậnk
        public const int REFUSE = 2;//Từ chói
        public const int PENDING_PRIMEMINISTER = 3; //chờ trình thủ tướng
        public const int GAVE_PRIMEMINISTER = 4; //đã trình thủ tướng

        public const int NOT_PRINT = 0;//chưa in
        public const int PRINTED = 1;//đã in 
        public const string Day = "dd";
        public const string Month = "MM";
        public const string Year = "yyyy";
        //type hồ sơ khi thêm mới
        public const int TYPE_NEW_PROFILE = 1; //hồ sơ cấp mới
        public const string TEXT_TYPE_NEW_PROFILE = "Hồ sơ cấp mới"; //hồ sơ cấp mới
        public const int TYPE_RENEW_PROFILE = 2; //hồ sơ cấp lại\
        public const string TEXT_TYPE_RENEW_PROFILE = "Hồ sơ cấp lại"; //hồ sơ cấp lại

        //type nhom hồ sơ khi thêm mới
        public const int TYPE_GROUP_NEW_PROFILE = 1; //hồ sơ cấp mới
        public const int TYPE_GROUP_RENEW_PROFILE = 2; //hồ sơ cấp lại
        public const int IMG = 1; //ảnh
        public const int DOC = 2; //tài liệu

        public const int RECORD_IN_GROUP = 3;
        public const int ACCOMPANYINGTEXT = 3;// tài liệu đi kèm


        //Status group
        public const int STATUS_GROUP_REPORT_PENDING = 1; //chờ trình duyệt
        public const string TEXT_STATUS_GROUP_REPORT_PENDING = "Chờ trình duyệt";
        public const int STATUS_GROUP_PENDING = 2; //chờ duyệt
        public const string TEXT_STATUS_GROUP_PENDING = "Chờ duyệt";
        public const int STATUS_GROUP_ACCEPTING = 3; //Đang duyệt
        public const string TEXT_STATUS_GROUP_ACCEPTING = "Đang duyệt";
        public const int STATUS_GROUP_SUCCESS = 4; //Hoàn thành
        public const string TEXT_STATUS_GROUP_SUCCESS = "Hoàn thành";
        public const int STATUS_GROUP_PENDING_PRIMEMINISTER = 5; //chờ trình thủ tướng
        public const string TEXT_STATUS_GROUP_PENDING_PRIMEMINISTER = "Chờ trình thủ tướng";
        public const int STATUS_GROUP_GAVE_PRIMEMINISTER = 6; //đã trình thủ tướng
        public const string TEXT_STATUS_GROUP_GAVE_PRIMEMINISTER = "Đã trình thủ tướng";
        public const string TEXT_STATUS_GROUP_REJECT = "Đã từ chối"; //Đã từ chối

        public const int SEEN = 1; //đã đọc
        public const int SEND = 0; //chưa đọc

        public const int REJECT = 1; //media từ chối
        public const int WAITING = 0; //media chờ trình
    }

    public class Message
    {
        public const string DELETE_DONE = "Xóa thành công";
    }
}
