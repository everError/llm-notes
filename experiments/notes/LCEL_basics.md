# LangChain Expression Language (LCEL)

## LCEL이란?
**LangChain Expression Language**는 LangChain 컴포넌트를 조합하여 복잡한 작업 흐름을 구성할 수 있게 해주는 도구이자 문법 체계입니다. 이를 통해 사용자는 간단한 작업에서부터 복잡한 파이프라인까지 유연하게 설정할 수 있습니다.

---

## 주요 특징
1. **유연한 구성**
   - 다양한 LangChain 컴포넌트를 조합하여 작업 흐름을 설계할 수 있음.
2. **표준화된 문법**
   - 직관적인 문법 체계를 제공하여 작업 설정을 단순화.
3. **확장성**
   - 커스텀 컴포넌트 및 연산자를 추가하여 고유한 작업 흐름을 구성 가능.

---

## 동작 방식
1. **프롬프트 작성**
   - 템플릿 기반 또는 문자열 형태로 프롬프트를 구성.
2. **LLM과 연결**
   - 구성된 프롬프트를 LLM에 제공하여 작업을 수행.
3. **연산자 사용**
   - `|` (pipe) 연산자를 활용하여 컴포넌트를 체인 형태로 연결.

### 예제
```python
from langchain.prompts import PromptTemplate
from langchain.chains import LLMChain
from langchain.llms import OpenAI

# 프롬프트 템플릿 생성
prompt = PromptTemplate(template="Translate the following text to French: {text}", input_variables=["text"])

# LLM 연결
llm = OpenAI(temperature=0.7)
chain = LLMChain(llm=llm, prompt=prompt)

# 실행
output = chain.run({"text": "Hello, how are you?"})
print(output)
```
---

## 주요 구성 요소

### 1. **프롬프트 (Prompt)**
- **템플릿 기반**으로 동작하며, 동적 입력을 지원.
- 예: `Translate the following text to French: {text}`

#### Prompt 구성 방법
1. **정적 문자열 사용**: 단순히 고정된 프롬프트를 작성.
2. **동적 템플릿**: 변수와 함께 동작하는 템플릿 사용. LangChain의 `PromptTemplate` 클래스를 활용.

### 2. **연산자 (Operators)**
- **파이프 연산자 (`|`)**
  - LangChain에서 컴포넌트를 연결하는 데 사용.
  - 입력 데이터를 가공하거나, 여러 단계를 연결하여 복잡한 작업 수행.

#### 파이프 연산자 예제
```python
# 데이터 가공
processed_data = data | preprocess | embed | retrieve | summarize
```
- `preprocess`: 데이터 전처리
- `embed`: 데이터를 벡터화
- `retrieve`: 벡터화된 데이터를 검색
- `summarize`: 결과 요약

### 3. **LLM (Large Language Models)**
- 텍스트 생성의 핵심 엔진으로 사용.
- OpenAI, Anthropic 등 다양한 모델을 지원.

---

## 활용 사례
1. **다중 작업 체인 구성**
   - 예: 번역 → 요약 → 키워드 추출.
2. **프롬프트 설계 및 테스트**
   - 다양한 템플릿을 테스트하며 최적화된 프롬프트 설계 가능.
3. **복잡한 파이프라인 구현**
   - 여러 컴포넌트를 조합하여 데이터 전처리, 검색, 생성 등의 작업 흐름을 자동화.

---

## 추가적으로 고려할 점
1. **문법 설계의 직관성**
   - 사용자 친화적인 문법 체계로 쉽게 학습 가능.
2. **컴포넌트 확장성**
   - 기본 제공 컴포넌트 외에도 사용자 정의 컴포넌트를 추가 가능.
3. **성능 최적화**
   - 복잡한 체인 작업 시 효율성을 고려한 설계가 필요.

---

LangChain Expression Language는 직관적이고 유연한 문법 체계를 통해 복잡한 작업을 간단히 구성할 수 있도록 지원하며, LangChain의 기능을 극대화하는 데 중요한 역할을 합니다.
