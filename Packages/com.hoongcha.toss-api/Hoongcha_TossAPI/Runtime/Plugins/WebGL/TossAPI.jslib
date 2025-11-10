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
  },

  RequestStorageSetItem: function(gameObjectName, callbackMethod, keyString, valueString) {
    var objName = UTF8ToString(gameObjectName);
    var method = UTF8ToString(callbackMethod);
    var key = UTF8ToString(keyString);
    var value = UTF8ToString(valueString);
    
    if (window.TossStorageSetItem) {
      window.TossStorageSetItem(key,value)
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
              message: `${key}에 ${value}를 저장했습니다.`
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
      console.error('TossStorageSetItem 함수가 정의되지 않았습니다.');
      SendMessage(objName, method, JSON.stringify({
        status: 'error',
        message: 'React bridge function not found'
      }));
    }
  },
  RequestStorageGetItem: function(gameObjectName, callbackMethod, keyString) {
    var objName = UTF8ToString(gameObjectName);
    var method = UTF8ToString(callbackMethod);
    var key = UTF8ToString(keyString);
    
    if (window.TossStorageGetItem) {
        window.TossStorageGetItem(key)
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
                        message: result.value
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
        console.error('TossStorageGetItem 함수가 정의되지 않았습니다.');
        SendMessage(objName, method, JSON.stringify({
            status: 'error',
            message: 'React bridge function not found'
        }));
      }
  },

  RequestStorageRemoveItem: function(gameObjectName, callbackMethod, keyString) {
      var objName = UTF8ToString(gameObjectName);
      var method = UTF8ToString(callbackMethod);
      var key = UTF8ToString(keyString);
      
      if (window.TossStorageRemoveItem) {
          window.TossStorageRemoveItem(key)
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
                          message: `${key}를 삭제했습니다.`
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
          console.error('TossStorageRemoveItem 함수가 정의되지 않았습니다.');
          SendMessage(objName, method, JSON.stringify({
              status: 'error',
              message: 'React bridge function not found'
          }));
      }
  },

  RequestStorageAllClearItem: function(gameObjectName, callbackMethod) {
      var objName = UTF8ToString(gameObjectName);
      var method = UTF8ToString(callbackMethod);
      
      if (window.TossStorageAllClearItem) {
          window.TossStorageAllClearItem()
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
                          message: '모든 항목을 삭제했습니다.'
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
          console.error('TossStorageAllClearItem 함수가 정의되지 않았습니다.');
          SendMessage(objName, method, JSON.stringify({
              status: 'error',
              message: 'React bridge function not found'
          }));
      }
  }

  });