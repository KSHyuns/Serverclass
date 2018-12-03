var express = require('express');
var util = require('../util');
const { ObjectId } = require('mongodb'); 
var router = express.Router();
const bcrypt = require('bcrypt');
const saltRounds = 10;

var ResponseType = {
  INVALID_USERNAME: 0,
  INVALID_PASSWORD: 1,
  SUCCESS: 2,
}

/* GET users listing. */
router.get('/', function(req, res, next) {
  res.send('respond with a resource');
});

// User Info
router.get('/info',  util.isLogined, function(req, res, next) 
{
  var username = req.session.username;
  var nickname = req.session.nickname;
  var isAuthenticated = req.session.isAuthenticated;

  var database = req.app.get('database');
  var users = database.collection('users');


 if (req.session.isAuthenticated == true) {

    var resuotObj = {
      isAuthenticated : isAuthenticated,
      username:  username,
      nickname : nickname
    };

    res.json(resuotObj);
  } 
  else
  {
    var resuotObj2 = {
      isAuthenticated : req.session.isAuthenticated,
      username:  req.session.username,
      nickname : req.session.nickname
    };

    res.json(resuotObj2);
  }


});
// 로그인
router.post('/signin', function(req, res, next) {
  var username = req.body.username;
  var password = req.body.password;

  var database = req.app.get('database');
  var users = database.collection('users');

  if (username !== undefined && password !== undefined) {
      users.findOne({ username: username }, function(err, result) {
        if (result) {
//          if (password === result.password) {
          var compareResult = bcrypt.compareSync(password, result.password);
          if (compareResult) {
            req.session.isAuthenticated = true;
            req.session.userid = result._id.toString();
            req.session.username = result.username;
            req.session.nickname = result.nickname;
            res.json({ result: ResponseType.SUCCESS });
          } else {
            res.json({ result:ResponseType.INVALID_PASSWORD });
          }
        } else {
          res.json({ result:ResponseType.INVALID_USERNAME });
        }
      });
  }
});

// 사용자 등록
router.post('/add', function(req, res, next) {
  var username = req.body.username;
  var password = req.body.password;
  var nickname = req.body.nickname;
  // var score = req.body.score;

  var salt = bcrypt.genSaltSync(saltRounds);
  var hash = bcrypt.hashSync(password, salt);

  var database = req.app.get("database");
  var users = database.collection('users');

  if (username !== undefined && password !== undefined 
    && nickname !== undefined) {
      users.insert([{ "username" : username, "password" : hash, "nickname" : nickname }], function(err, result) {
        res.status(200).send("success");
      });
  }
});

// Score 추가
router.get('/addscore/:score',  util.isLogined, function(req, res, next) {

  var score = req.params.score;
  var userid = req.session.userid;

  var database = req.app.get("database");
  var users = database.collection('users');

  if (userid != undefined) {
    result = users.updateOne({ _id: ObjectId(userid) }, 
      { $set: {
        score: Number(score),
        updatedAt: Date.now()
      }}, { upsert: true }, function(err) {
        if (err) {
          res.status(200).send("failure");
        }
        res.status(200).send("success");
      });
  }
});

// Score 불러오기
router.get('/score', util.isLogined, function(req, res, next) {
  var userid = req.session.userid;
  var database = req.app.get("database");
  var users = database.collection('users');

  users.findOne({ _id: ObjectId(userid) }, function(err, result) {
    if (err) throw err;

    var resultObj = {
      _id: result._id.toString(),
      score: result.score,
      nickname : result.nickname
    };

    res.json(resultObj);
  });
});


module.exports = router;
