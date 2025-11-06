# Unity-App-in-TossAPI

유니티 앱인토스 API 플러그인 부분 모음

현재 있는기능
- 토스 로그인 [getUserKeyForGame]
- 리더보드 열기/점수 제출 [openGameCenterLeaderboard,submitGameCenterLeaderBoardScore]

앱인토스에서 쓰는 API를 유니티에서 호출 하기 위해 만들었습니다.<br>
[Vite Wrap](https://developers-apps-in-toss.toss.im/porting_tutorials/vite_unity.html)했다는 기준으로 생성되었습니다.<br>
만약 Warp에서 UnityCanvas에서 에러가 생겼다면 [전역 설정](https://techchat-apps-in-toss.toss.im/t/npm-run-build/839/2)해보시길 바랍니다.


현재 지원하는 기능은 다음과 같습니다.
- 토스 로그인 [getUserKeyForGame]
- 리더보드 열기/점수 제출 [openGameCenterLeaderboard,submitGameCenterLeaderBoardScore]

---

## A) Unity 패키지 매니저(UPM)로 설치 (권장)

Unity 메뉴 **`Window > Package Manager`** → 좌상단 **`+` 버튼** →  
**`Add package from git URL...`** 선택 후, 아래 주소를 하나씩 입력하고 **Add** 클릭:

**TossAPI**
```
https://github.com/Hoongcha/Unity-App-in-TossAPI.git?path=Packages/com.hoongcha.toss-api
```

## B) `manifest.json` 직접 수정

`Packages/manifest.json` 파일의 `dependencies` 블록 안에 아래처럼 추가:

```json
{
    "dependencies": {
        "com.hoongcha.toss-api": "https://github.com/Hoongcha/Unity-App-in-TossAPI.git?path=Packages/com.hoongcha.toss-api"
    }
}
```
