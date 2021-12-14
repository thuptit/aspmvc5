$(document).ready(function () {
        $(function () {
        $('.chosen-select').chosen();
        $('.chosen-select-deselect').chosen({ allow_single_deselect: true });
    });

    FocusTabMenu();
    GetSessionLogin();
    $('.date').datepicker({
        closeText: "Đóng",
        prevText: "Trước",
        nextText: "Sau",
        currentText: "Hôm nay",
        monthNames: ["Tháng một", "Tháng hai", "Tháng ba", "Tháng tư", "Tháng năm", "Tháng sáu", "Tháng bảy", "Tháng tám", "Tháng chín", "Tháng mười", "Tháng mười một", "Tháng mười hai"],
        monthNamesShort: ["Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín", "Mười", "Mười một", "Mười hai"],
        dayNames: ["Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy"],
        dayNamesShort: ["CN", "Hai", "Ba", "Tư", "Năm", "Sáu", "Bảy"],
        dayNamesMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
        weekHeader: "Tuần",
        dateFormat: "dd/mm/yy",
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ""
    });

    $(document).on("wheel", "input[type=number]", function (e) {
        $(this).blur();
    });


}); //end document.ready


//
function FocusTabMenu() {

    var url = window.location.pathname;
    $.ajax({
        url: '/Home/GetUserLogin',
        type: 'GET',
        success: function (response) {
            var role = response.Role;
            switch (url) {
                case "/Home/Index":
                    $('#tabHome').addClass('active');
                    break;
                case "/Setting/Index":
                    $('#tabSystemSetting').addClass('active');
                    break;
                case "/User/Index":
                    $('#tabUsers').addClass('active');
                    break;

                case "/NewProfile/Index":
                    $("#ulProfile").attr("aria-expanded", "true");
                    $("#ulProfile").addClass("collapse in");
                    $('#tabFile').addClass('active');
                    $('#tabNewProfile').addClass('active');
                    break;
                case "/RenewProfile/Index":
                    $("#ulProfile").attr("aria-expanded", "true");
                    $("#ulProfile").addClass("collapse in");
                    $('#tabFile').addClass('active');
                    $('#tabRenewProfile').addClass('active');
                    break;
                case "/ProfilePending/Index":
                    $("#ulProfile").attr("aria-expanded", "true");
                    $("#ulProfile").addClass("collapse in");
                    $('#tabFile').addClass('active');
                    $('#tabProfilePending').addClass('active');
                    break;

                case "/GroupNewProfile/Index":
                    $("#ulGroupFile").attr("aria-expanded", "true");
                    $("#ulGroupFile").addClass("collapse in");
                    $('#tabGroupProfile').addClass('active');
                    $('#tabGroupNewProfile').addClass('active');
                    break;
                case "/GroupRenewProfile/Index":
                    $("#ulGroupFile").attr("aria-expanded", "true");
                    $("#ulGroupFile").addClass("collapse in");
                    $('#tabGroupProfile').addClass('active');
                    $('#tabGroupRenewProfile').addClass('active');
                    break;

                case "/Object/Index":
                        $("#ulSystemCategory").attr("aria-expanded", "true");
                        $("#ulSystemCategory").addClass("collapse in");
                        $('#SystemCategory').addClass('active');
                        $('#tabObject').addClass('active');
                    break;

                case "/DecisionCode/Index":
                    $("#ulSystemCategory").attr("aria-expanded", "true");
                    $("#ulSystemCategory").addClass("collapse in");
                    $('#SystemCategory').addClass('active');
                    $('#tabDecisionCode').addClass('active');
                    break;
                case "/Position/Index":
                    $("#ulSystemCategory").attr("aria-expanded", "true");
                    $("#ulSystemCategory").addClass("collapse in");
                    $('#SystemCategory').addClass('active');
                    $('#tabPosition').addClass('active');
                    break;
                case "/Period/Index":
                    $("#ulSystemCategory").attr("aria-expanded", "true");
                    $("#ulSystemCategory").addClass("collapse in");
                    $('#SystemCategory').addClass('active');
                    $('#tabPeriod').addClass('active');
                    break;

                case "/Province/Index":
                    $("#ulAdministrativeUnits").attr("aria-expanded", "true");
                    $("#ulAdministrativeUnits").addClass("collapse in");
                    $('#AdministrativeUnits').addClass('active');
                    $('#tabProvince').addClass('active');
                    break;
                case "/Village/Index":
                    $("#ulAdministrativeUnits").attr("aria-expanded", "true");
                    $("#ulAdministrativeUnits").addClass("collapse in");
                    $('#AdministrativeUnits').addClass('active');
                    $('#tabVillage').addClass('active');
                    break;
                case "/District/Index":
                    $("#ulAdministrativeUnits").attr("aria-expanded", "true");
                    $("#ulAdministrativeUnits").addClass("collapse in");
                    $('#AdministrativeUnits').addClass('active');
                    $('#tabDistrict').addClass('active');
                    break;
                case "/ProfileDeclined/Index":
                    $("#ulProfile").attr("aria-expanded", "true");
                    $("#ulProfile").addClass("collapse in");
                    $('#AdministrativeUnits').addClass('active');
                    $('#tabProfileDeclined').addClass('active');
                    break;
                 default:
                    break;
            }
        
        }
    })
    

}

