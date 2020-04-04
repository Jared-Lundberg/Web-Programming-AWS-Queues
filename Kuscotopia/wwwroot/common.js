var mainUrl = "api/values";

function simpleResult(data) {
    document.getElementById("result").innerHTML = JSON.stringify(data);
}

function simpleError(data) {
    document.getElementById("error").innerHTML = JSON.stringify(data);
}

function runPost() {
    $.ajax(mainUrl + "/" + document.getElementById("number").value,
        {
            method: "POST",
            success: simpleResult,
            error: simpleError,
            contentType: "application/json",
            processData: false,
            data: JSON.stringify(
                {
                    WorkerCount: document.getElementById("number").value
                })
        });
}

window.onload = function () {
    document.getElementById("start").onclick = runPost;
}