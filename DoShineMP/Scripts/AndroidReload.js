var oldIn = 0;  //用户报修列表
var oldIn1 = 0; //企业报修列表

function setOldIndex() {
    setInterval(function () {
        if (window.localStorage['oldIndexLi'] != null && window.localStorage['oldIndexLi'] != '') {
            window.localStorage['oldIndexLi'] = null;
            window.location.reload();
        }
    }, 1000)
}


function setOldIndex1() {
    setInterval(function () {
        if (window.localStorage['oldIndexLi1'] != null && window.localStorage['oldIndexLi1'] != '') {
            window.localStorage['oldIndexLi1'] = null;
            window.location.reload();
        }
    }, 1000)
}