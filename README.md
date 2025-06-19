# Project_Combined
사전합반 프로젝트 Convention

### Code Convention
#### 1. 변수명
  - private : _stringName (카멜 표기법)
  - public : StringName (파스칼 표기법)
  - param : stringName (카멜 표기법)
  
#### 2. 접근 한정자 명시적으로 작성할 것

#### 3. Property는 `get; set;` 으로 통일 및 `Property<T>` 활용
```cs
public class Property<T>
{
    private T _value;

    public T Value
    {
        get { return _value; }
        set
        {
        	// 변경된 값이 기존의 값과 일치하지 않는 경우에만
            if(!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value; 
                OnChanged?.Invoke(_value); 
            }
        }
    }
    public event Action<T> OnChanged;

    public Property(T value)
    {
        _value = value;
    }
}
```

#### 4. 주석(annotation)
  - public 메서드에는 summary까지는 아니어도 어디에서 어떻게 사용되는지에 대한 설명 필요
  - 매개변수가 많을 경우 summary를 활용하여 매개변수에 대한 설명도 언급
  - public 필드나 `Property<T>`의 경우 필요하면 설명

### Comit Message Convention
#### 1. Comit Type</span>
  - [Feat] : 기능 구현 및 추가
  - [Chore] : 단순 작업 (폴더 정리)
  - [Docs] : 문서 작업
  - [Fix] : 이슈 해결
  - [Issue] : 이슈 발생
  
#### 2. Subject
  - 한글, 영어 혼용 가능
  - 50글자 내외, 영어 사용시 첫글자 대문자 사용
  - 마침표, 특수기호, 과거시제 사용x

### Git Convention
#### 1. projects
  - Issue 발생시 projects에 추가할 것
  - 버그는 [Bug] 기능 구현은 [Feat]으로 작성
  - 예시 : [Bug] Camera shake issue / [Feat] Add Backpack script
  
#### 2. Branch
  - Main : 정상적으로 작동되는 기능들을 Main Branch에 병합
  - Develop : 기능을 개발하는 Branch. 매일 6시 전 머지, 충돌 확인. 개인 Branch는 Develop에서 생성
  - Initial Branch : 개인이 전체적으로 작업하는 Branch
  - Initial_Feature : 개인이 구현하는 기능을 담당하는 브랜치
  
#### 3. Pull Request
풀리퀘스트는 팀장 승인만 있어도 가능하도록 설정

### Foldering Convention
#### 1. Shared folder
  0. Imports : 에셋, 외부 파일 관리. private으로 repository 생성
  1. Scenes : 씬 파일 관리
  2. Scripts : 모든 스크립트 파일 관리, S.O도 함께 관리
  3. Prefabs : 프리팹 파일 관리
  
#### 2. Personal folder
  - 이름 이니셜로 폴더 생성, 하위에 작업 내역 정리
  - 작업이 완성되는 경우 위의 공용 폴더에 이름에 이니셜을 적은 후 파일 이동
  - 예시 : LSW_PlayerController
  
### Feedback
1. 코딩이 잘 풀리지 않는 경우 팀원들과 문제 상황 공유하기
2. 서로 참조하는 클래스 관계에서 특정 메서드의 알고리즘을 구현할 때 의견 교환 활발하게 진행
3. 미리 구현이 필요한 기능에 대한 R&D를 미리 진행
