//import { success } from "toastr";

//import { success } from "toastr";


$(document).ready(function () {
    function cms_decode_currency_format(obs) {
        if (obs == '')
            return '';
        else
            return parseInt(obs.replace(/,/g, ''));
    }
    //tự động chọn option có cùng giá trị
    var typeNews = $("#cbbType").attr("value");
    $("#cbbType option").each(function () {
        if (typeNews == $(this).val()) {
            $(this).attr('selected', 'selected');
        }
    });

    var typeSendNews = $("#cbbTypeSend").attr("value");
    $("#cbbTypeSend option").each(function () {
        if (typeSendNews == $(this).val()) {
            $(this).attr('selected', 'selected');
        }
    });

    //clear text when close modal
    $('.modal').on('hidden.bs.modal', function () {
        $(this).find("input,textarea").val('');
    });

    //change option in Combobox
    //$('#status').on("change", function () {
    //    searchWarrantyCard();
    //});

    $('#type').on('change', function () {
        searchPoint();
    });

    $('#itemStatus').on('change', function () {
        SearchItem();
    });

    //auto trim input text
    $('input[type="text"]').change(function () {
        this.value = $.trim(this.value);
    });

    $('#place').on('change', function () {
        LoadPlaceCreateShop();
    });

    //auto format number input
    $('.number').keyup(function () {
        $val = cms_decode_currency_format($(this).val());
        $(this).val(cms_encode_currency_format($val));
    });
}); //end document.ready


const SUCCESS = 1;
const ERROR = 0;
const DUPLICATE_NAME = 2;
const CAN_NOT_DELETE = 2;
const WRONG_PASSWORD = 2;
const NOT_ADMIN = 3;
const EXISTING = 2;
const FAIL_LOGIN = 2;
const CHECK_COINCIDENT = 99;
const URL_ADD_IMG_DEFAULT = "/Uploads/files/add_img.png";

function ValidDateTime(day, month, year) {
    if (month == 2) {
        if (year % 100 == 0 && year % 10 == 0) {
            if (year % 400 == 0) {
                if (day > 29) {
                    return "Năm nhuận tháng 2 có tối đa 29 ngày";
                }

            }
        }
        else if (year % 40 == 0) {
            if (day > 29) return "Năm nhuận tháng 2 chỉ có tối đa 29 ngày";
        }
        else {
            if (day > 28) return "Năm này chỉ có tối đa 28 ngày";
        }
    }
    else {
        if (month == 4 || month == 6 || month == 9 || month == 11) {
            if (month > 30) return "Các tháng này chỉ có tối đa 30 ngày";
        }
    }
    return "oke";
}

//đăng nhập
function Login() {
    if (!navigator.onLine) {
        swal({
            title: "Kiểm tra kết nối internet!",
            text: "",
            icon: "warning"
        })
        return;
    }
    var phone = $("#txtUsernameLogin").val();
    var password = $("#txtPasswordLogin").val();
    if (phone == "" || password == "") {
        swal({
            title: "Vui lòng nhập đầy đủ!",
            text: "",
            icon: "warning"
        })
        return;
    }
    $.ajax({
        url: '/Home/UserLogin',
        data: { phone: phone, password: password },
        type: 'POST',
        success: function (response) {
            if (response.Role == 1) {
                window.location.assign("/Home/Index");
            } else if (response.Role == 3) {
                window.location.assign("/Request/Index");
            }
            else if (response.Role == 2) {
                window.location.assign("/Point/Index");
                //cập nhật giao diện chat cho sale
                $('#groupnews').removeClass("active");
                $('#news').addClass("active");
                $('a.groupnews').hide();
                $('.news').addClass("active");
            }

            else if (response.FAIL_LOGIN == FAIL_LOGIN) {
                swal({
                    title: "Sai thông tin đăng nhập!",
                    text: "",
                    icon: "warning"
                })
                $("#txtUsernameLogin").val("");
                $("#txtPasswordLogin").val("");
            } else {
                swal({
                    title: "Hệ thống đang bảo trì",
                    text: "",
                    icon: "warning"
                })
            }
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}

function logout() {
    if (!navigator.onLine) {
        swal({
            title: "Kiểm tra kết nối internet!",
            text: "",
            icon: "warning"
        })
        return;
    }
    $.ajax({
        url: '/Home/Logout',
        data: {},
        type: 'POST',
        success: function (response) {
            if (response == SUCCESS) {
                location.reload();
            }
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}

//đổi mật khẩu
function changePassword() {
    if (!navigator.onLine) {
        swal({
            title: "Kiểm tra kết nối internet!",
            text: "",
            icon: "warning"
        })
        return;
    }
    var currentPassword = $.trim($("#txtCurrentPassword").val());
    var newPassword = $.trim($("#txtNewPassword").val());
    var confirmPassword = $.trim($("#txtConfirmPassword").val());

    if (currentPassword == "" || newPassword == "" || confirmPassword == "") {
        swal({
            title: "Vui lòng nhập đầy đủ!",
            text: "",
            icon: "warning"
        })
        return;
    }
    if (newPassword != confirmPassword) {
        $("#txtConfirmPassword").val("");
        swal({
            title: "Mật khẩu xác nhận không đúng",
            text: "",
            icon: "warning"
        })
        return;
    }

    $.ajax({
        url: '/User/ChangePassword',
        data: {
            CurrentPassword: currentPassword,
            NewPassword: newPassword
        },
        type: 'POST',
        success: function (response) {
            if (response == SUCCESS) {
                $("#changePassword").modal("hide");
                swal({
                    title: "Đổi mật khẩu thành công",
                    text: "",
                    icon: "success"
                })
            } else
                if (response == WRONG_PASSWORD) {
                    $("#txtCurrentPassword").val("");
                    swal({
                        title: "Mật khẩu cũ không đúng",
                        text: "",
                        icon: "warning"
                    })
                } else {
                    swal({
                        title: "Không thể đổi mật khẩu",
                        text: "",
                        icon: "warning"
                    })
                }
        },
        error: function (result) {
            console.log(result.responseText);
            swal({
                title: "Có lỗi",
                text: "",
                icon: "warning"
            })
        }
    });
}



