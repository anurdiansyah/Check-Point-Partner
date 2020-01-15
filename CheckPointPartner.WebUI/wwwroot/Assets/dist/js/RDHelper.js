/*! RDFormHelper.js
* ================
* Helper JS application file for RDF MVC v 1.0. This file
* should be included in all pages. It controls some layout
* options and implements exclusive AdminLTE plugins.
*
* @Author  RD
* @Support <0899 8771 566>
* @Email   <ardinurdiansyah.2734@gmail.com>
* @version 1.0
*/

//Layout Helper

function fSetNavigation() {
    var path = window.location.pathname;
    path = path.replace(/\/$/, "");
    path = decodeURIComponent(path);

    $(".sidebar-menu a").each(function () {
        var href = $(this).attr('href');

        if (path.substring(0, href.length) == href && $(this).attr('id') != "liDashboard") {
            $(this).closest('li').closest('ul').closest('li').addClass('menu-open');
            $(this).closest('li').closest('ul').css("display", "block");
            $(this).closest('li').addClass('active');
        }
    });
}

function RefreshDataTable(p_sTableName) {
    $('#' + p_sTableName + '').DataTable().ajax.reload();
};

function formatJSONDate(jsonDate) {
    var newDate = new Date(parseInt(jsonDate.substr(6)));
    return newDate;
}


//=====================================================================================================================================================================//

function Edit() {

}