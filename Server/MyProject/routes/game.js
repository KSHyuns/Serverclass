

module.exports = function(server)
{
    //socket.io = 웹서버는 단방향으로만 리퀘스트를 주고 형태
    var io = require('socket.io')(server,{
        transports : ['websocket']
    });

    //접속했을때 호출
    io.on('connection',function(socket){
            console.log("Connected : " + socket.id);

            //접속중일때,  접속이 끈켯다면
            //접속이 끈켯을때
            socket.on('disconnect', function(reason){
                console.log('Disconnected : '+socket.id);
             });

            //  socket.on('hi',function(reason){
            //     console.log('hi');
            //     //socket.emit('hello'); //한명에게만 소켓을 보낸다.
            //     //io.emit('hello');   //모든 소켓들에게 메시지를 보낸다.
            //     //socket.broadcast.emit('hello');   //메세지를 직접받은 클라이언트 외 나머지 클라이언트들에게 메세지를 보낸다.
            //  });

            //메세지가 왔는지 알리는 식별자
             socket.on('message',function(msg){
                //메세지가 잘 들어오는지 확인
                //들어오는 메세지의 구조를 볼 수 있다
                console.dir(msg);
                socket.broadcast.emit('chat',msg);
                
                //socket.broadcast.emit('chat');
             });


    });

    //서버에접속한 모든 클라이언트가 할일들을 감지할 수 있게된다.   io.on

   


    //소켓이 접속한 후 일어난 이벤트들은 socket.on

};