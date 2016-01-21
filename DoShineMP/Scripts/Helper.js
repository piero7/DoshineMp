
//微信多图轮播
$(document).on('click', '.imgyulan', function () {

    var imglist = $(this).parent().find('.imgyulan');
    var imgurl = $(this).attr('src');
    var imgstr = '';
    for (var i = 0; i < imglist.length; i++) {
        if (imgstr.length == 0) {
            //console.log(imglist[i]);
            imgstr += imglist[i].attributes['src'].value;
        } else {
            imgstr += ',' + imglist[i].attributes['src'].value;
        }
    }
    wx.previewImage({
        current: imgurl,
        urls: imgstr.split(',')
    });
})


