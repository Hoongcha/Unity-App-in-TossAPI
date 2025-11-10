using System;
using System.Threading;
using System.Threading.Tasks;
using Hoongcha_TossAPI;
using UnityEngine;

public class TossStorageJSBridge : MonoBehaviour
{
    #region SetItem
    
    #if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void RequestStorageSetItem(string gameObjectName, string callbackMethod, string key, string value);
    #endif

    private Action<string> onSuccessSetItem;
    private Action<string> onSetItemError;

    /// <summary>
    /// Storage에 값을 저장합니다.
    /// </summary>
    /// <param name="key">저장할 키</param>
    /// <param name="value">저장할 값</param>
    /// <param name="onSuccessReceived">성공 했을때 호출</param>
    /// <param name="onError">에러날때 호출</param>
    public void SetItem(string key, string value, Action<string> onSuccessReceived, Action<string> onError)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        this.onSuccessSetItem = onSuccessReceived;
        this.onSetItemError = onError;
        
        RequestStorageSetItem(gameObject.name, "OnSetItemCallback", key, value);
    #else
        Debug.LogWarning("Storage.setItem은 WebGL 빌드에서만 동작합니다.");
        onSetItemError("WebGL 환경이 아닙니다.");
    #endif
    }

    public void OnSetItemCallback(string jsonResponse)
    {
        try
        {
            JSBridgeResponse response = JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

            if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
            {
                onSuccessSetItem(response.message);
            }
            else
            {
                onSetItemError(response.message);
            }
        }
        catch (Exception e)
        {
            onSetItemError($"{e.Message}");
        }
    }

    #endregion

    #region GetItem
    
    #if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void RequestStorageGetItem(string gameObjectName, string callbackMethod, string key);
    #endif

    private Action<string> onSuccessGetItem;
    private Action<string> onGetItemError;

    /// <summary>
    /// Storage에서 값을 가져옵니다.
    /// </summary>
    /// <param name="key">가져올 키</param>
    /// <param name="onSuccessReceived">성공 했을때 호출 (저장된 값 반환)</param>
    /// <param name="onError">에러날때 호출</param>
    public void GetItem(string key, Action<string> onSuccessReceived, Action<string> onError)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        this.onSuccessGetItem = onSuccessReceived;
        this.onGetItemError = onError;
        
        RequestStorageGetItem(gameObject.name, "OnGetItemCallback", key);
    #else
        Debug.LogWarning("Storage.getItem은 WebGL 빌드에서만 동작합니다.");
        onGetItemError("WebGL 환경이 아닙니다.");
    #endif
    }

    public void OnGetItemCallback(string jsonResponse)
    {
        try
        {
            JSBridgeResponse response = JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

            if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
            {
                onSuccessGetItem(response.message);
            }
            else
            {
                onGetItemError(response.message);
            }
        }
        catch (Exception e)
        {
            onGetItemError($"{e.Message}");
        }
    }

    #endregion

    #region RemoveItem
    
    #if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void RequestStorageRemoveItem(string gameObjectName, string callbackMethod, string key);
    #endif

    private Action<string> onSuccessRemoveItem;
    private Action<string> onRemoveItemError;

    /// <summary>
    /// Storage에서 값을 삭제합니다.
    /// </summary>
    /// <param name="key">삭제할 키</param>
    /// <param name="onSuccessReceived">성공 했을때 호출</param>
    /// <param name="onError">에러날때 호출</param>
    public void RemoveItem(string key, Action<string> onSuccessReceived, Action<string> onError)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        this.onSuccessRemoveItem = onSuccessReceived;
        this.onRemoveItemError = onError;
        
        RequestStorageRemoveItem(gameObject.name, "OnRemoveItemCallback", key);
    #else
        Debug.LogWarning("Storage.removeItem은 WebGL 빌드에서만 동작합니다.");
        onRemoveItemError("WebGL 환경이 아닙니다.");
    #endif
    }

    public void OnRemoveItemCallback(string jsonResponse)
    {
        try
        {
            JSBridgeResponse response = JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

            if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
            {
                onSuccessRemoveItem(response.message);
            }
            else
            {
                onRemoveItemError(response.message);
            }
        }
        catch (Exception e)
        {
            onRemoveItemError($"{e.Message}");
        }
    }

    #endregion
    
    #region AllClearItem
    
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void RequestStorageAllClearItem(string gameObjectName, string callbackMethod);
#endif

    private Action<string> onSuccessAllClearItem;
    private Action<string> onAllClearItemError;

    /// <summary>
    /// Storage의 모든 항목을 초기화합니다.
    /// </summary>
    /// <param name="onSuccessReceived">성공 했을때 호출</param>
    /// <param name="onError">에러날때 호출</param>
    public void AllClearItem(Action<string> onSuccessReceived, Action<string> onError)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        this.onSuccessAllClearItem = onSuccessReceived;
        this.onAllClearItemError = onError;
        
        RequestStorageAllClearItem(gameObject.name, "OnAllClearItemCallback");
