!function ($) {

    // Picker object
    var fadeTime = 400;
    var idElement;
    var idElementTree;
    var dataID;
    var dataName;
    var Department = function (element, options) {
        this.element = $(element);
        idElement = $(element).attr('id');
        if (idElement == null || idElement.length == 0)
            idElement = departmentTree;
        idElementTree = idElement + "_DepartTree";
        //alert(element);
        //console.log(options.setValue);
        //var parent = $(element).parent();
        var contentTree;
        var strDefaultDisplay = "-- Tất cả --";
        if (options.defaultDisplay != null && options.defaultDisplay.length > 0) {
            strDefaultDisplay = options.defaultDisplay;
        }
        //options
        //Xử lý dữ liệu đầu vào.
        // Lấy giá trị Id từ thuộc tính val() hoặc data-id của đối tượng
        // Ưu tiên lấy thông tin ở thuộc tính setValue của options
        if (options.dataID != null && options.dataID.length > 0) {
            dataID = options.dataID;
        }
        else {
            //Lấy ở thuộc tính data-id trước.
            if ($(element).attr('data-id') != null && $(element).attr('data-id').length > 0)
                dataID = $(element).attr('data-id');
            else
                dataID = $(element).val().trim();
        }

        //Tìm tên
        if (options.dataName != null && options.dataName.length > 0) {
            dataName = options.dataName;
        }
        else {
            //Lấy ở thuộc tính data-name trước.
            if ($(element).attr('data-name') != null && $(element).attr('data-name').length > 0)
                dataName = $(element).attr('data-name');
        }
        // paramester department root id, default vaule is 0.
        //deptRootID
        var deprtRootId;
        if (options.DeprtRootId != null && options.DeprtRootId.length > 0) {
            deprtRootId = options.dataName;
        }
        else {
            //Lấy ở thuộc tính data-department-root-id trước.
            if ($(element).attr('data-department-root-id') != null && $(element).attr('data-department-root-id').length > 0)
                deprtRootId = $(element).attr('data-department-root-id');
        }

        //@WebUtility.HtmlDecode((ViewBag.LM_DEPARTMENT_NAME)
        // Get TREE

        var deptBox = "<div id='" + idElementTree + "' class='gpcp-department form-control'><button type='button' class='btn btn-default dropdown-toggle btn-sm'>"
           + "<span data-id='' class='filter-option pull-left'>" + strDefaultDisplay + "</span><span class='caret'></span>"
           + "</button> <div class='tree-data-content dropdown-menu'></div></div>";
        $(element).after(deptBox);

        if (deprtRootId != null && deprtRootId.length > 0) {
            var multi = false;
            var selectedlist = "";
            if (typeof (options.multiselect) !== typeof undefined) {
                multi = options.multiselect;
            }
            if(typeof (options.selectedlist!== typeof undefined))
            {
      
                selectedlist=options.selectedlist;
            }
            $.when($("#" + idElementTree + " .tree-data-content").load('/Publish/DepartmentTree?elmID=' + idElement + "TreeContent&deptRootID=" + deprtRootId+"&multiselect="+multi+"&selectedlist="+selectedlist)).done(function (response) {
            });
        } else {
            var multi = false;
            if (typeof (options.multiselect) !== typeof undefined) {
                multi = options.multiselect;
            }
            if (typeof (options.selectedlist !== typeof undefined)) {
                selectedlist = options.selectedlist;
            }
            $.when($("#" + idElementTree + " .tree-data-content").load('/Publish/DepartmentTree?elmID=' + idElement + "TreeContent&multiselect=" + multi + "&selectedlist=" + selectedlist)).done(function (response) {
                contentTree = $(response);
            });
        }
        //Sự kiện hiển thị tree
        $("#" + idElementTree + " .btn").live('click', function (event) {
            if ($(contentTree).is(":visible")) {
                $(contentTree).fadeOut(fadeTime);
            }
            else {
                $(contentTree).fadeIn(fadeTime);
            }
        })
        $('html').bind('click', function (e) {
            if ($(e.target).parents('div.tree-data-content').length == 0) {
                $(contentTree).fadeOut(fadeTime);
            }
        });
        //Sự kiện click chọn
        $("#" + idElementTree + " li .deprt-element").live('click', function () {
            if (typeof (options.multiselect) !== typeof undefined) {
                $(this).find("input[type='checkbox']").prop('checked', true);
                set_id_department($(this).find("input[type='checkbox']"));
            }
            else {
                dataID = $(this).attr('data-id');
                dataName = $(this).text().trim();
                $("#" + idElementTree + " .btn.btn-default .filter-option").text(dataName);
                $("#" + idElementTree + " .btn.btn-default .filter-option").attr('data-id', dataID);
                $(element).attr('data-id', dataID);
                $(element).val(dataID).change();
                $(element).attr('data-name', dataName);
            }
            $(contentTree).fadeOut(fadeTime / 2);
        });

        // Sự kiện chọn (multiselect)
        // duonglh3
        $("#" + idElementTree + " input[type='checkbox']").live('click', function () {
            
                //var checked = $(this).is(":checked");
                //var lichild = $(this).parent('li').eq(0);
                //var liparent = $(this).parents('li[data-expanded="true"]').eq(0);
                //if (lichild.attr('class').indexOf("lichild")==-1) {
                //    liparent.find("input[type='checkbox']").each(function (index, ele) {
                //        $(ele).prop('checked', checked);
                //        set_id_department(ele);
                //    });
                //    liparent.find('ul').each(function (index, ele) {
                //        $(ele).css('display', 'block');
                //    });
                    
                //    liparent.removeClass('expandable').addClass('collapsable');
                //    liparent.find("div[class*='hitarea']").eq(0).removeClass('expandable-hitarea').addClass('collapsable-hitarea"');
                //}
                //check_ischecked_all($(this));
                set_id_department(this);
        });

        function set_id_department(checkbox) {
            var checked = $(checkbox).is(":checked");
            var data_id = $(element).val();
            var button=$("#" + idElementTree).find("button span[class*='filter-option pull-left']");

            var arr_id = data_id.split(",");
            if (checked === true) {
                var index_id = arr_id.indexOf($(checkbox).attr('data-id'));
                if(index_id===-1)
                arr_id.push($(checkbox).attr('data-id'));
            }
            else {
                var index_id = arr_id.indexOf($(checkbox).attr('data-id'));
                if (index_id !== -1)
                    arr_id.splice(index_id, 1);
            }
            if (arr_id.length==1) {
                button.text(options.defaultDisplay);
            }
            else {
                var data_text = button.text();
                var arr_text = data_text.split(";");
                var index_default = arr_text.indexOf(options.defaultDisplay);
                if (index_default !== -1)
                    arr_text.splice(index_default, 1);
                var str = $(checkbox).siblings("a[class='deprt-element']").text();
                if (checked===true) {
                    var index_text = arr_text.indexOf(str);
                    if (index_text===-1) {
                        arr_text.push(str);
                    }
                    button.text(arr_text.join(";"));
                    $(element).attr('data-name', arr_text.join(";"));
                }
                else {
                    var index_text = arr_text.indexOf(str);
                    if (index_text !== -1) {
                        arr_text.splice(index_text, 1);
                    }
                    button.text(arr_text.join(";"));
                     $(element).attr('data-name', arr_text.join(";"));
                }
            }
            $(element).val(arr_id.toString());
            $(element).valid();
        }

        function check_ischecked_all(checkbox) {
            set_id_department(checkbox);
            var liparent = $(checkbox).parents('li[data-expanded="true"]');
                $(liparent).each(function (index_parent, ele_parent) {
                    var checkall = true;
                    $(ele_parent).find("input[type='checkbox']").each(function (index, ele) {
                        if (index != 0 && $(ele).is(":checked") === false) {
                            checkall = false;
                            return;
                        }
                    });
                    $(ele_parent).find("input[type='checkbox']").eq(0).prop('checked', checkall);
                    
                });
        }
        // ---------------------------------------------------------------------
        if (dataID != null && dataID.length > 0) {
            //Set data id for element
            dataID = dataID;
            $("#" + idElementTree + " .btn.btn-default .filter-option").text(dataName);
            $("#" + idElementTree + " .btn.btn-default .filter-option").attr('data-id', dataID);
            $(element).attr('data-id', dataID);
            $(element).val(dataID);
        };
        if (dataName != null && dataName.length > 0) {
            //Set data id for element
            $(element).attr('data-name', dataName);
            $("#" + idElementTree + " .btn.btn-default .filter-option").text(dataName);
        };
        $(element).hide();
    };

    Department.setValues = function (id, name) {
        alert(id + name);
        $("#" + idElementTree + " .btn.btn-default .filter-option").text(name);
        $("#" + idElementTree + " .btn.btn-default .filter-option").attr('data-id', id);
        $("#" + idElement).val(id);
        $("#" + idElement).text(id);
        $("#" + idElement).attr('data-id', id);
        $("#" + idElement).attr('data-name', name);
    }


    $.fn.Department = function (option, val) {

        return this.each(function () {
            var $this = $(this),
                data = $this.data('Department'),
                options = typeof option === 'object' && option;
            if (!data) {
                $this.data('Department', (data = new Department(this, $.extend({}, $.fn.Department.defaults, options))));
            }
            if (typeof option === 'string') data[option](val);
        });
    };

    $.fn.Department.defaults = {
        onRender: function (date) {
            return '';
        }
    };
    $.fn.Department.Constructor = Department;


}(window.jQuery);