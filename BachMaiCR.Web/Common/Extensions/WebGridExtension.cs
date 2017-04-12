/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;
using System.Web.Mvc;

namespace System.Web.Helpers
{
    public static class WebGridExtension
    {
        public static IHtmlString GetHtmlWithSelectAllCheckBox(this WebGrid webGrid, string tableStyle = null, string headerStyle = null, string footerStyle = null, string rowStyle = null, string alternatingRowStyle = null, string selectedRowStyle = null, string caption = null, bool displayHeader = true, bool fillEmptyRows = false, string emptyRowCellValue = null, IEnumerable<WebGridColumn> columns = null, IEnumerable<string> exclusions = null, WebGridPagerModes mode = WebGridPagerModes.All, string firstText = null, string previousText = null, string nextText = null, string lastText = null, int numericLinksCount = 5, object htmlAttributes = null, string checkBoxValue = "ID")
        {
            var newColumn = webGrid.Column(header: "{}",// This header will be replaced by checkbox
            format: item => new HelperResult(writer =>
            {
                writer.Write("<input class=\"singleCheckBox\" name=\"selectedRows\" value=\"" + item.Value.GetType().GetProperty(checkBoxValue).GetValue(item.Value, null).ToString() + "\" type=\"checkbox\" />");
            }));
            var newColumns = columns.ToList();
            newColumns.Insert(0, newColumn);
            var script = @"<script>
                if (typeof jQuery == 'undefined')
                {
                    document.write(unescape(""%3Cscript src='http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js'%3E%3C/script%3E""));
                }                
                (function(){
                    window.setTimeout(function() { initializeCheckBoxes();  }, 1000);
                    function initializeCheckBoxes(){    
                        $(function () {
                            $('#allCheckBox').click(function () {
                                var isChecked = $(this).attr('checked');                        
                                $('.singleCheckBox').attr('checked', isChecked  ? true: false);
                                $('.singleCheckBox').closest('tr').addClass(isChecked  ? 'selected-row': 'not-selected-row');
                                $('.singleCheckBox').closest('tr').removeClass(isChecked  ? 'not-selected-row': 'selected-row');
                            });
                            $('.singleCheckBox').click(function () {
                                var isChecked = $(this).attr('checked');
                                $(this).closest('tr').addClass(isChecked  ? 'selected-row': 'not-selected-row');
                                $(this).closest('tr').removeClass(isChecked  ? 'not-selected-row': 'selected-row');
                                if(isChecked && $('.singleCheckBox').length == $('.selected-row').length)
                                     $('#allCheckBox').attr('checked',true);
                                else
                                    $('#allCheckBox').attr('checked',false);
                            });
                        });
                    }
                })();
            </script>";
            var html = webGrid.GetHtml(tableStyle, headerStyle, footerStyle, rowStyle, alternatingRowStyle, selectedRowStyle, caption, displayHeader, fillEmptyRows, emptyRowCellValue, newColumns, exclusions, mode, firstText, previousText, nextText, lastText, numericLinksCount, htmlAttributes);
            return MvcHtmlString.Create(html.ToString().Replace("{}", "<input type='checkbox' id='allCheckBox'/>") + script);// Replace {} with a checkbox
        }
    }
}