#else
        Debug.LogWarning("Storage.clearItems은 WebGL 빌드에서만 동작합니다.");
        onAllClearItemError("WebGL 환경이 아닙니다.");
#endif
    }

    public void OnAllClearItemCallback(string jsonResponse)
    {
        try
        {
            JSBridgeResponse response = JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

            if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
            {
                onSuccessAllClearItem(response.message);
            }
            else
            {
                onAllClearItemError(response.message);
            }
        }
        catch (Exception e)
        {
            onAllClearItemError($"{e.Message}");
        }
    }

    #endregion

    #region SetItemAsync

    /// <summary>
    /// 현재 진행 중인 setItem 요청의 완료를 통지하는 TCS
    /// </summary>
    private TaskCompletionSource<string> setItemTCS;

    /// <summary>
    /// 호출자가 넘긴 취소 토큰 등록 핸들(취소 시 해제)
    /// </summary>
    private CancellationTokenRegistration setItemCtr;

    /// <summary>
    /// Storage에 값을 저장합니다. (Task 기반 / 타임아웃 미사용)
    /// </summary>
    /// <param name="key">저장할 키</param>
    /// <param name="value">저장할 값</param>
    /// <param name="cancellationToken">캔슬 토큰</param>
    /// <returns>성공 시 JS 측에서 넘어온 message</returns>
    /// <exception cref="InvalidOperationException">이미 진행 중인 요청이 있을 때</exception>
    /// <exception cref="OperationCanceledException">취소 시</exception>
    /// <exception cref="NotSupportedException">WebGL 환경이 아닐 때</exception>
    public Task<string> SetItemAsync(string key, string value, CancellationToken cancellationToken = default)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        // 동시 호출 방지: 이전 요청이 완료되지 않았다면 예외
        if (setItemTCS != null && !setItemTCS.Task.IsCompleted)
            throw new InvalidOperationException("이미 진행 중인 SetItem 요청이 있습니다.");

        // 콜백 스레드 안전성을 위해 RunContinuationsAsynchronously 사용
        setItemTCS = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

        // 취소 토큰이 트리거되면 Task 취소 및 정리
        if (cancellationToken.CanBeCanceled)
        {
            setItemCtr = cancellationToken.Register(() =>
            {
                setItemTCS?.TrySetCanceled(cancellationToken);
                CleanupSetItemTaskState();
            });
        }

        // JS 쪽에 콜백 대상 메서드 이름을 넘겨 호출
        RequestStorageSetItem(gameObject.name, "SetItemCallbackAsync", key, value);
        return setItemTCS.Task;
    #else
        return Task.FromException<string>(new NotSupportedException("Storage.setItem은 WebGL 빌드에서만 동작합니다."));
    #endif
    }

    public void SetItemCallbackAsync(string jsonResponse)
    {
        try
        {
            var response = JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

            if (setItemTCS == null)
            {
                // 대기 중인 요청이 없을 때 들어온 콜백 — 로그만 남김
                Debug.LogWarning("대기 중인 SetItem 요청이 없는데 콜백이 도착했습니다.");
                return;
            }

            if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
            {
                setItemTCS.TrySetResult(response.message);
            }
            else
            {
                setItemTCS.TrySetException(new Exception(response.message));
            }
        }
        catch (Exception e)
        {
            setItemTCS?.TrySetException(new Exception($"JSON PARSE ERROR\n{e.Message}"));
        }
        finally
        {
            CleanupSetItemTaskState();
        }
    }

    /// <summary>
    /// 내부 상태/리소스 정리 
    /// </summary>
    private void CleanupSetItemTaskState()
    {
        try { setItemCtr.Dispose(); } catch { /* 무시 */ }
        setItemCtr = default;
        setItemTCS = null;
    }

    #endregion

    #region GetItemAsync

    /// <summary>
    /// 현재 진행 중인 getItem 요청의 완료를 통지하는 TCS
    /// </summary>
    private TaskCompletionSource<string> getItemTCS;

    /// <summary>
    /// 호출자가 넘긴 취소 토큰 등록 핸들(취소 시 해제)
    /// </summary>
    private CancellationTokenRegistration getItemCtr;

    /// <summary>
    /// Storage에서 값을 가져옵니다. (Task 기반 / 타임아웃 미사용)
    /// </summary>
    /// <param name="key">가져올 키</param>
    /// <param name="cancellationToken">캔슬 토큰</param>
    /// <returns>성공 시 저장된 값</returns>
    /// <exception cref="InvalidOperationException">이미 진행 중인 요청이 있을 때</exception>
    /// <exception cref="OperationCanceledException">취소 시</exception>
    /// <exception cref="NotSupportedException">WebGL 환경이 아닐 때</exception>
    public Task<string> GetItemAsync(string key, CancellationToken cancellationToken = default)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        // 동시 호출 방지: 이전 요청이 완료되지 않았다면 예외
        if (getItemTCS != null && !getItemTCS.Task.IsCompleted)
            throw new InvalidOperationException("이미 진행 중인 GetItem 요청이 있습니다.");

        // 콜백 스레드 안전성을 위해 RunContinuationsAsynchronously 사용
        getItemTCS = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

        // 취소 토큰이 트리거되면 Task 취소 및 정리
        if (cancellationToken.CanBeCanceled)
        {
            getItemCtr = cancellationToken.Register(() =>
            {
                getItemTCS?.TrySetCanceled(cancellationToken);
                CleanupGetItemTaskState();
            });
        }

        // JS 쪽에 콜백 대상 메서드 이름을 넘겨 호출
        RequestStorageGetItem(gameObject.name, "GetItemCallbackAsync", key);
        return getItemTCS.Task;
    #else
        return Task.FromException<string>(new NotSupportedException("Storage.getItem은 WebGL 빌드에서만 동작합니다."));
    #endif
    }

    public void GetItemCallbackAsync(string jsonResponse)
    {
        try
        {
            var response = JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

            if (getItemTCS == null)
            {
                // 대기 중인 요청이 없을 때 들어온 콜백 — 로그만 남김
                Debug.LogWarning("대기 중인 GetItem 요청이 없는데 콜백이 도착했습니다.");
                return;
            }

            if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
            {
                getItemTCS.TrySetResult(response.message);
            }
            else
            {
                getItemTCS.TrySetException(new Exception(response.message));
            }
        }
        catch (Exception e)
        {
            getItemTCS?.TrySetException(new Exception($"JSON PARSE ERROR\n{e.Message}"));
        }
        finally
        {
            CleanupGetItemTaskState();
        }
    }

    /// <summary>
    /// 내부 상태/리소스 정리 
    /// </summary>
    private void CleanupGetItemTaskState()
    {
        try { getItemCtr.Dispose(); } catch { /* 무시 */ }
        getItemCtr = default;
        getItemTCS = null;
    }

    #endregion

    #region RemoveItemAsync

    /// <summary>
    /// 현재 진행 중인 removeItem 요청의 완료를 통지하는 TCS
    /// </summary>
    private TaskCompletionSource<string> removeItemTCS;

    /// <summary>
    /// 호출자가 넘긴 취소 토큰 등록 핸들(취소 시 해제)
    /// </summary>
    private CancellationTokenRegistration removeItemCtr;

    /// <summary>
    /// Storage에서 값을 삭제합니다. (Task 기반 / 타임아웃 미사용)
    /// </summary>
    /// <param name="key">삭제할 키</param>
    /// <param name="cancellationToken">캔슬 토큰</param>
    /// <returns>성공 시 JS 측에서 넘어온 message</returns>
    /// <exception cref="InvalidOperationException">이미 진행 중인 요청이 있을 때</exception>
    /// <exception cref="OperationCanceledException">취소 시</exception>
    /// <exception cref="NotSupportedException">WebGL 환경이 아닐 때</exception>
    public Task<string> RemoveItemAsync(string key, CancellationToken cancellationToken = default)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        // 동시 호출 방지: 이전 요청이 완료되지 않았다면 예외
        if (removeItemTCS != null && !removeItemTCS.Task.IsCompleted)
            throw new InvalidOperationException("이미 진행 중인 RemoveItem 요청이 있습니다.");

        // 콜백 스레드 안전성을 위해 RunContinuationsAsynchronously 사용
        removeItemTCS = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

        // 취소 토큰이 트리거되면 Task 취소 및 정리
        if (cancellationToken.CanBeCanceled)
        {
            removeItemCtr = cancellationToken.Register(() =>
            {
                removeItemTCS?.TrySetCanceled(cancellationToken);
                CleanupRemoveItemTaskState();
            });
        }

        // JS 쪽에 콜백 대상 메서드 이름을 넘겨 호출
        RequestStorageRemoveItem(gameObject.name, "RemoveItemCallbackAsync", key);
        return removeItemTCS.Task;
    #else
        return Task.FromException<string>(new NotSupportedException("Storage.removeItem은 WebGL 빌드에서만 동작합니다."));
    #endif
    }

    public void RemoveItemCallbackAsync(string jsonResponse)
    {
        try
        {
            var response = JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

            if (removeItemTCS == null)
            {
                // 대기 중인 요청이 없을 때 들어온 콜백 — 로그만 남김
                Debug.LogWarning("대기 중인 RemoveItem 요청이 없는데 콜백이 도착했습니다.");
                return;
            }

            if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
            {
                removeItemTCS.TrySetResult(response.message);
            }
            else
            {
                removeItemTCS.TrySetException(new Exception(response.message));
            }
        }
        catch (Exception e)
        {
            removeItemTCS?.TrySetException(new Exception($"JSON PARSE ERROR\n{e.Message}"));
        }
        finally
        {
            CleanupRemoveItemTaskState();
        }
    }

    /// <summary>
    /// 내부 상태/리소스 정리 
    /// </summary>
    private void CleanupRemoveItemTaskState()
    {
        try { removeItemCtr.Dispose(); } catch { /* 무시 */ }
        removeItemCtr = default;
        removeItemTCS = null;
    }

    #endregion
    
    #region AllClearItemAsync

    /// <summary>
    /// 현재 진행 중인 allClearItem 요청의 완료를 통지하는 TCS
    /// </summary>
    private TaskCompletionSource<string> allClearItemTCS;

    /// <summary>
    /// 호출자가 넘긴 취소 토큰 등록 핸들(취소 시 해제)
    /// </summary>
    private CancellationTokenRegistration allClearItemCtr;

    /// <summary>
    /// Storage의 모든 항목을 초기화합니다. (Task 기반 / 타임아웃 미사용)
    /// </summary>
    /// <param name="cancellationToken">캔슬 토큰</param>
    /// <returns>성공 시 JS 측에서 넘어온 message</returns>
    /// <exception cref="InvalidOperationException">이미 진행 중인 요청이 있을 때</exception>
    /// <exception cref="OperationCanceledException">취소 시</exception>
    /// <exception cref="NotSupportedException">WebGL 환경이 아닐 때</exception>
    public Task<string> AllClearItemAsync(CancellationToken cancellationToken = default)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        // 동시 호출 방지: 이전 요청이 완료되지 않았다면 예외
        if (allClearItemTCS != null && !allClearItemTCS.Task.IsCompleted)
            throw new InvalidOperationException("이미 진행 중인 AllClearItem 요청이 있습니다.");

        // 콜백 스레드 안전성을 위해 RunContinuationsAsynchronously 사용
        allClearItemTCS = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

        // 취소 토큰이 트리거되면 Task 취소 및 정리
        if (cancellationToken.CanBeCanceled)
        {
            allClearItemCtr = cancellationToken.Register(() =>
            {
                allClearItemTCS?.TrySetCanceled(cancellationToken);
                CleanupAllClearItemTaskState();
            });
        }

        // JS 쪽에 콜백 대상 메서드 이름을 넘겨 호출
        RequestStorageAllClearItem(gameObject.name, "AllClearItemCallbackAsync");
        return allClearItemTCS.Task;
    #else
        return Task.FromException<string>(new NotSupportedException("Storage.clearItems은 WebGL 빌드에서만 동작합니다."));
    #endif
    }

    public void AllClearItemCallbackAsync(string jsonResponse)
    {
        try
        {
            var response = JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

            if (allClearItemTCS == null)
            {
                // 대기 중인 요청이 없을 때 들어온 콜백 — 로그만 남김
                Debug.LogWarning("대기 중인 AllClearItem 요청이 없는데 콜백이 도착했습니다.");
                return;
            }

            if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
            {
                allClearItemTCS.TrySetResult(response.message);
            }
            else
            {
                allClearItemTCS.TrySetException(new Exception(response.message));
            }
        }
        catch (Exception e)
        {
            allClearItemTCS?.TrySetException(new Exception($"JSON PARSE ERROR\n{e.Message}"));
        }
        finally
        {
            CleanupAllClearItemTaskState();
        }
    }

    /// <summary>
    /// 내부 상태/리소스 정리 
    /// </summary>
    private void CleanupAllClearItemTaskState()
    {
        try { allClearItemCtr.Dispose(); } catch { /* 무시 */ }
        allClearItemCtr = default;
        allClearItemTCS = null;
    }

    #endregion
}