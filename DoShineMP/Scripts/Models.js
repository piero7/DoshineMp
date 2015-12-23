///消息对象
function NickIp(NickID, NickName, Msg, IsFirst, IsUser, PairingServiceID, PairingUserID, PairingServiceIP, PairingUserIP, Headimgurl) {
    this.NickID = NickID;                                       //唯一凭证
    this.NickName = NickName;                                   //昵称
    this.Msg = Msg;                                             //消息主体
    this.IsFirst = IsFirst;                                     //是否第一次链接
    this.IsUser = IsUser;                                       //是否用户
    this.PairingServiceID = PairingServiceID;                   //配对客服昵称
    this.PairingUserID = PairingUserID;                         //配对用户昵称
    this.PairingServiceIP = PairingServiceIP;                   //配对客服IP
    this.PairingUserIP = PairingUserIP;                         //配对用户IP
    this.Headimgurl = Headimgurl;
}