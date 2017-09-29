var dropZone = document.getElementById('dropZone');

// Optional.   Show the copy icon when dragging over.  Seems to only work for chrome.
dropZone.addEventListener('dragover', function (e) {
    e.stopPropagation();
    e.preventDefault();
    e.dataTransfer.dropEffect = 'copy';
});

// Get file data on drop
dropZone.addEventListener('drop', function (e) {
    
    e.stopPropagation();
    e.preventDefault();
    var dt = e.srcElement;
    alert(dt.childElementCount);
    alert("Items: " + dt.valueOf() + "\n");

    for (var i = 0, typ; typ = item[i].size; i++)
    {
        alert(typ)
    }
});

function upload(postUrl, fieldName, filePath) {
    var formData = new FormData();
    formData.append(fieldName, new File(filePath));

    var req = new XMLHttpRequest();
    req.open("POST", postUrl);
    req.onload = function (event) { alert(event.target.responseText); };
    req.send(formData);
}