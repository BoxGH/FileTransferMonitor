$(":button[value='Delete']").click(function (e) {
    var _this = this;
    var index = $(this).attr('data-index');
    $.getJSON('/FilesViewAndAction/Delete', { id: index }, function (data, textStatus, jqXHR) {
        if (index == data.result) {
            $(_this).parent().parent().fadeOut(1200);
        }
        else if(data.result === 'Registration required'){
            alert('This action requires registration');
        }
        else {
            alert('Error: ' + data.result);
            $(_this).parent().parent().fadeOut(1200);
        }
    }).fail(function (jqxhr, settings, ex) {
        alert('The request is not processed: ' + ex);
        $(_this).parent().parent().fadeOut(1200);
    })
    //.done(function () {alert('Request done!');}) 
});
$(":button[name='details']").click(function (e) {
    var point = $(this).offset();
    var index = $(this).attr('data-index');

    $.getJSON('/FilesViewAndAction/Details', { id: index}, function (data, textStatus, jqXHR) {
        if (data.result == 'nul') { alert('HttpStatusCode.BadRequest'); }
        if (data.result == 'null') { alert('HttpStatusCode.NoContent'); }

        var dt = document;

        var div = dt.createElement('div');
        $(div).addClass('detail');
        $(div).css({ 'top': point.top - 10, 'left': point.left - 250 });

        $(div).append(' Name: '+data.Name);
        $(div).append('<br />')
        $(div).append('Type: ' + data.Type);
        $(div).append('<br />')
        $(div).append('Size: ' + data.Size + ' Mb');
        $(div).append('<br />')
        $(div).append('TadeTime: ' + new Date(parseInt(data.DataTimeUpload.substr(6))));
        $(div).append('<br />')
        $(div).append('User uploaded file: ' + data.UserName);
        $(div).append('<br />')

        var baton = dt.createElement('button');
        $(baton).text('Close');
        $(baton).addClass('btn-success');
        $(baton).click(function () { $(this).parent().remove() });
        $(div).append(baton);

        dt.body.appendChild(div);
    }).fail(function (jqxhr, settings, ex) { alert('Bad request: ' + ex); });
});
$(":button[name='edit']").click(function (e) {
    var _this = this;
    var point = $(this).offset();
    var index = $(this).attr('data-index');
    var file_name = $(this).attr('data-name');
    var name = $.trim($(this).parent().parent().children().first().text());
    var input_value = name.substring(0, name.lastIndexOf('.'));

    $.getJSON('/FilesViewAndAction/Details', { id: index }, function (data, textStatus, jqXHR) {
        if (data.result == 'nul') { alert('HttpStatusCode.BadRequest'); }
        if (data.result == 'null') { alert('HttpStatusCode.NoContent'); }

        var dt = document;
        var div = dt.createElement('div');
        $(div).addClass('edit');
        $(div).css({ 'top': point.top, 'left': point.left - 250 });
        $(div).append(' Name: ');
        $(div).append($('<input>', { type: 'text', id: index, value: input_value, class: 'textfield' }));
          $(div).append('<br />');
        $(div).append('Type: ' + data.Type);
          $(div).append('<br />');
        $(div).append('Size: ' + data.Size + ' Mb');
          $(div).append('<br />');
        $(div).append('TadeTime: ' + new Date(parseInt(data.DataTimeUpload.substr(6))));
          $(div).append('<br />');
        $(div).append('User uploaded file: ' + data.UserName);
        $(div).append('<br />');

        var button_edit = dt.createElement('button');
        $(button_edit).text('Edit');
        $(button_edit).addClass('btn-warning');
        $(button_edit).click(function () {
                   var f_name = $('#' + index).val();
                   if (f_name.length < 1 || f_name.length > 256) {
                       alert('Name length  must be - " 0 < file_name < 256 " -');
                       return false;
                   }
                   //alert(f_name)
            $.getJSON('/FilesViewAndAction/Edit', { id: index, moniker: f_name }, function (data, textStatus, jqXHR) {
                if (data.response == 'No User Identities')
                {
                    alert('Status : :' + data.response);
                    return false;
                }
                
                       $(_this).parent().parent().fadeOut(500);
                       $(_this).parent().parent().children().first().text(data.response);
                       $(_this).parent().parent().fadeIn(1500);
                   }).fail(function (jqxhr, settings, ex) { alert('Request failed, ' + ex); });
                  
                   $(this).parent().remove();
               });          
        div.appendChild(button_edit);

        var button_close = dt.createElement('button');
        $(button_close).text('Close');
        $(button_close).addClass('btn-success');
        $(button_close).click(function () { $(this).parent().remove() });
        div.appendChild(button_close);

        dt.body.appendChild(div);
    }).fail(function (jqxhr, settings, ex) { alert('Bad request: ' + ex); }); 
        
        //var batton_edit = dt.createElement('button');
        //$(batton_edit).text('Edit');
        //$(batton_edit).click(function () {
        //    var f_name = $('#' + index).val();
        //    if (f_name.length < 1 || f_name.length > 256) {
        //        alert('Name length  must be - " 0 < file_name < 256 " -');
        //        return false;
        //    }
        //    alert(f_name)
        //    $.getJSON('/FilesViewAndAction/Edit', { id: index, moniker: f_name }, function (data, textStatus, jqXHR) {
        //        alert(data.response);
        //    }).fail(function (jqxhr, settings, ex) { alert('failed, ' + ex); });
        //      .done(function () { alert('Request done!'); })
        //    $(this).parent().remove();
        //});

        //var batton_close = dt.createElement('button');
        //$(batton_close).text('Close');
        //$(batton_close).click(function () {

        //    $(this).parent().remove()
        //});

        //div.appendChild(batton_edit);
        //div.appendChild(batton_close); 
});
$(":button[value='Download']").click(function (e) {
    var _this = this;
    var index = $(this).attr('data-index');
    var name = $.trim($(this).parent().parent().children().first().text());

    $.get('/FilesViewAndAction/Preload', { id: index, moniker: name }, function (data, textStatus, jqXHR) {
        if ('Request handled' == data.answer)
            window.document.location = '/FilesViewAndAction/Download/' + index;
        else
            alert("Server response: " + data.answer);
    }).fail(function (jqxhr, settings, ex) { alert('The request is not processed: ' + ex); })
});
$('#Create-report-excell-file').click(function () {
    //alert("Report created");
    window.document.location = '/FilesDownloadNotes/CreateSendExcellFile';
});
