//              $.get(url, [data], [callback])
//              $.getJSON(url, [data], [callback])
//              $.getScript(url, [callback])
//              $.post(url,[data],[callback],[type]) 
//              $.load(url, [data], [callback]);


$.get('/data.txt',  // url
      function (data, textStatus, jqXHR) {  // success callback
          alert('status: ' + textStatus + ', data:' + data);
      });

//Example: Retrieve JSON data using get() method
$.get('/jquery/getjsondata', {name:'Steve'}, function (data, textStatus, jqXHR) {
    $('p').append(data.firstName);
});
$.getJSON('/jquery/getjsondata', { name: 'Steve' }, function (data, textStatus, jqXHR) {
    alert(data.firstName);
}) .done(function () { alert('Request done!'); })
   .fail(function (jqxhr, settings, ex) { alert('failed, ' + ex); });

//Example: jQuery getScript() method
$.getScript('/Scripts/myJavaScriptFile.js', function(script,status,jqxhr){
    alert(status);
});

//----  $.post(url,[data],[callback],[type])  -----
//Example: jQuery post() method  
$.post('/jquery/submitData',
        { myData: 'This is my data.' },
        function (data, status, xhr) {

            $('p').append('status: ' + status + ', data: ' + data);

        }).done(function () { alert('Request done!'); })
        .fail(function (jqxhr, settings, ex) { alert('failed, ' + ex); });

//Example: submit JSON data using post() method
$.post('/submitJSONData',  // url
       { myData: 'This is my data.' }, // data to be submit
       function(data, status, xhr) {   // success callback function
           alert('status: ' + status + ', data: ' + data.responseData);
       },
        'json'); // response data format

//   Example: jQuery load() method
//      $.load(url,[data],[callback]);
$('#msgDiv').load('getData', // url 
                  { name: 'bill' },    // data 
                  function(data, status, jqXGR) {  // callback function 
                      alert('data loaded')
                  });

//   -----------   <div id="msgDiv"></div>



       