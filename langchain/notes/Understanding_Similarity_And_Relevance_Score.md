# Understanding Similarity and Relevance Score in Vector Databases

벡터 데이터베이스에서 **유사도(Similarity)**와 **관련성 점수(Relevance Score)**는 검색의 핵심 요소입니다. 이 문서는 두 개념의 차이와 이를 활용하는 방법, 개발자가 알아야 할 실무적인 내용을 정리합니다.

---

## 1. 유사도 (Similarity)

### 1.1 개념

- **유사도**는 두 벡터 간의 수학적 유사성을 측정하는 값입니다.
- 주로 벡터 공간에서 각 벡터의 방향과 거리를 기반으로 계산됩니다.

### 1.2 주요 계산 방법

1.  **코사인 유사도 (Cosine Similarity)**

    - 두 벡터의 방향적 유사성을 측정.
    - 값의 범위: -1(반대 방향) ~ 1(같은 방향).
    - 공식:

          \[
          \text{Cosine Similarity} = \frac{\mathbf{A} \cdot \mathbf{B}}{\|\mathbf{A}\| \|\mathbf{B}\|}
          \]

2.  **유클리드 거리 (Euclidean Distance)**

    - 두 벡터 간의 직선 거리.
    - 값이 작을수록 유사도가 높음.
    - 공식:

          \[
          \text{Euclidean Distance} = \sqrt{\sum_{i=1}^{n} (A_i - B_i)^2}
          \]

3.  **점수의 정규화**
    - 유사도 값은 특정 애플리케이션 요구 사항에 따라 0~1 범위로 정규화됩니다.

---

## 2. 관련성 점수 (Relevance Score)

### 2.1 개념

- **관련성 점수**는 유사도를 기반으로, 검색 결과가 사용자 쿼리에 얼마나 관련이 있는지를 나타내는 값입니다.
- 유사도 값을 응용하여 사용자의 검색 의도와 문서의 적합도를 평가합니다.

### 2.2 주요 특징

1. **정규화**:

   - 유사도를 0~1 또는 0~100 범위로 변환하여 직관적으로 표현.

2. **가중치 적용**:

   - 특정 필드(예: 제목, 본문, 메타데이터)에 더 높은 중요도를 부여.

3. **결과 정렬**:
   - 관련성 점수에 따라 검색 결과를 정렬하여 최상의 결과를 상위에 노출.

---

## 3. 유사도와 관련성 점수의 차이

| **항목**        | **유사도(Similarity)**                          | **관련성 점수(Relevance Score)**                        |
| --------------- | ----------------------------------------------- | ------------------------------------------------------- |
| **정의**        | 두 벡터 간의 수학적 유사성.                     | 유사도를 기반으로, 검색 쿼리와 결과 간의 관련성을 평가. |
| **값의 범위**   | -1 ~ 1 (코사인 유사도), 0 이상 (유클리드 거리). | 보통 0 ~ 1 또는 0 ~ 100.                                |
| **활용 목적**   | 두 데이터 간의 비슷한 정도를 측정.              | 검색 결과의 중요도 및 적합성을 결정.                    |
| **가중치 적용** | 일반적으로 없음.                                | 특정 필드나 조건에 따라 적용 가능.                      |

---

## 4. 개발자를 위한 실무 가이드

### 4.1 유사도 계산 선택 기준

1. **코사인 유사도**:

   - 벡터의 방향이 중요한 경우 사용.
   - 텍스트 데이터에서 가장 일반적으로 사용됨.

2. **유클리드 거리**:

   - 벡터의 크기 차이를 고려해야 하는 경우 사용.
   - 고차원 공간에서의 검색에 적합.

3. **Jaccard 유사도**:
   - 집합 간의 유사성을 측정하며, 문서의 키워드 비교에 사용 가능.

### 4.2 관련성 점수 최적화

1. **정규화 전략**:

   - 다양한 유사도 계산 결과를 통일된 척도로 변환.

2. **가중치 적용**:

   - 예: 제목과 본문의 점수를 다르게 가중치를 부여하여 검색 품질 향상.

3. **임계값 설정**:
   - 관련성 점수가 특정 임계값 이하인 결과를 필터링하여 노이즈 제거.

### 4.3 벡터 데이터베이스 활용

1. **LangChain과 FAISS**:

   - LangChain은 FAISS와 같은 벡터 데이터베이스와 통합되어 유사도 및 관련성 점수를 쉽게 계산 가능.

   ```python
   from langchain.vectorstores import FAISS
   from langchain.embeddings import OpenAIEmbeddings

   # 벡터 데이터베이스 초기화
   embedding_model = OpenAIEmbeddings()
   vectorstore = FAISS.from_documents(documents, embedding_model)

   # 검색 수행
   query = "What is the capital of France?"
   results = vectorstore.similarity_search(query, k=5)

   # 결과 출력
   for result in results:
       print(f"Document: {result['content']}, Score: {result['score']}")
   ```

2. **Pinecone**:

   - 클라우드 기반 벡터 데이터베이스로, 대규모 벡터 데이터 처리에 적합.

3. **Milvus**:
   - 분산형 벡터 데이터베이스로, 고성능 검색 작업에 유리.

---

## 5. 추가 고려 사항

### 5.1 고차원 데이터 문제

- 벡터의 차원이 높아지면 계산 복잡도가 증가하는 문제가 발생합니다.
- **차원 축소 기술**(예: PCA, t-SNE)을 활용해 효율성을 높일 수 있습니다.

### 5.2 하드웨어 요구 사항

- GPU를 활용하면 대규모 데이터셋에서 유사도 계산 속도를 크게 향상시킬 수 있습니다.

### 5.3 데이터 전처리

- 텍스트 데이터를 벡터화하기 전에 **중복 제거**, **정규화**, **불용어 제거** 등의 전처리 작업이 필요합니다.

---

유사도와 관련성 점수는 효과적인 검색 시스템 구축에 필수적인 요소입니다. 이 문서에서 제공한 개념과 실무 팁을 기반으로, 검색 시스템의 정확성과 효율성을 극대화할 수 있습니다.
