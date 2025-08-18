## \#\# 가상 환경 관리 ⚙️

프로젝트별로 독립된 개발 공간을 만들고 관리하는 명령어입니다.

- **환경 생성:**
  새로운 가상 환경을 만듭니다.

  ```bash
  # 'myenv'라는 이름으로 Python 3.12 버전 환경 생성
  mamba create -n myenv python=3.12
  ```

- **환경 활성화:**
  만들어진 가상 환경으로 들어갑니다.

  ```bash
  mamba activate myenv
  ```

- **환경 비활성화:**
  현재 활성화된 가상 환경에서 빠져나옵니다.

  ```bash
  mamba deactivate
  ```

- **환경 목록 보기:**
  지금까지 만든 모든 가상 환경의 목록을 확인합니다.

  ```bash
  mamba env list
  ```

- **환경 삭제:**
  더 이상 사용하지 않는 가상 환경을 제거합니다.

  ```bash
  mamba env remove -n myenv
  ```

## \#\# 패키지 관리 📦

활성화된 환경 안에서 필요한 라이브러리를 설치하고 관리하는 명령어입니다.

- **패키지 설치:**
  특정 라이브러리를 설치합니다.

  ```bash
  # numpy 단일 패키지 설치
  mamba install numpy

  # pandas와 matplotlib 여러 패키지 동시 설치
  mamba install pandas matplotlib
  ```

- **패키지 삭제:**
  설치된 라이브러리를 제거합니다.

  ```bash
  mamba remove numpy
  ```

- **패키지 업데이트:**
  라이브러리를 최신 버전으로 업데이트합니다.

  ```bash
  # pandas 패키지만 업데이트
  mamba update pandas

  # 현재 환경의 모든 패키지 업데이트
  mamba update --all
  ```

- **설치된 패키지 목록 보기:**
  현재 환경에 설치된 모든 라이브러리를 확인합니다.

  ```bash
  mamba list
  ```
