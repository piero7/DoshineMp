/* 
* @Author: 晴
* @Date:   2015-12-24 11:46:18
* @Last Modified by:   晴
* @Last Modified time: 2015-12-24 13:30:04
*/

'use strict';

var gle = function(ckk){
	var Time = document.getElementsByClassName('list-group')[ckk];
	var TimeLi = Time.getElementsByTagName('li');
	for(var i=0;i<TimeLi.length;i++){
		TimeLi[i].addEventListener('touchstart',starthanlder)
		TimeLi[i].addEventListener('touchend',endhanlder)
	}
	function starthanlder(evt){
		this.style.backgroundColor = '#ddd';
	}

	function endhanlder(evt){
		this.style.backgroundColor = '#fff';
	}
}

gle(0);

