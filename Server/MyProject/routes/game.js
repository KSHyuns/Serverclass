

module.exports = function(server)
{
    //socket.io = 웹서버는 단방향으로만 리퀘스트를 주고 형태
    var io = require('socket.io')(server,{
        transports : ['websocket']
    });

    //접속했을때 호출
    io.on('connection',function(socket){
            console.log("Connected : " + socket.id);

            //접속중일때,  접속이 끈켯을때 
            //접속이 끈켯을때
            socket.on('disconnect', function(reason){
                console.log('Disconnected : '+socket.id);
             });

    });

    //서버에접속한 모든 클라이언트가 할일들을 감지할 수 있게된다.   io.on

   


    //소켓이 접속한 후 일어난 이벤트들은 socket.on

};