//lấy thông tin đối tượng vừa đăng nhập
function GetSessionLogin() {
    $.ajax({
        url: '/Home/GetUserLogin',
        type: 'GET',
        success: function (response) {
            var role = response.Role;

            if (role == 1) {
                $(".metismenu").append(
                    "<li id='tabHome'><a href = '/Home/Index'><i class='fa fa-tachometer ic-menu'></i>Tổng quan</a ></li >" +
                    "<li id='tabFile'><a href = '#'> <i class='fa fa-book ic-menu'></i>Hồ sơ</a >" +
                    "<ul id='ulProfile'><li id = 'tabNewProfile' > <a href='/NewProfile/Index'>Hồ sơ cấp mới</a></li >"+
                    "<li id='tabRenewProfile'><a href='/RenewProfile/Employee'>Hồ sơ cấp lại</a></li>" +
                    "<li id = 'tabProfileDeclined'><a href='/ProfileDeclined/Index'>Hồ sơ từ chối</a></li>"+
                    "<li id='tabProfilePending'><a href='/ProfilePending/Index'>Hồ sơ trình cấp</a></li></ul ></li >" +
                    //Nhóm hồ sơ
                    "<li id='tabGroupProfile'><a  href='#'><i class='fa fa-book ic-menu' ></i>Nhóm hồ sơ</a>"+
                    "<ul id='ulGroupFile'>"+
                        "<li id='tabGroupNewProfile'><a href='/GroupNewProfile/Index'>Nhóm hồ sơ cấp mới</a></li>"+
                        "<li id='tabGroupRenewProfile'><a href='/GroupRenewProfile/Index'>Nhóm hồ sơ cấp lại</a></li>"+
                    "</ul>"+
                    "</li >"+
                    "<li id='SystemCategory'><a href = '#'> <i class='fa fa-bars ic-menu ic-menu'></i>Danh mục hệ thống</a >"+
                    "<ul id = 'ulSystemCategory' ><li id='tabObject'><a href='/Object/Index'>Đối tượng</a></li>"+
                    "<li id='tabDecisionCode'><a href='/DecisionCode/Index'>Ký hiệu đuôi</a></li>"+
                    "<li id='tabPosition'><a href='/Position/Index'>Chức vụ</a></li>"+
                    "<li id='tabPeriod'><a href='/Period/Index'>Thời kỳ</a></li></ul >"+
                    "</li > " +
                   "<li id='AdministrativeUnits'><a href = '#'> <i class='fa fa-sun-o ic-menu ic-menu'></i>Đơn vị hành chính</a >" +
                    "<ul id='ulAdministrativeUnits'>"+
                        "<li id='tabProvince'><a href='/Province/Index'>Tỉnh/ thành phố</a></li>"+
                        //"<li id='tabDistrict'><a href='/District/Index'>Quận/ huyện</a></li>"+
                        //"<li id='tabVillage'><a href='/Village/Index'>Phường/ xã</a></li>"+
                    "</ul>"+
                    "</li > " +
                    "<li id='tabSystemSetting'><a href = '/Setting/Index'><i class='fa fa-cogs  ic-menu'></i>Tham số hệ thống</a ></li >" +
                    "<li id='tabUsers'><a href = '/User/Index'> <i class='fa fa-user-circle-o  ic-menu' aria-hidden = 'true'></i>Tài khoản</a ></li >" +
                    "<li id='tabEmpG'><a href = '/E_GroupNewProfile/Index'> <i class='fa fa-tachometer ic menu'></i>Công văn</a ></li >"
                );
                FocusTabMenu();
            }
            else if (role == 2) {
                $(".metismenu").append(
                    "<li id='tabHome'><a href = '/Home/Index'><i class='fa fa-tachometer ic-menu'></i>Tổng quan</a ></li >" +
                    "<li id='tabFile'><a href = '#'> <i class='fa fa-book ic-menu'></i>Hồ sơ</a >" +
                        "<ul id='ulProfile'>" +
                            "<li id = 'tabNewProfile' > <a href='/NewProfile/Index'>Hồ sơ cấp mới</a></li> " +
                            "<li id='tabRenewProfile'><a href='/RenewProfile/Employee'>Hồ sơ cấp lại</a></li>" +
                        "</ul>" +
                    "</li> " +
                    "<li id='AdministrativeUnits'><a href = '#'> <i class='fa fa-sun-o ic-menu ic-menu'></i>Đơn vị hành chính</a >" +
                    "<ul id='ulAdministrativeUnits'>" +
                    "<li id='tabProvince'><a href='/Province/Index'>Tỉnh/ thành phố</a></li>" +
                    //"<li id='tabDistrict'><a href='/District/Index'>Quận/ huyện</a></li>" +
                    //"<li id='tabVillage'><a href='/Village/Index'>Phường/ xã</a></li>" +
                    "</ul>" +
                    "</li > " +
                    "<li id = 'SystemCategory'> <a href='#'><i class='fa fa-bars ic-menu ic-menu'></i>Danh mục hệ thống</a>"+
                    "<ul id = 'ulSystemCategory'><li id='tabObject'><a href='/Object/Index'>Đối tượng</a></li>" +
                    "<li id='tabDecisionCode'><a href='/DecisionCode/Index'>Ký hiệu đuôi</a></li>" +
                    "<li id='tabPosition'><a href='/Position/Index'>Chức vụ</a></li>" +
                    "<li id='tabPeriod'><a href='/Period/Index'>Thời kỳ</a></li></ul>" +
                    "</li > "  +
                    "<li id='tabUsers'><a href = '/User/Index'> <i class='fa fa-user-circle-o  ic-menu' aria-hidden = 'true'></i>Tài khoản</a ></li >"

                );
                FocusTabMenu();
            }
            else if (role == 3) {
                $(".metismenu").append(
                    "<li id='tabHome'><a href = '/Home/Index'><i class='fa fa-tachometer ic-menu'></i>Tổng quan</a ></li >" +
                    "<li id='tabEmp'><a href = '/RenewProfile/Employee'> <i class='fa fa-tachometer ic-menu'></i>Hồ sơ cấp lại</a ></li >" +
                    "<li id='tabEmpG'><a href = '/E_GroupNewProfile/Index'><i class='fa fa-file-text ic-menu'></i> Công văn</a ></li >" +
                    "<li id='AdministrativeUnits'><a href = '#'> <i class='fa fa-sun-o ic-menu ic-menu'></i>Đơn vị hành chính</a >" +
                    "<ul id='ulAdministrativeUnits'>" +
                        "<li id='tabProvince'><a href='/Province/Index'>Tỉnh/ thành phố</a></li>" +
                        //"<ss id='tabDistrict'><a href='/District/Index'>Quận/ huyện</a></li>" +
                        //"<li id='tabVillage'><a href='/Village/Index'>Phường/ xã</a></li>" +
                    "</ul>" +
                    "</li > " +
                    "<li id='SystemCategory'><a href = '#'> <i class='fa fa-bars ic-menu ic-menu'></i>Danh mục hệ thống</a ></li >" +
                    "<ul id='ulSystemCategory'>" +
                        "<li id='tabPosition'><a href='/Position/Index'>Chức vụ</a></li>"+
                    "</ul > " +
                    "<li id='tabUsers'><a href = '/User/Index'> <i class='fa fa-user-circle-o  ic-menu' aria-hidden = 'true'></i>Tài khoản</a ></li >"

                );
                FocusTabMenu();
            }
        }
    })
}