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
            SendMessage(objName, method, JSON.stringify({
              status: 'success',
              message: `${key}에 ${value}를 저장했습니다.`
            }));
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
              if(result == null){
                 result = '';
              }
                SendMessage(objName, method, JSON.stringify({
                    status: 'success',
                    message: result
                }));
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
                  SendMessage(objName, method, JSON.stringify({
                          status: 'success',
                          message: `${key}를 삭제했습니다.`
                  }));
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
                  SendMessage(objName, method, JSON.stringify({
                      status: 'success',
                      message: '모든 항목을 삭제했습니다.'
                  }));
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
  },
  AppStateSubscribe: function (gameObjectName, callbackMethod) {
    var goName = UTF8ToString(gameObjectName);
    var method = UTF8ToString(callbackMethod);

    // 이전 구독 해제
    if (typeof window.__tossVisUnsub === 'function') {
      try { window.__tossVisUnsub(); } catch (e) {}
      window.__tossVisUnsub = null;
    }
    
    function send(state, evt) {
      var payload = JSON.stringify({
        state: state, // "visible" | "hidden"
        eventType: evt, // "visibilitychange" | "pagehide" | "pageshow" | "blur" | "focus" | "init"
        hidden: state === 'hidden',
        ts: Date.now()
      });
      try { SendMessage(goName, method, payload); } catch (e) {}
    }

    // 캡처 단계로 가장 먼저 잡는다(숨기기 직전에도 최대한 빨리 유니티 호출)
    var opts = { capture: true, passive: true };

    function onVisibility() { send(document.hidden ? 'hidden' : 'visible', 'visibilitychange'); }
    function onPageHide()   { send('hidden',  'pagehide'); }
    function onPageShow()   { send('visible', 'pageshow'); }
    function onBlur()       { send(document.hidden ? 'hidden' : 'visible', 'blur'); }
    function onFocus()      { send(document.hidden ? 'hidden' : 'visible', 'focus'); }
    function onFreeze()     { send('hidden', 'freeze'); }
    
    // 이벤트 구독
    document.addEventListener('visibilitychange', onVisibility, opts);
    window.addEventListener('pagehide', onPageHide, opts);
    window.addEventListener('pageshow', onPageShow, opts);
    window.addEventListener('blur', onBlur, opts);
    window.addEventListener('focus', onFocus, opts);
    window.addEventListener('freeze', onFreeze, opts);

    // 초기 1회 상태 통지(필요 시)
    send(document.hidden ? 'hidden' : 'visible', 'init');

    // 이벤트 구독 해제 함수 정의
    window.__tossVisUnsub = function () {
      document.removeEventListener('visibilitychange', onVisibility, opts);
      window.removeEventListener('pagehide', onPageHide, opts);
      window.removeEventListener('pageshow', onPageShow, opts);
      window.removeEventListener('blur', onBlur, opts);
      window.removeEventListener('focus', onFocus, opts);
      window.removeEventListener('freeze', onFreeze, opts);
    };
  },

  RequestLoadAD: function(gameObjectName, callbackMethod, AD_GROUP_ID) {
    var objName = UTF8ToString(gameObjectName);
    var method = UTF8ToString(callbackMethod);
    var adID = UTF8ToString(AD_GROUP_ID);
    
    if (window.TossLoadAD) {
      try {
        window.TossLoadAD(adID, function(result) {
          if (result.type === 'loaded') {
            SendMessage(objName, method, JSON.stringify({
              status: 'success',
              message: '광고 로드 성공'
            }));
          }else{
            SendMessage(objName, method, JSON.stringify({
              status: 'error',
              message: result.message
            }));
          }
        });
      } catch (error) {
        SendMessage(objName, method, JSON.stringify({
          status: 'error',
          message: error.toString()
        }));
      }
    } else {
      console.error('TossLoadAD 함수가 정의되지 않았습니다.');
      SendMessage(objName, method, JSON.stringify({
        status: 'error',
        message: 'React bridge function not found'
      }));
    }
  },
  RequesShowAD: function(gameObjectName, callbackMethod, AD_GROUP_ID) {
    var objName = UTF8ToString(gameObjectName);
    var method = UTF8ToString(callbackMethod);
    var adID = UTF8ToString(AD_GROUP_ID);
    
    if (window.TossShowAD) {
      try {
        window.TossShowAD(adID, function(result) {
          switch (result.type) {
            case 'requested':
              SendMessage(objName, method, JSON.stringify({
                status: 'requested',
                message: '광고 보여주기 요청 완료'
              }));
              break;
            case 'clicked':
              SendMessage(objName, method, JSON.stringify({
                status: 'clicked',
                message: '광고 클릭'
              }));
              break;
            case 'dismissed':
              SendMessage(objName, method, JSON.stringify({
                status: 'dismissed',
                message: '광고 닫힘'
              }));
              break;
            case 'impression':
              SendMessage(objName, method, JSON.stringify({
                status: 'impression',
                message: '광고 노출'
              }));
              break;
            case 'userEarnedReward':
              SendMessage(objName, method, JSON.stringify({
                status: 'userEarnedReward',
                unitType: result.data.unitType,
                unitAmount: result.data.unitAmount,
                message: '사용자가 광고 시청을 완료했음'
              }));
              break;
            case 'show':
              SendMessage(objName, method, JSON.stringify({
                status: 'show',
                message: '광고 컨텐츠 보여졌음'
              }));
              break;
            case 'failedToShow':
              SendMessage(objName, method, JSON.stringify({
                status: 'failedToShow',
                message: '광고 보여주기 실패'
              }));
              break;
            default:
              SendMessage(objName, method, JSON.stringify({
                status: 'error',
                message: 'status Something Weird'
              }));
              break;
          }
        });
      } catch (error) {
        SendMessage(objName, method, JSON.stringify({
          status: 'error',
          message: error.toString()
        }));
      }
    } else {
      console.error('TossShowAD 함수가 정의되지 않았습니다.');
      SendMessage(objName, method, JSON.stringify({
        status: 'error',
        message: 'React bridge function not found'
      }));
    }
  }
});