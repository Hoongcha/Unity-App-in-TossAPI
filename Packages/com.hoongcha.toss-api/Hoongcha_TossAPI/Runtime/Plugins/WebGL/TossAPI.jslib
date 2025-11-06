// Assets/Plugins/WebGL/GetUserKey.jslib
mergeInto(LibraryManager.library, {

  RequestUserKey: function(gameObjectName, callbackMethod) {
    var objName = UTF8ToString(gameObjectName);
    var method = UTF8ToString(callbackMethod);
    
    if (window.TossGetUserKeyForGame) {
      window.TossGetUserKeyForGame()
        .then(function(result) {
          if (!result) {
            SendMessage(objName, method, JSON.stringify({
              status: 'error',
              message: '지원하지 않는 앱 버전이에요.'
            }));
            return;
          }
          
          // if (result === 'INVALID_CATEGORY') {
          //   SendMessage(objName, method, JSON.stringify({
          //     status: 'error',
          //     message: 'INVALID_CATEGORY'
          //   }));
          //   return;
          // }
          
          // if (result === 'ERROR') {
          //   SendMessage(objName, method, JSON.stringify({
          //     status: 'error',
          //     message: 'ERROR'
          //   }));
          //   return;
          // }
          
          if (result.type === 'HASH') {
            SendMessage(objName, method, JSON.stringify({
              status: 'success',
              hash: result.hash
            }));
          }
        })
        .catch(function(error) {
          SendMessage(objName, method, JSON.stringify({
            status: 'error',
            message: error.toString()
          }));
        });
    } else {
      console.error('TossGetUserKeyForGame 함수가 정의되지 않았습니다.');
      SendMessage(objName, method, JSON.stringify({
        status: 'error',
        message: 'React bridge function not found'
      }));
    }
  },

  RequestOpenLeaderboard: function(gameObjectName, callbackMethod) {
    var objName = UTF8ToString(gameObjectName);
    var method = UTF8ToString(callbackMethod);
    
    if (window.TossOpenGameCenterLeaderboard) {
      try {
        var result = window.TossOpenGameCenterLeaderboard();
        
        if (result) {
          SendMessage(objName, method, JSON.stringify({
            status: 'success',
            message: '리더보드가 열렸습니다.'
          }));
        } else {
          SendMessage(objName, method, JSON.stringify({
            status: 'error',
            message: '지원하지 않는 앱 버전이에요.'
          }));
        }
      } catch (error) {
        SendMessage(objName, method, JSON.stringify({
          status: 'error',
          message: error.toString()
        }));
      }
    } else {
      console.error('TossOpenGameCenterLeaderboard 함수가 정의되지 않았습니다.');
      SendMessage(objName, method, JSON.stringify({
        status: 'error',
        message: 'React bridge function not found'
      }));
    }
  },

  RequestSubmitLeaderBoardScore: function(gameObjectName, callbackMethod, scoreString) {
    var objName = UTF8ToString(gameObjectName);
    var method = UTF8ToString(callbackMethod);
    var score = UTF8ToString(scoreString);
    
    if (window.TossSubmitGameCenterLeaderBoardScore) {
      window.TossSubmitGameCenterLeaderBoardScore(score)
        .then(function(result) {
          if (!result) {
            SendMessage(objName, method, JSON.stringify({
              status: 'error',
              message: '지원하지 않는 앱 버전이에요.'
            }));
            return;
          }
          if (result.statusCode === 'SUCCESS') {
            SendMessage(objName, method, JSON.stringify({
              status: 'success',
              message: '점수 제출 성공!'
            }));
          } else {
            SendMessage(objName, method, JSON.stringify({
              status: 'error',
              message: result.statusCode
            }));
          }
        })
        .catch(function(error) {
          SendMessage(objName, method, JSON.stringify({
            status: 'error',
            message: error.toString()
          }));
        });
    } else {
      console.error('TossSubmitGameCenterLeaderBoardScore 함수가 정의되지 않았습니다.');
      SendMessage(objName, method, JSON.stringify({
        status: 'error',
        message: 'React bridge function not found'
      }));
    }
  }

});